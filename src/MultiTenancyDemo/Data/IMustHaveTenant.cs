namespace MultiTenancyDemo.Data
{
    public interface IMustHaveTenant
    {
        int TenantId{get;set;}
    }

    public interface IMayHaveTenant
    {
        int? TenantId{get;set;}
    }
}