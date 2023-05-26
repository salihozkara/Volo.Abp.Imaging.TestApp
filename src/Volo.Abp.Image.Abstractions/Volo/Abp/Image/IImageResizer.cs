namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public interface IImageResizer
{
    Task<Stream> ResizeAsync(Stream stream, IImageResizeParameter resizeParameter, CancellationToken cancellationToken = default);
    
    bool CanResize(IImageFormat? imageFormat);
}