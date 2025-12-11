using Inventory;
using UI.NearbyItemCanvas;

namespace UI.EventType
{
    /// <summary>
    /// Nearby-容器交互事件
    /// </summary>
    public class EventNearbyContainer
    {
        public bool IsClose;
        public Item ContainerItemInfo;
    }

    /// <summary>
    /// Nearby-物品交互事件
    /// </summary>
    public class EventNearbyItem
    {
        public bool IsClose;
        public Item WorldItemInfo;
    }

    /// <summary>
    /// Event-处理物品与容器的交互问题
    /// </summary>
    public class EventNearbyItemEnterAndExit
    {
        public bool IsClose;
        public Item ItemInfo;
        public INearbyContainer CurrentContainer;
    }

    /// <summary>
    /// Nearby Event
    /// 容器列表的内容显示事件
    /// 传递容器面板对应的容器交互接口
    /// </summary>
    public class EventNearbyDisplayContainerItems
    {
        public INearbyContainer ContainerItem;

        public EventNearbyDisplayContainerItems(INearbyContainer containerItem)
        {
            ContainerItem = containerItem;
        }
    }

    /// <summary>
    /// Event-显示物品详细信息
    /// </summary>
    public struct EventNearbyDisplayDitalItemInfo
    {
        public bool Display; // 是否显示详细信息
        public Item Item; // 物品
    }
}