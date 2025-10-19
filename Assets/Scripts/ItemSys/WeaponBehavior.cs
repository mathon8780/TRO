using UnityEngine;

namespace ItemSys
{
    [CreateAssetMenu(menuName = "Items/Behaviors/Weapon")]
    public class WeaponBehavior : ItemBehavior
    {
        public float damage = 20f;
        public float attackSpeed = 1f; // 秒
        public bool isRanged = false;

        public override bool CanEquip() => true;

        public override void OnEquip(Player player, Item item)
        {
            // player.WeaponSlot = item;
            // player.AttackDamage = damage;
        }

        public override void OnUnequip(Player player, Item item)
        {
            // if (player.WeaponSlot == item)
            //     player.WeaponSlot = null;
        }

        public override void Use(Player player, Item item)
        {
            // 攻击逻辑（需结合输入系统）
            // player.Attack();
        }
    }
}