namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public interface IImageCompressorSelector
{
    IImageCompressor? FindCompressor(IImageFormat? imageFormat);
}