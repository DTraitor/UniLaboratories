using System.Runtime.Serialization;
using DataAccessLayer.Abilities;

namespace DataAccessLayer.Entities;

internal class Entity : ISerializable
{
    public Entity() { }

    public Entity(SerializationInfo info, StreamingContext context)
    {
        name = (string)info.GetValue("Name", typeof(string));
        surname = (string)info.GetValue("Surname", typeof(string));
        var type = (string)info.GetValue("BigNumbers", typeof(string));
        var typeToSet = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(ICalculateBigNumbers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .FirstOrDefault(x => x.Name == type);
        if (typeToSet == null)
            throw new CustomException("This type does not exist!");
        BigNumbers = (ICalculateBigNumbers)Activator.CreateInstance(typeToSet);
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Name", name);
        info.AddValue("Surname", surname);
        info.AddValue("BigNumbers", BigNumbers.GetType().Name);
    }

    public static List<string> GetPossibleTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(Entity).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(x => x.Name)
            .ToList();
    }

    public static Entity Create(string type)
    {
        var typeToCreate = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(Entity).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .FirstOrDefault(x => x.Name == type);
        if (typeToCreate == null)
            throw new CustomException("This type does not exist!");
        return (Entity)Activator.CreateInstance(typeToCreate);
    }

    public virtual string GetData()
    {
        return $"Name: {name}\nSurname: {surname}\n";
    }

    public virtual List<string> GetEditableData()
    {
        return new List<string> { "Name", "Surname" };
    }

    public virtual void SetData(string data, string value)
    {
        switch (data)
        {
            case "Name":
                name = value;
                return;
            case "Surname":
                surname = value;
                return;
        }
    }

    public virtual List<string> GetAbilities()
    {
        return new List<string>() { "Calculate Big Numbers" };
    }

    public virtual string UseAbility(string ability)
    {
        switch (ability)
        {
            case "Calculate Big Numbers":
                return BigNumbers.Calculate();
        }
        throw new CustomException("This ability does not exist!");
    }

    public virtual List<string> GetAbilityTypes(string ability)
    {
        switch (ability)
        {
            case "Calculate Big Numbers":
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => typeof(ICalculateBigNumbers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(x => x.Name)
                    .ToList();
        }
        throw new CustomException("This ability does not exist!");
    }

    public virtual void SetAbilityType(string ability, string type)
    {
        switch (ability)
        {
            case "Calculate Big Numbers":
                var typeToSet = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => typeof(ICalculateBigNumbers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .FirstOrDefault(x => x.Name == type);
                if (typeToSet == null)
                    throw new CustomException("This type does not exist!");
                BigNumbers = (ICalculateBigNumbers)Activator.CreateInstance(typeToSet);
                break;
        }
        throw new CustomException("This ability does not exist!");
    }

    private string name;
    private string surname;
    private ICalculateBigNumbers BigNumbers;
}