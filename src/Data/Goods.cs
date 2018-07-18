namespace MultiTenancyDemo.Data
{
    public class Goods : IMustHaveTenant
    {
        public int Id{get;set;}

        public string Name{get;set;}
        public double Price{get;set;}

        public string Image{get;set;}
        public int TenancyId { get ;set; }

        public GoodsStatus Status{get;set;}
    }
}