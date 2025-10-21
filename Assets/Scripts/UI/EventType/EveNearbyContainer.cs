using ItemInventory;

namespace UI.EventType
{
    /// <summary>
    /// Nearby Event
    /// 容器交互事件
    /// </summary>
    public class EveNearbyContainer
    {
        public E_NearbyContainerInteract InteractType;
        public Item ContainerItem;
    }

    /// <summary>
    /// 容器交互类型
    /// </summary>
    public enum E_NearbyContainerInteract
    {
        CloseToContainer, // 靠近容器
        AwayFromContainer, // 远离容器
        OpenContainer, // 打开容器
        CloseContainer // 关闭容器
    }
}