using Inventory.Property;

namespace Inventory.PropertyData
{
    [System.Serializable]
    public abstract class ItemPropertyData
    {
        public abstract ItemProperty CreateProperty();
    }
}