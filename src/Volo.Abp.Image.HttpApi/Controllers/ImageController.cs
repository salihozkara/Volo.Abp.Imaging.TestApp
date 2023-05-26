using Volo.Abp.Image.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Image.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ImageController : AbpControllerBase
{
    protected ImageController()
    {
        LocalizationResource = typeof(ImageResource);
    }
}
