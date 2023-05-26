using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace Volo.Abp.Image;

public class ImageWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<ImageWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
