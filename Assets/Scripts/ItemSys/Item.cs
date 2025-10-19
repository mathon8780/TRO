using UnityEngine;

namespace ItemSys
{
    [CreateAssetMenu(menuName = "Items/Item Data")]
    public class Item : ScriptableObject
    {
        public string itemId; // 唯一ID，如 "banana"
        public string itemName;
        public string description;
        public Sprite icon;
        public float weight;
        public float volume;
        public int maxStack = 1;

        public float maxDurability = 100f;

        // 行为组件（ScriptableObject 引用）
        public ItemBehavior[] behaviors;
    }
}