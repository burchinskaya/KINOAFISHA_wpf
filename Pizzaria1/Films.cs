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
    
    public partial class Films
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Films()
        {
            this.FilmsDates = new HashSet<FilmsDates>();
            this.FilmsGenres = new HashSet<FilmsGenres>();
            this.Subscriptions = new HashSet<Subscriptions>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; }
        public string Country { get; set; }
        public float RatingIMDb { get; set; }
        public float RatingKinopoisk { get; set; }
        public string Slogan { get; set; }
        public byte[] PosterByte { get; set; }
        public System.DateTime PremierDate { get; set; }
        public bool IsPremiere { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilmsDates> FilmsDates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilmsGenres> FilmsGenres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subscriptions> Subscriptions { get; set; }
    }
}
