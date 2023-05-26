namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public class ImageResizeParameter : IImageResizeParameter
{
    public ImageResizeParameter(int? width = null, int? height = null,ImageResizeMode? mode = null)
    {
        Mode = mode;
        Width = width;
        Height = height;
    }

    public int? Width { get; }
    public int? Height { get; }
    public ImageResizeMode? Mode { get; }
}