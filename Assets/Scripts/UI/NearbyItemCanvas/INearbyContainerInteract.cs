using ItemInventory;
using UnityEngine.EventSystems;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby
    /// 容器交互接口
    /// </summary>
    public interface INearbyContainerInteract : IPointerClickHandler
    {
        void CloseToContainer(Item containerItem);
        void AwayFromContainer();
        void OpenContainer(Item containerItem);
        void CloseContainer(Item containerItem);
        Item GetContainerItem();
    }
}