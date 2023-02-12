using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RPA.Shared.Configs;
using RPA.Shared.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RPAStudio.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SettingsPageViewModel : ViewModelBase
    {
        public enum enUpdateSettings
        {
            PythonPath,
            ProjectsDefaultCreatePath,
        }

        /// <summary>
        /// Initializes a new instance of the SettingsPageViewModel class.
        /// </summary>
        public SettingsPageViewModel()
        {
            Init();
        }

        private void Init()
        {
            PythonCustomLocation = (Environment.GetEnvironmentVariable("RPA_PYTHON_PATH", EnvironmentVariableTarget.User)
                ?? Environment.GetEnvironmentVariable("RPA_PYTHON_PATH", EnvironmentVariableTarget.Machine));

            ProjectsCustomLocation = UserKeyValueConfig.GetValue("Project.DefaultCreatePath");

            UserKeyValueConfig.ValueChangedEvent -= UserKeyValueConfig_ValueChangedEvent;
            UserKeyValueConfig.ValueChangedEvent += UserKeyValueConfig_ValueChangedEvent;
        }

        private void UserKeyValueConfig_ValueChangedEvent(object sender, string key)
        {
            if(key == "Project.DefaultCreatePath")
            {
                Common.InvokeAsyncOnUI(()=> {
                    ProjectsCustomLocation = UserKeyValueConfig.GetValue("Project.DefaultCreatePath");
                });
            }
        }


        /// <summary>
        /// The <see cref="IsPythonCustomLocationCorrect" /> property's name.
        /// </summary>
        public const string IsPythonCustomLocationCorrectPropertyName = "IsPythonCustomLocationCorrect";

        private bool _isPythonCustomLocationCorrectProperty = true;

        /// <summary>
        /// Sets and gets the IsPythonCustomLocationCorrect property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPythonCustomLocationCorrect
        {
            get
            {
                return _isPythonCustomLocationCorrectProperty;
            }

            set
            {
                if (_isPythonCustomLocationCorrectProperty == value)
                {
                    return;
                }

                _isPythonCustomLocationCorrectProperty = value;
                RaisePropertyChanged(IsPythonCustomLocationCorrectPropertyName);
            }
        }




        /// <summary>
        /// The <see cref="PythonCustomLocationValidatedWrongTip" /> property's name.
        /// </summary>
        public const string PythonCustomLocationValidatedWrongTipPropertyName = "PythonCustomLocationValidatedWrongTip";

        private string _pythonCustomLocationValidatedWrongTipProperty = "";

        /// <summary>
        /// Sets and gets the PythonCustomLocationValidatedWrongTip property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PythonCustomLocationValidatedWrongTip
        {
            get
            {
                return _pythonCustomLocationValidatedWrongTipProperty;
            }

            set
            {
                if (_pythonCustomLocationValidatedWrongTipProperty == value)
                {
                    return;
                }

                _pythonCustomLocationValidatedWrongTipProperty = value;
                RaisePropertyChanged(PythonCustomLocationValidatedWrongTipPropertyName);
            }
        }



        /// <summary>
        /// The <see cref="PythonCustomLocation" /> property's name.
        /// </summary>
        public const string PythonCustomLocationPropertyName = "PythonCustomLocation";

        private string _pythonCustomLocationProperty = "";

        /// <summary>
        /// Sets and gets the PythonCustomLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PythonCustomLocation
        {
            get
            {
                return _pythonCustomLocationProperty;
            }

            set
            {
                if (_pythonCustomLocationProperty == value)
                {
                    return;
                }

                _pythonCustomLocationProperty = value;
                RaisePropertyChanged(PythonCustomLocationPropertyName);

                if (!string.IsNullOrEmpty(value))
                {
                    IsPythonCustomLocationCorrect = System.IO.Directory.Exists(value);
                    if (!IsPythonCustomLocationCorrect)
                    {
                        PythonCustomLocationValidatedWrongTip = "指定的路径不存在，请检查";
                        return;
                    }

                    //判断路径里面是否含有python3.dll
                    var python3_dll_file = Path.Combine(value, "python3.dll");
                    if (!File.Exists(python3_dll_file))
                    {
                        IsPythonCustomLocationCorrect = false;
                        PythonCustomLocationValidatedWrongTip = "指定的路径不是合法的Python3目录，请检查";
                        return;
                    }
                }
                else
                {
                    IsPythonCustomLocationCorrect = true;
                }

                UpdateSettings(enUpdateSettings.PythonPath);
            }
        }



        private RelayCommand _selectPythonCustomLocationCommand;

        /// <summary>
        /// Gets the SelectPythonCustomLocationCommand.
        /// </summary>
        public RelayCommand SelectPythonCustomLocationCommand
        {
            get
            {
                return _selectPythonCustomLocationCommand
                    ?? (_selectPythonCustomLocationCommand = new RelayCommand(
                    () =>
                    {
                        string dst_dir = "";
                        if (CommonDialog.ShowSelectDirDialog("请选择python.exe所在的路径", ref dst_dir))
                        {
                            PythonCustomLocation = dst_dir;
                        }
                    }));
            }
        }

        /// <summary>
        /// The <see cref="ProjectsCustomLocation" /> property's name.
        /// </summary>
        public const string ProjectsCustomLocationPropertyName = "ProjectsCustomLocation";

        private string _projectsCustomLocationProperty = "";

        /// <summary>
        /// Sets and gets the ProjectsCustomLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProjectsCustomLocation
        {
            get
            {
                return _projectsCustomLocationProperty;
            }

            set
            {
                if (_projectsCustomLocationProperty == value)
                {
                    return;
                }

                _projectsCustomLocationProperty = value;
                RaisePropertyChanged(ProjectsCustomLocationPropertyName);


                if (!string.IsNullOrEmpty(value))
                {
                    IsProjectsCustomLocationCorrect = System.IO.Directory.Exists(value);
                    if (!IsProjectsCustomLocationCorrect)
                    {
                        ProjectsCustomLocationValidatedWrongTip = "指定的路径不存在，请检查";
                        return;
                    }
                }
                else
                {
                    IsProjectsCustomLocationCorrect = true;
                }

                UpdateSettings(enUpdateSettings.ProjectsDefaultCreatePath);
            }
        }

        /// <summary>
        /// The <see cref="ProjectsCustomLocationValidatedWrongTip" /> property's name.
        /// </summary>
        public const string ProjectsCustomLocationValidatedWrongTipPropertyName = "ProjectsCustomLocationValidatedWrongTip";

        private string _projectsCustomLocationValidatedWrongTipProperty = "";

        /// <summary>
        /// Sets and gets the ProjectsCustomLocationValidatedWrongTip property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ProjectsCustomLocationValidatedWrongTip
        {
            get
            {
                return _projectsCustomLocationValidatedWrongTipProperty;
            }

            set
            {
                if (_projectsCustomLocationValidatedWrongTipProperty == value)
                {
                    return;
                }

                _projectsCustomLocationValidatedWrongTipProperty = value;
                RaisePropertyChanged(ProjectsCustomLocationValidatedWrongTipPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="IsProjectsCustomLocationCorrect" /> property's name.
        /// </summary>
        public const string IsProjectsCustomLocationCorrectPropertyName = "IsProjectsCustomLocationCorrect";

        private bool _isProjectsCustomLocationCorrectProperty = false;

        /// <summary>
        /// Sets and gets the IsProjectsCustomLocationCorrect property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsProjectsCustomLocationCorrect
        {
            get
            {
                return _isProjectsCustomLocationCorrectProperty;
            }

            set
            {
                if (_isProjectsCustomLocationCorrectProperty == value)
                {
                    return;
                }

                _isProjectsCustomLocationCorrectProperty = value;
                RaisePropertyChanged(IsProjectsCustomLocationCorrectPropertyName);
            }
        }


        private RelayCommand _selectProjectsCustomLocationCommand;

        /// <summary>
        /// Gets the SelectProjectsCustomLocationCommand.
        /// </summary>
        public RelayCommand SelectProjectsCustomLocationCommand
        {
            get
            {
                return _selectProjectsCustomLocationCommand
                    ?? (_selectProjectsCustomLocationCommand = new RelayCommand(
                    () =>
                    {
                        string dst_dir = "";
                        if (CommonDialog.ShowSelectDirDialog("请选择新建项目时的默认路径", ref dst_dir))
                        {
                            ProjectsCustomLocation = dst_dir;
                        }
                    }));
            }
        }


        private void UpdateSettings(enUpdateSettings operate)
        {
            Task.Run(() =>
            {
                try
                {
                    switch (operate)
                    {
                        case enUpdateSettings.PythonPath:
                            {
                                if (string.IsNullOrEmpty(PythonCustomLocation))
                                {
                                    Environment.SetEnvironmentVariable("RPA_PYTHON_PATH", null, EnvironmentVariableTarget.User);
                                    Environment.SetEnvironmentVariable("RPA_PYTHON_PATH", null, EnvironmentVariableTarget.Machine);
                                }
                                else
                                {
                                    Environment.SetEnvironmentVariable("RPA_PYTHON_PATH", PythonCustomLocation, EnvironmentVariableTarget.User);
                                }
                            }
                            break;
                        case enUpdateSettings.ProjectsDefaultCreatePath:
                            {
                                UserKeyValueConfig.SetKeyValue("Project.DefaultCreatePath", ProjectsCustomLocation);
                            }
                            break;
                    }
                }
                catch (Exception err)
                {
                    Common.InvokeOnUI(() => {
                        CommonMessageBox.ShowError(err.ToString());
                    });

                }

            });
        }





    }
}