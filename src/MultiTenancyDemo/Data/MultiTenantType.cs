namespace MultiTenancyDemo.Data
{
    public enum MultiTenantType
    {
         /// <summary>
        /// Tenant side.
        /// </summary>
        Tenant = 1,
        
        /// <summary>
        /// Host (tenancy owner) side.
        /// </summary>
        Host = 2
    }
}