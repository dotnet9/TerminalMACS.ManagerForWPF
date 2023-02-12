using System;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Windows;

namespace RPA.Services.ExpressionEditor
{
    public class EditorConfig
    {
        public string InitialText { get; set; }

        public bool IsMultiLine { get; set; } = true;

        public int MaxLines { get; set; } = 1;

        public Thickness Margin { get; set; } = new Thickness(0.0);

        public bool HasTransparentBackground { get; set; }

        public VariablesSession VariablesSession { get; set; }

        public ImportedNamespaceContextItem ImportedNamespaces { get; set; }

        public Size? InitialSize { get; set; }

        public bool HasInitialSize
        {
            get
            {
                return this.InitialSize != null;
            }
        }

        public double InitialHeight
        {
            get
            {
                if (!this.HasInitialSize)
                {
                    return 0.0;
                }
                return Math.Max(Math.Ceiling(this.InitialSize.Value.Height), 20.0);
            }
        }

        public bool? IsPopup { get; set; }

        public string Watermark { get; set; }

        private const int MinHeight = 20;
    }
}