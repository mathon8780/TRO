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
        void DisplayContainerInfo(Item containerItem);
        void HideContainerInfo();
        void ClearContainerInfo();
        Item GetContainer();
    }
}