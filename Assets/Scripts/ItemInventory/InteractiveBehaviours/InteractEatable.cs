using UnityEngine;

namespace ItemInventory.InteractiveBehaviours
{
    /// <summary>
    /// 物品行为
    /// 吃
    /// 所属物品类型-食物
    /// </summary>
    [CreateAssetMenu(fileName = "Eatable", menuName = "Item/Behavior Eatable", order = 0)]
    public class InteractEatable : ItemBehaviorComponent
    {
        public override E_ItemBehaviorType GetBehaviorType => E_ItemBehaviorType.Eatable;

        public override void ExecuteBehavior(ItemInteractiveContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}