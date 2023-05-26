using Volo.Abp.Modularity;

namespace Volo.Abp.Image;

[DependsOn(
    typeof(ImageApplicationModule),
    typeof(ImageDomainTestModule)
    )]
public class ImageApplicationTestModule : AbpModule
{

}
