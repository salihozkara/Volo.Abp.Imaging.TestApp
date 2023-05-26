using Volo.Abp.Settings;

namespace Volo.Abp.Image.Settings;

public class ImageSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ImageSettings.MySetting1));
    }
}
