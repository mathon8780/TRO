using System;
using System.Collections.Generic;
using CoreComponents.DataRecord;
using DataStructure;
using UnityEngine;
using UnityEngine.Serialization;


namespace CoreComponents
{
    public class DataStorageComponent : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private NutritionAndMobility nutritionAndMobility; // 角色的营养和移动数据ScriptableObject
        [SerializeField] private List<MovementAndConsumption> movementAndConsumptions; // 移动和消耗数据列表

        #endregion

        #region RunTime Data

        private NutritionAndMobility _runTimeNutritionAndMobilityData; // 运行时的营养和移动数据实例
        private MovementCost _movementCost; // 运行时的移动消耗实例

        #endregion

        private void Start()
        {
            #region Initialization Data

            // 初始化营养和移动数据
            if (nutritionAndMobility == null)
                Debug.LogError("NutritionAndMobility ScriptableObject is not assigned in the inspector.");
            else
                _runTimeNutritionAndMobilityData = Instantiate(nutritionAndMobility);

            // 初始化移动和消耗数据
            if (movementAndConsumptions == null || movementAndConsumptions.Count == 0)
                Debug.LogError("MovementAndConsumptions list is empty or not assigned in the inspector.");
            else
                _movementCost = new MovementCost(movementAndConsumptions);

            #endregion
        }


        /// <summary>
        /// 获取营养和移动数据的实例
        /// </summary>
        /// <returns></returns>
        public NutritionAndMobility GetNutritionAndMobilityData()
        {
            if (_runTimeNutritionAndMobilityData == null)
                _runTimeNutritionAndMobilityData = Instantiate(nutritionAndMobility);

            return _runTimeNutritionAndMobilityData;
        }

        public MovementCost GetMovementCostData()
        {
            return _movementCost ??= new MovementCost(movementAndConsumptions);
        }
    }
}