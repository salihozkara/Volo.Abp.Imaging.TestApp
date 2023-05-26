using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Image.Pages;

public class Index_Tests : ImageWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
