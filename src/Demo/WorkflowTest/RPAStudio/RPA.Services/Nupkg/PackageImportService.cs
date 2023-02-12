using NLog;
using NuGet;
using RPA.Interfaces.Nupkg;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Nupkg
{
    public class PackageImportService : IPackageImportService
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private ZipPackage _zipPackage;

        public PackageImportService()
        {
        }

        public bool Init(string nupkgFile)
        {
            try
            {
                _zipPackage = new ZipPackage(nupkgFile);
            }
            catch (Exception err)
            {
                _logger.Error(err);
                return false;
            }

            return true;
        }

        public string GetDescription()
        {
            return _zipPackage.Description;
        }

        public string GetId()
        {
            return _zipPackage.Id;
        }

        public string GetVersion()
        {
            return _zipPackage.Version.ToString();
        }


        public bool ExtractToDirectory(string path)
        {
            try
            {
                var files = _zipPackage.GetFiles();
                foreach (var file in files)
                {
                    var effectivePath = file.EffectivePath;
                    var stream = file.GetStream();
                    var outputFilePath = Path.Combine(path, effectivePath);

                    var outputDir = Path.GetDirectoryName(outputFilePath);
                    if (!System.IO.Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create))
                    {
                        stream.CopyTo(outputFileStream);
                    }
                }

            }
            catch (Exception e)
            {
                //创建项目失败
                _logger.Error(e);

                CommonMessageBox.ShowError("导入Nupkg包的过程中发生异常，请检查！");
                return false;
            }

            return true;
        }


    }
}
