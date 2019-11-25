using Artech.AspNetCore.Kendo.Descriptors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Artech.AspNetCore.Kendo.Binding
{ 
    public class KendoDataSourceModelBinder : IModelBinder
    {
        readonly INamingService _namingService;

        public KendoDataSourceModelBinder(INamingService namingService, IMapper mapper)
        {
            _namingService = namingService ?? new IdentityNamingService();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            KendoDataSourceRequest request = new KendoDataSourceRequest();

            string sort, group, filter/*, aggregates*/;
            int currentPage;
            int pageSize;

            KendoGridHelper helper = new KendoGridHelper(_namingService);

            if (TryGetValue(bindingContext, "page", out currentPage))
            {
                request.Page = currentPage;
            }

            if (TryGetValue(bindingContext, "pageSize", out pageSize))
            {
                request.PageSize = pageSize;
            }

            if (TryGetValue(bindingContext, "sort", out sort))
            {
                request.Sorts = helper.Deserialize<SortDescriptor>(sort);
            }

            if (TryGetValue(bindingContext, "filter", out filter))
            {
                request.FilterWrapper = helper.ParseFilter(filter);
            }

            if (TryGetValue(bindingContext, "group", out group))
            {
                request.Groups = helper.Deserialize<GroupDescriptor>(group);
            }

            //if (TryGetValue(bindingContext, GridUrlParameters.Aggregates, out aggregates))
            //{
            //    request.Aggregates = GridDescriptorSerializer.Deserialize<AggregateDescriptor>(aggregates);
            //}

            bindingContext.Result = ModelBindingResult.Success(request);

            return Task.CompletedTask;
        }

        public string Prefix { get; set; }

        private bool TryGetValue<T>(ModelBindingContext bindingContext, string key, out T result)
        {
            if (!string.IsNullOrEmpty(Prefix))
            {
                key = Prefix + "-" + key;
            }

            var value = bindingContext.ValueProvider.GetValue(key);

            if (value == ValueProviderResult.None)
            {
                result = default(T);
                return false;
            }
            else
            {
                result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value.FirstValue);
            }

            return true;
        }
    }
}
