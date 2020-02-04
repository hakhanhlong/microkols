using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebInfluencer.Code.Binders
{
    static class NormalizeString
    {
        public static string TrimAndNullIfWhiteSpace(this string text) =>
           string.IsNullOrWhiteSpace(text)
           ? string.Empty
           : text.Trim();
    }
    public class StringModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            if (string.IsNullOrEmpty(modelName))
                return Task.CompletedTask;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.Result = ModelBindingResult.Success(
                valueProviderResult.FirstValue.TrimAndNullIfWhiteSpace());

            return Task.CompletedTask;
        }
    }

    public class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(string))
                return new BinderTypeModelBinder(typeof(StringModelBinder));

            return null;
        }
    }

}
