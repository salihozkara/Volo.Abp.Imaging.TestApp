namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public interface IImageCompressor
{
    Task<Stream> CompressAsync(Stream stream, CancellationToken cancellationToken = default);
    
    bool CanCompress(IImageFormat? imageFormat);
}