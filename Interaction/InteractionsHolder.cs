using System.Reflection;
using Database;
using Database.Entries;
using Database.Entries.Abilities;

namespace Interaction;

public class InteractionsHolder
{
    public InteractionsHolder()
    {
        database = new DatabaseIO("Database.txt");
        databaseData = database.ReadData();
        ChildTypes = Assembly.GetAssembly(typeof(Entry)).GetTypes().Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(typeof(Entry))).ToArray();
    }

    public string GetEntriesAsString(uint page)
    {
        if(page > databaseData.Length / EntriesPerPage)
            throw new ArgumentException("Page number is too big.");
        Entry[] entries = GetEntries(page);
        if(entries.Length == 0)
            return "No entries found.\n";

        string result = "";
        foreach (Entry entry in entries)
        {
            result += entry.GetSmallString() + "\n";
        }
        return result;
    }
    
    public Entry[] GetEntries(uint page)
    {
        if(page > databaseData.Length / EntriesPerPage)
            throw new ArgumentException("Page number is too big.");
        Entry[] result = new Entry[Math.Min(EntriesPerPage, databaseData.Length - page * EntriesPerPage)];
        for(uint i = 0; i < result.Length; i++)
            result[i] = databaseData[page * EntriesPerPage + i];
        return result;
    }
    
    public string GetPossibleFunctions(uint key)
    {
        if(key >= databaseData.Length)
            throw new ArgumentException("Entry key is too big.");
        return $"This object can execute the following functions:\n{databaseData[key].GetPossibleFunctionsAsString()}";
    }
    
    public Entry CreateTemplateEntry(string stype)
    {
        Type? type = Type.GetType("Database.Entries." + stype +", Database");
        if(type == null)
            throw new ArgumentException("Wrong type.");
        if(type.IsAbstract || !type.IsSubclassOf(typeof(Entry)))
            throw new ArgumentException("Wrong type.");
        return Activator.CreateInstance(type) as Entry;
    }
    
    public int GetEntriesCount()
    {
        return databaseData.Length;
    }
    
    public void SetEntryValue(Entry entry, string property, string value)
    {
        PropertyInfo? propertyInfo = entry.GetType().GetProperty(property);
        if(propertyInfo == null || propertyInfo.GetCustomAttribute<DatabaseVariable>() == null)
            throw new ArgumentException("Wrong property.");
        if(propertyInfo.PropertyType != typeof(string))
            throw new ArgumentException("Wrong property.");
        propertyInfo.SetValue(entry, value);
    }

    public void AddEntry(Entry entry, uint position)
    {
        if(position > databaseData.Length)
            throw new ArgumentException("Position is too big.");
        Array.Resize(ref databaseData, databaseData.Length + 1);
        for (uint i = (uint)databaseData.Length - 1; i > position; i--)
        {
            databaseData[i] = databaseData[i - 1];
            databaseData[i].PositionDB = i;
        }
        entry.PositionDB = position;
        databaseData[position] = entry;
        database.AddEntry(entry, position);
    }
    
    public string CallFunction(uint key, uint functionKey)
    {
        if(key > databaseData.Length)
            throw new ArgumentException("Entry key is too big.");
        Func<string> function;
        try
        {
            function = Delegate.CreateDelegate(typeof(Func<string>), databaseData[key],
                databaseData[key].GetPossibleFunctions()[functionKey]) as Func<string>;
        }
        catch (IndexOutOfRangeException e)
        {
            throw new ArgumentException("Function doesn't exist.");
        }
        return function();
    }

    public readonly Type[] ChildTypes;
    private Entry[] databaseData;
    private DatabaseIO database;
    
    public const uint EntriesPerPage = 10;
}