using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image.Web;

[Dependency(ReplaceServices = true)]
public class ImageBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Image";
}
