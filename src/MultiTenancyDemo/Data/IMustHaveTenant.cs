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

    public interface ISoftDelete
    {
        bool IsDeleted{get;set;}
    }

    public interface IHasCreateTime
    {
        System.DateTime CreateTime{get;set;}
    }

    public interface IHasUpdateTime
    {
        System.DateTime UpdateTime{get;set;}
    }

    public interface IHasDeletionTime
    {
        System.DateTime DeletionTime{get;set;}
    }

    public interface IDeletionAudited
    {
        long? DeleterUserId{get;set;}
    }
}