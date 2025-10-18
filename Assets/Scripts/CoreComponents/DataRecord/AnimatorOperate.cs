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

        private E_MovementActions _curMovementAction = E_MovementActions.Idle; //控制当前的移动行为

        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="animator">Animator</param>
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
        public E_MovementActions GetCurrentPosture() => _curPosture;

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

        // public void TriggerJump()
        // {
        //     // 只有在站立状态下才能跳跃
        //     if (_curPosture == E_MovementActions.Stand)
        //     {
        //         _animator.SetTrigger(GetHash(E_MovementActions.Jumping));
        //     }
        //     // 蹲下或趴下时起身
        //     else if (_curPosture == E_MovementActions.Squat || _curPosture == E_MovementActions.Lie)
        //     {
        //         _animator.SetBool(GetHash(E_MovementActions.Squat), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Lie), false);
        //         // 切换到站立状态
        //         _curPosture = E_MovementActions.Stand;
        //         // 设置站立动画参数
        //         _animator.SetBool(GetHash(E_MovementActions.Stand), true);
        //     }
        // }
        //
        // public void TriggerSit()
        // {
        //     UpdateState(E_MovementActions.Sit);
        // }
        //
        // public void TriggerSquat()
        // {
        //     UpdateState(E_MovementActions.Squat);
        // }
        //
        // public void TriggerLie()
        // {
        //     UpdateState(E_MovementActions.Lie);
        // }
        //
        // public void TriggerMove(bool move)
        // {
        //     _animator.SetBool(GetHash(E_MovementActions.Move), move);
        //     UpdateAction(E_MovementActions.Move);
        // }
        //
        // public void TriggerRun(bool run)
        // {
        //     _animator.SetBool(GetHash(E_MovementActions.Move), run);
        //     UpdateAction(E_MovementActions.QuickMove);
        // }
        //
        //
        // public void TriggerRush(bool rush)
        // {
        //     _animator.SetBool(GetHash(E_MovementActions.QuickMove), rush);
        //     UpdateAction(E_MovementActions.Rush);
        // }
        //
        //
        // /// <summary>
        // /// 更新状态
        // /// </summary>
        // /// <param name="newState"></param>
        // private void UpdateState(E_MovementActions newState)
        // {
        //     // 设置newState为true 并设置_curState为newState 其余的状态为false
        //     if (newState == E_MovementActions.Sit)
        //     {
        //         _animator.SetBool(GetHash(E_MovementActions.Stand), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Squat), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Lie), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Sit), true);
        //         _curPosture = E_MovementActions.Sit;
        //     }
        //     else if (newState == E_MovementActions.Stand)
        //     {
        //         _animator.SetBool(GetHash(E_MovementActions.Sit), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Squat), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Lie), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Stand), true);
        //         _curPosture = E_MovementActions.Stand;
        //     }
        //     else if (newState == E_MovementActions.Lie)
        //     {
        //         _animator.SetBool(GetHash(E_MovementActions.Squat), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Stand), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Squat), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Lie), true);
        //         _curPosture = E_MovementActions.Lie;
        //     }
        //     else if (newState == E_MovementActions.Squat)
        //     {
        //         _animator.SetBool(GetHash(E_MovementActions.Sit), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Stand), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Lie), false);
        //         _animator.SetBool(GetHash(E_MovementActions.Squat), true);
        //         _curPosture = E_MovementActions.Squat;
        //     }
        //     else
        //     {
        //         Debug.LogError("Invalid state transition + " + newState);
        //     }
        // }
        //
        // /// <summary>
        // /// 更新行为
        // /// </summary>
        // /// <param name="newAction">新的行为</param>
        // /// <exception cref="ArgumentOutOfRangeException"></exception>
        // private void UpdateAction(E_MovementActions newAction)
        // {
        //     switch (newAction)
        //     {
        //         case E_MovementActions.Idle:
        //             break;
        //         case E_MovementActions.Move:
        //             switch (_curPosture)
        //             {
        //                 case E_MovementActions.Stand:
        //                     _curMovementAction = E_MovementActions.Walk;
        //
        //                     break;
        //                 case E_MovementActions.Squat:
        //                     _curMovementAction = E_MovementActions.SquatMove;
        //                     break;
        //                 case E_MovementActions.Lie:
        //                     _curMovementAction = E_MovementActions.LieMove;
        //                     break;
        //                 default:
        //                     Debug.LogError("Invalid state during Move action: " + _curPosture);
        //                     throw new ArgumentOutOfRangeException();
        //             }
        //
        //             break;
        //         case E_MovementActions.QuickMove:
        //             switch (_curPosture)
        //             {
        //                 case E_MovementActions.Stand: _curMovementAction = E_MovementActions.Run; break;
        //                 case E_MovementActions.Squat: _curMovementAction = E_MovementActions.SquatQuickMove; break;
        //                 case E_MovementActions.Lie: _curMovementAction = E_MovementActions.LieQuickMove; break;
        //                 default:
        //                     Debug.LogError("Invalid state during QuickMove action: " + _curPosture);
        //                     throw new ArgumentOutOfRangeException();
        //             }
        //
        //             break;
        //         case E_MovementActions.Rush:
        //             if (_curPosture == E_MovementActions.Stand)
        //             {
        //                 _curMovementAction = E_MovementActions.Rush;
        //             }
        //
        //             break;
        //         default:
        //         {
        //             Debug.LogError("Invalid action transition + " + newAction);
        //             throw new ArgumentOutOfRangeException();
        //         }
        //     }
        // }
        //
        //
        // public E_MovementActions GetCurrentMovementAction()
        // {
        //     UpdateState(_curPosture);
        //     UpdateAction(_curMovementAction);
        //     return _curMovementAction;
        // }
        //
        // #endregion
    }
}