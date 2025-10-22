using System.Collections.Generic;
using CoreListener;
using ItemInventory;
using UI.EventType;
using UnityEngine;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby
    /// 管理[附近的物品]面板中容器的显示
    /// </summary>
    public class NearbyContainersDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform containerContent; // 容器内容区域
        [SerializeField] private GameObject containerPrefab; // 容器预制体

        private List<INearbyContainerInteract> _displayContainersStore; // 显示容器的空闲池
        private List<INearbyContainerInteract> _displayContainersInUse; // 正在使用的显示容器列表
        private INearbyContainerInteract _groundContainer; // 地面容器交互接口
        private INearbyContainerInteract _currentDisplayContainer; // 当前显示的容器交互接口


        private void Awake()
        {
            // 初始化显示容器列表
            _displayContainersStore ??= new List<INearbyContainerInteract>(10);
            _displayContainersInUse ??= new List<INearbyContainerInteract>(10);
        }

        private void OnEnable()
        {
            EventCenter.Instance.AddListener<EventNearbyContainer>(CloseToContainer);
            EventCenter.Instance.AddListener<EventNearbyContainer>(AwayFromContainer);
        }

        private void OnDisable()
        {
            EventCenter.Instance?.RemoveListener<EventNearbyContainer>(CloseToContainer);
            EventCenter.Instance?.RemoveListener<EventNearbyContainer>(AwayFromContainer);
        }

        /// <summary>
        /// 靠近容器
        /// </summary>
        private void CloseToContainer(EventNearbyContainer containerItem)
        {
            if (!ContainerParameterCheck(containerItem, true))
            {
                return;
            }

            // 尝试从空闲池中获取一个可用的交互组件
            INearbyContainerInteract interact;
            if (_displayContainersStore.Count > 0)
            {
                interact = _displayContainersStore[0];
                _displayContainersStore.RemoveAt(0);
            }
            else
            {
                // 2. 空闲池为空，实例化新预制体
                GameObject newInstance = Instantiate(containerPrefab, containerContent);
                interact = newInstance.GetComponent<INearbyContainerInteract>();
                if (interact == null)
                {
                    Debug.LogError("Instantiated prefab does not implement INearbyContainerInteract.");
                    Destroy(newInstance); // 防止内存泄漏
                    return;
                }
            }

            // 显示容器信息 加入使用中列表
            interact.DisplayContainerInfo(containerItem.ContainerItem);
            _displayContainersInUse.Add(interact);
        }

        /// <summary>
        /// 远离容器
        /// </summary>
        private void AwayFromContainer(EventNearbyContainer containerItem)
        {
            if (!ContainerParameterCheck(containerItem, false))
            {
                return;
            }

            // todo: Add：如果一个正在显示的容器离开了范围 清除对应的内容 并切换到地面容器
            if (containerItem.ContainerItem == _currentDisplayContainer.GetContainer())
            {
                _groundContainer.DisplayContainerInfo(_groundContainer.GetContainer());
            }

            // 在显示列表中查找对应容器
            INearbyContainerInteract targetInteract = null;
            for (int i = 0; i < _displayContainersInUse.Count; i++)
            {
                if (_displayContainersInUse[i].GetContainer() == containerItem.ContainerItem)
                {
                    targetInteract = _displayContainersInUse[i];
                    _displayContainersInUse.RemoveAt(i);
                    break;
                }
            }

            if (targetInteract != null)
            {
                // 清理 隐藏 回收
                targetInteract.ClearContainerInfo(); // 清除数据（如物品列表引用）
                targetInteract.HideContainerInfo(); // 隐藏UI
                _displayContainersStore.Add(targetInteract);
            }
            else
            {
                Debug.LogWarning("Attempted to remove a container that was not in use.");
            }
        }


        /// <summary>
        /// 靠近物品
        /// </summary>
        private void CloseToItem(EventNearbyItem worldItem)
        {
            //todo:物品放入容器后 对容器内容的更新
        }

        /// <summary>
        /// 远离物品
        /// </summary>
        private void AwayFromItem(EventNearbyItem worldItem)
        {
        }


        /// <summary>
        /// 参数合法性检查
        /// </summary>
        /// <param name="containerItem">交互传递内容</param>
        /// <param name="expectedType">目标枚举</param>
        /// <returns></returns>
        private bool ContainerParameterCheck(EventNearbyContainer containerItem, bool expectedType)
        {
            // 预制体非空 显示区域非空 传入参数非空 物品BaseInfo非空 物品类型为容器 物品实例非空
            if (containerPrefab == null)
            {
                Debug.LogError("Container prefab is not assigned.");
                return false;
            }

            if (containerContent == null)
            {
                Debug.LogError("Container content area is not assigned.");
                return false;
            }

            if (containerItem == null || containerItem.IsClose != expectedType)
            {
                Debug.LogError($"Container item is null or type is wrong containerItem: {containerItem} + type: {containerItem.IsClose} .");
                return false;
            }

            if (containerItem.ContainerItem.BaseInfo == null)
            {
                Debug.Log("Container item base info is null.");
                //todo:通过ID获取BaseInfo
                if (containerItem.ContainerItem.BaseInfo == null)
                {
                    Debug.LogError("Container item base info is still null after attempting to retrieve by ID.");
                    return false;
                }
            }

            if (containerItem.ContainerItem.BaseInfo.itemType != E_ItemType.ItemContainer)
            {
                Debug.LogError("The provided item is not a container type.");
                return false;
            }

            if (containerItem.ContainerItem.ItemInstance == null)
            {
                Debug.LogError("Container item instance is null.");
                return false;
            }

            return true;
        }
    }
}