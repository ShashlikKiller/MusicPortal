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
    
    public partial class Album
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Album()
        {
            this.Composition = new HashSet<Composition>();
        }
    
        public int id { get; set; }
        public int style_id { get; set; }
        public System.DateTime releaseDate { get; set; }
        public int musicalgroup_id { get; set; }
        public string title { get; set; }
        public Nullable<bool> isValid { get; set; }
    
        public virtual AlbumStyle AlbumStyle { get; set; }
        public virtual MusicalGroup MusicalGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Composition> Composition { get; set; }
    }
}
