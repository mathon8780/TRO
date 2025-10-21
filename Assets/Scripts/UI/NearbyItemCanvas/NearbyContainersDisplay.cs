using System.Collections.Generic;
using System.Linq;
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

        private List<INearbyContainerInteract> _containerInteracts = new(20);


        private void OnEnable()
        {
            EventCenter.Instance.AddListener<EveNearbyContainer>(CloseToContainer);
            EventCenter.Instance.AddListener<EveNearbyContainer>(AwayFromContainer);
        }

        private void OnDisable()
        {
            EventCenter.Instance?.RemoveListener<EveNearbyContainer>(CloseToContainer);
            EventCenter.Instance?.RemoveListener<EveNearbyContainer>(AwayFromContainer);
        }

        /// <summary>
        /// 靠近容器
        /// </summary>
        /// <param name="containerItem"></param>
        private void CloseToContainer(EveNearbyContainer containerItem)
        {
            // 参数检查
            if (!ParameterCheck(containerItem, E_NearbyContainerInteract.CloseToContainer))
            {
                return;
            }

            // 显示过程： 靠近 显示容器信息 添加到列表缓存 点击-显示容器物品列表
            GameObject newContainerObj = Instantiate(containerPrefab, containerContent);
            INearbyContainerInteract containerInteract = newContainerObj.GetComponent<INearbyContainerInteract>();
            if (containerInteract != null)
            {
                containerInteract.CloseToContainer(containerItem.ContainerItem);
                _containerInteracts.Add(containerInteract);
            }
            else
            {
                Debug.LogError("The instantiated container prefab does not have an INearbyContainerInteract component.");
            }
        }

        /// <summary>
        /// 远离容器
        /// </summary>
        /// <param name="containerItem"></param>
        private void AwayFromContainer(EveNearbyContainer containerItem)
        {
            // 参数检查
            if (!ParameterCheck(containerItem, E_NearbyContainerInteract.AwayFromContainer))
            {
                return;
            }

            // 查找对应的容器交互组件 并调用远离方法 然后从列表中移除
            INearbyContainerInteract targetInteract =
                _containerInteracts.FirstOrDefault(container => container.GetContainerItem() == containerItem.ContainerItem);
            if (targetInteract != null)
            {
                targetInteract.AwayFromContainer();
                _containerInteracts.Remove(targetInteract);
            }
        }


        /// <summary>
        /// 参数合法性检查
        /// </summary>
        /// <param name="containerItem">交互传递内容</param>
        /// <param name="expectedType">目标枚举</param>
        /// <returns></returns>
        private bool ParameterCheck(EveNearbyContainer containerItem, E_NearbyContainerInteract expectedType)
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

            if (containerItem == null || containerItem.InteractType != expectedType)
            {
                Debug.LogError($"Container item is null or type is wrong containerItem: {containerItem} + type: {containerItem.InteractType} .");
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