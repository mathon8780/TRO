using ItemInventory.InteractiveBehaviours;
using Newtonsoft.Json;

namespace ItemInventory
{
    /// <summary>
    /// 存储一个物品的基本单位
    /// </summary>
    public class Item
    {
        public int ID; //物品唯一ID
        [JsonIgnore] public ItemBaseInfo BaseInfo; //物品基础信息
        public ItemInstance ItemInstance; //物品实例属性

        public Item(ItemBaseInfo baseInfo, ItemInstance itemInstance)
        {
            BaseInfo = baseInfo;
            ItemInstance = itemInstance;
            ID = baseInfo.itemId;
        }

        public Item(int id, ItemInstance itemInstance)
        {
            ID = id;
            ItemInstance = itemInstance;
            //todo:通过ID获取BaseInfo
        }
    }
}