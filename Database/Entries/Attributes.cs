using System.ComponentModel;

namespace Database.Entries.Abilities;

[AttributeUsage(AttributeTargets.Property)]
public class DatabaseVariable : Attribute
{
    public DatabaseVariable(string name)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
}

[AttributeUsage(AttributeTargets.Method)]
public class DatabaseAbilityFunction : Attribute
{
    public DatabaseAbilityFunction(string name)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class DatabaseAbilityVariable : Attribute
{
    public DatabaseAbilityVariable(string name)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
}

[AttributeUsage(AttributeTargets.Class)]
public class DatabaseAbilityDescription : Attribute
{
    public DatabaseAbilityDescription(string name)
    {
        Name = name;
    }
    
    public string Name { get; private set; }
}

