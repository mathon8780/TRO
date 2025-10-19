using UnityEngine;

namespace ItemSys
{
    [CreateAssetMenu(menuName = "Items/Behaviors/Container")]
    public class ContainerBehavior : ItemBehavior
    {
        public float maxVolume = 10f;

        // 容器内部库存（可递归！背包里放小包）
        // public Inventory innerInventory;

        public override void Use(Player player, Item item)
        {
            // 打开容器UI
            // UIManager.OpenContainer(innerInventory);
        }
    }
}