using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace ItemInventory.InteractiveBehaviours
{
    /// <summary>
    /// 存储物品行为
    /// </summary>
    [CreateAssetMenu(fileName = "Storable", menuName = "Item/Behavior Storable", order = 0)]
    public class InteractStorable : ItemBehaviorComponent
    {
        public override E_ItemBehaviorType GetBehaviorType => E_ItemBehaviorType.Storable;

        public override void ExecuteBehavior(ItemInteractiveContext context)
        {
            #region 合法性检查

            // 参数检查
            // context 检查
            // 操作行为 检查
            // 操作对象 检查
            // 操作对象基础信息 检查
            // 操作对象实例信息 检查
            // 操作对象类型 检查
            // 操作对象容器 检查

            if (context == null)
            {
                Debug.LogError("ItemInteractiveContext is null.");
                return;
            }

            if (context.BehaviorType != E_ItemBehaviorType.Storable)
            {
                Debug.LogError("BehaviorType mismatch. Expected Storable.");
                return;
            }

            if (context.Item == null)
            {
                Debug.LogError("Item in context is null.");
                return;
            }

            if (context.Item.BaseInfo == null)
            {
                //todo:通过ID获取BaseInfo
                //如果BaseInfo依旧为空 则报错
                if (context.Item.BaseInfo == null)
                {
                    Debug.LogError("BaseInfo in Item is null after search BaseInfo.");
                }

                return;
            }

            if (context.Item.ItemInstance == null)
            {
                Debug.LogError("ItemInstance in Item is null.");
                return;
            }

            if (context.Item.BaseInfo.itemType == E_ItemType.ItemContainer)
            {
                Debug.LogError("Cannot store an ItemContainer inside another ItemContainer.");
                return;
            }


            if (context.OriginItemContainer == null || context.TargetItemContainer == null)
            {
                Debug.LogError("OriginItemContainer or TargetItemContainer is null.");
            }

            #endregion

            switch (context.InteractiveType)
            {
                case E_ItemInteractiveType.StoreItem:
                    StoreItem(context);
                    break;
                case E_ItemInteractiveType.RemovalItem:
                    // 取出的本质就是把存储的两个容器交换
                    (context.OriginItemContainer, context.TargetItemContainer) = (context.TargetItemContainer, context.OriginItemContainer);
                    RemoveItem(context);
                    break;
                default:
                    throw new System.NotImplementedException();
            }


            throw new System.NotImplementedException();
        }


        private void StoreItem(ItemInteractiveContext context)
        {
            Item operationItem = context.Item;
            ItemInstance originItemContainer = operationItem.ItemInstance;
            ItemInstance targetContainerInstance = context.TargetItemContainer.ItemInstance;
            if (targetContainerInstance.IsDirty)
            {
                float occupiedCapacity = 0;
                foreach (var item in targetContainerInstance.ContainedItems)
                {
                    occupiedCapacity += item.BaseInfo.weight * item.ItemInstance.StackNum;
                }

                targetContainerInstance.OccupiedCapacity = occupiedCapacity;
            }

            // 判断容器是否已满
            if (!targetContainerInstance.CanStore)
            {
                return;
            }

            // 判断物品是否能放入容器
            if (targetContainerInstance.OccupiedCapacity
                + operationItem.BaseInfo.weight > targetContainerInstance.MaxCapacity)
            {
                return;
            }

            // 放入容器
            targetContainerInstance.OccupiedCapacity += operationItem.BaseInfo.weight;
            // 深拷贝
            string json = JsonConvert.SerializeObject(operationItem);
            Item newItem = JsonConvert.DeserializeObject<Item>(json);
            // 关联基础数据 更新数量
            newItem.BaseInfo = operationItem.BaseInfo;
            operationItem.ItemInstance.StackNum -= 1;
            // 更新两个容器内容
            if (operationItem.ItemInstance.StackNum <= 0)
            {
                originItemContainer.ContainedItems.Remove(operationItem);
                originItemContainer.IsDirty = true;
            }


            // 添加物品
            if (!newItem.BaseInfo.canStack)
            {
                // 不能堆叠 直接添加
                targetContainerInstance.ContainedItems.Add(newItem);
                // 缓存ID值
                targetContainerInstance.ContainedItemIds.Add(newItem.ID);
            }
            else
            {
                if (targetContainerInstance.ContainedItemIds.Contains(newItem.ID))
                {
                    foreach (var item in targetContainerInstance.ContainedItems.Where(item => item.ID == newItem.ID))
                    {
                        item.ItemInstance.StackNum += 1;
                        break;
                    }
                }
                else
                {
                    targetContainerInstance.ContainedItemIds.Add(newItem.ID);
                    targetContainerInstance.ContainedItems.Add(newItem);
                }
            }

            targetContainerInstance.IsDirty = true;
        }

        //todo: 移除物品 本质就是把存储物品的两个容器互换即可
        private void RemoveItem(ItemInteractiveContext context)
        {
        }
    }
}