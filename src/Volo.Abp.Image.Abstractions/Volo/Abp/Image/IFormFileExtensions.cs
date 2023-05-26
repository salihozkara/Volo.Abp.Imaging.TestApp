namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public static class IFormFileExtensions
{
    public static async Task<IFormFile> CompressImageAsync(
        this IFormFile formFile,
        IImageFormatDetector imageFormatDetector,
        IImageCompressorSelector imageCompressorSelector,
        CancellationToken cancellationToken = default)
    {
        
        if (!formFile.ContentType.StartsWith("image"))
        {
            return formFile;
        }
        var stream = await formFile.OpenReadStream().ConvertToWritableStreamAsync(false, cancellationToken);
        var format = imageFormatDetector.FindFormat(stream);
        var compressor = imageCompressorSelector.FindCompressor(format);
        
        if (compressor == null)
        {
            return formFile;
        }
        
        var compressedImageStream = await compressor.CompressAsync(stream, cancellationToken);
        
        var newFormFile = new FormFile(compressedImageStream, 0, compressedImageStream.Length, formFile.Name,
            formFile.FileName)
        {
            Headers = formFile.Headers
        };
        
        // dispose original stream when the new stream not equal to original stream
        if (compressedImageStream != stream)
        {
            await stream.DisposeAsync();
        }
        
        return newFormFile;
    }
    
    public static async Task<IFormFile> ResizeImageAsync(
        this IFormFile formFile,
        IImageResizeParameter imageResizeParameter,
        IImageFormatDetector imageFormatDetector,
        IImageResizer imageResizer,
        CancellationToken cancellationToken = default)
    {
        if (!formFile.ContentType.StartsWith("image"))
        {
            return formFile;
        }
        
        var stream = await formFile.OpenReadStream().ConvertToWritableStreamAsync(false, cancellationToken);
        var format = imageFormatDetector.FindFormat(stream);
        
        if (!imageResizer.CanResize(format))
        {
            return formFile;
        }
        
        var resizedImageStream = await imageResizer.ResizeAsync(stream, imageResizeParameter, cancellationToken);
        
        var newFormFile = new FormFile(resizedImageStream, 0, resizedImageStream.Length, formFile.Name,
            formFile.FileName)
        {
            Headers = formFile.Headers
        };
        
        // dispose original stream when the new stream not equal to original stream
        if (resizedImageStream != stream)
        {
            await stream.DisposeAsync();
        }
        
        return newFormFile;
    }

    public static Task<IFormFile> ResizeImageAsync(this IFormFile formFile,
        IImageResizeParameter imageResizeParameter, IServiceProvider serviceProvider)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageResizerSelector = serviceProvider.GetRequiredService<IImageResizerSelector>();
        var format = imageFormatDetector.FindFormat(formFile.OpenReadStream());
        return formFile.ResizeImageAsync(imageResizeParameter, imageFormatDetector, imageResizerSelector.FindResizer(format));
    }
}