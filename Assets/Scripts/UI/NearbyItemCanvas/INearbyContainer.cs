using Inventory;
using UnityEngine.EventSystems;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby 容器交互接口 
    /// </summary>
    public interface INearbyContainer : IPointerClickHandler
    {
        /// <summary>
        /// 物品进入容器
        /// </summary>
        /// <param name="item"></param>
        void ContainerEnter(Item item);

        /// <summary>
        /// 物品离开容器
        /// </summary>
        /// <param name="item"></param>
        void ContainerExit(Item item);

        /// <summary>
        /// 初始化容器内容
        /// </summary>
        /// <param name="containerItem">容器对象</param>
        void InitContainer(Item containerItem);

        /// <summary>
        /// 清除容器内容 用于下一次的复用
        /// </summary>
        void ClearContainerContent();

        /// <summary>
        /// 获取容器内容
        /// </summary>
        /// <returns></returns>
        Item GetContainer();
        // 显示容器内容的实现在接口中 不在此接口 此接口只负责数据的存储
    }
}