using Volo.Abp.Image.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Image;

[DependsOn(
    typeof(ImageEntityFrameworkCoreTestModule)
    )]
public class ImageDomainTestModule : AbpModule
{

}
