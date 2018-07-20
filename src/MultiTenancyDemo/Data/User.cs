using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenancyDemo.Data
{
    public class User : IMustHaveTenant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserStatus Status{get;set;}

        public int TenantId{get;set;}

        [ForeignKey("TenantId")]
        public Tenant Tenant{get;set;}
        
        public IList<Order> Orders { get; set; }
    }
}