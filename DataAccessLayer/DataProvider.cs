using DataAccessLayer.Entities;
using DataAccessLayer.Providers;

namespace DataAccessLayer;

public class DataProvider
{
    public DataProvider(string file, SerializerType serializerType)
    {
        switch (serializerType)
        {
            case SerializerType.Json:
                serializationProvider = new JsonProvider<Entity>(file);
                break;
            case SerializerType.Xml:
                serializationProvider = new XmlProvider<Entity>(file);
                break;
            case SerializerType.Binary:
                serializationProvider = new BinaryProvider<Entity>(file);
                break;
            default:
                throw new CustomException("Something went horribly wrong!");
        }
        entities = serializationProvider.Read();
    }

    public List<string> GetEntityTypes()
    {
        return Entity.GetPossibleTypes();
    }

    public void CreateEntity(string type)
    {
        entities.Add(Entity.Create(type));
        serializationProvider.Write(entities);
    }

    public List<string> GetEditableData(int index)
    {
        return entities[index].GetEditableData();
    }

    public void SetValue(int index, string data, string value)
    {
        entities[index].SetData(data, value);
        serializationProvider.Write(entities);
    }

    public void DeleteEntity(int index)
    {
        entities.RemoveAt(index);
        serializationProvider.Write(entities);
    }

    public string GetData(int index)
    {
        return entities[index].GetData();
    }

    public string UseAbility(int index, string ability)
    {
        string s = entities[index].UseAbility(ability);
        serializationProvider.Write(entities);
        return s;
    }

    public List<string> GetAbilities(int index)
    {
        return entities[index].GetAbilities();
    }

    public List<string> GetAbilityTypes(int index, string ability)
    {
        return entities[index].GetAbilityTypes(ability);
    }

    public void SetAbilityType(int index, string ability, string type)
    {
        entities[index].SetAbilityType(ability, type);
        serializationProvider.Write(entities);
    }

    public int GetEntityCount()
    {
        return entities.Count;
    }

    private List<Entity> entities;
    private ISerializationProvider<Entity> serializationProvider;

    public enum SerializerType
    {
        Json,
        Xml,
        Binary
    }
}