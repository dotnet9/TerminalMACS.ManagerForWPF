using DataGridDemo.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DataGridDemo.ViewModels;

public class StudentViewModel : BindableBase
{
    public ObservableCollection<Student> Students { get; } = new();

    private ICommand? _deleteCommand;

    public ICommand DeleteCommand
    {
        get { return _deleteCommand ??= new DelegateCommand<Student>(DelExecute, CanDelExecute); }
    }

    public StudentViewModel()
    {
        Students.Add(new Student("刘备", 53));
        Students.Add(new Student("关羽", 43));
        Students.Add(new Student("张飞", 42));
    }

    private bool CanDelExecute(Student student)
    {
        return true;
    }

    private void DelExecute(Student student)
    {
        Students?.Remove(student);
    }
}