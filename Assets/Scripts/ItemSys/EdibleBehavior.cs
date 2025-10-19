using UnityEngine;

namespace ItemSys
{
    [CreateAssetMenu(menuName = "Items/Behaviors/Edible")]
    public class EdibleBehavior : ItemBehavior
    {
        public int hungerRestore = 10;
        public int thirstRestore = 0;
        public bool isPoisonous = false;

        public override void Use(Player player, Item item)
        {
        }
    }
}