namespace Volo.Abp.Image.Abstractions.Volo.Abp.Image;

public interface IImageResizeParameter
{
    int? Width { get; }
    int? Height { get; }
    
    ImageResizeMode? Mode { get; }
}

public enum ImageResizeMode
{
    Stretch,
    BoxPad,
    Min,
    Max,
    Crop,
    Pad,
    Fill,
    Distort
}