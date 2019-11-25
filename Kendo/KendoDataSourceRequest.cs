using Artech.AspNetCore.Kendo.Descriptors;
using System.Collections.Generic;

namespace Artech.AspNetCore.Kendo
{
    public class KendoDataSourceRequest
    {
        public KendoDataSourceRequest()
        {
            Page = 1;
            PageSize = 20;
            //Aggregates = new List<AggregateDescriptor>();
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public IList<SortDescriptor> Sorts { get; set; }
        public FilterDescriptor FilterWrapper { get; set; }
        public IList<GroupDescriptor> Groups { get; set; }
        //public IList<AggregateDescriptor> Aggregates  { get; set; }
    }
}
