namespace MultiTenancyDemo.Data
{
    public class Order : IMustHaveTenant
    {
        public int Id{get;set;}
        
        public int TenancyId { get;set; }
        public int UserId{get;set;}
        public User User{get;set;}

        public string OrderDes{get;set;}

    }
}