using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace ConsoleMenu;

[Serializable]
public class CustomString : ISerializable
{
    public CustomString(string value, int key)
    {
        Value = value;
        Key = key;
    }

    public CustomString(SerializationInfo info, StreamingContext context)
    {
        Value = info.GetString("Value");
        Key = info.GetInt32("Key");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Value", Value);
        info.AddValue("Key", Key);
    }

    public void Encrypt()
    {
        var encrypted = new StringBuilder();
        foreach (var c in Value)
        {
            encrypted.Append((char)(c + Key));
        }
        Value = encrypted.ToString();
    }

    public void Decrypt()
    {
        var decrypted = new StringBuilder();
        foreach (var c in Value)
        {
            decrypted.Append((char)(c - Key));
        }
        Value = decrypted.ToString();
    }

    public string Value { get; set; }
    [JsonIgnore, XmlIgnore]
    public int Length => Value.Length;
    public int Key { get; set; }
}