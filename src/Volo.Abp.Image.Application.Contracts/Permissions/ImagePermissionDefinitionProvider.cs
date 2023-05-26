﻿using Volo.Abp.Image.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Volo.Abp.Image.Permissions;

public class ImagePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ImagePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ImagePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ImageResource>(name);
    }
}
