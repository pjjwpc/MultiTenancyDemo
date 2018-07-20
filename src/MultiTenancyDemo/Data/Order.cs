using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenancyDemo.Data
{
    public class Order : IMustHaveTenant
    {
        public int Id{get;set;}
        public int UserId{get;set;}

        [ForeignKey("UserId")]
        public virtual User User{get;set;}
        public int TenantId{get;set;}

        [ForeignKey("TenantId")]
        public Tenant Tenant{get;set;}
        public string OrderDes{get;set;}

    }
}