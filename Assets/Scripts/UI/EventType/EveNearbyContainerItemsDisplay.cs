using ItemInventory;

namespace UI.EventType
{
    /// <summary>
    /// Nearby Event
    /// 容器列表的内容显示事件
    /// </summary>
    public class EveNearbyContainerItemsDisplay
    {
        public Item ContainerItem;

        public EveNearbyContainerItemsDisplay(Item containerItem)
        {
            ContainerItem = containerItem;
        }
    }
}