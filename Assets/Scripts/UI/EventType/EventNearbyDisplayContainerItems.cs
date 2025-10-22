using ItemInventory;
using UI.NearbyItemCanvas;

namespace UI.EventType
{
    /// <summary>
    /// Nearby Event
    /// 容器列表的内容显示事件
    /// </summary>
    public class EventNearbyDisplayContainerItems
    {
        public INearbyContainerInteract ContainerItemInteract;

        public EventNearbyDisplayContainerItems(INearbyContainerInteract containerItemInteract)
        {
            ContainerItemInteract = containerItemInteract;
        }
    }
}