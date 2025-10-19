using UnityEngine;

namespace ItemInventory.InteractiveBehaviours
{
    /// <summary>
    /// 物品交互行为上下文
    /// </summary>
    public class ItemInteractiveContext
    {
        public E_ItemBehaviorType BehaviorType; //行为类型
        public GameObject Player; //执行行为的玩家

        public Item Item; //操作物品实例

        public E_ItemInteractiveType InteractiveType; //交互类型

        public Item OriginItemContainer; //源物品容器
        public Item TargetItemContainer; //目标物品容器
    }

    public enum E_ItemBehaviorType
    {
        Pickable, // 可拾取
        Dropable, // 可丢弃
        Eatable, // 可食用
        Usable, // 可使用
        Equipable, // 可装备
        Storable, // 可存储
    }

    public enum E_ItemInteractiveType
    {
        StoreItem, //存储物品
        RemovalItem, //移除物品
        TransferItem, //转移物品
    }
}