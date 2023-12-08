using DataAccessLayer;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer;

public class EntityService
{
    public EntityService(string file)
    {
        dataProvider = new DataProvider(file);
        dataProvider.Read();
    }

    public List<string> GetEntityTypes()
    {
        return Entity.GetPossibleTypes();
    }

    public void CreateEntity(string type)
    {
        dataProvider.Entities.Add(Entity.Create(type));
    }

    public List<string> GetEditableData(int index)
    {
        return dataProvider.Entities[index].GetEditableData();
    }

    public void SetValue(int index, string data, string value)
    {
        dataProvider.Entities[index].SetData(data, value);
    }

    public void DeleteEntity(int index)
    {
        dataProvider.Entities.RemoveAt(index);
    }

    public string GetData(int index)
    {
        return dataProvider.Entities[index].GetData();
    }

    public string UseAbility(int index, string ability)
    {
        return dataProvider.Entities[index].UseAbility(ability);
    }

    public List<string> GetAbilities(int index)
    {
        return dataProvider.Entities[index].GetAbilities();
    }

    public List<string> GetAbilityTypes(int index, string ability)
    {
        return dataProvider.Entities[index].GetAbilityTypes(ability);
    }

    public void SetAbilityType(int index, string ability, string type)
    {
        dataProvider.Entities[index].SetAbilityType(ability, type);
    }

    public int GetEntityCount()
    {
        return dataProvider.Entities.Count;
    }
    public int CalculateSpecialTask()
    {
        return dataProvider.Entities.Count(e =>
        {
            if (e is Student { Course: 5, Sex: "F", Residence: "Kyiv" })
                return true;
            return false;
        });
    }

    public void Close()
    {
        dataProvider.Close();
    }

    public void SaveChanges()
    {
        dataProvider.Save();
    }

    private readonly DataProvider dataProvider;
}
