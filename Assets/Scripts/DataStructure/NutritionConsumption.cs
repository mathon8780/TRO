using UnityEngine;

namespace DataStructure
{
    /// <summary>
    /// 营养消耗数据
    /// </summary>
    [CreateAssetMenu(fileName = "New Nutritional Consumption", menuName = "Data/Nutritional Consumption", order = 1)]
    public class NutritionConsumption : ScriptableObject
    {
        [Header("每秒消耗量")] [Header("基础物质消耗量")] public float DailyCarbohydratesConsumption; //基础碳水化合物消耗
        public float DailyFatConsumption; //基础脂肪消耗
        public float DailyProteinConsumption; //基础蛋白质消耗
        // public float DailyHydrationConsumption; //基础水分消耗


        [Header("运动时的消耗量")] public float MovementCarbohydratesQuantity; //运动时碳水化合物消耗倍率
        public float MovementFatQuantity; //运动时脂肪消耗倍率
        public float MovementProteinQuantity; //运动时蛋白质消耗倍率
        // public float MovementHydrationQuantity; //运动时水分消耗倍率


        // [Header("运动时的能量消耗比例")] public float CarbohydrateConsumeRate; //碳水化合物能量比例
        // public float FatConsumeRate; //脂肪能量比例
        // public float ProteinConsumeRate; //蛋白质能量比例

        //todo: 不同含量时对不同的物质有不同的消耗比例 用于平衡物质消耗

        [Header("物质转化为能量的量")] public float CarbohydrateConversion; //碳水化合物转化量
        public float FatConversion; //脂肪转化量
        public float ProteinConversion; //蛋白质转化量


        //消耗公式 = 消耗的量 * （基础转化率 70% + 体魄等级修正 0%-30%） * 转化量
        //消耗的量：当前状态下的物质消耗量
        //基础转化率：当前状态下每单位物质转化为能量的比例
        //体魄等级修正：体魄等级对基础转化率的影响系数
        //转化量：每单位物质转化为能量的量
    }
}