using System.Text.Json.Serialization;

namespace Database.Entries.Abilities;

[JsonDerivedType(typeof(CanStudy), "canStudy")]
[JsonDerivedType(typeof(CanNotStudy), "cantStudy")]
public abstract class StudyAbility : EntryAbility
{
    public abstract string Study();
}

[DatabaseAbilityDescription("Can study.")]
public class CanStudy : StudyAbility
{
    public override string Study() => "Studied successfully!";
}

[DatabaseAbilityDescription(".")]
public class CanNotStudy : StudyAbility
{
    public override string Study() => "They are unable study!";
}
