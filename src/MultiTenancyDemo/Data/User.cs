using System.Collections.Generic;

namespace MultiTenancyDemo.Data
{
    public class User : IMustHaveTenant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TenancyId { get ;set; }

        public UserStatus Status{get;set;}
        
        public TenantInfo TenantInfo { get; set; }
        
        public IList<Order> Orders { get; set; }
    }
}