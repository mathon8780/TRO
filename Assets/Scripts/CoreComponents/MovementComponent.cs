using CoreComponents.DataRecord;
using CoreListener;
using DataStructure;
using UnityEngine;

namespace CoreComponents
{
    /// <summary>
    /// 玩家移动组件
    /// </summary>
    public class MovementComponent : MonoBehaviour
    {
        #region Component References

        [SerializeField] private CharacterController cc; // 角色控制器
        [SerializeField] private Animator animator; // 动画控制器

        #endregion

        #region Data References

        private NutritionAndMobility _movementData; // 角色的移动数据
        private MovementCost _movementCost; // 运行时的移动消耗及速度信息实例
        private AnimatorOperate _animatorOperate; // 动画操作实例

        #endregion


        private static Vector3 _velocity = Vector3.zero; // 角色的当前速度

        private const float Gravity = -9.81f; // 重力加速度


        private void Awake()
        {
            if (cc == null)
            {
                cc = GetComponent<CharacterController>();
                if (cc == null)
                {
                    Debug.LogError("CharacterController component is missing.");
                }
            }

            if (animator == null)
            {
                animator = GetComponent<Animator>();
                if (animator == null)
                {
                    Debug.LogError("Animator component is missing.");
                }
            }


            _animatorOperate = new AnimatorOperate(animator);
        }

        private void OnEnable()
        {
            InputManager.Instance.OnMove += HandleMove;
            InputManager.Instance.OnJump += HandleJump;
            InputManager.Instance.OnSquat += HandleSquat;
            InputManager.Instance.OnLie += HandleLie;
            InputManager.Instance.OnRun += HandleRun;
            InputManager.Instance.OnRush += HandleRush;
            _movementData = GetComponent<DataStorageComponent>().GetNutritionAndMobilityData();
            _movementCost = GetComponent<DataStorageComponent>().GetMovementCostData();
        }

        private void OnDisable()
        {
            // if (!InputManager.Instance)
            //     return;
            //
            // InputManager.Instance.OnMove -= HandleMove;
            // InputManager.Instance.OnJump -= HandleJump;
            // InputManager.Instance.OnSquat -= HandleSquat;
            // InputManager.Instance.OnLie -= HandleLie;
            // InputManager.Instance.OnRun -= HandleRun;
            // InputManager.Instance.OnRush -= HandleRush;
        }

        #region Movement Handling

        /// <summary>
        /// 处理移动需求
        /// </summary>
        /// <param name="direction"></param>
        private void HandleMove(Vector2 direction)
        {
            //todo: move to camera forward
            // 处理移动逻辑
            if (direction == Vector2.zero)
            {
                _animatorOperate.TriggerMove(false);
                return;
            }

            _animatorOperate.TriggerMove(true);

            // 计算移动方向和速度
            _velocity.x = direction.x;
            _velocity.z = direction.y;
            float speed = 0f;
            E_MovementActions currentAction = _animatorOperate.GetCurrentMoveAction();
            speed = _movementCost.GetMovementSpeed(
                _movementCost.CanTakeMove(currentAction, _movementData.Energy)
                    ? currentAction
                    : E_MovementActions.Walk);

            _velocity *= speed;
            // 应用重力
            ApplyGravity();


            // //调试
            // Debug.Log($"Current Action: {currentAction}+" +
            //           $"{_movementCost.CanTakeMove(currentAction, _movementData.Energy)}+" +
            //           $"{speed}");

            // 移动角色
            cc.Move(_velocity * (Time.deltaTime * 5f));
            _velocity = Vector3.zero;
        }

        private void HandleJump()
        {
            // if (!cc.isGrounded) return;

            Debug.Log("Jump Triggered");
            _animatorOperate.TriggerJump();
            _velocity.y = Mathf.Sqrt(_movementData.JumpHeight * -2f * Gravity);
        }

        private void HandleSquat()
        {
            _animatorOperate.TriggerSquat();
        }

        private void HandleLie()
        {
            _animatorOperate.TriggerLie();
        }

        private void HandleRun(bool run)
        {
            _animatorOperate.TriggerQuickMove(run);
            // 处理跑步逻辑
        }

        private void HandleRush(bool rush)
        {
            _animatorOperate.TriggerRush(rush);
            // 处理冲刺逻辑
        }


        private void ApplyGravity()
        {
            if (cc.isGrounded)
            {
                _velocity.y = -1f; // 保持角色贴地
                return;
            }

            _velocity.y += Gravity * Time.deltaTime;
        }

        #endregion
    }
}