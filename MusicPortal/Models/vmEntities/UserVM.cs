using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPortal.Models.vmEntities
{
    public class UserVM
    {
        public int id {  get; set; }
        [DisplayName("Имя пользователя")]
        public string nickname {  get; set; }
        [Required]
        [DisplayName("Пароль")]
        public string password {  get; set; }
        [Required]
        [DisplayName("Логин")]
        public string login { get; set; }
    }
}