using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Utils
{
    public class DirOrFileItem
    {
        public string Path { get; set; }
        public string Name { get; set; }

        public string ParentPath { get; set; }

        public FileSystemInfo FileSystemInfo { get; set; }
    }
}
