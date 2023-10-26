using System.Reflection;
using System.Text.Json.Serialization;
using Database.Entries.Abilities;

namespace Database.Entries;

public abstract class Entry
{
    [DatabaseAbilityFunction("Multiply")]
    public string MultiplyAbility() => MultiplyBigNumbers.Multiply();
    
    [DatabaseVariable("Last Name")]
    public string LastName { get; set; }
    [DatabaseVariable("First Name")]
    public string FirstName { get; set; }
    [DatabaseAbilityVariable("Multiply Big Numbers")]
    public MultiplyBigNumbers MultiplyBigNumbers { get; set; }

    public MethodInfo[] GetPossibleFunctions()
    {
        return GetType().GetMethods().Where(method => Attribute.IsDefined(method, typeof(DatabaseAbilityFunction))).ToArray();
    }
    
    public string GetPossibleFunctionsAsString()
    {
        MethodInfo[] methods = GetType().GetMethods().Where(method => Attribute.IsDefined(method, typeof(DatabaseAbilityFunction))).ToArray();
        string result = "";
        for (int i = 0; i < methods.Length; i++)
        {
            result += $"\n{methods[i].GetCustomAttribute<DatabaseAbilityFunction>().Name}";
        }
        return result;
    }
    
    public PropertyInfo[] GetPossibleVariables()
    {
        return GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DatabaseVariable))).ToArray();
    }

    public PropertyInfo[] GetPossibleAbilityVariables()
    {
        return GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DatabaseAbilityVariable))).ToArray();
    }

    public string GetPossibleAbilitiesAsString()
    {
        return "";
    }
    
    public override string ToString()
    {
        string result = $"Type: {GetType().Name}\nPosition: {PositionDB}";
        foreach (PropertyInfo property in GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DatabaseVariable))).Reverse())
        {
            result += $"\n{property.GetCustomAttribute<DatabaseVariable>().Name}: {property.GetValue(this)}";
        }
        return result;
    }

    public string GetSmallString()
    {
        return $"{PositionDB}. {GetType().Name} | {FirstName} | {LastName}";
    }
    
    // Position in the database. Tracked by the user
    [JsonIgnore]
    public uint PositionDB { get; set; }
}
