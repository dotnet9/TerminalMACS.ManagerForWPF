using System.Windows;

namespace Activities.Shared.Editors
{
    public partial class EditorTemplates
    {
        static ResourceDictionary resources;

        internal static ResourceDictionary ResourceDictionary
        {
            get
            {
                if (resources == null)
                {
                    resources = new EditorTemplates();
                }

                return resources;
            }
        }

        public EditorTemplates()
        {
            InitializeComponent();
        }
    }
}
