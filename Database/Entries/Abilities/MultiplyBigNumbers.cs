using System.Text.Json.Serialization;

namespace Database.Entries.Abilities;

[JsonDerivedType(typeof(CanMultiply), "canMultiply")]
[JsonDerivedType(typeof(CanNotMultiply), "cantMultiply")]
public interface IMultiplyBigNumbers : IAbility
{
    public string Multiply();
}

[DatabaseAbilityDescription("Can multiply big numbers.")]
public class CanMultiply : IMultiplyBigNumbers
{
    public string Multiply() => "They know how to multiply big numbers.";
}

[DatabaseAbilityDescription("Can't multiply big numbers.")]
public class CanNotMultiply : IMultiplyBigNumbers
{
    public string Multiply() => "They don't know how to multiply big numbers.";
}

