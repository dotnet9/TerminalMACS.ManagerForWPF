using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RPA.Shared.Utils
{
    public class FileItem : DirOrFileItem
    {
        public string Extension { get; set; }

        public ImageSource AssociatedIcon { get; set; }
    }
}
