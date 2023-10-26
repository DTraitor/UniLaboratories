using System.Text.Json.Serialization;

namespace Database.Entries.Abilities;

[JsonDerivedType(typeof(CanMultiply), "canMultiply")]
[JsonDerivedType(typeof(CanNotMultiply), "cantMultiply")]
public abstract class MultiplyBigNumbers : EntryAbility
{
    public abstract string Multiply();
}

[DatabaseAbilityDescription("Can multiply big numbers.")]
public class CanMultiply : MultiplyBigNumbers
{
    public override string Multiply() => "They know how to multiply big numbers.";
}

[DatabaseAbilityDescription("Can't multiply big numbers.")]
public class CanNotMultiply : MultiplyBigNumbers
{
    public override string Multiply() => "They don't know how to multiply big numbers.";
}

