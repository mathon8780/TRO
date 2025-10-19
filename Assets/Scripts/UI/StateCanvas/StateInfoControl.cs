using System;
using CoreListener;
using DataStructure;
using UI.EventType;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.StateCanvas
{
    public class StateInfoControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private E_PlayerProperty _state;

        [SerializeField] private Text stateNameText;
        [SerializeField] private Slider slider;
        [SerializeField] private Text percentageDisplayText;
        [SerializeField] private Text numericalDisplayText;
        [SerializeField] private Image fillImage;

        private Vector2 OffsetPosition = new Vector2(240f, 0f);


        private void Awake()
        {
            if (slider == null)
                slider = GetComponent<Slider>();

            if (!percentageDisplayText)
                percentageDisplayText = GetComponent<Text>();

            if (!numericalDisplayText)
                numericalDisplayText = GetComponent<Text>();

            percentageDisplayText.rectTransform.anchoredPosition += OffsetPosition;
        }

        public void Init(E_PlayerProperty state)
        {
            _state = state;
            gameObject.name = _state.ToString();
            string nameStr = state switch
            {
                E_PlayerProperty.Health => "健康",
                E_PlayerProperty.Hydration => "水分",
                E_PlayerProperty.Satiety => "饱食度",
                E_PlayerProperty.Carbohydrates => "碳水化合物",
                E_PlayerProperty.Fat => "脂肪",
                E_PlayerProperty.Protein => "蛋白质",
                E_PlayerProperty.Energy => "能量",
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };

            if (stateNameText != null)
            {
                stateNameText.text = nameStr;
            }
        }

        private void OnEnable()
        {
            EventCenter.Instance.AddListener<StateInfo>(UpdateStateInfo);
        }

        private void OnDisable()
        {
            EventCenter.Instance?.RemoveListener<StateInfo>(UpdateStateInfo);
        }

        /// <summary>
        /// 更新状态信息
        /// </summary>
        /// <param name="stateInfo"></param>
        private void UpdateStateInfo(StateInfo stateInfo)
        {
            if (stateInfo.State != _state)
                return;

            float currentValue = stateInfo.CurrentValue;
            float maxValue = stateInfo.MaxValue;

            // 更新 Slider 数值
            slider.maxValue = maxValue;
            slider.value = currentValue;

            // 计算百分比（用于颜色插值）
            float ratio = (maxValue == 0) ? 0f : Mathf.Clamp01(currentValue / maxValue);

            // 更新百分比文本
            if (percentageDisplayText != null)
            {
                percentageDisplayText.text = $"{ratio * 100f:F1}%";
            }

            // 更新数值文本
            if (numericalDisplayText != null)
            {
                numericalDisplayText.text = $"{currentValue:F0} / {maxValue:F0}";
            }

            // ✅ 更新 Slider 填充颜色：红 → 黄 → 绿（更自然的渐变）
            if (fillImage != null)
            {
                // 方案1：红 → 绿（简单线性）
                // Color color = Color.Lerp(Color.red, Color.green, ratio);

                // 方案2：红 → 黄 → 绿（更符合健康/状态直觉）
                Color color;
                if (ratio < 0.5f)
                {
                    // 0.0 ~ 0.5: 红 → 黄
                    color = Color.Lerp(Color.red, Color.yellow, ratio * 2f);
                }
                else
                {
                    // 0.5 ~ 1.0: 黄 → 绿
                    color = Color.Lerp(Color.yellow, Color.green, (ratio - 0.5f) * 2f);
                }

                fillImage.color = color;
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            percentageDisplayText.rectTransform.anchoredPosition -= OffsetPosition;
            numericalDisplayText.rectTransform.anchoredPosition += OffsetPosition;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            percentageDisplayText.rectTransform.anchoredPosition += OffsetPosition;
            numericalDisplayText.rectTransform.anchoredPosition -= OffsetPosition;
        }
    }
}