using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby Item
    /// 物品信息展示
    /// </summary>
    public class NearbyItemDisplay : MonoBehaviour, INearbyItemInteract
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text itemName;
        [SerializeField] private Text itemCount;
        private Item _item;

        public void DisplayItemInfo(Item item)
        {
            icon.sprite = item.ItemData.icon;
            itemName.text = item.ItemData.name;
            itemCount.text = item.ItemStackCount.ToString();
            _item = item;
        }

        public void HideItemInfo()
        {
        }

        public void ClearItemInfo()
        {
            icon.sprite = null;
            itemName.text = "";
            itemCount.text = "";
            _item = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // 隐藏物品信息悬浮窗
        }
    }
}