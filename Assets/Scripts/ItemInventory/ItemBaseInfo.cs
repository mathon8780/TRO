using ItemInventory.InteractiveBehaviours;
using UnityEngine;

namespace ItemInventory
{
    /// <summary>
    /// 物品基础信息
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Item Info", order = 0)]
    public class ItemBaseInfo : ScriptableObject
    {
        [Header("基本信息")] public int itemId; // 唯一ID
        public string itemName; // 物品名称
        public E_ItemType itemType;
        public string description; // 物品描述
        public float weight; // 物品重量
        public bool canStack; // 是否可堆叠


        [Header("资源信息")] public Sprite icon; // 物品图标

        [Header("资源加载信息")] public string iconLoadKey; // 图标加载关键字
        public string resourceLoadKey; // 资源加载关键字

        [Header("物品行为组件")] public E_ItemBehaviorType[] behaviorTypes; // 物品行为组件枚举数组
    }
}