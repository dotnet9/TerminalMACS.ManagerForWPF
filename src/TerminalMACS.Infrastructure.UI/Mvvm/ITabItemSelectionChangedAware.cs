namespace TerminalMACS.Infrastructure.UI.Mvvm;

public interface ITabItemSelectionChangedAware
{
    void OnSelected();

    void OnUnselected();
}