using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// 关联所有数据的一个数据库内容
    /// </summary>
    [CreateAssetMenu(fileName = "New Item DataBase", menuName = "Inventory/Item DataBase")]
    public class ItemDataBase : ScriptableObject
    {
        [SerializeReference] public List<ItemData> items;
    }
}