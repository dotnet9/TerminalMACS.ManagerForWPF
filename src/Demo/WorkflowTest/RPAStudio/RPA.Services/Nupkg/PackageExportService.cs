using NuGet;
using RPA.Interfaces.Nupkg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Nupkg
{
    public class PackageExportService : IPackageExportService
    {
        private PackageBuilder _packageBuilder;

        public void Init(string id, string version, string description, string authors, string owners, string releaseNotes)
        {
            _packageBuilder = new PackageBuilder();

            var metadata = new ManifestMetadata()
            {
                Authors = authors,
                Owners = authors,
                Version = version,
                Id = id,
                Description = description,
                ReleaseNotes = releaseNotes,
            };

            _packageBuilder.Populate(metadata);
        }

        public void WithDependencies(IEnumerable<NugetPackageItem> dependencies)
        {
            _packageBuilder.DependencySets.Add(new PackageDependencySet(new FrameworkName(".NETFramework, Version=v4.6.1"), from dep in dependencies select new NuGet.PackageDependency(dep.Id, VersionUtility.ParseVersionSpec(dep.Version))));
        }

        public void WithFiles(string projectPath, string exclude)
        {
            _packageBuilder.PopulateFiles(projectPath, new[] { new ManifestFile() { Source = @"**", Exclude = exclude, Target = @"lib/net45" } });
        }

        public string ExportToDir(string nupkgLocation)
        {
            if (!NuGet.PackageIdValidator.IsValidPackageId(_packageBuilder.Id))
            {
                throw new Exception("项目名称不能有特殊字符，不能用空格、顿号、逗号等符号，或以特殊符号结尾等形式");
            }

            var outputPath = System.IO.Path.Combine(nupkgLocation, _packageBuilder.Id) + "." + _packageBuilder.Version + ".nupkg";
            using (FileStream fileStream = File.Create(outputPath))
            {
                _packageBuilder.Save(fileStream);
            }

            return outputPath;
        }


    }
}
