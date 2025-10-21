using ItemInventory;
using UnityEngine.EventSystems;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby
    /// 物品交互接口
    /// </summary>
    public interface INearbyItemInteract :
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler, IDropHandler
    {
        void DisplayItemInfo(Item item);
        void HideItemInfo();
        void ClearItemInfo();
    }
}