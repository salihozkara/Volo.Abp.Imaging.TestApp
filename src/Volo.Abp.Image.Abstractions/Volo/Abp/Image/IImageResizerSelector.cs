namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public interface IImageResizerSelector
{
    IImageResizer? FindResizer(IImageFormat? imageFormat);
}