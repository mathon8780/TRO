using CoreListener;
using Inventory.Property;
using Inventory.WorldInteract;
using UI.EventType;
using UnityEngine;

namespace CoreComponents
{
    /// <summary>
    /// 玩家的检测组件 用于检测周边物品
    /// </summary>
    public class DetectionComponent : MonoBehaviour
    {
        // Nearby 物品交互事件缓存
        private readonly EventNearbyContainer _nearbyContainerEvent = new();
        private readonly EventNearbyItem _nearbyItemEvent = new();

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "WorldItem":
                    HandleWorldItem(other, true);
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            switch (other.tag)
            {
                case "WorldItem":
                    HandleWorldItem(other, false);
                    break;
            }
        }

        private void HandleWorldItem(Collider other, bool isClose)
        {
            // todo:重写交互逻辑
            // 获取世界物品交互组件 非空判断 类型判断 触发对应的显示逻辑
            WorldItem worldItem = other.GetComponent<WorldItem>();

            if (!worldItem) return;

            if (worldItem.item.GetProperty<ItemContainerProperty>() != null)
            {
                _nearbyContainerEvent.IsClose = isClose;
                _nearbyContainerEvent.ContainerItemInfo = worldItem.item;
                EventCenter.Instance.TriggerEvent<EventNearbyContainer>(_nearbyContainerEvent);
                return;
            }
            else
            {
                _nearbyItemEvent.IsClose = isClose;
                _nearbyItemEvent.WorldItemInfo = worldItem.item;
                EventCenter.Instance.TriggerEvent<EventNearbyItem>(_nearbyItemEvent);
                return;
            }
        }
    }
}