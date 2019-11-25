using Artech.AspNetCore.Kendo.Descriptors;
using AutoMapper;
using NetCoreKendoAngularGridBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Artech.AspNetCore.Kendo
{
    public static class KendoQueryableExtensions
    {
        public static KendoDataSourceResponse<T, TDto> ToDataSourceResult<T, TDto>(this IQueryable<T> queryable, KendoDataSourceRequest request, IMapper mapper = null)
        {
            return new KendoDataSourceResponse<T, TDto>(queryable, request, mapper);
        }

        public static KendoDataSourceResponse<T,T> ToDataSourceResult<T>(this IQueryable<T> queryable, KendoDataSourceRequest request)
        {
            return new KendoDataSourceResponse<T, T>(queryable, request, null);
        }
    }
}
