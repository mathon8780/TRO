using UnityEngine;

namespace ItemSys
{
    [CreateAssetMenu(fileName = "New Item Behavior", menuName = "Items", order = 0)]
    public abstract class ItemBehavior : ScriptableObject
    {
        // 是否可使用（例如：食物可吃，但石头不能）
        public virtual bool CanUse(Player player) => true;

        // 使用逻辑
        public abstract void Use(Player player, Item item);

        // 可选：是否可装备（如武器、衣服）
        public virtual bool CanEquip() => false;

        public virtual void OnEquip(Player player, Item item)
        {
        }

        public virtual void OnUnequip(Player player, Item item)
        {
        }

        // 可选：是否可组合（如打火机 + 汽油 = 燃烧瓶）
        public virtual bool CanCombineWith(Item other) => false;
        public virtual Item Combine(Item other) => null;
    }

    public class Player
    {
    }
}