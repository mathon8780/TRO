using ItemInventory;
using UI.NearbyItemCanvas;

namespace UI.EventType
{
    /// <summary>
    /// Nearby Event
    /// 容器列表的内容显示事件
    /// 传递容器面板对应的容器交互接口
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