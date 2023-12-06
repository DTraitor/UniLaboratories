using DataAccessLayer;

namespace BusinessLogicLayer;

public class EntityService
{
    public EntityService(string file, DataProvider.SerializerType serializerType)
    {
        dataProvider = new DataProvider(file, serializerType);
    }

    public int GetEntityCount()
    {
        return dataProvider.GetEntityCount();
    }

    public List<string> GetEntityTypes()
    {
        return dataProvider.GetEntityTypes();
    }

    public void CreateEntity(string type)
    {
        dataProvider.CreateEntity(type);
    }

    public List<string> GetEditableData(int index)
    {
        return dataProvider.GetEditableData(index);
    }

    public void SetValue(int index, string data, string value)
    {
        dataProvider.SetValue(index, data, value);
    }

    public void DeleteEntity(int index)
    {
        dataProvider.DeleteEntity(index);
    }

    public string GetData(int index)
    {
        return dataProvider.GetData(index);
    }

    public string UseAbility(int index, string ability)
    {
        return dataProvider.UseAbility(index, ability);
    }

    public List<string> GetAbilities(int index)
    {
        return dataProvider.GetAbilities(index);
    }

    public List<string> GetAbilityTypes(int index, string ability)
    {
        return dataProvider.GetAbilityTypes(index, ability);
    }

    public void SetAbilityType(int index, string ability, string type)
    {
        dataProvider.SetAbilityType(index, ability, type);
    }

    public void SaveChanges()
    {
        dataProvider.SaveChanges();
    }

    public int CalculateSpecialTask()
    {
        return dataProvider.CalculateSpecialTask();
    }

    private readonly DataProvider dataProvider;
}
