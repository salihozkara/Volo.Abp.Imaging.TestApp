using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Image.Localization;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Image;

/* Inherit your application services from this class.
 */
public abstract class ImageAppService : ApplicationService
{
    protected ImageAppService()
    {
        LocalizationResource = typeof(ImageResource);
    }
}
