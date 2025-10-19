using BaseTools;
using UnityEngine;
using UnityEngine.Events;

namespace CoreListener
{
    /// <summary>
    /// 中心输入管理器
    /// </summary>
    public class InputManager : SingletonMono<InputManager>
    {
        // 定义事件（可按需扩展）

        #region Input Events

        public event UnityAction<Vector2> OnMove; //移动
        public event UnityAction OnJump; //跳跃
        public event UnityAction OnSquat; //蹲下
        public event UnityAction OnLie; //趴下
        public event UnityAction<bool> OnRun; //跑步
        public event UnityAction<bool> OnRush; //冲刺

        #endregion

        private static Vector2 _moveInput;
        private static bool _isRunning;
        private static bool _isRushing;


        private Player_Actions _inputActions;

        protected override void Awake()
        {
            base.Awake();
            _inputActions = new Player_Actions();

            #region 绑定输入回调

            _inputActions.Character_Movement.Move.performed += ctx => { _moveInput = ctx.ReadValue<Vector2>(); };
            _inputActions.Character_Movement.Move.canceled += ctx => { _moveInput = ctx.ReadValue<Vector2>(); };
            _inputActions.Character_Movement.Jump.performed += ctx => OnJump?.Invoke();
            _inputActions.Character_Movement.Squat.performed += ctx => OnSquat?.Invoke();
            _inputActions.Character_Movement.Lie.performed += ctx => OnLie?.Invoke();
            _inputActions.Character_Movement.Run.performed += ctx => OnRun?.Invoke(true);
            _inputActions.Character_Movement.Run.canceled += ctx => OnRun?.Invoke(false);
            _inputActions.Character_Movement.Rush.performed += ctx => OnRush?.Invoke(true);
            _inputActions.Character_Movement.Rush.canceled += ctx => OnRush?.Invoke(false);

            #endregion
        }

        private void Update()
        {
            OnMove?.Invoke(_moveInput);
            // Debug.Log(_moveInput);
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions?.Disable();
        }

        protected override void OnDestroy()
        {
            _inputActions?.Dispose();
        }
    }
}