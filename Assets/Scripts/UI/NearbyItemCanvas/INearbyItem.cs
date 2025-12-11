using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby-物品交互接口
    /// </summary>
    public interface INearbyItem :
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler, IDropHandler
    {
        /// <summary>
        /// 展示物品的详细信息
        /// </summary>
        /// <param name="item"></param>
        void InitItemInfo(Item item);

        /// <summary>
        /// 激活面板
        /// </summary>
        void ShowItemInfo();

        /// <summary>
        /// 隐藏物品信息 有用？
        /// </summary>
        void HideItemInfo();

        /// <summary>
        /// 清除UI上的物品信息
        /// </summary>
        void ClearItemInfo();
    }
}