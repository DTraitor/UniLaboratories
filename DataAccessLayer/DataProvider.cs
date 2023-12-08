using System.Text.Json;
using DataAccessLayer.Entities;

namespace DataAccessLayer;

public class DataProvider
{
    public DataProvider(string file)
    {
        stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
    }

    public void Read()
    {
        stream.Position = 0;
        using StreamReader reader = new StreamReader(stream, leaveOpen: true);
        try
        {
            Entities = JsonSerializer.Deserialize<List<Entity>>(reader.ReadToEnd());
        }
        catch (JsonException e)
        {
            Entities = new List<Entity>();
        }
    }

    public void Save()
    {
        stream.SetLength(0);
        using StreamWriter writer = new StreamWriter(stream, leaveOpen: true);
        writer.Write(JsonSerializer.Serialize(Entities));
    }

    public void Close()
    {
        stream.Close();
    }

    public List<Entity> Entities;
    public FileStream stream;
}