using UnityEngine;
using UnityEngine.Serialization;

namespace DataStructure
{
    [CreateAssetMenu(fileName = "New Movement And Consumption", menuName = "Data/Movement And Consumption", order = 0)]
    public class MovementAndConsumption : ScriptableObject
    {
        public E_MovementActions movementActions; //移动状态
        public float movementSpeed; //移动速度
        public float energyConsumptionPreSecond; //能量每秒消耗量
        public float hydrationConsumptionPreSecond; //水分每秒消耗量
        public float minimumEnergyConstraint; //最低能量约束
    }


    /// <summary>
    /// 玩家状态枚举
    /// </summary>
    public enum E_MovementActions
    {
        // 状态
        Idle, //静止
        Sit, //坐下
        Stand, //站立
        Squat, //蹲下
        Lie, //趴下

        // Animator需求

        Move, //移动 有输入
        QuickMove, //快速移动 Shift输入


        // 行为
        Walk, //走路
        Run, //跑步

        Rush, //冲刺 单独控制

        SquatMove, //蹲下移动
        SquatQuickMove, //蹲下快速移动

        LieMove, //趴下移动
        LieQuickMove, //趴下快速移动

        Jumping, //跳跃中
        Falling, //下落中
    }
}