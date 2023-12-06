using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using DataAccessLayer.Abilities;

namespace DataAccessLayer.Entities;

[JsonDerivedType(typeof(Photographer), "Photographer")]
[JsonDerivedType(typeof(Joiner), "Joiner")]
[JsonDerivedType(typeof(Student), "Student")]
[Serializable]
[XmlInclude(typeof(Photographer))]
[XmlInclude(typeof(Joiner))]
[XmlInclude(typeof(Student))]
public class Entity : ISerializable, IXmlSerializable
{
    public Entity() { }

    public Entity(SerializationInfo info, StreamingContext context)
    {
        Name = (string)info.GetValue("Name", typeof(string));
        Surname = (string)info.GetValue("Surname", typeof(string));
        var type = (string)info.GetValue("BigNumbers", typeof(string));
        var typeToSet = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(ICalculateBigNumbers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .FirstOrDefault(x => x.Name == type);
        if (typeToSet == null)
            throw new CustomException("This type does not exist!");
        BigNumbers = (ICalculateBigNumbers)Activator.CreateInstance(typeToSet);
    }

    public virtual void ReadXml(XmlReader reader)
    {
        if(!reader.HasAttributes)
            throw new CustomException("Something went horribly wrong!");
        if (reader.MoveToAttribute("Name") && reader.ReadAttributeValue())
            Name = reader.Value;
        if (reader.MoveToAttribute("Surname") && reader.ReadAttributeValue())
            Surname = reader.Value;
        if (reader.MoveToAttribute("BigNumbers") && reader.ReadAttributeValue())
        {
            var typeToSet = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(ICalculateBigNumbers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList()
                .FirstOrDefault(x => x.Name == reader.Value);
            if (typeToSet == null)
                throw new CustomException("This type does not exist!");
            BigNumbers = (ICalculateBigNumbers)Activator.CreateInstance(typeToSet);
        }
    }

    public virtual void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("Name", Name);
        writer.WriteAttributeString("Surname", Surname);
        writer.WriteAttributeString("BigNumbers", BigNumbers.GetType().Name);
    }

    public System.Xml.Schema.XmlSchema? GetSchema()
    {
        return null;
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Name", Name);
        info.AddValue("Surname", Surname);
        info.AddValue("BigNumbers", BigNumbers.GetType().FullName);
    }

    public static List<string> GetPossibleTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(Entity).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
            .Select(x => x.Name)
            .ToList();
    }

    public static Entity Create(string type)
    {
        var typeToCreate = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(Entity).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
            .FirstOrDefault(x => x.Name == type);
        if (typeToCreate == null)
            throw new CustomException("This type does not exist!");
        return (Entity)Activator.CreateInstance(typeToCreate);
    }

    public virtual string GetData()
    {
        return $"Name: {Name}\nSurname: {Surname}\n";
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
                Name = value;
                return;
            case "Surname":
                Surname = value;
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
                return;
        }
        throw new CustomException("This ability does not exist!");
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    public ICalculateBigNumbers BigNumbers { get; set; }
}