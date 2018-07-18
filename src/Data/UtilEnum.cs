namespace MultiTenancyDemo.Data
{

    public enum GoodsStatus
    {
        待发布 = 0,
        已发布 = 1,
        编辑中 = 2,
        已下架 = 3
    }

    public enum UserStatus
    {
        正常 = 0

    }

    public enum TenantType
    {
        普通租户=0,
        
        有钱租户=1
    }

    public enum TenantDbType
    {
        Mysql=0,
        SqlServer=1
    }

}