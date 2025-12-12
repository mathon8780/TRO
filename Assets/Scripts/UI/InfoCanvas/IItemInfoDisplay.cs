using Inventory;
using UnityEngine;

namespace UI.InfoCanvas
{
    public interface IItemInfoDisplay
    {
        /// <summary>
        /// 初始化显示的信息
        /// </summary>
        /// <param name="item"></param>
        void InitItemInfo(Item item);

        //todo:如何解决在显示信息时 另一个玩家更改了正在显示物品信息的问题？
        /// <summary>
        /// 更新显示的内容
        /// </summary>
        /// <param name="item"></param>
        void UpdateItemInfo(Item item);

        /// <summary>
        /// 更新信息面板显示的位置
        /// </summary>
        /// <param name="position">鼠标位置</param>
        void UpdatePanelPosition(Vector2 position);

        /// <summary>
        /// 隐藏物品信息
        /// </summary>
        void HideItemInfo();

        /// <summary>
        /// 清除UI上的物品信息
        /// </summary>
        void ClearItemInfo();
    }
}