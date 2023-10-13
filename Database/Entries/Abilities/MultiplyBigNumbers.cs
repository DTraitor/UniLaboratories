namespace Database.Entries.Abilities;

public interface IMultiplyBigNumbers
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

