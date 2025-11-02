using CoreListener;
using UI.EventType;
using UnityEngine;

namespace CoreComponents
{
    /// <summary>
    /// 玩家的检测组件 用于检测周边物品
    /// </summary>
    public class DetectionComponent : MonoBehaviour
    {
        private EventNearbyContainer _nearbyContainerEvent = new();
        private EventNearbyItem _nearbyItemEvent = new();

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
            // WorldItemInteraction worldItemInteraction = other.GetComponent<WorldItemInteraction>();
            // if (!worldItemInteraction) return;
            // if (worldItemInteraction.Item.ItemBaseInfo.itemType == E_ItemType.ItemContainer)
            // {
            //     _nearbyContainerEvent.IsClose = isClose;
            //     _nearbyContainerEvent.ContainerItem = worldItemInteraction.Item;
            //     EventCenter.Instance.TriggerEvent<EventNearbyContainer>(_nearbyContainerEvent);
            //     return;
            // }
            // else
            // {
            //     _nearbyItemEvent.IsClose = isClose;
            //     _nearbyItemEvent.WorldItem = worldItemInteraction.Item;
            //     EventCenter.Instance.TriggerEvent<EventNearbyItem>(_nearbyItemEvent);
            //     return;
            // }
        }


        private void ClearEvent()
        {
            _nearbyContainerEvent.ContainerItem = null;
            _nearbyItemEvent.WorldItem = null;
        }
    }
}