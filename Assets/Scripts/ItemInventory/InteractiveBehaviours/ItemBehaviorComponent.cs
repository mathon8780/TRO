using UnityEngine;

namespace ItemInventory.InteractiveBehaviours
{
    /// <summary>
    /// 物品的行为组件
    /// 在子类中定义执行的行为内容
    /// </summary>
    public abstract class ItemBehaviorComponent : ScriptableObject
    {
        /// <summary>
        /// 获取行为名称
        /// </summary>
        public abstract E_ItemBehaviorType GetBehaviorType { get; }

        /// <summary>
        /// 执行对应行为
        /// </summary>
        public abstract void ExecuteBehavior(ItemInteractiveContext context); //传入参数为 玩家 和 综合物品数据
    }
}