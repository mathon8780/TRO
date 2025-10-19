using System;
using System.Collections.Generic;
using DataStructure;
using UnityEngine;

namespace CoreComponents.DataRecord
{
    /// <summary>
    /// 动画状态控制器，负责将游戏逻辑状态映射到 Animator 的 bool 参数。
    /// 注意：Animator 中仅使用以下 bool 参数：
    /// - 姿态：Stand, Squat, Lie, Sit
    /// - 输入：Move, QuickMove
    /// - 特殊动作：Rush, Jumping
    /// 其他枚举值（如 Walk, Run 等）仅为内部逻辑状态，不直接对应 Animator 参数。
    /// </summary>
    public class AnimatorOperate
    {
        private readonly Animator _animator;

        private readonly Dictionary<E_MovementActions, int> _actionHashes;

        // 当前姿态状态（互斥：Stand/Sit/Squat/Lie）
        private E_MovementActions _curPosture = E_MovementActions.Stand; //控制当前的状态 站-蹲-趴

        // 当前移动输入状态（用于内部逻辑判断，不直接设给 Animator）
        private bool _isMoving = false;
        private bool _isQuickMoving = false;
        private bool _isRushing = false;

        // 定义所有姿态状态，用于互斥设置
        private static readonly E_MovementActions[] PostureStates =
        {
            E_MovementActions.Stand,
            E_MovementActions.Sit,
            E_MovementActions.Squat,
            E_MovementActions.Lie
        };


        /// <summary>
        /// 初始化构造
        /// </summary>
        public AnimatorOperate(Animator animator)
        {
            _animator = animator ?? throw new ArgumentNullException(nameof(animator));
            _actionHashes = new Dictionary<E_MovementActions, int>();
            foreach (E_MovementActions action in Enum.GetValues(typeof(E_MovementActions)))
            {
                _actionHashes[action] = Animator.StringToHash(action.ToString());
            }

            SetPosture(E_MovementActions.Stand);
        }

        private int GetHash(E_MovementActions action)
        {
            return _actionHashes[action];
        }

        #region Posture Control

        /// <summary>
        /// 设置角色姿态（站立、坐、蹲、趴），自动互斥
        /// </summary>
        private void SetPosture(E_MovementActions posture)
        {
            if (!IsPostureState(posture))
            {
                Debug.LogError($"Invalid posture state: {posture}");
                return;
            }

            // 互斥：关闭其他姿态，开启当前姿态
            foreach (var state in PostureStates)
            {
                _animator.SetBool(GetHash(state), state == posture);
            }

            _curPosture = posture;
        }

        /// <summary>
        /// 判断是否为对应姿态状态
        /// </summary>
        /// <param name="state">目标姿势</param>
        /// <returns></returns>
        private bool IsPostureState(E_MovementActions state)
        {
            return state
                is E_MovementActions.Stand
                or E_MovementActions.Sit
                or E_MovementActions.Squat
                or E_MovementActions.Lie;
        }

        public void TriggerSit() => SetPosture(E_MovementActions.Sit);
        public void TriggerStand() => SetPosture(E_MovementActions.Stand);

        public void TriggerSquat()
        {
            if (_curPosture == E_MovementActions.Stand)
            {
                SetPosture(E_MovementActions.Squat);
            }
            else if (_curPosture == E_MovementActions.Squat)
            {
                SetPosture(E_MovementActions.Stand);
            }
            else
            {
                // 从 Sit 或 Lie 状态直接进入蹲下
                SetPosture(E_MovementActions.Squat);
            }
        }

        public void TriggerLie()
        {
            if (_curPosture == E_MovementActions.Stand)
            {
                SetPosture(E_MovementActions.Lie);
            }
            else if (_curPosture == E_MovementActions.Lie)
            {
                SetPosture(E_MovementActions.Stand);
            }
            else
            {
                // 从 Sit 或 Squat 状态直接进入趴下
                SetPosture(E_MovementActions.Lie);
            }
        }

        #endregion


        #region Movement Input

