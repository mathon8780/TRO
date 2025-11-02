using Inventory.Property;

namespace Inventory.PropertyData
{
    [System.Serializable]
    public class ItemContainerPropertyData : ItemPropertyData
    {
        public float Capacity;
        public float EncumbranceReduceRate;

        public override ItemProperty CreateProperty()
        {
            return new ItemContainerProperty(Capacity, EncumbranceReduceRate);
        }
    }
}