namespace Database.Entries.Abilities;

public interface IStudy
{
    public string Study();
}

[DatabaseAbilityDescription("Can study.")]
public class CanStudy : IStudy
{
    public string Study() => "Studied successfully!";
}

[DatabaseAbilityDescription(".")]
public class CanNotStudy : IStudy
{
    public string Study() => "They are unable study!";
}
