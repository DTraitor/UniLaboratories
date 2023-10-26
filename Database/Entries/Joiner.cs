using Database.Entries.Abilities;

namespace Database.Entries;

public class Joiner : Entry
{
    [DatabaseAbilityFunction("Fix Table")]
    public string FixTableAbility() => FixTable.FixTable();
    [DatabaseAbilityVariable("Fix Table")]
    public FixTableAbility FixTable { get; set; }
}