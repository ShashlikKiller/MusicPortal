using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicPortal.Models.vmEntities
{
    public class AlbumVM
    {
        public int album_id {  get; set; }
        [Required]
        [DisplayName("Название альбома")]
        public string title { get; set; }
        [Required]
        [DisplayName("Дата выхода")]
        public DateTime releaseDate { get; set; }
        [Required]
        [DisplayName("Стиль альбома")]
        public int style_id { get; set; }
        [DisplayName("Исполнитель")]
        public int musicalgroup_id { get; set; }


    }
}