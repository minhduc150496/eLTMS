//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eLTMS.Web.DbConnection
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hospital
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hospital()
        {
            this.HospitalDepartments = new HashSet<HospitalDepartment>();
            this.HospitalSuggestings = new HashSet<HospitalSuggesting>();
        }
    
        public int HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public string Hotline { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HospitalDepartment> HospitalDepartments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HospitalSuggesting> HospitalSuggestings { get; set; }
    }
}
