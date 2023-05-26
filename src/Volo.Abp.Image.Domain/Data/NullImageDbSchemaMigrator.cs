using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image.Data;

/* This is used if database provider does't define
 * IImageDbSchemaMigrator implementation.
 */
public class NullImageDbSchemaMigrator : IImageDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
