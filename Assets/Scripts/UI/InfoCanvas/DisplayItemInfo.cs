using System;
using System.Collections.Generic;
using CoreListener;
using Inventory.Property;
using UI.EventType;
using UnityEngine;
using UnityEngine.Events;

namespace UI.InfoCanvas
{
    public class DisplayItemInfo : MonoBehaviour
    {
        [SerializeField] private IItemInfoDisplay _baseInfo;
        [SerializeField] private readonly Dictionary<ItemProperty, IItemInfoDisplay> _propertyInfoDisplays = new();
        private UnityAction _updateInfoPanelPosAction;

        #region Life Func

        private void Update()
        {
            _updateInfoPanelPosAction?.Invoke();
        }

        private void OnEnable()
        {
            EventCenter.Instance.AddListener<EventItemInfoDisplay>(OnDisplayItemInfo);
        }

        private void OnDisable()
        {
            EventCenter.Instance?.RemoveListener<EventItemInfoDisplay>(OnDisplayItemInfo);
        }

        #endregion

        #region Interaction Func

        private void UpdateInfoPanelPos()
        {
            var rt = transform as RectTransform;
            if (rt == null) return;

            var canvas = GetComponentInParent<Canvas>();
            if (canvas == null) return;

            // 在没有父 RectTransform 时使用 Canvas 的 RectTransform 作为参考
            var parentRect = rt.parent as RectTransform ?? canvas.transform as RectTransform;
            var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, cam, out Vector2 localPoint))
            {
                rt.anchoredPosition = localPoint;
            }
        }

        private void OnDisplayItemInfo(EventItemInfoDisplay eventData)
        {
            if (eventData.IsDisplay == true && eventData.DisplayItem != null)
            {
                _baseInfo.InitItemInfo(eventData.DisplayItem);
                _updateInfoPanelPosAction += UpdateInfoPanelPos;
            }
            else
            {
                _updateInfoPanelPosAction -= UpdateInfoPanelPos;
                _baseInfo.HideItemInfo();
                _baseInfo.ClearItemInfo();
                foreach (var propertyDisplay in _propertyInfoDisplays.Values)
                {
                    propertyDisplay.HideItemInfo();
                    propertyDisplay.ClearItemInfo();
                }
            }
        }

        #endregion
    }
}