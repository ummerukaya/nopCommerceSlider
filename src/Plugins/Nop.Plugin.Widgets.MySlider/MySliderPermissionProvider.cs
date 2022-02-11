using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Security;
using Nop.Services.Security;

namespace Nop.Plugin.Widgets.MySlider
{
    public class MySliderPermissionProvider : IPermissionProvider
    {
        public HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PermissionRecord> GetPermissions()
        {
            throw new NotImplementedException();
        }
    }
}
