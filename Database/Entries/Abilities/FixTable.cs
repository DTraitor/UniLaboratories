using System.Text.Json.Serialization;

namespace Database.Entries.Abilities;

[JsonDerivedType(typeof(CanFixTable), "canFixTable")]
[JsonDerivedType(typeof(CanNotFixTable), "cantFixTable")]
public interface IFixTable : IAbility
{
    public string FixTable();
}

[DatabaseAbilityDescription("Can fix tables.")]
public class CanFixTable : IFixTable
{
    public string FixTable() => "They fixed a table.";
}

[DatabaseAbilityDescription("Can't fix tables.")]
public class CanNotFixTable : IFixTable
{
    public string FixTable() => "They are unable o fix tables.";
}

