using ItemInventory;

namespace UI.EventType
{
    /// <summary>
    /// Nearby Event
    /// 容器交互事件
    /// </summary>
    public class EventNearbyContainer
    {
        public E_NearbyContainerInteractType InteractTypeType;
        public Item ContainerItem;
    }

    /// <summary>
    /// 容器交互类型
    /// </summary>
    public enum E_NearbyContainerInteractType
    {
        CloseToContainer, // 靠近容器
        AwayFromContainer, // 远离容器
    }
}