using System.Collections.Generic;

namespace Inventory.Save
{
    public interface ISerializableProperty
    {
        void SetPropertyData(string propertyName, object propertyValue);
        Dictionary<string, object> SerializeState();
        void DeserializeState(Dictionary<string, object> serializedData);
    }
}