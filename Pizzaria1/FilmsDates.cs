//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KINOwpf
{
    using System;
    using System.Collections.Generic;
    
    public partial class FilmsDates
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FilmsDates()
        {
            this.FilmsDatesSeances = new HashSet<FilmsDatesSeances>();
        }
    
        public int Id { get; set; }
        public Nullable<int> FilmId { get; set; }
        public Nullable<int> DateId { get; set; }
    
        public virtual Dates Dates { get; set; }
        public virtual Films Films { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilmsDatesSeances> FilmsDatesSeances { get; set; }
    }
}
