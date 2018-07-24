using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenancyDemo.Data
{
    public class Order : IMustHaveTenant,IHasCreateTime,IHasUpdateTime
    {
        public int Id{get;set;}
        public int UserId{get;set;}

        [ForeignKey("UserId")]
        public virtual User User{get;set;}
        public int TenantId{get;set;}

        public string OrderDes{get;set;}
        public DateTime CreateTime { get ;set; }
        public DateTime UpdateTime { get ;set; }
    }
}