using System.Collections.Generic;

namespace MultiTenancyDemo.Models
{
    public class PageModel<TModel>
    {
        public int Total{get;set;}

        public IList<TModel> Rows{get;set;}
    }
}