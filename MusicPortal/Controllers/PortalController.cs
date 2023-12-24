using MusicPortal.Models.dbEntities;
using MusicPortal.Models.vmEntities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Security;
using System;
using System.Web;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text.RegularExpressions;

namespace MusicPortal.Controllers
{
    public class PortalController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // Вывод всех музыкальных групп
        public ActionResult ListOfGroups()
        {
            List<MusicalGroup> groups = new List<MusicalGroup>();
            using (var db = new PortalEntities())
            {
                groups = db.MusicalGroup.OrderByDescending(x => x.groupName).ToList();
            }
            return View(groups);
        }

        // Вывод всех альбомов группы
        public ActionResult ListOfAlbumsOfGroup(int groupID)
        {
            List<Album> Albums = new List<Album>();
            using (var db = new PortalEntities())
            {
                Albums = db.Album.Where(x => x.musicalgroup_id == groupID).ToList();
            }
            return View(Albums);
        }

        // Вывод всех композиций в альбоме
        public ActionResult ListOfCompositionsInAlbum(int albumID)
        {
            List<Album> albums = new List<Album>();
            List<Language> languages = new List<Language>();
            List<Composition> albumSongs = new List<Composition>();
            using (var db = new PortalEntities())
            {
                albums = db.Album.ToList();
                languages = db.Language.ToList();
                foreach (var composition in db.Composition.Where(x => x.album_id == albumID))
                {
                    albumSongs.Add(composition);
                }
            }
            ViewBag.albums = albums;
            ViewBag.languages = languages;
            return View(albumSongs);
        }

        public List<MusicalGroupType> GroupTypes()
        {
            List<MusicalGroupType> grouptypes = new List<MusicalGroupType>();
            using (var db = new PortalEntities())
            {
                foreach (var grouptype in db.MusicalGroupType)
                {
                    grouptypes.Add(grouptype);
                }
            }
            return grouptypes;
        }
        public List<Genre> Genres()
        {
            List<Genre> genres = new List<Genre>();
            using (var db = new PortalEntities())
            {
                foreach (var genre in db.Genre)
                {
                    genres.Add(genre);
                }
            }
            return genres;
        }

