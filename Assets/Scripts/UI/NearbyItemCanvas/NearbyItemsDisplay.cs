using System.Collections.Generic;
using ItemInventory;
using UnityEngine;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby
    /// 管理[附近的物品]面板中物品的显示
    /// </summary>
    public class NearbyItemsDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform itemsContent; // 容器内容区域
        [SerializeField] private GameObject itemPrefab; // 容器预制体

        private INearbyContainerInteract _currentOpenContainer; // 当前打开的容器
        private List<INearbyItemInteract> _displayItems; // 当前显示的物品


        /// <summary>
        /// 展示对应容器中的物品
        /// </summary>
        /// <param name="containerInteract">容器交互接口</param>
        private void DisplayItems(INearbyContainerInteract containerInteract)
        {
            if (containerInteract == null)
            {
                return;
            }

            if (!ParameterCheck(containerInteract.GetContainerItem()))
            {
                return;
            }

            Item containerItem = containerInteract.GetContainerItem();

            // 切换的容器为当前显示的容器 跳过显示
            if (containerItem == _currentOpenContainer.GetContainerItem())
                return;


            // 清空当前显示的物品
            ClearDisplayInfo();
            // 显示新的物品列表
            DisplayItemInfo(containerItem.ItemInstance.ContainedItems);
        }


        /// <summary>
        /// 清除物品显示面板的信息 用于下一次显示时的复用
        /// </summary>
        private void ClearDisplayInfo()
        {
            _displayItems ??= new List<INearbyItemInteract>(20);

            foreach (var itemInteract in _displayItems)
            {
                // 清除显示内容并隐藏
                itemInteract.ClearItemInfo();
                itemInteract.HideItemInfo();
            }
        }

        /// <summary>
        /// 显示物品信息
        /// </summary>
        /// <param name="items">需要显示的物品的列表</param>
        private void DisplayItemInfo(List<Item> items)
        {
            if (items == null || items.Count == 0)
            {
                Debug.LogError("No items to display.");
                return;
            }

            // 如果现有的显示面板不够用 则扩充
            if (items.Count >= _displayItems.Count)
            {
                int itemsToAdd = items.Count - _displayItems.Count + 1;
                for (int i = 0; i < itemsToAdd; i++)
                {
                    GameObject newItemObj = Instantiate(itemPrefab, itemsContent);
                    INearbyItemInteract itemInteract = newItemObj.GetComponent<INearbyItemInteract>();
                    if (itemInteract != null)
                    {
                        itemInteract.ClearItemInfo();
                        itemInteract.HideItemInfo();
                        _displayItems.Add(itemInteract);
                    }
                    else
                    {
                        Debug.LogError("The instantiated item prefab does not have an INearbyItemInteract component.");
                    }
                }
            }

            // 显示物品信息
            for (int i = 0; i < items.Count; i++)
            {
                _displayItems[i].DisplayItemInfo(items[i]);
            }
        }

        /// <summary>
        /// 合法性检测
        /// </summary>
        /// <param name="container">容器</param>
        /// <returns>是否通过检测</returns>
        private bool ParameterCheck(Item container)
        {
            //todo:完善检测内容
            return true;
        }
    }
}