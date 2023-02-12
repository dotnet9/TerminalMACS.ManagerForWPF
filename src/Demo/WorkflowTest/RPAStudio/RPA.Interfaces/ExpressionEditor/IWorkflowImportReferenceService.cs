using Microsoft.VisualBasic.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.ExpressionEditor
{
    public interface IWorkflowImportReferenceService
    {
        VisualBasicSettings DefaultSettings { get; }
    }
}