        // Создание группы
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateGroup()
        {
            ViewBag.grouptypes = new SelectList(GroupTypes(), "id", "groupType");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateGroup(MusicalGroupVM newgroup)
        {
            if (ModelState.IsValid)
            {
                using (var context = new PortalEntities())
                {
                    MusicalGroup band = new MusicalGroup()
                    {
                        id = 1, // whatever
                        groupName = newgroup.groupName,
                        musicalgrouptype_id = newgroup.musicalgrouptype_id
                    };
                    context.Entry(band).State = System.Data.Entity.EntityState.Added;
                    //context.MusicalGroup.Add(band);
                    context.SaveChanges();
                }
                return RedirectToAction("ListOfGroups");
            }
            ViewBag.grouptypes = new SelectList(GroupTypes(), "id", "groupType");
            return View(newgroup);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditGroup(int groupID)
        {
            MusicalGroupVM model;
            using (var context = new PortalEntities())
            {
                MusicalGroup editedgroup = context.MusicalGroup.Find(groupID);
                model = new MusicalGroupVM
                {
                    groupName = editedgroup.groupName,
                    musicalgrouptype_id = editedgroup.musicalgrouptype_id,
                };
            }
            ViewBag.grouptypes = new SelectList(GroupTypes(), "id", "groupType");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditGroup(MusicalGroupVM model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new PortalEntities())
                {
                    MusicalGroup editedGroup = new MusicalGroup
                    {
                        groupName = model.groupName,
                        musicalgrouptype_id = model.musicalgrouptype_id,
                    };
                    context.MusicalGroup.Attach(editedGroup);
                    context.Entry(editedGroup).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                return RedirectToAction("ListOfCategories");
            }
            ViewBag.grouptypes = new SelectList(GroupTypes(), "id", "groupType");
            return View(model);
        }

        // Удаление группы
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteGroup(int groupID)
        {
            MusicalGroup groupToDelete;
            using (var context = new PortalEntities())
            {
                groupToDelete = context.MusicalGroup.Find(groupID);
            }
            ViewBag.grouptypes = GroupTypes().First(x => x.id == groupToDelete.musicalgrouptype_id).groupType;
            return View(groupToDelete);
        }

        // Подтвержденное удаление
        [HttpPost, ActionName("DeleteGroup")]
        public ActionResult DeleteConfirmed(int groupID)
        {
            using (var context = new PortalEntities())
            {
                MusicalGroup groupToDelete = new MusicalGroup
                {
                    id = groupID
                };
                foreach (var song in context.Composition.Where(x => x.Album.musicalgroup_id == groupToDelete.id))
                {
                    context.Entry(song).State = System.Data.Entity.EntityState.Deleted;
                }
                foreach (var album in context.Album.Where(x=>x.musicalgroup_id == groupToDelete.id))
                {
                    context.Entry(album).State = System.Data.Entity.EntityState.Deleted;
                }
                context.Entry(groupToDelete).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
            return RedirectToAction("ListOfGroups");
        }

        // Личный кабинет пользователя
        public ActionResult UserProfile()
        {
            List<Album> albums = new List<Album>();
            List<Language> languages = new List<Language>();
            string currentUserLogin = HttpContext.User.Identity.Name;
            List<Composition> downloadedSongs = new List<Composition>();
            List<Composition> listenedSongs = new List<Composition>();
            List<Composition> favoriteSongs = new List<Composition>();
            List<MusicalGroup> favoriteGroups = new List<MusicalGroup>();
            User currentUser = new User();

            using (var db = new PortalEntities())
            {
                albums = db.Album.ToList();
                languages = db.Language.ToList();
                currentUser = db.User.Where(x => x.login == currentUserLogin).FirstOrDefault();

                downloadedSongs = db.UserDownloadedCompositions
                                    .Where(x => x.user_id == currentUser.id)
                                    .Select(x => x.Composition)
                                    .ToList();

                listenedSongs = db.UserListenedComposition
                                    .Where(x => x.user_id == currentUser.id)
                                    .Select(x => x.Composition)
                                    .ToList();

                favoriteGroups = db.UserFavoriteMusicalGroup
                                    .Where(x => x.user_id == currentUser.id)
                                    .Select(x => x.MusicalGroup)
                                    .ToList();

                favoriteSongs = db.UserFavoriteComposition
                                   .Where(x => x.user_id == currentUser.id)
                                   .Select(x => x.Composition)
                                   .ToList();
            }
            ViewBag.albums = albums;
            ViewBag.languages = languages;
            UserProfile model = new UserProfile(downloadedSongs, listenedSongs, favoriteSongs, favoriteGroups);
            return View(model);
        }

        [HttpPost]
        public void AddGroupToFavorites(int groupID)
        {
            string userLogin = User.Identity.Name;
            using (var db = new PortalEntities())
            {
                var user = db.User.FirstOrDefault(u => u.login == userLogin);
                if (user == null)
                {
                    HttpNotFound();
                }
                UserFavoriteMusicalGroup favoriteGroup = new UserFavoriteMusicalGroup();
                favoriteGroup.musicalgroup_id = groupID;
                favoriteGroup.user_id = user.id;
                favoriteGroup.likeDate = DateTime.Now;
                db.UserFavoriteMusicalGroup.Add(favoriteGroup);
                db.SaveChanges();
            }
            RedirectToAction("ListOfGroups", "Portal");
        }

        [HttpPost]
        public void AddFavoriteComposition(int compositionID)
        {
            string userLogin = User.Identity.Name;
            using (var db = new PortalEntities())
            {
                var user = db.User.FirstOrDefault(u => u.login == userLogin);
                if (user == null)
                {
                    HttpNotFound();
                }
                UserFavoriteComposition favoriteSong = new UserFavoriteComposition();
                favoriteSong.composition_id = compositionID;
                favoriteSong.user_id = user.id;
                favoriteSong.likeDate = DateTime.Now;
                db.UserFavoriteComposition.Add(favoriteSong);
                //db.Entry(favoriteGroup).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            RedirectToAction("ListOfCompositionsInAlbum", "Portal");
        }
        // Удаление песни из списка любимых
        [HttpPost]
        public void RemoveSongFromFavorites(int songId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                RedirectToAction("Login", "Portal");

            }
            string userLogin = User.Identity.Name;
            using (var db = new PortalEntities())
            {

                var user = db.User.FirstOrDefault(u => u.login == userLogin);
                if (user == null)
                {
                    HttpNotFound();
                }

                var favoriteSong = db.UserFavoriteComposition.FirstOrDefault(fs => fs.user_id == user.id && fs.composition_id == songId);
                if (favoriteSong == null)
                {
                    HttpNotFound();
                }

                db.Entry(favoriteSong).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            RedirectToAction("UserProfile", "Portal");
        }

        // Удаление группы из списка избранных
        [HttpPost]
        public void RemoveGroupFromFavorites(int groupID)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                RedirectToAction("Login", "Portal");

            }
            string userLogin = User.Identity.Name;
            using (var db = new PortalEntities())
            {

                var user = db.User.FirstOrDefault(u => u.login == userLogin);
                if (user == null)
                {
                    HttpNotFound();
                }

                var favoriteGroup = db.UserFavoriteMusicalGroup.FirstOrDefault(fs => fs.user_id == user.id && fs.musicalgroup_id == groupID);
                if (favoriteGroup == null)
                {
                    HttpNotFound();
                }

                db.Entry(favoriteGroup).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            RedirectToAction("UserProfile", "Portal");
        }

        #region Авторизация/Выход
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserVM webUser)
        {
            if (ModelState.IsValid)
            {
                using (PortalEntities db = new PortalEntities())
                {
                    User user = null;
                    user = db.User.Where(u => u.login == webUser.login).FirstOrDefault();
                    if (user != null)
                    {
                        string passwordweb = webUser.password;
                        if (passwordweb == user.password)
                        {
                            string userRole = "";
                            switch (user.role_id)
                            {
                                case 1:
                                    userRole = "Admin";
                                    break;
                                case 2:
                                    userRole = "User";
                                    break;
                            }
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                                                        1,
                                                        user.login, //name
                                                        DateTime.Now,
                                                        DateTime.Now.AddDays(1),
                                                        false,
                                                        userRole //role
                                                        );
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                            return RedirectToAction("ListOfGroups", "Portal");
                        }
                    }
                }
            }
            ViewBag.Error = "Пользователя с таким логином и паролем нет.";
            return View(webUser);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("ListOfGroups", "Portal");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        #endregion
    }
}