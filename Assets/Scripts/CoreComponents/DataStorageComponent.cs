using System.Collections.Generic;
using CoreComponents.DataRecord;
using CoreListener;
using DataStructure;
using Inventory;
using UI.EventType;
using UnityEngine;


namespace CoreComponents
{
    /// <summary>
    /// 数据管理组件
    /// </summary>
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

        private void Update()
        {
            UpdateNutritionData();
        }


        #region UpdateInfo

        private void UpdateNutritionData()
        {
            EveStateInfo eveStateInfo = new EveStateInfo();

            eveStateInfo.State = E_PlayerProperty.Health;
            eveStateInfo.CurrentValue = _runTimeNutritionAndMobilityData.Health;
            eveStateInfo.MaxValue = _runTimeNutritionAndMobilityData.MaxHealth;
            EventCenter.Instance?.TriggerEvent<EveStateInfo>(eveStateInfo);
            eveStateInfo.State = E_PlayerProperty.Hydration;
            eveStateInfo.CurrentValue = _runTimeNutritionAndMobilityData.Hydration;
            eveStateInfo.MaxValue = _runTimeNutritionAndMobilityData.MaxHydration;
            EventCenter.Instance?.TriggerEvent<EveStateInfo>(eveStateInfo);
            //饱食度 = 碳水化合物 * 0.5 + 脂肪 * 0.3 + 蛋白质 * 0.2
            eveStateInfo.State = E_PlayerProperty.Satiety;
            eveStateInfo.CurrentValue = _runTimeNutritionAndMobilityData.Carbohydrates * 0.5f +
                                        _runTimeNutritionAndMobilityData.Fat * 0.3f +
                                        _runTimeNutritionAndMobilityData.Protein * 0.2f;
            eveStateInfo.MaxValue = _runTimeNutritionAndMobilityData.MaxSatiety;
            EventCenter.Instance?.TriggerEvent<EveStateInfo>(eveStateInfo);
            eveStateInfo.State = E_PlayerProperty.Carbohydrates;
            eveStateInfo.CurrentValue = _runTimeNutritionAndMobilityData.Carbohydrates;
            eveStateInfo.MaxValue = _runTimeNutritionAndMobilityData.MaxCarbohydrates;
            EventCenter.Instance?.TriggerEvent<EveStateInfo>(eveStateInfo);
            eveStateInfo.State = E_PlayerProperty.Fat;
            eveStateInfo.CurrentValue = _runTimeNutritionAndMobilityData.Fat;
            eveStateInfo.MaxValue = _runTimeNutritionAndMobilityData.MaxFat;
            EventCenter.Instance?.TriggerEvent<EveStateInfo>(eveStateInfo);
            eveStateInfo.State = E_PlayerProperty.Protein;
            eveStateInfo.CurrentValue = _runTimeNutritionAndMobilityData.Protein;
            eveStateInfo.MaxValue = _runTimeNutritionAndMobilityData.MaxProtein;
            EventCenter.Instance?.TriggerEvent<EveStateInfo>(eveStateInfo);
            eveStateInfo.State = E_PlayerProperty.Energy;
            eveStateInfo.CurrentValue = _runTimeNutritionAndMobilityData.Energy;
            eveStateInfo.MaxValue = _runTimeNutritionAndMobilityData.MaxEnergy;
            EventCenter.Instance?.TriggerEvent<EveStateInfo>(eveStateInfo);
        }

        #endregion

        #region GetInfo

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

        #endregion
    }
}