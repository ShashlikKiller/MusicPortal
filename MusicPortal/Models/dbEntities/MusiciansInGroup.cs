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
    
    public partial class MusiciansInGroup
    {
        public int musicalgroup_id { get; set; }
        public int musician_id { get; set; }
        public Nullable<System.DateTime> joinDate { get; set; }
    
        public virtual MusicalGroup MusicalGroup { get; set; }
        public virtual Musician Musician { get; set; }
    }
}
