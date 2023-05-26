using Volo.Abp.Image.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.Image.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class ImagePageModel : AbpPageModel
{
    protected ImagePageModel()
    {
        LocalizationResourceType = typeof(ImageResource);
    }
}
