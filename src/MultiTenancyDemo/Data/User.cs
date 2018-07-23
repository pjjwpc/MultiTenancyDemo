using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenancyDemo.Data
{
    public class User : IMustHaveTenant,IHasCreateTime,IHasUpdateTime
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserStatus Status{get;set;}

        public int TenantId{get;set;}

        [ForeignKey("TenantId")]
        public Tenant Tenant{get;set;}
        
        public IList<Order> Orders { get; set; }
        public DateTime CreateTime { get ;set; }
        public DateTime UpdateTime { get ;set; }
    }
}