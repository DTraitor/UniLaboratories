using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DataAccessLayer.Providers;

internal class XmlProvider : ISerializationProvider
{
    public XmlProvider(string file)
    {
        stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        serializer = new XmlSerializer(typeof(EntityList));
    }

    public EntityList Read()
    {
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        try
        {
            return (EntityList)serializer.Deserialize(reader.BaseStream);
        }
        catch (InvalidOperationException e)
        {
            return new EntityList();
        }
    }

    public void Write(EntityList entities)
    {
        stream.SetLength(0);
        using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
        serializer.Serialize(writer.BaseStream, entities);
    }

    private FileStream stream;
    private XmlSerializer serializer;
}