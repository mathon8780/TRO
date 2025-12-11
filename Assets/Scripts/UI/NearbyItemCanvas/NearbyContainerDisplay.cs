using CoreListener;
using Inventory;
using UI.EventType;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby 容器列表的内容显示
    /// </summary>
    public class NearbyContainerDisplay : MonoBehaviour, INearbyContainer
    {
        private Image _containerIcon; // 容器图标
        private Item _containerItem; // 容器信息
        private EventNearbyDisplayContainerItems _displayContainerItemsEvent;

        private void Awake()
        {
            _containerIcon = GetComponentInChildren<Image>();
            if (_containerIcon == null)
            {
                Debug.LogError("Image component is missing on Near_ContainerInteract.");
            }
        }

        #region INearbyContainer

        public void ContainerEnter(Item item)
        {
            // todo:重写靠近物品容器的逻辑
            // todo:添加 若当前显示的容器为该容器 则刷新物品列表 以显示新的物品内容
        }

        public void ContainerExit(Item item)
        {
        }

        /// <summary>
        /// 初始化容器信息
        /// </summary>
        /// <param name="containerItem">容器内容</param>
        public void InitContainer(Item containerItem)
        {
            if (containerItem == null || containerItem.ItemID == 0)
            {
                Debug.LogError("Container item or its ID is null.");
                return;
            }

            if (containerItem.ItemData == null)
            {
                // todo:加载基础资源
                // 资源加载后依旧为空 则报错
                if (containerItem.ItemData == null)
                {
                    Debug.LogError("Container item's BaseInfo is null.");
                    return;
                }
            }

            // 初始化显示信息
            gameObject.SetActive(true);
            _containerIcon.sprite = containerItem.ItemData.icon;
            gameObject.name = containerItem.ItemData.itemName;
            _containerItem = containerItem;
            // 创建事件缓存
            _displayContainerItemsEvent = new EventNearbyDisplayContainerItems(this);
        }

        /// <summary>
        /// 清除容器显示信息 用于下一次显示时的复用
        /// </summary>
        public void ClearContainerContent()
        {
            _containerIcon.sprite = null;
            gameObject.name = "Nearby_ContainerInteract";
            _containerItem = null;
            _displayContainerItemsEvent = null;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 获取容器
        /// </summary>
        /// <returns></returns>
        public Item GetContainer()
        {
            if (gameObject.activeSelf && _containerItem != null)
            {
                return _containerItem;
            }

            Debug.LogError($"UI-Container is {gameObject.activeSelf} and {_containerItem == null}");
            return null;
        }

        #endregion

        /// <summary>
        /// 点击容器Icon显示内容
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Debug.Log($"Click");
                EventCenter.Instance.TriggerEvent(_displayContainerItemsEvent);
            }
            // todo: 右键部分容器 类似于各类背包 可以将对应的背包直接装备到身上
        }
    }
}