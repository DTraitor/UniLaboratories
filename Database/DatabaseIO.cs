using System.Text.Json;
using Database.Entries;

namespace Database;

public class DatabaseIO
{
    public DatabaseIO(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Database file doesn't exist.");
        this.path = Path.GetFullPath(path);
    }

    public Entry[] ReadData()
    {
        string[] lines = File.ReadAllLines(path);
        Entry[] entries = new Entry[lines.Length / 2];
        
        for(uint i = 0; i < lines.Length; i+=2)
        {
            string objectData = lines[i+1];
            Type objectType = Type.GetType("Database." + lines[i]); 
            entries[i/2] = JsonSerializer.Deserialize(objectData, objectType) as Entry;
            entries[i/2].PositionDB = i;
        }

        return entries;
    }
    
    public void DeleteEntry(uint key)
    {  
        string[] lines = File.ReadAllLines(path);
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            for(uint i = 0; i < lines.Length; i+=2)
            {
                if(i == key)
                    continue;
                writer.WriteLine(lines[i]);
                writer.WriteLine(lines[i+1]);
            }
        }
    }
    
    public void UpdateEntry(Entry entry)
    {
        string[] lines = File.ReadAllLines(path);
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            for(uint i = 0; i < lines.Length; i+=2)
            {
                if (i == entry.PositionDB)
                {
                    writer.WriteLine($"{entry.GetType()}");
                    writer.WriteLine(JsonSerializer.Serialize(entry));
                    continue;
                }
                writer.WriteLine(lines[i]);
                writer.WriteLine(lines[i+1]);
            }
        }
    }

    public void AddEntry(Entry entry, uint position)
    {
        string[] lines = File.ReadAllLines(path);
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            for (uint i = 0; i < position * 2; i += 2)
            {
                writer.WriteLine(lines[i]);
                writer.WriteLine(lines[i + 1]);
            }
            writer.WriteLine($"{entry.GetType()}");
            writer.WriteLine(JsonSerializer.Serialize(entry));
            for (uint i = position * 2; i < lines.Length; i += 2)
            {
                writer.WriteLine(lines[i]);
                writer.WriteLine(lines[i + 1]);
            }
        }
    }
    
    private readonly string path;
}