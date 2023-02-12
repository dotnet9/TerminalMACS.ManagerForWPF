using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Utils
{
    public static class CommonNuget
    {
        public static bool IsValidPackageId(string packageId)
        {
            if (!NuGet.PackageIdValidator.IsValidPackageId(packageId))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool VersionRangeIsMinInclusive(string verRange,out string minVersion)
        {
            var ver_range = VersionRange.Parse(verRange);
            if (ver_range.IsMinInclusive)
            {
                minVersion = ver_range.MinVersion.ToString();
                return true;
            }

            minVersion = null;

            return false;
        }
    }
}
