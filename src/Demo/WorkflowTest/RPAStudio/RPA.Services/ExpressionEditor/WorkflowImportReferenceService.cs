using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic.Activities;
using RPA.Interfaces.ExpressionEditor;
using System.Windows.Shapes;

namespace RPA.Services.ExpressionEditor
{
    public class WorkflowImportReferenceService : IWorkflowImportReferenceService
    {
        private static VisualBasicSettings _defaultImports;

        public VisualBasicSettings DefaultSettings
        {
            get
            {
                return this.BuildSettings();
            }
        }


        public WorkflowImportReferenceService()
        {
            _defaultImports = GetGeneralImports();
        }

        private static VisualBasicImportReference FromType(Type t)
        {
            return new VisualBasicImportReference
            {
                Assembly = t.Assembly.GetName().FullName,
                Import = t.Namespace
            };
        }

        private static VisualBasicSettings GetGeneralImports()
        {
            VisualBasicSettings visualBasicSettings = new VisualBasicSettings();
            Type[] array = new Type[]
            {
                typeof(string),
                typeof(DataTable),
                typeof(DataTableExtensions),
                typeof(Process),
                typeof(Rectangle),
                typeof(Enumerable),
                typeof(XmlDocument),
                typeof(XNode),
                typeof(MailMessage),
                typeof(Directory),
                typeof(Dictionary<, >),
                typeof(ArrayList)
            };
            foreach (Type t in array)
            {
                visualBasicSettings.ImportReferences.Add(FromType(t));
            }
            foreach (VisualBasicImportReference item in VisualBasicSettings.Default.ImportReferences)
            {
                visualBasicSettings.ImportReferences.Add(item);
            }
            return visualBasicSettings;
        }


        private VisualBasicSettings BuildSettings()
        {
            VisualBasicSettings defaultImports = _defaultImports;
            defaultImports.ImportReferences.Add(FromType(typeof(Microsoft.VisualBasic.VBCodeProvider)));

            return defaultImports;
        }




    }
}
