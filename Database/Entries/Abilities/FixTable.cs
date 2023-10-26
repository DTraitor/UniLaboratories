using System.Text.Json.Serialization;

namespace Database.Entries.Abilities;

[JsonDerivedType(typeof(CanFixTable), "canFixTable")]
[JsonDerivedType(typeof(CanNotFixTable), "cantFixTable")]
public abstract class FixTableAbility : EntryAbility
{
    public abstract string FixTable();
}

[DatabaseAbilityDescription("Can fix tables.")]
public class CanFixTable : FixTableAbility
{
    public override string FixTable() => "They fixed a table.";
}

[DatabaseAbilityDescription("Can't fix tables.")]
public class CanNotFixTable : FixTableAbility
{
    public override string FixTable() => "They are unable o fix tables.";
}

