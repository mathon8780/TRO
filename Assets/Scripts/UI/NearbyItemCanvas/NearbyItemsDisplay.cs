using System.Collections.Generic;
using CoreListener;
using Inventory;
using Inventory.Property;
using UI.EventType;
using UnityEngine;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby 管理[附近的物品]面板中物品的显示
    /// </summary>
    public class NearbyItemsDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform itemsContent; // 物品内容区域
        [SerializeField] private GameObject itemPrefab; // 物品UI预制体

        private INearbyContainer _currentOpenContainer; // 当前打开的容器
        private Queue<INearbyItem> _freeItems; // 物品UI接口的空闲池
        private List<INearbyItem> _useItems; // 当前显示的物品


        // private bool _isDisplayDitalItemInfo = false; // 是否正在显示物品详细信息

        #region Life Func

        private void Awake()
        {
            // 初始化显示物品列表
            _freeItems ??= new Queue<INearbyItem>(20);
            _useItems ??= new List<INearbyItem>(20);
        }

        private void OnEnable()
        {
            EventCenter.Instance.AddListener<EventNearbyDisplayContainerItems>(DisplayItems);
            EventCenter.Instance.AddListener<EventNearbyDisplayDitalItemInfo>(OnDisplayItemDetailInfo);
            EventCenter.Instance.AddListener<EventNearbyItemEnterAndExit>(HandleItemEnterAndExit);
        }

        private void OnDisable()
        {
            EventCenter.Instance?.RemoveListener<EventNearbyDisplayContainerItems>(DisplayItems);
            EventCenter.Instance?.RemoveListener<EventNearbyDisplayDitalItemInfo>(OnDisplayItemDetailInfo);
            EventCenter.Instance?.RemoveListener<EventNearbyItemEnterAndExit>(HandleItemEnterAndExit);
        }

        #endregion


        #region Interaction Func

        /// <summary>
        /// 展示对应容器中的物品
        /// </summary>
        /// <param name="containerContext">容器交互接口</param>
        private void DisplayItems(EventNearbyDisplayContainerItems containerContext)
        {
            if (containerContext == null)
            {
                Debug.LogError($"containerContext is null.");
                return;
            }

            if (!ParameterCheck(containerContext.ContainerItem.GetContainer()))
                return;

            var containerItem = containerContext.ContainerItem;

            // 切换的容器为当前显示的容器 跳过显示
            if (_currentOpenContainer == containerItem)
                return;

            // 清空当前显示的物品
            ClearDisplayInfo();
            DisplayContainerItems(containerItem.GetContainer().GetProperty<ItemContainerProperty>().Content);
        }

        /// <summary>
        /// 处理物品的靠近远离容器的事件
        /// </summary>
        /// <param name="eventData"></param>
        private void HandleItemEnterAndExit(EventNearbyItemEnterAndExit eventData)
        {
            //todo: logic
        }

        /// <summary>
        /// 清除物品显示面板的信息 用于下一次显示时的复用
        /// </summary>
        private void ClearDisplayInfo()
        {
            foreach (var itemInteract in _useItems)
            {
                // 清除显示内容并隐藏
                itemInteract.ClearItemInfo();
                itemInteract.HideItemInfo();
                // 回收到空闲池
                _freeItems.Enqueue(itemInteract);
            }
        }

        /// <summary>
        /// 显示物品信息
        /// </summary>
        /// <param name="items">需要显示的物品的列表</param>
        private void DisplayContainerItems(List<Item> items)
        {
            if (items == null)
            {
                Debug.LogError($"items is null.");
                return;
            }

            // 如果现有的显示面板不够用 则扩充
            if (items.Count >= _freeItems.Count)
            {
                int itemsToAdd = items.Count - _useItems.Count;
                for (int i = 0; i < itemsToAdd; i++)
                {
                    GameObject newItemObj = Instantiate(itemPrefab, itemsContent, false);
                    INearbyItem itemInteract = newItemObj.GetComponent<INearbyItem>();
                    if (itemInteract != null)
                    {
                        itemInteract.ClearItemInfo();
                        itemInteract.HideItemInfo();
                        _freeItems.Enqueue(itemInteract);
                    }
                    else
                    {
                        Debug.LogError("The instantiated item prefab does not have an INearbyItem component.");
                    }
                }
            }

            // 显示物品信息
            foreach (var item in items)
            {
                INearbyItem itemInteract;
                if (_freeItems.Count > 0)
                {
                    itemInteract = _freeItems.Dequeue();
                }
                else
                {
                    Debug.LogError("No free item interact available, this should not happen.");
                    continue;
                }

                itemInteract.InitItemInfo(item);
                itemInteract.ShowItemInfo();
                _useItems.Add(itemInteract);
            }
        }

        #endregion


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

        private void OnDisplayItemDetailInfo(EventNearbyDisplayDitalItemInfo eventData)
        {
            // _isDisplayDitalItemInfo = eventData.Display;
        }
    }
}