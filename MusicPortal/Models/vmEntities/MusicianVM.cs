using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPortal.Models.vmEntities
{
    public class MusicianVM
    {
        public int id { get; set; }
        [Required]
        [DisplayName("Имя")]
        public string firstName { get; set; }
        [Required]
        [DisplayName("Фамилия")]
        public string surName { get; set; }
    }
}