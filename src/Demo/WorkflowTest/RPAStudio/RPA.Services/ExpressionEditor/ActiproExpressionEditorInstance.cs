using ActiproSoftware.Text;
using ActiproSoftware.Windows.Controls.SyntaxEditor;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt;
using RPA.Interfaces.ExpressionEditor;
using System;
using System.Activities.Presentation.Hosting;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace RPA.Services.ExpressionEditor
{
    public class ActiproExpressionEditorInstance : IExpressionEditorInstance
    {
        private SyntaxEditor _editor;
        private VariablesSession _variablesSession;

        private readonly ISyntaxService _syntaxService = new SyntaxService();
        private ExpressionLanguage _expressionLanguage = ExpressionLanguage.VisualBasic;


        public ISyntaxService SyntaxService {
            get {
                return _syntaxService;
            }
        }



        public bool AcceptsReturn { get; set; }

        public bool AcceptsTab
        {
            get
            {
                return this.Editor.AcceptsTab;
            }
            set
            {
                this.Editor.AcceptsTab = value;
            }
        }

        public bool HasAggregateFocus
        {
            get
            {
                return true;
            }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get
            {
                return this.Editor.HorizontalScrollBarVisibility;
            }
            set
            {
            }
        }

        public Control HostControl
        {
            get
            {
                return this.Editor;
            }
        }


        public int MaxLines { get; set; }

        public int MinLines { get; set; }

        public string Text
        {
            get
            {
                return this.Editor.Document.CurrentSnapshot.Text;
            }
            set
            {
                this.Editor.Document.SetText(value);
                this.CloseIntelliPrompt();
            }
        }

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get
            {
                return this.Editor.VerticalScrollBarVisibility;
            }
            set
            {
            }
        }

        public event EventHandler Closing;
        public event EventHandler GotAggregateFocus;
        public event EventHandler LostAggregateFocus;
        public event EventHandler TextChanged;

        public bool CanCompleteWord()
        {
            return true;
        }

        public bool CanCopy()
        {
            return true;
        }



        internal void OpenEditor(EditorConfig config)
        {
            if (this.Editor.Parent != null)
            {
                EventHandler closing = this.Closing;
                if (closing != null)
                {
                    closing(this, EventArgs.Empty);
                }
                this.Editor.Dispatcher.Invoke(delegate ()
                {
                }, DispatcherPriority.ApplicationIdle);
            }
            this._variablesSession = config.VariablesSession;
            this.MaxLines = config.MaxLines;
            this.Editor.IsMultiLine = config.IsMultiLine;
            this.Editor.Margin = config.Margin;

            if (config.HasInitialSize)
            {
                this.Editor.MinHeight = config.InitialHeight;
            }

            this.Update(config.InitialText, null);
            this.InitializeHeaderAndFooter(config.ImportedNamespaces);                
        }

        internal void Update(string text, int? caretPosition = null)
        {
            this.CloseIntelliPrompt();
            if (string.IsNullOrEmpty(text))
            {
                this.Editor.Document.SetText(string.Empty);
            }
            else
            {
                this.Editor.Document.SetText(text);
                if (caretPosition == null)
                {
                    caretPosition = new int?(text.Length);
                }

                Editor.ActiveView.Selection.CaretPosition = new TextPosition(0, caretPosition.Value, hasFarAffinity: true);
            }
            Keyboard.Focus(this.Editor);
        }

        private void InitializeHeaderAndFooter(ImportedNamespaceContextItem importedNamespaces)
        {
            this.InitializeVbHeaderAndFooter(importedNamespaces);
        }

        private void InitializeVbHeaderAndFooter(ImportedNamespaceContextItem importedNamespaces)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string str in importedNamespaces.ImportedNamespaces)
            {
                stringBuilder.AppendLine("Imports " + str);
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Shared Class Expression");
            stringBuilder.AppendLine("Shared Sub ExpressionValue");
            VariablesSession variablesSession = this._variablesSession;
            if (((variablesSession != null) ? variablesSession.Variables : null) != null)
            {
                foreach (KeyValuePair<string, Type> keyValuePair in this._variablesSession.Variables)
                {
                    stringBuilder.Append("Dim ");
                    stringBuilder.Append(keyValuePair.Key);
                    stringBuilder.Append(" As ");
                    StringBuilder stringBuilder2 = new StringBuilder(keyValuePair.Value.FriendlyName(true, false));
                    stringBuilder2 = stringBuilder2.Replace("<", "(of ").Replace(">", ")").Replace("[", "(").Replace("]", ")");
                    stringBuilder.Append(stringBuilder2);
                    stringBuilder.AppendLine();
                }
            }
            stringBuilder.Append("\r\nReturn ");
            this.Editor.Document.SetHeaderAndFooterText(stringBuilder.ToString(), "\r\nEnd Sub\r\nEnd Class");
        }

        public bool CanCut()
        {
            return true;
        }

        public bool CanDecreaseFilterLevel()
        {
            return false;
        }

        public bool CanGlobalIntellisense()
        {
            return true;
        }

        public bool CanIncreaseFilterLevel()
        {
            return false;
        }

        public bool CanParameterInfo()
        {
            return true;
        }

        public bool CanPaste()
        {
            return true;
        }

        public bool CanQuickInfo()
        {
            return true;
        }

        public bool CanRedo()
        {
            return this.Editor.Document.UndoHistory.CanRedo;
        }

        public bool CanUndo()
        {
            return this.Editor.Document.UndoHistory.CanUndo;
        }

        public void ClearSelection()
        {
            this.Editor.ActiveView.Selection.Collapse();
        }

        public void Close()
        {
            this.CloseIntelliPrompt();
        }

        public bool CompleteWord()
        {
            return true;
        }

        public bool Copy()
        {
            this.Editor.ActiveView.CopyToClipboard();
            return true;
        }

        public bool Cut()
        {
            this.Editor.ActiveView.CutToClipboard();
            return true;
        }

        public bool DecreaseFilterLevel()
        {
            return false;
        }

        public void Focus()
        {
            this.Editor.Dispatcher.InvokeAsync(delegate ()
            {
                this.Editor.Focus();
                this.Editor.ActiveView.Focus();
            });
            EventHandler gotAggregateFocus = this.GotAggregateFocus;
            if (gotAggregateFocus == null)
            {
                return;
            }
            gotAggregateFocus(null, EventArgs.Empty);
        }

        public string GetCommittedText()
        {
            return this.Editor.Document.CurrentSnapshot.Text;
        }

        public bool GlobalIntellisense()
        {
            return true;
        }

        public bool IncreaseFilterLevel()
        {
            return false;
        }

        public bool ParameterInfo()
        {
            this.Editor.ActiveView.IntelliPrompt.RequestParameterInfoSession();
            return this.Editor.IntelliPrompt.Sessions[IntelliPromptSessionTypes.ParameterInfo] != null;
        }

        public bool Paste()
        {
            this.Editor.ActiveView.PasteFromClipboard();
            return true;
        }

        public bool QuickInfo()
        {
            this.Editor.ActiveView.IntelliPrompt.RequestQuickInfoSession();
            return this.Editor.IntelliPrompt.Sessions[IntelliPromptSessionTypes.QuickInfo] != null;
        }

        public bool Redo()
        {
            return this.Editor.Document.UndoHistory.Redo();
        }

        public bool Undo()
        {
            return this.Editor.Document.UndoHistory.Undo();
        }




        private void CloseIntelliPrompt()
        {
            this.Editor.IntelliPrompt.CloseAllSessions();
        }


        private void Editor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private SyntaxEditor Editor
        {
            get
            {
                if (this._editor == null)
                {
                    this._editor = new SyntaxEditor();
                    this._editor.ContextMenu = new ContextMenu();//屏蔽语法编辑框的右键菜单
                    this._editor.DataContextChanged += this.Editor_DataContextChanged;
                    this.InitSyntaxEditor();
                }
                return this._editor;
            }
        }


        private void InitSyntaxEditor()
        {
            this.SetEditorStyleProperties();
            this.Editor.PreviewKeyUp += this.Editor_PreviewKeyUp;
            this.Editor.PreviewKeyDown += this.Editor_PreviewKeyDown;
            this.Editor.DocumentTextChanged += this.Editor_DocumentTextChanged;
            this.Editor.IsKeyboardFocusWithinChanged += this.Editor_IsKeyboardFocusWithinChanged;
            this.Editor.Unloaded += this.Editor_Unloaded;
            this.Editor.ActiveView.VisualElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.EditorVisualElement_LostFocus);
            this.Editor.ActiveView.VisualElement.LostFocus += this.EditorVisualElement_LostFocus;
            this.Focus();
            this.InitSyntaxLanguageAsync();
        }

        internal void Unload()
        {
            this.Editor.PreviewKeyUp -= this.Editor_PreviewKeyUp;
            this.Editor.PreviewKeyDown -= this.Editor_PreviewKeyDown;
            this.Editor.DocumentTextChanged -= this.Editor_DocumentTextChanged;
            this.Editor.IsKeyboardFocusWithinChanged -= this.Editor_IsKeyboardFocusWithinChanged;
            this.Editor.Unloaded -= this.Editor_Unloaded;
            this.Editor.ActiveView.VisualElement.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.EditorVisualElement_LostFocus);
            this.Editor.ActiveView.VisualElement.LostFocus -= this.EditorVisualElement_LostFocus;
        }

        private void Editor_DocumentTextChanged(object sender, EditorSnapshotChangedEventArgs e)
        {
            if (e.TextChange.Source == this.Editor.ActiveView)
            {
                if(e.OldSnapshot.Text.Trim()=="")
                {
                    if (!this.Editor.IntelliPrompt.Sessions.Contains(IntelliPromptSessionTypes.Completion))
                    {
                        EditorCommands.RequestIntelliPromptCompletionSession.Execute(this.Editor.ActiveView);
                    }
                }
            }
        }

        private void EditorVisualElement_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }


        private void SetEditorStyleProperties()
        {
            try
            {
                this.Editor.CanSplitHorizontally = false;
                this.Editor.IsOutliningMarginVisible = false;
                this.Editor.IsSelectionMarginVisible = false;
                this.Editor.Padding = new Thickness(0.0);
                this.Editor.AcceptsTab = false;
                this.Editor.BorderThickness = new Thickness(0.0);
                this.Editor.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                this.Editor.VerticalContentAlignment = VerticalAlignment.Stretch;
                this.Editor.HorizontalScrollBarVisibility = (this.Editor.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }


        internal bool IsIntelliPromptVisible
        {
            get
            {
                return this.Editor.IntelliPrompt.Sessions.Count > 0;
            }
        }

        private void OnEditingCanceled()
        {
            this.CloseIntelliPrompt();
        }

        private void Editor_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && !this.IsIntelliPromptVisible)
            {
                this.OnEditingCanceled();
            }
        }

        private void Editor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && e.KeyboardDevice.Modifiers == ModifierKeys.None && !this.IsIntelliPromptVisible)
            {
                this.OnTextCommited();
                Keyboard.Focus(null);
            }
        }

        private void Editor_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Editor.IsKeyboardFocusWithin)
            {
                IEditorView activeView = this.Editor.ActiveView;
                bool? flag;
                if (activeView == null)
                {
                    flag = null;
                }
                else
                {
                    FrameworkElement visualElement = activeView.VisualElement;
                    flag = ((visualElement != null) ? new bool?(visualElement.IsKeyboardFocusWithin) : null);
                }
                if (!(flag ?? false))
                {
                    this.Editor.Dispatcher.InvokeAsync(delegate ()
                    {
                        this.Editor.ActiveView.Focus();
                    });
                    EventHandler gotAggregateFocus = this.GotAggregateFocus;
                    if (gotAggregateFocus == null)
                    {
                        return;
                    }
                    gotAggregateFocus(null, EventArgs.Empty);
                    return;
                }
            }
            if (!this.Editor.ActiveView.HasFocus)
            {
                this.LostAggregateFocus?.Invoke(null, EventArgs.Empty);
                this.OnTextCommited();
            }
        }


        private void OnTextCommited()
        {
            this.CloseIntelliPrompt();
        }

        private void Editor_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Closing?.Invoke(null, e);
            this.Editor.ZoomLevel = 1.0;
        }



        private async void InitSyntaxLanguageAsync()
        {
            ISyntaxLanguage syntaxLanguage = await this._syntaxService.GetLanguageAsync(this._expressionLanguage);
            ISyntaxLanguage syntaxLanguage2 = syntaxLanguage;
            if (syntaxLanguage2 == null)
            {
                Trace.TraceError("ActiproExpressionEditorInstance: ISyntaxLanguage not available");
            }
            else
            {
                this.Editor.Document.Language = syntaxLanguage2;
            }
        }






    }
}
