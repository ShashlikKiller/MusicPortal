using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MusicPortal.Models.vmEntities
{
    public class CompositionVM
    {
        public int composition_id {  get; set; }
        [Required]
        [DisplayName("Название композиции")]
        public string name {  get; set; }
        [Required]
        [DisplayName("Продолжительность композиции")]
        public TimeSpan duration { get; set; }
    }
}