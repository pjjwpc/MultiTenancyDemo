namespace MultiTenancyDemo.Data
{
    public class TenantInfo
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public TenantType TenantType { get; set; }
        
        public string Connection { get; set; }
        
        public TenantDbType TenantDbType { get; set; }
        
        public User User { get; set; }
    }
}