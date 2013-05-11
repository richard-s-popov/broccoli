﻿/****************************************************************
 *  Theme: The Metro Theme for Orchard
 *  Author: Marco Siniscalco
 *  Copyright: 2012, Marco Siniscalco. All Rights Reserved
 *  Project Site: http://orchardmetrotheme.codeplex.com/
 ***************************************************************/

using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace TheMetroTheme
{
    public class Permissions : IPermissionProvider {

        public static readonly Permission ManageThemeSettings = new Permission { Description = "Manage Theme Settings", Name = "ManageThemeSettings" };
        public static readonly Permission EditCustomCss = new Permission { Description = "Edit Custom CSS", Name = "EditCustomCss" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageThemeSettings,
                EditCustomCss,
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] { ManageThemeSettings, EditCustomCss }
                },
            };
        }

    }
}