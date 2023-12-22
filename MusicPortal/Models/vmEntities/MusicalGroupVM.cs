using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPortal.Models.vmEntities
{
    public class MusicalGroupVM
    {
        public int id {  get; set; }
        [Required]
        [DisplayName("Название группы")]
        public string groupName { get; set; }

        [Required]
        [DisplayName("Тип музыкальной группы")]
        public int musicalgrouptype_id { get; set; }
    }
}