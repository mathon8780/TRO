using CoreListener;
using Inventory;
using UI.EventType;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby Item 物品信息展示
    /// </summary>
    public class NearbyItemDisplay : MonoBehaviour, INearbyItem
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemCount;
        private Item _item;
        private EventItemInfoDisplay _itemInfoDisplay = new();

        /// <summary>
        /// 初始化面板信息
        /// </summary>
        /// <param name="item"></param>
        public void InitItemInfo(Item item)
        {
            icon.sprite = item.ItemData.icon;
            itemName.text = item.ItemData.name;
            itemCount.text = item.ItemStackCount.ToString();
            _item = item;
            _itemInfoDisplay.IsDisplay = false;
            _itemInfoDisplay.DisplayItem = item;
        }

        /// <summary>
        /// 激活显示
        /// </summary>
        public void ShowItemInfo()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏面板
        /// </summary>
        public void HideItemInfo()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 清除面板信息
        /// </summary>
        public void ClearItemInfo()
        {
            icon.sprite = null;
            itemName.text = "";
            itemCount.text = "";
            _item = null;
            _itemInfoDisplay.DisplayItem = null;
        }

        #region Interact With Item

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                // 隐藏信息面板
                // 显示交互面板
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
            }
            else
            {
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnDrop(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 显示物品信息悬浮窗
            _itemInfoDisplay.IsDisplay = true;
            EventCenter.Instance.TriggerEvent<EventItemInfoDisplay>(_itemInfoDisplay);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // 隐藏物品信息悬浮窗
            _itemInfoDisplay.IsDisplay = false;
            EventCenter.Instance.TriggerEvent<EventItemInfoDisplay>(_itemInfoDisplay);
        }

        #endregion
    }
}