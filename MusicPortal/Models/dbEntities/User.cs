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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.UserDownloadedCompositions = new HashSet<UserDownloadedCompositions>();
            this.UserFavoriteComposition = new HashSet<UserFavoriteComposition>();
            this.UserFavoriteMusicalGroup = new HashSet<UserFavoriteMusicalGroup>();
            this.UserListenedComposition = new HashSet<UserListenedComposition>();
        }
    
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public Nullable<int> role_id { get; set; }
    
        public virtual Role Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserDownloadedCompositions> UserDownloadedCompositions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFavoriteComposition> UserFavoriteComposition { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFavoriteMusicalGroup> UserFavoriteMusicalGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserListenedComposition> UserListenedComposition { get; set; }
    }
}
