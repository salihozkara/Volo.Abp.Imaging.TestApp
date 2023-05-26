using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Image.Abstractions.Volo.Abp.Image;
using IFormFile = Microsoft.AspNetCore.Http.IFormFile;

namespace Volo.Abp.Image.Abstractions;


public class AbpImageCompressFilter : IAsyncPageFilter, ITransientDependency
{
    private readonly IImageCompressorSelector _imageCompressorSelector;
    private readonly IImageFormatDetector _imageFormatDetector;

    public AbpImageCompressFilter(IImageCompressorSelector imageCompressorSelector, IImageFormatDetector imageFormatDetector)
    {
        _imageCompressorSelector = imageCompressorSelector;
        _imageFormatDetector = imageFormatDetector;
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        var method = context.HandlerMethod?.MethodInfo;
        var attribute = method?.GetCustomAttribute<AbpImageCompressAttribute>();
        if (attribute == null)
        {
            await next();
            return;
        }
        
        foreach (var parameter in context.ActionDescriptor.BoundProperties.OfType<PageBoundPropertyDescriptor>())
        {
            if (parameter.ParameterType == typeof(IFormFile) && context.HandlerInstance is not null && parameter.Property.GetValue(context.HandlerInstance) is IFormFile formFile)
            {
                if (!formFile.ContentType.StartsWith("image") || !(attribute.Parameters.Any() || attribute.Parameters.Contains(formFile.Name)))
                {
                    continue;
                }
                var newFormFile = await formFile.CompressImageAsync(_imageFormatDetector, _imageCompressorSelector);
                if(formFile == newFormFile)
                {
                    continue;
                }
                parameter.Property.SetValue(context.HandlerInstance, newFormFile);
            }
        }
        
        await next();
    }
}


public class AbpImageCompressMiddleware : IMiddleware, ITransientDependency
{
    private readonly IImageCompressorSelector _imageCompressorSelector;
    private readonly IImageFormatDetector _imageFormatDetector;


    public AbpImageCompressMiddleware(IImageCompressorSelector imageCompressorSelector, IImageFormatDetector imageFormatDetector)
    {
        _imageCompressorSelector = imageCompressorSelector;
        _imageFormatDetector = imageFormatDetector;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        var methodInfo = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>()?.MethodInfo;
        var methodInfos = endpoint?.Metadata.GetMetadata<ActionDescriptor>();
        
        var methodAttribute = methodInfo?.GetCustomAttribute<AbpImageCompressAttribute>();
        if(methodAttribute != null)
        {
            await Asd(context, methodAttribute);
        }
        
        var attributeParams = methodInfo?.GetParameters().Where(x => x.GetCustomAttribute<AbpImageCompressAttribute>() != null).ToList() ?? new List<ParameterInfo>();

        foreach (var parameterInfo in attributeParams)
        {
            
        }

        await next(context);
    }

    private async Task Asd(HttpContext context, AbpImageCompressAttribute attribute)
    {
        if (context.Request is { HasFormContentType: true, Form.Files: FormFileCollection formFileCollection })
        {
            for (var i = 0; i < formFileCollection.Count; i++)
            {
                var formFile = formFileCollection[i];
                if (!(attribute.Parameters.Any() || attribute.Parameters.Contains(formFile.Name)))
                {
                    continue;
                }

                var newFormFile = await formFile.CompressImageAsync(_imageFormatDetector, _imageCompressorSelector);
                formFileCollection[i] = newFormFile;
            }
        }
    }
}