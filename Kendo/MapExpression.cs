using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NetCoreKendoAngularGridBinding.Kendo
{
    public class MapExpression<TEntity>
    {
        public string Path { get; set; }
        public Expression<Func<TEntity, object>> Expression { get; set; }
    }
}
