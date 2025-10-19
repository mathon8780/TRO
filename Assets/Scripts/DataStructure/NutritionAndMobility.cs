using UnityEngine;

namespace DataStructure
{
    /// <summary>
    /// 角色的移动数据
    /// </summary>
    [CreateAssetMenu(fileName = "New Nutrition And Mobility Data", menuName = "Data/Nutrition And Mobility Data", order = 0)]
    public class NutritionAndMobility : ScriptableObject
    {
        [Header("玩家属性")]

        #region 玩家属性

        public float Health; //健康

        public float MaxHealth; //最大健康值

        public float Hydration; //水分
        public float MaxHydration; //最大水分含量

        //饱食度由碳水化合物、脂肪、蛋白质共同计算得出
        //饱食度 = 碳水化合物 * 0.5 + 脂肪 * 0.3 + 蛋白质 * 0.2
        public float Satiety; //饱食度
        public float MaxSatiety; //最大饱食度含量

        public float Carbohydrates; //碳水化合物
        public float MaxCarbohydrates; //最大碳水化合物含量

        public float Fat; //脂肪
        public float MaxFat; //最大脂肪含量

        public float Protein; //蛋白质
        public float MaxProtein; //最大蛋白质含量

        public float Energy; //能量
        public float MaxEnergy; //最大能量含量

        #endregion

        [Header("移动属性")]

        #region 移动速度

        public float WalkSpeed; //走路速度

        public float RunSpeed; //跑步速度
        public float RushSpeed; //冲刺速度

        public float SquatMoveSpeed; //蹲下移动速度
        public float SquatQuickMoveSpeed; //蹲下快速移动速度

        public float LieMoveSpeed; //趴下移动速度
        public float LieQuickMoveSpeed; //趴下快速移动速度

        public float JumpHeight; //跳跃高度

        public float RotationSpeed; //转身速度

        #endregion

        [Header("移动消耗属性")]

        #region 移动消耗属性

        public float WalkConsume; //走

        public float RunConsume; //跑
        public float RushConsume; //冲刺

        public float SquatMoveConsume; //蹲下移动
        public float SquatQuickMoveConsume; //蹲下快速移动

        public float LieMoveConsume; //趴下移动
        public float LieQuickMoveConsume; //趴下快速移动

        public float JumpConsume; //跳跃

        #endregion


        [Header("最低移动能量需求")]

        #region 最低移动能量需求

        public float WalkMinEnergyRequire; //走

        public float RunMinEnergyRequire; //跑
        public float RushMinEnergyRequire; //冲刺

        public float SquatMoveMinEnergyRequire; //蹲下移动
        public float SquatQuickMoveMinEnergyRequire; //蹲下快速移动

        public float LieMoveMinEnergyRequire; //趴下移动
        public float LieQuickMoveMinEnergyRequire; //趴下快速移动

        public float JumpMinEnergyRequire; //跳跃

        #endregion

        #region 移动合法性判断

        public bool CanWalk => Energy >= WalkMinEnergyRequire; //是否可以走
        public bool CanRun => Energy >= RunMinEnergyRequire; //是否可以跑
        public bool CanRush => Energy >= RushMinEnergyRequire; //是否可以冲刺
        public bool CanSquatMove => Energy >= SquatMoveMinEnergyRequire; //是否可以蹲下移动
        public bool CanSquatQuickMove => Energy >= SquatQuickMoveMinEnergyRequire; //是否可以蹲下快速移动
        public bool CanLieMove => Energy >= LieMoveMinEnergyRequire; //是否可以趴下移动
        public bool CanLieQuickMove => Energy >= LieQuickMoveMinEnergyRequire; //是否可以趴下快速移动
        public bool CanJump => Energy >= JumpMinEnergyRequire; //是否可以跳跃

        #endregion
    }

    /// <summary>
    /// 玩家属性枚举
    /// </summary>
    public enum E_PlayerProperty
    {
        Health, //健康
        Hydration, //水分
        Satiety, //饱食度
        Carbohydrates, //碳水化合物
        Fat, //脂肪
        Protein, //蛋白质
        Energy //能量
    }
}