using System.Threading.Tasks;

namespace Volo.Abp.Image.Data;

public interface IImageDbSchemaMigrator
{
    Task MigrateAsync();
}
