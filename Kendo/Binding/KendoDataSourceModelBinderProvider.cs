using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace Artech.AspNetCore.Kendo.Binding
{
    public class KendoDataSourceModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(KendoDataSourceRequest))
            {
                return new BinderTypeModelBinder(typeof(KendoDataSourceModelBinder));
            }

            return null;
        }
    }
}
