//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicPortal.Models.dbEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class MusicalGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MusicalGroup()
        {
            this.Album = new HashSet<Album>();
            this.MusiciansInGroup = new HashSet<MusiciansInGroup>();
            this.UserFavoriteMusicalGroup = new HashSet<UserFavoriteMusicalGroup>();
        }
    
        public int id { get; set; }
        public string groupName { get; set; }
        public int musicalgrouptype_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Album> Album { get; set; }
        public virtual MusicalGroupType MusicalGroupType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MusiciansInGroup> MusiciansInGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFavoriteMusicalGroup> UserFavoriteMusicalGroup { get; set; }
    }
}
