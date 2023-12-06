using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataAccessLayer.Providers;

#pragma warning disable SYSLIB0011
internal class BinaryProvider : ISerializationProvider
{
    public BinaryProvider(string file)
    {
        stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    public EntityList Read()
    {
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        try
        {
            return (EntityList)new BinaryFormatter().Deserialize(reader.BaseStream);
        }
        catch (SerializationException e)
        {
            return new EntityList();
        }
    }

    public void Write(EntityList entities)
    {
        stream.SetLength(0);
        using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
        new BinaryFormatter().Serialize(writer.BaseStream, entities);
    }

    private FileStream stream;
}