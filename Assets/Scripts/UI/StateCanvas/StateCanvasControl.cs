using System;
using DataStructure;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.StateCanvas
{
    public class StateCanvasControl : MonoBehaviour
    {
        [SerializeField] private RectTransform stateCanvasContent;


        [SerializeField] private GameObject stateInfoPrefab;


        private void Awake()
        {
            if (stateCanvasContent == null)
                Debug.LogError("StateCanvasContent is null!");
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            // 创建并初始化状态信息控件
            foreach (var property in Enum.GetValues(typeof(E_PlayerProperty)))
            {
                GameObject stateInfoObj = Instantiate(stateInfoPrefab, stateCanvasContent);
                StateInfoControl stateInfoControl = stateInfoObj.GetComponent<StateInfoControl>();
                stateInfoControl.Init((E_PlayerProperty)property);
            }
        }
    }
}