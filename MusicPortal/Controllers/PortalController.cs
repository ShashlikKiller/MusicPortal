using Microsoft.Ajax.Utilities;
using MusicPortal.Models.dbEntities;
using MusicPortal.Models.vmEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

        #region Авторизация
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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        #endregion
    }
}