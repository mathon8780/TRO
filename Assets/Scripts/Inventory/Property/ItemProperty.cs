namespace Inventory.Property
{
    public abstract class ItemProperty
    {
        protected Item owner;

        public virtual void Initialize(Item owner)
        {
            this.owner = owner;
        }
    }
}