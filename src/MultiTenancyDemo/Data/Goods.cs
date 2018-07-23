using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenancyDemo.Data
{
    public class Goods : IMustHaveTenant,IHasCreateTime,IHasUpdateTime
    {
        public int Id{get;set;}

        public string Name{get;set;}
        public double Price{get;set;}

        public string Image{get;set;}

        public int UserId{get;set;}
        
        [ForeignKey("UserId")]
        public User User{get;set;}

        public int TenantId{get;set;}

        [ForeignKey("TenantId")]
        public Tenant Tenant{get;set;}

        public GoodsStatus Status{get;set;}
        public DateTime CreateTime { get ;set; }
        public DateTime UpdateTime { get ;set; }
    }
}