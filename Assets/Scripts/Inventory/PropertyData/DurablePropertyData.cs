using Inventory.Property;

namespace Inventory.PropertyData
{
    [System.Serializable]
    public class DurablePropertyData : ItemPropertyData
    {
        public float MaxDurability; // 最大耐久度

        public override ItemProperty CreateProperty()
        {
            return new DurableProperty(MaxDurability);
        }
    }
}