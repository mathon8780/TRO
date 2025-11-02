using Inventory.Property;

namespace Inventory.PropertyData
{
    [System.Serializable]
    public class EatablePropertyData : ItemPropertyData
    {
        public float Cab;
        public float Fat;
        public float Protein;
        public float Water;
        public float Energy;

        public override ItemProperty CreateProperty()
        {
            return new EatableProperty(Cab, Fat, Protein, Water, Energy);
        }
    }
}