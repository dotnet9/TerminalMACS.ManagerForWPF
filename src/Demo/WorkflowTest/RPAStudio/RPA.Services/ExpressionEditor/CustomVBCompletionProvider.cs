using ActiproSoftware.Text.Languages.VB.Implementation;
using ActiproSoftware.Text.Utility;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.ExpressionEditor
{
    internal class CustomVBCompletionProvider : VBCompletionProvider, ICompletionProvider, IKeyedObject, IOrderable
    {
        protected override bool OnSessionOpening(ICompletionSession session)
        {
            CompletionSession completionSession = session as CompletionSession;
            if (completionSession == null)
            {
                return true;
            }

            completionSession.MatchOptions = CompletionMatchOptions.IsCaseInsensitive;
            completionSession.CanFilterUnmatchedItems = true;
            return true;
        }

    }
}
