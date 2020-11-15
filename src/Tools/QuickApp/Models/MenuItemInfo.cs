using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickApp.Models
{
    public class MenuItemInfo
    {
        /// <summary>
        /// 文件路径或者CMD命令行
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 图标路径
        /// </summary>
        public string IconPath { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型，0:exe,1:web,2:cmd
        /// </summary>
        public MenuItemType Type { get; set; }
    }

    public enum MenuItemType
    {
        Exe, Web, Cmd
    }

    class ConfigInfo
    {
        public bool IsPowerOn { get; set; } = false;
        public List<MenuItemInfo> MenuItemInfos { get; set; }
    }

    public class ConfigHelper
    {

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public static List<MenuItemInfo> GetAllMenuItems()
        {
            return config.MenuItemInfos;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        public static bool AddNewMenuItem(MenuItemInfo menuItem)
        {
            bool exist = config.MenuItemInfos.Exists(cu => cu.Name.Equals(menuItem.Name));
            if (exist)
            {
                return false;
            }

            config.MenuItemInfos.Add(menuItem);
            SaveConfig();
            return true;
        }

        public static bool IsPowerOn()
        {
            return config.IsPowerOn;
        }

        /// <summary>
        /// 设置是否开机启动
        /// </summary>
        /// <param name="isPowerOn"></param>
        public static void SetIsPowerOn(bool isPowerOn)
        {
            config.IsPowerOn = isPowerOn;
            SaveConfig();
        }

        private static string menuFile = "menu.json";
        private static ConfigInfo config;
        static ConfigHelper()
        {
            if (System.IO.File.Exists(menuFile))
            {
                string str = System.IO.File.ReadAllText(menuFile, Encoding.UTF8);
                config = JsonConvert.DeserializeObject<ConfigInfo>(str);
            }
            else
            {
                config = new ConfigInfo();
                config.MenuItemInfos = new List<MenuItemInfo>();
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        private static void SaveConfig()
        {
            var str = JsonConvert.SerializeObject(config);
            System.IO.File.WriteAllText(menuFile, str, Encoding.UTF8);
        }
    }
}
