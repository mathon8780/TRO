using UnityEngine;
using UnityEngine.UI;

namespace UI.NearbyItemCanvas
{
    public class NearbyItemCanvasControl : MonoBehaviour
    {
        #region Components References

        [SerializeField] private Text backpackWeight; // 背包负重显示
        [SerializeField] private RectTransform containerContent; // 容器内容区域
        [SerializeField] private RectTransform itemContent; // 物品内容区域
        [SerializeField] private Image quickEquip; // 快捷装备
        [SerializeField] private Image quickPick; // 快速拾取 优先级 背包 > 随身容器 > 背心
        [SerializeField] private RectTransform backpackContent; // 背包内容区域

        #endregion
    }
}