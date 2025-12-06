using Inventory.Property;

namespace Inventory.PropertyData
{
    [System.Serializable]
    public class ItemContainerPropertyData : ItemPropertyData
    {
        public float Capacity; //容量

        public override ItemProperty CreateProperty()
        {
            return new ItemContainerProperty(Capacity);
        }
    }
}