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
    
    public partial class ReservationCodes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReservationCodes()
        {
            this.ReservationPlaces = new HashSet<ReservationPlaces>();
        }
    
        public int Id { get; set; }
        public int Code { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> FilmDateSeanceId { get; set; }
        public int TotalPrice { get; set; }
    
        public virtual FilmsDatesSeances FilmsDatesSeances { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReservationPlaces> ReservationPlaces { get; set; }
    }
}
