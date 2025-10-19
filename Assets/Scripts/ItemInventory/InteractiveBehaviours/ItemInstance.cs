using System.Collections.Generic;

namespace ItemInventory.InteractiveBehaviours
{
    /// <summary>
    /// 存储所有物品的实例属性
    /// </summary>
    public class ItemInstance
    {
        public int StackNum; //堆叠数量

        #region 食物

        public float Carbohydrates; // 碳水化合物
        public float Fat; // 脂肪
        public float Protein; // 蛋白质
        public float Water; // 水分

        #endregion

        #region 容器

        public float OccupiedCapacity; // 当前占用的容量
        public float MaxCapacity; // 最大的容量
        public bool IsDirty; //数据已脏 有存取操作未重新计算占用容量
        public bool CanStore => OccupiedCapacity < MaxCapacity;
        public HashSet<int> ContainedItemIds;
        public List<Item> ContainedItems; // 物品的容器内的物品列表

        #endregion

        #region Attackable

        #endregion
    }
}