namespace Database.Entries.Abilities;

public interface IFixTable
{
    public string FixTable();
}

[DatabaseAbilityDescription("Can fix tables.")]
public class CanFixTable
{
    public string FixTable() => "They fixed a table.";
}

[DatabaseAbilityDescription("Can't fix tables.")]
public class CanNotFixTable
{
    public string FixTable() => "They are unable o fix tables.";
}

