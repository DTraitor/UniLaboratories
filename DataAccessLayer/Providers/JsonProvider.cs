using System.Text.Json;

namespace DataAccessLayer.Providers;

internal class JsonProvider : ISerializationProvider
{
    public JsonProvider(string file)
    {
        stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    public EntityList Read()
    {
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        try
        {
            return JsonSerializer.Deserialize<EntityList>(reader.ReadToEnd(), new JsonSerializerOptions());
        }
        catch (JsonException e)
        {
            return new EntityList();
        }
    }

    public void Write(EntityList entities)
    {
        stream.SetLength(0);
        using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
        writer.Write(JsonSerializer.Serialize(entities));
    }

    private FileStream stream;
}