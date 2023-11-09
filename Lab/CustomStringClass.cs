namespace Console;

public class CustomStringClass  : IComparable<CustomStringClass>
{
    public int Length { get => Value.Length; }
    public string Value { get; private set; }
    private sbyte _key;
    
    public CustomStringClass(string value, sbyte key)
    {
        Value = value;
        _key = key;
    }
    
    public void Encrypt()
    {
        var encryptedValue = "";
        foreach (var character in Value)
        {
            encryptedValue += (char)(character + _key);
        }
        Value = encryptedValue;
    }
    
    public void Decrypt()
    {
        var decryptedValue = "";
        foreach (var character in Value)
        {
            decryptedValue += (char)(character - _key);
        }
        Value = decryptedValue;
    }
    
    public int CompareTo(CustomStringClass? other)
    {
        if (other == null)
        {
            return 1;
        }
        return _key.CompareTo(other._key);
    }
}