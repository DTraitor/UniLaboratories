using System.Reflection;
using System.Text.Json.Serialization;
using Database.Entries.Abilities;

namespace Database.Entries;

public abstract class Entry
{
    [DatabaseAbilityFunction("Multiply")]
    public string MultiplyAbility() => MultiplyBigNumbers.Multiply();
    
    [DatabaseVariable("First Name")]
    public string FirstName { get; set; }
    [DatabaseVariable("Last Name")]
    public string LastName { get; set; }
    [DatabaseAbilityVariable("Multiply Big Numbers")]
    public IMultiplyBigNumbers MultiplyBigNumbers { get; set; }

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
            result += $"{i}. {methods[i].GetCustomAttribute<DatabaseAbilityFunction>().Name}\n";
        }
        return result;
    }
    
    public PropertyInfo[] GetPossibleVariables()
    {
        return GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DatabaseVariable))).ToArray();
    }

    public override string ToString()
    {
        string result = $"{GetType()} {PositionDB}:\n";
        foreach (PropertyInfo property in GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DatabaseVariable))))
        {
            result += $"{property.GetCustomAttribute<DatabaseVariable>().Name}: {property.GetValue(this)}\n";
        }
        return result;
    }

    public string GetSmallString()
    {
        return $"{PositionDB}. {GetType()} | {FirstName} | {LastName}";
    }
    
    // Position in the database. Tracked by the user
    [JsonIgnore]
    public uint PositionDB { get; set; }
}
