using System.Collections.Generic;
using CoreListener;
using UI.EventType;
using UnityEngine;

namespace UI.NearbyItemCanvas
{
    /// <summary>
    /// Nearby 管理[附近的物品]面板中容器的显示
    /// </summary>
    public class NearbyContainersDisplay : MonoBehaviour
    {
        [SerializeField] private Sprite defaultGroundIcon; // 默认地面图标
        [SerializeField] private RectTransform containerContent; // 容器内容区域
        [SerializeField] private GameObject containerPrefab; // 容器预制体

        private Queue<INearbyContainer> _freeContainer; // 容器接口的空闲池
        private List<INearbyContainer> _useContainer; // 正在使用的显示容器列表
        private INearbyContainer _ground; // 地面容器交互接口

        private EventNearbyDisplayContainerItems _displayGroundEvent;


        #region Life Func

        private void Awake()
        {
            // 初始化显示容器列表
            _freeContainer ??= new Queue<INearbyContainer>(15);
            _useContainer ??= new List<INearbyContainer>(15);
        }

        private void Start()
        {
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
            EventCenter.Instance?.RemoveListener<EventNearbyItem>(CloseToItem);
            EventCenter.Instance?.RemoveListener<EventNearbyItem>(AwayFromItem);
        }

        #endregion


        /// <summary>
        /// 初始化地面容器
        /// </summary>
        private void InitGroundContainer()
        {
            GameObject obj = Instantiate(containerPrefab, containerContent, false);
            _ground = obj.GetComponent<INearbyContainer>();
            _ground.InitContainer(InventoryInfo.Instance.GetItem(110000, 1)); //获取并初始化容器信息
            _displayGroundEvent = new EventNearbyDisplayContainerItems(_ground);
        }

        #region 检测交互

        /// <summary>
        /// 靠近容器
        /// </summary>
        private void CloseToContainer(EventNearbyContainer eventData)
        {
            if (eventData.IsClose)
            {
                if (!ContainerParameterCheck(eventData, true))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            if (!ContainerParameterCheck(eventData, true))
                return;


            // 尝试从空闲池中获取一个可用的交互组件
            INearbyContainer interact;
            if (_freeContainer.Count <= 0)
            {
                // 1.2 空闲池为空，实例化新预制体
                GameObject newInstance = Instantiate(containerPrefab, containerContent, false);
                interact = newInstance.GetComponent<INearbyContainer>();
                if (interact == null)
                {
                    Debug.LogError("Instantiated prefab does not implement INearbyContainerInteract.");
                    Destroy(newInstance); // 防止内存泄漏
                    return;
                }

                _freeContainer.Enqueue(interact);
            }

            // 获得接口 初始化 添加到使用列表
            interact = _freeContainer.Dequeue();
            interact.InitContainer(eventData.ContainerItemInfo);
            _useContainer.Add(interact);
        }

        /// <summary>
        /// 远离容器
        /// </summary>
        private void AwayFromContainer(EventNearbyContainer eventData)
        {
            if (!eventData.IsClose)
            {
                if (!ContainerParameterCheck(eventData, false))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            // 在显示列表中查找对应容器
            INearbyContainer target = null;
            for (int i = 0; i < _useContainer.Count; i++)
            {
                if (_useContainer[i].GetContainer() == eventData.ContainerItemInfo)
                {
                    target = _useContainer[i];
                    _useContainer.RemoveAt(i);
                    break;
                }
            }


            if (target != null)
            {
                // 清理 隐藏 回收
                target.ClearContainerContent(); // 清除数据及隐藏
                _freeContainer.Enqueue(target);
                EventCenter.Instance.TriggerEvent(_displayGroundEvent);
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

            if (!ParameterCheck(worldItem, true)) return;

            _ground.ContainerEnter(worldItem.WorldItemInfo);
        }

        /// <summary>
        /// 远离物品
        /// </summary>
        private void AwayFromItem(EventNearbyItem worldItem)
        {
            if (!ParameterCheck(worldItem, false))
                return;
        }

        #endregion


        /// <summary>
        /// 容器参数合法性检查
        /// </summary>
        /// <param name="item">交互传递内容</param>
        /// <param name="expectedOp">目标操作</param>
        /// <returns></returns>
        private bool ContainerParameterCheck(EventNearbyContainer item, bool expectedOp)
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

            if (item == null || item.IsClose != expectedOp)
            {
                Debug.LogError($"Container item is null or type is wrong containerItem: {item} + type: {item.IsClose} .");
                return false;
            }

            if (item.ContainerItemInfo.ItemData == null)
            {
                Debug.Log("Container item base info is null.");
                //todo:通过ID获取BaseInfo
                if (item.ContainerItemInfo.ItemData == null)
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
        private bool ParameterCheck(EventNearbyItem worldItem, bool expectedOp)
        {
            if (worldItem == null || worldItem.IsClose != expectedOp)
            {
                Debug.LogError("World item is null or type is wrong.");
                return false;
            }

            if (worldItem.WorldItemInfo.ItemData == null)
            {
                Debug.LogError("World item base info is null.");
                if (worldItem.WorldItemInfo.ItemData == null)
                {
                    Debug.LogError("World item base info is still null after attempting to retrieve by ID.");
                    return false;
                }
            }

            return true;
        }
    }
}