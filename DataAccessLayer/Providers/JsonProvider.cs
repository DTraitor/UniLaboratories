using System.Text.Json;

namespace DataAccessLayer.Providers;

internal class JsonProvider<T> : ISerializationProvider<T> where T : class
{
    public JsonProvider(string file)
    {
        stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    public List<T> Read()
    {
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        try
        {
            return JsonSerializer.Deserialize<List<T>>(reader.ReadToEnd());
        }
        catch (JsonException e)
        {
            return new List<T>();
        }
    }

    public void Write(List<T> entities)
    {
        using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
        writer.Write(JsonSerializer.Serialize(entities));
    }

    private FileStream stream;
}