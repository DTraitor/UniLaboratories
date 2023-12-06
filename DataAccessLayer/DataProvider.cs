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
                serializationProvider = new JsonProvider(file);
                break;
            case SerializerType.Xml:
                serializationProvider = new XmlProvider(file);
                break;
            case SerializerType.Binary:
                serializationProvider = new BinaryProvider(file);
                break;
            default:
                throw new CustomException("Something went horribly wrong!");
        }
        entityList = serializationProvider.Read();
        entities = entityList.Entities;
    }

    public List<string> GetEntityTypes()
    {
        return Entity.GetPossibleTypes();
    }

    public void CreateEntity(string type)
    {
        entities.Add(Entity.Create(type));
    }

    public List<string> GetEditableData(int index)
    {
        return entities[index].GetEditableData();
    }

    public void SetValue(int index, string data, string value)
    {
        entities[index].SetData(data, value);
    }

    public void DeleteEntity(int index)
    {
        entities.RemoveAt(index);
    }

    public string GetData(int index)
    {
        return entities[index].GetData();
    }

    public string UseAbility(int index, string ability)
    {
        return entities[index].UseAbility(ability);
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
    }

    public int GetEntityCount()
    {
        return entities.Count;
    }

    public void SaveChanges()
    {
        serializationProvider.Write(entityList);
    }

    public int CalculateSpecialTask()
    {
        return entities.Count(e =>
        {
            if (e is Student { Course: 5, Sex: "F", Residence: "Kyiv" })
                return true;
            return false;
        });
    }

    private EntityList entityList;
    private List<Entity> entities;
    private ISerializationProvider serializationProvider;

    public enum SerializerType
    {
        Json,
        Xml,
        Binary
    }
}