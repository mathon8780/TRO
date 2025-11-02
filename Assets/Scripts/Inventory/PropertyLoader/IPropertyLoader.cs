using System.Collections.Generic;
using System.Security;
using Inventory.PropertyData;

namespace Inventory.PropertyLoader
{
    public interface IPropertyLoader
    {
        /// <summary>
        /// 对应的加载文件名
        /// </summary>
        string PropertyFileName { get; }

        /// <summary>
        /// 将Csv文件转换为对应的属性数据并返回加载的内存数据
        /// </summary>
        /// <param name="propertyFolderPath"></param>
        /// <returns></returns>
        Dictionary<int, ItemPropertyData> LoadProperties(string propertyFolderPath);
    }
}