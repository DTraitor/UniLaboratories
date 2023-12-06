using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataAccessLayer.Providers;

#pragma warning disable SYSLIB0011
internal class BinaryProvider<T> : ISerializationProvider<T> where T : class
{
    public BinaryProvider(string file)
    {
        stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    public List<T> Read()
    {
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        try
        {
            return (List<T>)new BinaryFormatter().Deserialize(reader.BaseStream);
        }
        catch (SerializationException e)
        {
            return new List<T>();
        }
    }

    public void Write(List<T> entities)
    {
        using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
        new BinaryFormatter().Serialize(writer.BaseStream, entities);
    }

    private FileStream stream;
}