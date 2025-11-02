using System.Collections.Generic;
using CoreListener;
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

        #region Life Func

        private void Awake()
        {
            // 初始化显示容器列表
            _displayContainersStore ??= new List<INearbyContainerInteract>(10);
            _displayContainersInUse ??= new List<INearbyContainerInteract>(10);
            InitGroundContainer();
        }


        private void OnEnable()
        {
            EventCenter.Instance.AddListener<EventNearbyContainer>(CloseToContainer);
            EventCenter.Instance.AddListener<EventNearbyContainer>(AwayFromContainer);
            EventCenter.Instance.AddListener<EventNearbyItem>(CloseToItem);
            EventCenter.Instance.AddListener<EventNearbyItem>(AwayFromItem);
        }

        private void OnDisable()
        {
            EventCenter.Instance?.RemoveListener<EventNearbyContainer>(CloseToContainer);
            EventCenter.Instance?.RemoveListener<EventNearbyContainer>(AwayFromContainer);
            EventCenter.Instance?.AddListener<EventNearbyItem>(CloseToItem);
            EventCenter.Instance?.RemoveListener<EventNearbyItem>(AwayFromItem);
        }

        #endregion


        /// <summary>
        /// 添加地面容器交互接口
        /// </summary>
        private void InitGroundContainer()
        {
            //todo: 创建地面容器

            // ItemBaseInfo groundBaseInfo = ScriptableObject.CreateInstance<ItemBaseInfo>();
            // groundBaseInfo.itemId = 1;
            // groundBaseInfo.itemName = "Ground";
            // groundBaseInfo.itemType = E_ItemType.ItemContainer;
            // groundBaseInfo.resourceLoadKey = "Ground";
            // groundBaseInfo.behaviorTypes = new E_ItemBehaviorType[1];
            // groundBaseInfo.behaviorTypes[0] = E_ItemBehaviorType.ContainerInteract;
            // // ItemBaseInfo groundItemInfo = ScriptableObject.CreateInstance<ItemBaseInfo>();
            // // groundItemInfo.itemId = 1;
            // // groundItemInfo.itemName = "Ground";
            // // groundItemInfo.itemType = E_ItemType.ItemContainer;
            // // groundItemInfo.canStack = false;
            // // groundItemInfo.behaviorTypes = new[]
            // // {
            // //     E_ItemBehaviorType.ContainerInteract,
            // // };
            // StItemContainer groundDyItemStaticInfo = new StItemContainer
            // {
            //     ID = 1,
            //     MaxWeight = 999999
            // };
            // DyItemContainer groundDyItemDynamicInfo = new DyItemContainer
            // {
            //     ID = 1,
            //     Inventory = new List<Item>(50)
            // };
            //
            // Item groundItemContainer = new Item()
            // {
            //     ID = 1,
            //     StackNum = 1,
            //     ItemBaseInfo = groundBaseInfo,
            //     ItemStaticInfo = groundDyItemStaticInfo,
            //     ItemDynamicInfo = groundDyItemDynamicInfo,
            // };


            GameObject obj = Instantiate(containerPrefab, containerContent);
            _groundContainer = obj.GetComponent<INearbyContainerInteract>();
            // _groundContainer.LoadContainerInfo(groundItemContainer);
            _currentDisplayContainer = _groundContainer;
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
            interact.LoadContainerInfo(containerItem.ContainerItem);
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
                _groundContainer.LoadContainerInfo(_groundContainer.GetContainer());
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
                targetInteract.UnloadContainerInfo(); // 隐藏UI
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

            if (!ItemParameterCheck(worldItem, true)) return;

            if (_groundContainer == null)
            {
                Debug.LogError("Ground container is not assigned.");
                return;
            }

            _groundContainer.LoadContainerInfo(_groundContainer.GetContainer());
        }

        /// <summary>
        /// 远离物品
        /// </summary>
        private void AwayFromItem(EventNearbyItem worldItem)
        {
            if (!ItemParameterCheck(worldItem, false))
                return;
        }


        /// <summary>
        /// 容器参数合法性检查
        /// </summary>
        /// <param name="containerItem">交互传递内容</param>
        /// <param name="expectedOp">目标操作</param>
        /// <returns></returns>
        private bool ContainerParameterCheck(EventNearbyContainer containerItem, bool expectedOp)
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

            if (containerItem == null || containerItem.IsClose != expectedOp)
            {
                Debug.LogError($"Container item is null or type is wrong containerItem: {containerItem} + type: {containerItem.IsClose} .");
                return false;
            }

            if (containerItem.ContainerItem.ItemData == null)
            {
                Debug.Log("Container item base info is null.");
                //todo:通过ID获取BaseInfo
                if (containerItem.ContainerItem.ItemData == null)
                {
                    Debug.LogError("Container item base info is still null after attempting to retrieve by ID.");
                    return false;
                }
            }


            return true;
        }

        /// <summary>
        /// 物体参数合法性检查
        /// </summary>
        /// <param name="worldItem">交互传递内容</param>
        /// <param name="expectedOp">目标操作</param>
        /// <returns></returns>
        private bool ItemParameterCheck(EventNearbyItem worldItem, bool expectedOp)
        {
            if (worldItem == null || worldItem.IsClose != expectedOp)
            {
                Debug.LogError("World item is null or type is wrong.");
                return false;
            }

            if (worldItem.WorldItem.ItemData == null)
            {
                Debug.LogError("World item base info is null.");
                if (worldItem.WorldItem.ItemData == null)
                {
                    Debug.LogError("World item base info is still null after attempting to retrieve by ID.");
                    return false;
                }
            }


            return true;
        }
    }
}