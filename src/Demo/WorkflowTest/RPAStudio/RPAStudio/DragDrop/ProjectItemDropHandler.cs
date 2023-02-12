using RPAStudio.ViewModel;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ReflectionMagic;
using RPA.Interfaces.Service;
using NLog;
using RPA.Shared.Utils;
using RPA.Shared.Extensions;

namespace RPAStudio.DragDrop
{
    public class ProjectItemDropHandler : DefaultDropHandler
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        private IServiceLocator _serviceLocator;

        private DocksViewModel _docksViewModel;
        private ProjectViewModel _projectViewModel;

        public ProjectItemDropHandler(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            Common.InvokeAsyncOnUI(()=> {
                _docksViewModel = _serviceLocator.ResolveType<DocksViewModel>();
                _projectViewModel = _serviceLocator.ResolveType<ProjectViewModel>();
            });
        }

        public override void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.DragInfo.Data;
            var targetItem = dropInfo.TargetItem;

            dynamic drop = dropInfo.AsDynamic();
            drop.Data = sourceItem;

            bool bCanDrag = true;

            if (sourceItem is ProjectDirItemViewModel || sourceItem is ProjectFileItemViewModel)
            {
                //目录路径必须是个目录或者项目根节点才允许拖动过去
                if(targetItem is ProjectDirItemViewModel)
                {
                    var target = targetItem as ProjectDirItemViewModel;

                    do
                    {
                        if (!(System.IO.Directory.Exists(target.Path) && !target.IsScreenshots))
                        {
                            bCanDrag = false;
                            break;
                        }

                        if (sourceItem is ProjectDirItemViewModel)
                        {
                            var sourceDir = sourceItem as ProjectDirItemViewModel;
                            if (!(System.IO.Directory.Exists(sourceDir.Path) && !sourceDir.IsScreenshots))
                            {
                                bCanDrag = false;
                                break;
                            }

                            //源路径已经在目标路径下没必要拖拽
                            if ((target.Path + @"\" + sourceDir.Name).ToLower() == sourceDir.Path.ToLower())
                            {
                                bCanDrag = false;
                                break;
                            }
                        }

                        if (sourceItem is ProjectFileItemViewModel)
                        {
                            var sourceFile = sourceItem as ProjectFileItemViewModel;
                            if(sourceFile.IsProjectJsonFile)
                            {
                                bCanDrag = false;
                                break;
                            }

                            //源路径已经在目标路径下没必要拖拽
                            if ((target.Path + @"\" + sourceFile.Name).ToLower() == sourceFile.Path.ToLower())
                            {
                                bCanDrag = false;
                                break;
                            }

                        }


                    } while (false);
                }
                else if (targetItem is ProjectRootItemViewModel)
                {
                    var target = targetItem as ProjectRootItemViewModel;

                    do
                    {
                        if (sourceItem is ProjectDirItemViewModel)
                        {
                            var sourceDir = sourceItem as ProjectDirItemViewModel;
                            if (!(System.IO.Directory.Exists(sourceDir.Path) && !sourceDir.IsScreenshots))
                            {
                                bCanDrag = false;
                                break;
                            }

                            //源路径已经在目标路径下没必要拖拽
                            if ((target.Path + @"\" + sourceDir.Name).ToLower() == sourceDir.Path.ToLower())
                            {
                                bCanDrag = false;
                                break;
                            }
                        }

                        if (sourceItem is ProjectFileItemViewModel)
                        {
                            var sourceFile = sourceItem as ProjectFileItemViewModel;
                            if (sourceFile.IsProjectJsonFile)
                            {
                                bCanDrag = false;
                                break;
                            }

                            //源路径已经在目标路径下没必要拖拽
                            if ((target.Path + @"\" + sourceFile.Name).ToLower() == sourceFile.Path.ToLower())
                            {
                                bCanDrag = false;
                                break;
                            }
                        }

                    } while (false);

                }
                else
                {
                    bCanDrag = false;
                }

                if (bCanDrag)
                {
                    base.DragOver(dropInfo);

                    //禁用插入点显示
                    if (dropInfo.DropTargetAdorner == DropTargetAdorners.Insert)
                    {
                        dropInfo.Effects = DragDropEffects.None;
                        dropInfo.DropTargetAdorner = null;
                    }
                }

            }
            else
            {
                dropInfo.Effects = DragDropEffects.None;
            }
        }

        public override void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
            {
                var sourceItem = dropInfo.Data as ProjectBaseItemViewModel;
                var targetItem = dropInfo.TargetItem as ProjectBaseItemViewModel;

                _logger.Debug(string.Format("sourceItem.Path={0}", sourceItem?.Path));
                _logger.Debug(string.Format("targetItem.Path={0}", targetItem?.Path));

                if (sourceItem != null && targetItem != null)
                {
                    if (System.IO.File.Exists(sourceItem.Path))
                    {
                        if (MoveFileToDir(sourceItem as ProjectFileItemViewModel, targetItem))
                        {
                            base.Drop(dropInfo);
                            _projectViewModel.Refresh();
                        }

                        
                    }
                    else
                    {
                        //原路径是目录，则除了目录，目录下的所有子目录及文件也要复制过去
                        if (MoveDirToDir(sourceItem, targetItem))
                        {
                            base.Drop(dropInfo);
                            _projectViewModel.Refresh();
                        }

                       
                    }

                }


            }
        }




        private bool MoveDirToDir(ProjectBaseItemViewModel sourceItem, ProjectBaseItemViewModel targetItem)
        {
            var srcPath = sourceItem.Path;
            var dstPath = targetItem.Path;

            var dstPathCombine = System.IO.Path.Combine(dstPath, sourceItem.Name);

            if (System.IO.Directory.Exists(dstPathCombine))
            {
                CommonMessageBox.ShowWarning("目标目录有重名目录，无法移动目录");
            }
            else
            {
                System.IO.Directory.Move(srcPath, dstPathCombine);


                //遍历目录所有文件
                foreach (var file in System.IO.Directory.GetFiles(dstPathCombine, "*.*"))
                {
                    var relativeFile = Common.MakeRelativePath(dstPathCombine, file);
                    var srcFile = System.IO.Path.Combine(srcPath, relativeFile);

                    foreach (var doc in _docksViewModel.Documents)
                    {
                        if (doc.Path.EqualsIgnoreCase(srcFile))
                        {
                            doc.Path = file;
                            doc.ToolTip = doc.Path;
                            doc.UpdatePathCrossDomain(doc.Path);
                            break;
                        }
                    }
                }

                return true;
            }



            return false;
        }

        private bool MoveFileToDir(ProjectFileItemViewModel sourceItem, ProjectBaseItemViewModel targetItem)
        {
            var srcFile = sourceItem.Path;
            var dstPath = targetItem.Path;

            //拷贝源文件到目录路径中去，若源文件所对应的旧有路径已经在设计器中打开，则需要更新设计器中对应的路径
            var dstFile = System.IO.Path.Combine(dstPath, sourceItem.Name);
            if (System.IO.File.Exists(dstFile))
            {
                CommonMessageBox.ShowWarning("目标目录有重名文件，无法移动文件");
            }
            else
            {
                System.IO.File.Move(srcFile, dstFile);
                sourceItem.Path = dstFile;//更新VM
                foreach (var doc in _docksViewModel.Documents)
                {
                    if (doc.Path.EqualsIgnoreCase(srcFile))
                    {
                        doc.Path = dstFile;
                        doc.ToolTip = doc.Path;
                        doc.UpdatePathCrossDomain(doc.Path);
                        break;
                    }
                }

                if (sourceItem.IsMainXamlFile)
                {
                    //如果是主文件，则移动过去后还是主文件
                    sourceItem.SetAsMainCommand.Execute(null);
                }

                return true;
            }

            return false;
        }








    }
}
