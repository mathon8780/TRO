using CoreListener;
using Inventory;
using UI.EventType;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby
    /// 容器列表的内容显示
    /// </summary>
    public class NearbyContainerDisplay : MonoBehaviour, INearbyContainerInteract
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

        public void EnterContainer(Item item)
        {
            // todo:重写靠近物品容器的逻辑

            // ItemInteractiveContext context = new ItemInteractiveContext
            // {
            //     BehaviorType = E_ItemBehaviorType.ContainerInteract,
            //     Item = item,
            //     TargetItemContainer = _containerItem
            // };
            // bool isRight = ItemBehaviorCenter.Instance.ExistBehavior(context);
            // todo:添加 若当前显示的容器为该容器 则刷新物品列表 以显示新的物品内容
        }

        public void ExitContainer(Item item)
        {
        }

        /// <summary>
        /// 初始化容器信息
        /// </summary>
        /// <param name="containerItem">容器内容</param>
        public void LoadContainerInfo(Item containerItem)
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
            // _containerIcon.sprite = containerItem.BaseInfo.icon;
            gameObject.SetActive(true);
            gameObject.name = containerItem.ItemData.itemName;
            _containerItem = containerItem;
            _displayContainerItemsEvent = new EventNearbyDisplayContainerItems(this);
        }

        /// <summary>
        /// 清除容器信息
        /// </summary>
        public void UnloadContainerInfo()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 清除容器显示信息 用于下一次显示时的复用
        /// </summary>
        public void ClearContainerInfo()
        {
            _containerIcon.sprite = null;
            gameObject.name = "Nearby_ContainerInteract";
            _containerItem = null;
            _displayContainerItemsEvent = null;
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

        /// <summary>
        /// 点击容器Icon显示内容
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                EventCenter.Instance.TriggerEvent<EventNearbyDisplayContainerItems>(_displayContainerItemsEvent);
            }
            // todo: 右键部分容器 类似于各类背包 可以将对应的背包直接装备到身上
        }
    }
}