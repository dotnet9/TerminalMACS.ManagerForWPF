using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Project
{
    public interface IRecentProjectsConfigService
    {
        /// <summary>
        /// 最近项目列表改变时触发的事件
        /// </summary>
        event EventHandler ChangeEvent;

        List<object> Load();

        void Add(string projectConfigFilePath);

        void Remove(string projectConfigFilePath);

        void Update(string projectConfigFilePath, string name, string description);
    }
}
