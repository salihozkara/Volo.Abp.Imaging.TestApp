using Volo.Abp.Image.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Volo.Abp.Image.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ImageEntityFrameworkCoreModule),
    typeof(ImageApplicationContractsModule)
    )]
public class ImageDbMigratorModule : AbpModule
{

}
