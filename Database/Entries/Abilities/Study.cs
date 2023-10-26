using System.Text.Json.Serialization;

namespace Database.Entries.Abilities;

[JsonDerivedType(typeof(CanStudy), "canStudy")]
[JsonDerivedType(typeof(CanNotStudy), "cantStudy")]
public interface IStudy : IAbility
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
