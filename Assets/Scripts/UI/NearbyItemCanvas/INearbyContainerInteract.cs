using Inventory;
using UnityEngine.EventSystems;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby
    /// 容器交互接口
    /// </summary>
    public interface INearbyContainerInteract : IPointerClickHandler
    {
        /// <summary>
        /// 物品进入容器
        /// </summary>
        /// <param name="item"></param>
        void EnterContainer(Item item);

        /// <summary>
        /// 物品离开容器
        /// </summary>
        /// <param name="item"></param>
        void ExitContainer(Item item);

        /// <summary>
        /// 初始化容器内容
        /// </summary>
        /// <param name="containerItem">容器对象</param>
        void LoadContainerInfo(Item containerItem);

        /// <summary>
        /// 卸载容器信息
        /// </summary>
        void UnloadContainerInfo();

        /// <summary>
        /// 清除容器内容 用于下一次的复用
        /// </summary>
        void ClearContainerInfo();

        /// <summary>
        /// 获取容器内容
        /// </summary>
        /// <returns></returns>
        Item GetContainer();
        // 显示容器内容的实现在接口中 不在此接口 此接口只负责数据的存储
    }
}