using CoreListener;
using ItemInventory;
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
        private Image _containerIcon;
        private Item _containerItem;
        private EveNearbyContainerItemsDisplay _containerItemsDisplayEvent;

        private void Awake()
        {
            _containerIcon = GetComponentInChildren<Image>();
            if (_containerIcon == null)
            {
                Debug.LogError("Image component is missing on Near_ContainerInteract.");
            }
        }

        /// <summary>
        /// 靠近容器
        /// </summary>
        /// <param name="containerItem"></param>
        public void CloseToContainer(Item containerItem)
        {
            if (containerItem == null || containerItem.ID == 0)
            {
                Debug.LogError("Container item or its ID is null.");
                return;
            }

            if (containerItem.BaseInfo == null)
            {
                // todo:加载基础资源
                // 资源加载后依旧为空 则报错
                if (containerItem.BaseInfo == null)
                {
                    Debug.LogError("Container item's BaseInfo is null.");
                    return;
                }
            }

            _containerIcon.sprite = containerItem.BaseInfo.icon;
            gameObject.name = containerItem.BaseInfo.name;
            _containerItem = containerItem;
            _containerItemsDisplayEvent = new EveNearbyContainerItemsDisplay(_containerItem);
        }

        /// <summary>
        /// 远离容器
        /// </summary>
        public void AwayFromContainer()
        {
            Destroy(gameObject);
        }

        public void OpenContainer(Item containerItem)
        {
            //暂时无用
        }

        public void CloseContainer(Item containerItem)
        {
            //暂时无用
        }

        public Item GetContainerItem()
        {
            return _containerItem;
        }

        /// <summary>
        /// 点击容器Icon显示内容
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                EventCenter.Instance.TriggerEvent<EveNearbyContainerItemsDisplay>(_containerItemsDisplayEvent);
            }
        }
    }
}