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
        AbilityTypes = Assembly.GetAssembly(typeof(EntryAbility)).GetTypes().Where(t => t is {IsClass: true, IsAbstract: false} && t.IsSubclassOf(typeof(EntryAbility))).ToArray();
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
    
    public string GetEntryAsString(uint key)
    {
        if(key >= databaseData.Length)
            throw new ArgumentException("Entry key is too big.");
        return databaseData[key].ToString();
    }
    
    private Entry[] GetEntries(uint page)
    {
        if(page > databaseData.Length / EntriesPerPage)
            throw new ArgumentException("Page number is too big.");
        Entry[] result = new Entry[Math.Min(EntriesPerPage, databaseData.Length - page * EntriesPerPage)];
        for(uint i = 0; i < result.Length; i++)
            result[i] = databaseData[page * EntriesPerPage + i];
        return result;
    }

    public Entry GetEntry(uint key)
    {
        if (key >= databaseData.Length)
            throw new ArgumentException("Entry key is too big.");
        return databaseData[key];
    }
    
    public string GetPossibleFunctions(uint key)
    {
        if(key >= databaseData.Length)
            throw new ArgumentException("Entry key is too big.");
        return $"This object can execute the following functions:{databaseData[key].GetPossibleFunctionsAsString()}";
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
        try
        {
            propertyInfo.SetValue(entry, value);
        }
        catch (TargetInvocationException e)
        {
            throw e.InnerException ?? e;
        }
    }

    public void SetAbilityValue(Entry entry, PropertyInfo abilityVar, string className)
    {
        Type ability = AbilityTypes.FirstOrDefault(type => type.Name == className);
        if(ability == null)
            throw new ArgumentException("Wrong ability type.");
        EntryAbility instance = Activator.CreateInstance(ability) as EntryAbility;
        abilityVar.SetValue(entry, instance);
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
    
    public void DeleteEntry(uint key)
    {
        if(key >= databaseData.Length)
            throw new ArgumentException("Entry key is too big.");
        for (uint i = key; i < databaseData.Length - 1; i++)
        {
            databaseData[i] = databaseData[i + 1];
            databaseData[i].PositionDB = i;
        }
        Array.Resize(ref databaseData, databaseData.Length - 1);
        database.DeleteEntry(key);
    }
    
    public string CallFunction(uint key, string functionName)
    {
        if(key > databaseData.Length)
            throw new ArgumentException("Entry key is too big.");
        MethodInfo[] methods = databaseData[key].GetPossibleFunctions();
        MethodInfo? method = methods.FirstOrDefault(m => m.GetCustomAttribute<DatabaseAbilityFunction>().Name == functionName);
        if(method == null)
            throw new ArgumentException("Wrong function name.");
        Func<string> function = Delegate.CreateDelegate(typeof(Func<string>), databaseData[key], method) as Func<string>;
        return function();
    }

    public void UpdateEntry(Entry entry)
    {
        database.UpdateEntry(entry);
    }

    public string SpecialTask()
    {
        Student[] entries = Array.ConvertAll(databaseData.Where(entry => entry is Student).ToArray(), entry => entry as Student);
        Student[] requested = entries.Where(entry => entry is { Sex: "F", Residence: "Kyiv", YearOfStudy: "5" }).ToArray();

        if(requested.Length == 0)
            return "No entries found.";

        string result = "";
        foreach (Student entry in requested)
        {
            result += entry.GetSmallString() + "\n";
        }
        return result.TrimEnd('\n');
    }

    public readonly Type[] AbilityTypes;
    public readonly Type[] ChildTypes;
    private Entry[] databaseData;
    private DatabaseIO database;
    
    public const uint EntriesPerPage = 10;
}