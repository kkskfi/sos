//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace conference1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Moderators
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Moderators()
        {
            this.Activity = new HashSet<Activity>();
        }
    
        public int id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public System.DateTime Birthday { get; set; }
        public int Country { get; set; }
        public string Phone { get; set; }
        public int Direction { get; set; }
        public int Event { get; set; }
        public string Password { get; set; }
        public string PhotoPath { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity> Activity { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual Direction Direction1 { get; set; }
        public virtual Event Event1 { get; set; }
        public virtual Genders Genders { get; set; }
    }
}
