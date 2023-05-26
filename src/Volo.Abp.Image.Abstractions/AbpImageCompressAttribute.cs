using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Volo.Abp.Image.Abstractions;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AbpImageCompressAttribute : Attribute
{
    public AbpImageCompressAttribute(params string[] parameters)
    {
        Parameters = parameters;
    }

    public AbpImageCompressAttribute()
    {
        Parameters = new List<string>();
    }

    public IEnumerable<string> Parameters { get; }
    
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public class AbpImageCompressParameterAttribute : Attribute , IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var parameter = context.ActionArguments.FirstOrDefault(x => x.GetType().GetCustomAttribute<AbpImageCompressParameterAttribute>() != null);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}