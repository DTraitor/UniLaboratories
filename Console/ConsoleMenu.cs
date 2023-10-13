using System.Reflection;
using Database.Entries.Abilities;
using Interaction;

namespace Console;

public class ConsoleMenu
{
    public ConsoleMenu()
    {
        CommandsList = new[]
        {
            new FunctionInfo("help", Help),
            new FunctionInfo("list", ListEntries),
            new FunctionInfo("add", AddEntry)
        };
    }
    
    public void Start()
    {
        System.Console.Clear();
        System.Console.WriteLine("Reading the database...");
        holder = new InteractionsHolder();
        System.Console.WriteLine("Done!");
        System.Console.WriteLine("Welcome to the database!");
        System.Console.Write(holder.GetEntriesAsString(0));
        while (true)
        {
            System.Console.Write("> ");
            string[] arguments = System.Console.ReadLine().Split(' ');
            if(arguments[0] == "quit")
                break;
            bool commandExist = false;
            foreach (FunctionInfo command in CommandsList)
            {
                if (command.Command != arguments[0]) 
                    continue;
                commandExist = true;
                command.Function(arguments);
                break;
            }
            if(!commandExist)
                System.Console.WriteLine("Wrong command.");
        }
    }

    private void Help(string[] args)
    {
        System.Console.WriteLine("Help is on the way!");
    }
    
    private void ListEntries(string[] args)
    {
        if (args.Length == 1)
        {
            System.Console.Write(holder.GetEntriesAsString(0));
            return;
        }

        if(uint.TryParse(args[1], out var page))
            try
            {
                System.Console.Write(holder.GetEntriesAsString(page - 1));
            }
            catch(ArgumentException e)
            {
                System.Console.WriteLine(e.Message);
            }
        else
            System.Console.WriteLine("Wrong page number.");
    }

    private void AddEntry(string[] args)
    {
        System.Console.WriteLine("Adding new entry...");
        System.Console.Write("Possible types: ");
        string types = holder.ChildTypes[0].Name;
        for (int i = 1; i < holder.ChildTypes.Length; i++)
        {
            types += ", ";
            types += holder.ChildTypes[i].Name;
        }
        System.Console.WriteLine(types);
        
        while (true)
        {
            System.Console.Write("Choose type ('cancel' to exit): ");
            string input = System.Console.ReadLine();
            if(input.TrimEnd() == "cancel")
                return;
            
            try
            {
                var tempEntry = holder.CreateTemplateEntry(input);
                foreach (PropertyInfo property in tempEntry.GetPossibleVariables())
                {
                    while (true)
                    {
                        System.Console.Write($"Enter {property.GetCustomAttribute<DatabaseVariable>().Name} ('cancel' to exit): ");
                        string value = System.Console.ReadLine();
                        if(value.TrimEnd() == "cancel")
                            return;
                        
                        try
                        {
                            holder.SetEntryValue(tempEntry, property.Name, value);
                            break;
                        }
                        catch (ArgumentException e)
                        {
                            System.Console.WriteLine(e.Message);
                        }
                    }
                }
                while (true)
                {
                    System.Console.WriteLine($"Choose entry position (0-{holder.GetEntriesCount()}, 'cancel' to exit): ");
                    string value = System.Console.ReadLine();
                    if(value.TrimEnd() == "cancel")
                        return;

                    if (uint.TryParse(value, out var position))
                    {
                        try
                        {
                            holder.AddEntry(tempEntry, position);
                            break;   
                        }
                        catch (ArgumentException e)
                        {
                            System.Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            catch (ArgumentException e)
            {
                System.Console.WriteLine(e.Message);
            }  
        }
    }

    
    
    private InteractionsHolder holder;

    private readonly FunctionInfo[] CommandsList;
    
    private delegate void Command(string[] args);
    private struct FunctionInfo
    {
        public FunctionInfo(string command, Command function)
        {
            Command = command;
            Function = function;
        }
        
        public string Command { get; set; }
        public Command Function { get; set; }
    }
}