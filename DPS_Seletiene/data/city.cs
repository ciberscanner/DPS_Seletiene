//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DPS_Seletiene.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class city
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public city()
        {
            this.userapp = new HashSet<userapp>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string PostalCodeGeo { get; set; }
    
        public virtual department department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userapp> userapp { get; set; }
    }
}
