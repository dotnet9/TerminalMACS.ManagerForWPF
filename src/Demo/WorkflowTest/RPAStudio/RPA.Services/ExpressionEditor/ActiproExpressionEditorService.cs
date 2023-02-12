using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Windows;

namespace RPA.Services.ExpressionEditor
{
    public class ActiproExpressionEditorService : IExpressionEditorService
    {
        private EditingContext _currentEditingContext;
        private ActiproExpressionEditorInstance _expressionEditor;
        public EditorConfig Config { get; } = new EditorConfig();


        public ActiproExpressionEditorService(EditingContext editingContext)
        {
            this._currentEditingContext = editingContext;
            this._expressionEditor = new ActiproExpressionEditorInstance();
        }

        public void CloseExpressionEditors()
        {
            ActiproExpressionEditorInstance expressionEditor = this._expressionEditor;
            if (expressionEditor == null)
            {
                return;
            }
            expressionEditor.Unload();
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text)
        {
            return this.CreateEditor(text, variables, typeof(void), importedNamespaces, null);
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Size initialSize)
        {
            return this.CreateEditor(text, variables, typeof(void), importedNamespaces, new Size?(initialSize));
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType)
        {
            return this.CreateEditor(text, variables, expressionType, importedNamespaces, null);
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, List<ModelItem> variables, string text, Type expressionType, Size initialSize)
        {
            return this.CreateEditor(text, variables, expressionType, importedNamespaces, new Size?(initialSize));
        }

        private IExpressionEditorInstance CreateEditor(string initialText, List<ModelItem> variables, Type expressionType, ImportedNamespaceContextItem importedNamespaces, Size? initialSize = null)
        {
            this.Config.InitialText = initialText;
            this.Config.VariablesSession = new VariablesSession(variables, expressionType, this._currentEditingContext);
            this.Config.ImportedNamespaces = importedNamespaces;
            this.Config.InitialSize = initialSize;
            this._expressionEditor.OpenEditor(this.Config);
            return this._expressionEditor;
        }

        public void UpdateContext(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces)
        {
            this._expressionEditor.SyntaxService.AddAssemblyReferences((assemblies != null) ? assemblies.AllAssemblyNamesInContext : null);
        }
    }
}
