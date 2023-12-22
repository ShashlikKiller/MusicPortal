﻿using MusicPortal.Models.dbEntities;
using MusicPortal.Models.vmEntities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Security;
using System;
using System.Web;

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
        public ActionResult ListOfAlbumsOfGroup(int group_id)
        {
            List<Album> Albums = new List<Album>();
            using (var db = new PortalEntities())
            {
                Albums = db.Album.Where(x => x.musicalgroup_id == group_id).ToList();
            }
            return View(Albums);
        }

        // Вывод всех композиций в альбоме
        public ActionResult ListOfCompositionsInAlbum(int album_id)
        {
            List<Composition> albumSongs = new List<Composition>();
            using (var db = new PortalEntities())
            {
                foreach (var composition in db.Composition.Where(x => x.album_id == album_id))
                {
                    albumSongs.Add(composition);
                }
            }
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

        // Личный кабинет пользователя
        public ActionResult UserProfile()
        {
            List<Album> albums = new List<Album>();
            string currentUserLogin = HttpContext.User.Identity.Name;
            List<Composition> downloadedSongs = new List<Composition>();
            List<Composition> listenedSongs = new List<Composition>();
            //List<Group> favoriteGroups = new List<Group>();
            User currentUser = new User();

            using (var db = new PortalEntities())
            {
                albums = db.Album.ToList();
                currentUser = db.User.Where(x => x.login == currentUserLogin).FirstOrDefault();

                downloadedSongs = db.UserDownloadedCompositions
                                    .Where(x => x.user_id == currentUser.id)
                                    .Select(x => x.Composition)
                                    .ToList();

                listenedSongs = db.UserListenedComposition
                                    .Where(x => x.user_id == currentUser.id)
                                    .Select(x => x.Composition)
                                    .ToList();

                //favoriteGroups = db.Favorite
                //                    .Where(x => x.user_id == currentUser.ID)
                //                    .Select(x => x.Group)
                //                    .ToList();
            }
            ViewBag.albums = albums;
            UserProfile model = new UserProfile(downloadedSongs, listenedSongs);//, favoriteGroups);
            return View(model);
        }

        // Удаление песни из списка любимых
        // BIG WARNING
        // Изменить с прослушенных на избранные!!! Когда таблица добавиться!!!!!!
        // BIG WARNING
        [HttpPost]
        public void RemoveFromFavorites(int songId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                //return RedirectToAction("Login", "Portal");

            }

            string userLogin = User.Identity.Name;
            using (var db = new PortalEntities())
            {
                /////
                //var users = db.User.Include(a => a.MusicalGroup).ToList();
                /////
                var user = db.User.FirstOrDefault(u => u.login == userLogin);
                if (user == null)
                {
                    //return HttpNotFound();
                }

                var favoriteSong = db.UserListenedComposition.FirstOrDefault(fs => fs.user_id == user.id && fs.composition_id == songId);
                if (favoriteSong == null)
                {
                    //return HttpNotFound();
                }

                db.Entry(favoriteSong).State = System.Data.Entity.EntityState.Deleted;// UserListenedComposition.Remove(favoriteSong);
                db.SaveChanges();
            }
            //return View();
            RedirectToAction("UserProfile", "Portal");
        }
        //[HttpGet]
        //public ActionResult RemoveFromFavorites()
        //{

        //}

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