        /// <summary>
        /// 设置基础移动输入（WASD等）
        /// </summary>
        public void TriggerMove(bool isMoving)
        {
            _isMoving = isMoving;
            _animator.SetBool(GetHash(E_MovementActions.Move), isMoving);
        }

        /// <summary>
        /// 设置快速移动输入（Shift）
        /// </summary>
        public void TriggerQuickMove(bool isQuickMoving)
        {
            _isQuickMoving = isQuickMoving;
            _animator.SetBool(GetHash(E_MovementActions.QuickMove), _isQuickMoving);
        }

        /// <summary>
        /// 设置冲刺（仅在站立时生效）
        /// </summary>
        public void TriggerRush(bool isRushing)
        {
            // 仅站立时允许冲刺
            if (_curPosture == E_MovementActions.Stand)
            {
                _isRushing = isRushing;
                _animator.SetBool(GetHash(E_MovementActions.Rush), _isRushing);
            }
            else
            {
                // 非站立时，强制关闭冲刺
                if (isRushing)
                {
                    Debug.LogWarning("Rush is only allowed in Stand posture.");
                }

                _animator.SetBool(GetHash(E_MovementActions.Rush), false);
            }
        }

        #endregion

        #region Jump Control

        /// <summary>
        /// 触发跳跃（仅在站立且未冲刺时允许）
        /// </summary>
        public void TriggerJump()
        {
            if (_curPosture == E_MovementActions.Stand)
            {
                // 起身（如果之前是蹲/趴，但此处 posture 已为 Stand，故无需处理）
                _animator.SetTrigger(GetHash(E_MovementActions.Jumping)); // 建议用 Trigger
                // 如果必须用 bool，则：
                // _animator.SetBool(GetHash(E_MovementActions.Jumping), true);
                // 注意：需在动画事件或 Update 中设回 false
            }
            else
            {
                // 如果处于蹲/趴状态，先起身
                if (_curPosture == E_MovementActions.Squat || _curPosture == E_MovementActions.Lie)
                {
                    SetPosture(E_MovementActions.Stand);
                    _animator.SetTrigger(GetHash(E_MovementActions.Jumping));
                }
            }
        }

        #endregion

        #region Search Interface

        /// <summary>
        /// 获取当前姿态
        /// </summary>
        public E_MovementActions GetCurrentMoveAction()
        {
            // Debug.Log(_curPosture + "," + _isMoving + "," + _isQuickMoving + "," + _isRushing);
            //需要排序优先级 rush > quickMove > move
            switch (_curPosture)
            {
                case E_MovementActions.Sit:
                    return E_MovementActions.Sit;
                case E_MovementActions.Stand:
                    if (_isRushing) return E_MovementActions.Rush;
                    if (_isQuickMoving) return E_MovementActions.Run;
                    if (_isMoving) return E_MovementActions.Walk;
                    return E_MovementActions.Stand;
                case E_MovementActions.Squat:
                    if (_isQuickMoving) return E_MovementActions.SquatQuickMove;
                    if (_isMoving) return E_MovementActions.SquatMove;
                    return E_MovementActions.Squat;
                case E_MovementActions.Lie:
                    if (_isQuickMoving) return E_MovementActions.LieQuickMove;
                    if (_isMoving) return E_MovementActions.LieMove;
                    return E_MovementActions.Lie;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 获取当前是否正在移动
        /// </summary>
        public bool IsMoving() => _isMoving;

        /// <summary>
        /// 获取当前是否正在快速移动
        /// </summary>
        public bool IsQuickMoving() => _isQuickMoving;

        /// <summary>
        /// 获取当前是否正在冲刺
        /// </summary>
        public bool IsRushing() => _isRushing;
        // 注意：不再提供 GetCurrentMovementAction()，因为 Animator 不直接使用 Walk/Run 等参数
        // 如果外部需要知道“当前应播放什么动画”，应由 MovementSystem 根据 posture + input 自行判断

        #endregion


        //todo: 在状态切换时 无法立即切换 而是在当前的Posture中循环 直到停下后才开始切换对应的行为
    }
}