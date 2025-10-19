using System.Collections.Generic;
using BaseTools;
using ItemInventory;
using ItemInventory.InteractiveBehaviours;


namespace CoreListener
{
    /// <summary>
    /// 物品的行为交互调用
    /// </summary>
    public class ItemBehaviorCenter : SingletonMono<ItemBehaviorCenter>
    {
        private Dictionary<E_ItemBehaviorType, ItemBehaviorComponent> _behaviors = new();

        protected override void Awake()
        {
            base.Awake();
            //加载所有的物品行为组件
        }


        public ItemBehaviorComponent GetBehaviorComponent(E_ItemBehaviorType behaviorType)
        {
            return _behaviors.GetValueOrDefault(behaviorType);
        }

        public bool ExistBehavior(ItemInteractiveContext context)
        {
            var behavior = GetBehaviorComponent(context.BehaviorType);
            if (behavior != null)
            {
                behavior.ExecuteBehavior(context);
                return true;
            }

            return false;
        }
    }
}