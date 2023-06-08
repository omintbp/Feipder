using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace Feipder.Tools
{
    public class SwaggerEnumerable<T> : IModelBinder where T : IEnumerable<T>
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var val = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            var first = string.Concat("[", string.Join(",", val.ToList()), "]");
            var model = JsonSerializer.Deserialize<T>(first);
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
