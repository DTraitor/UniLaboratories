using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DataAccessLayer.Providers;

internal class XmlProvider<T> : ISerializationProvider<T> where T : class
{
    public XmlProvider(string file)
    {
        stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    public List<T> Read()
    {
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        try
        {
            return (List<T>)new XmlSerializer(typeof(List<T>)).Deserialize(reader.BaseStream);
        }
        catch (SerializationException e)
        {
            return new List<T>();
        }
    }

    public void Write(List<T> entities)
    {
        using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
        new XmlSerializer(typeof(List<T>)).Serialize(writer.BaseStream, entities);
    }

    private FileStream stream;
}