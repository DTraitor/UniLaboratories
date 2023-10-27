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
            new FunctionInfo("read", ReadEntryData),
            new FunctionInfo("add", AddEntry),
            new FunctionInfo("del", DeleteEntry),
            new FunctionInfo("edit", EditEntry),
            new FunctionInfo("call", CallEntryAbility),
            new FunctionInfo("special", SpecialFunction),
            new FunctionInfo("quit", args => System.Console.WriteLine("Bye!"))
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
            if(arguments[0] == "exit")
                break;
            bool commandExist = false;
            foreach (FunctionInfo command in CommandsList)
            {
                if (command.Name != arguments[0]) 
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
        System.Console.Write("Possible commands: ");
        System.Console.Write(CommandsList[0].Name);
        for (int i = 1; i < CommandsList.Length; i++)
        {
            System.Console.Write(", ");
            System.Console.Write(CommandsList[i].Name);
        }
        System.Console.WriteLine();
    }
    
    private void ListEntries(string[] args)
    {
        if (args.Length > 1)
        {
            System.Console.WriteLine("Wrong arguments.");
            return;
        }
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
                foreach (PropertyInfo property in tempEntry.GetPossibleVariables().Reverse())
                {
                    while (true)
                    {
                        System.Console.Write($"Enter {property.GetCustomAttribute<DatabaseVariable>().Name} ('cancel' to exit");
                        if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                            System.Console.Write(", 'skip' to set null");
                        System.Console.Write("): ");
                        string value = System.Console.ReadLine();
                        if(value.TrimEnd() == "cancel")
                            return;
                        if (Nullable.GetUnderlyingType(property.PropertyType) != null && value.TrimEnd() == "skip")
                            break;
                        
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
                foreach (PropertyInfo abilityProp in tempEntry.GetPossibleAbilityVariables())
                {
                    Type[] possibleAbilities = holder.AbilityTypes.Where(type => type is {IsClass: true, IsAbstract: false} && type.IsSubclassOf(abilityProp.PropertyType)).ToArray();
                    while (true)
                    {
                        System.Console.Write($"Choose one '{abilityProp.GetCustomAttribute<DatabaseAbilityVariable>().Name}' ability");
                        System.Console.Write(" ('cancel' to exit");
                        if (Nullable.GetUnderlyingType(abilityProp.PropertyType) != null)
                            System.Console.Write(", 'skip' to set null");
                        System.Console.WriteLine("):");
                        
                        for(int i = 1; i <= possibleAbilities.Length; i++)
                        {
                            System.Console.WriteLine($"{i}. {possibleAbilities[i - 1].Name}");
                        }
                        
                        string value = System.Console.ReadLine();
                        if(value.TrimEnd() == "cancel")
                            return;
                        if (Nullable.GetUnderlyingType(abilityProp.PropertyType) != null && value.TrimEnd() == "skip")
                            break;
                        
                        uint.TryParse(value, out var abilityNumber);
                        if (abilityNumber > possibleAbilities.Length)
                        {
                            System.Console.WriteLine("Wrong ability number.");   
                            continue;
                        }
                        
                        try
                        {
                            holder.SetAbilityValue(tempEntry, abilityProp, possibleAbilities[abilityNumber - 1].Name);
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
                break;
            }
            catch (ArgumentException e)
            {
                System.Console.WriteLine(e.Message);
            }  
        }
    }

    private void DeleteEntry(string[] args)
    {
        if (args.Length != 2)
        {
            System.Console.WriteLine("Wrong arguments.");
            return;
        }

        if(uint.TryParse(args[1], out var key))
            try
            {
                holder.DeleteEntry(key);
            }
            catch (ArgumentException e)
            {
                System.Console.WriteLine(e.Message);
            }
        else
            System.Console.WriteLine("Entry key should be a positive integer.");
    }

    private void ReadEntryData(string[] args)
    {
        if (args.Length != 2)
        {
            System.Console.WriteLine("Wrong arguments.");
            return;
        }

        if(uint.TryParse(args[1], out var key))
            try
            {
                System.Console.WriteLine(holder.GetEntryAsString(key));
            }
            catch (ArgumentException e)
            {
                System.Console.WriteLine(e.Message);
            }
        else
            System.Console.WriteLine("Entry key should be a positive integer.");
    }
    
    private void EditEntry(string[] args)
    {
        if (args.Length != 2)
        {
            System.Console.WriteLine("Wrong arguments.");
            return;
        }

        if (uint.TryParse(args[1], out var key))
        {
            var entryToEdit = holder.GetEntry(key);
            System.Console.Write("Choose what you want to edit (variable - 1 | ability - 2 | exit - 'cancel'): ");
            while (true)
            {
                switch (System.Console.ReadLine().TrimEnd(' '))
                {
                    case "1":
                        System.Console.WriteLine("Choose variable to edit ('cancel' to exit):");
                        PropertyInfo[] possibleVariables = entryToEdit.GetPossibleVariables().Reverse().ToArray();
                        for (int i = 1; i <= possibleVariables.Length; i++)
                        {
                            System.Console.WriteLine($"{i}. {possibleVariables[i - 1].GetCustomAttribute<DatabaseVariable>().Name}");
                        }

                        PropertyInfo variableToEdit;
                        while (true)
                        {
                            string input = System.Console.ReadLine();
                            if(input == "cancel")
                                return;
                            if (uint.TryParse(input, out var varNum) && varNum <= possibleVariables.Length)
                            {
                                
                                
                                variableToEdit = possibleVariables[varNum - 1];
                                break;
                            }
                            System.Console.WriteLine("Wrong variable number.");
                        }
                        System.Console.Write("Enter new value ('cancel' to exit): ");
                        while (true)
                        {
                            string input = System.Console.ReadLine();
                            if(input == "cancel")
                                return;
                            try
                            {
                                holder.SetEntryValue(entryToEdit, variableToEdit.Name, input);
                                break;
                            }
                            catch (ArgumentException e)
                            {
                                System.Console.WriteLine(e.Message);
                            }
                        }
                        holder.UpdateEntry(entryToEdit);
                        return;
                    case "2":
                        System.Console.WriteLine("Choose ability to edit ('cancel' to exit):");
                        PropertyInfo[] possibleAbilities = entryToEdit.GetPossibleAbilityVariables();
                        for (int i = 1; i <= possibleAbilities.Length; i++)
                        {
                            System.Console.WriteLine($"{i}. {possibleAbilities[i - 1].GetCustomAttribute<DatabaseAbilityVariable>().Name}");
                        }
                        PropertyInfo abilityToEdit;
                        while (true)
                        {
                            string input = System.Console.ReadLine();
                            if(input == "cancel")
                                return;
                            if (uint.TryParse(input, out var abilityNum) && abilityNum <= possibleAbilities.Length)
                            {
                                abilityToEdit = possibleAbilities[abilityNum - 1];
                                break;
                            }
                            System.Console.WriteLine("Wrong ability number.");
                        }
                        Type[] possibleAbilityTypes = holder.AbilityTypes.Where(type => type is {IsClass: true, IsAbstract: false} && type.IsSubclassOf(abilityToEdit.PropertyType)).ToArray();
                        System.Console.WriteLine("Choose new ability ('cancel' to exit):");
                        for (int i = 1; i <= possibleAbilityTypes.Length; i++)
                        {
                            System.Console.WriteLine($"{i}. {possibleAbilityTypes[i - 1].Name}");
                        }
                        while (true)
                        {
                            string input = System.Console.ReadLine();
                            if(input == "cancel")
                                return;
                            if (uint.TryParse(input, out var abilityNum) && abilityNum <= possibleAbilityTypes.Length)
                            {
                                try
                                {
                                    holder.SetAbilityValue(entryToEdit, abilityToEdit, possibleAbilityTypes[abilityNum - 1].Name);
                                    break;
                                }
                                catch (ArgumentException e)
                                {
                                    System.Console.WriteLine(e.Message);
                                }
                            }
                            System.Console.WriteLine("Wrong ability number.");
                        }
                        holder.UpdateEntry(entryToEdit);
                        return;
                    case "cancel":
                        return;
                }
                System.Console.WriteLine("Wrong answer");
            }
        }
        System.Console.WriteLine("Entry key should be a positive integer.");
    }

    private void SpecialFunction(string[] args)
    {
        System.Console.WriteLine(holder.SpecialTask());
    }

    private void CallEntryAbility(string[] args)
    {
        if (args.Length != 2 && args.Length != 3)
        {
            System.Console.WriteLine("Wrong arguments.");
            return;
        }

        if (uint.TryParse(args[1], out var key))
        {
            if(key >= holder.GetEntriesCount())
            {
                System.Console.WriteLine("Entry key is too big.");
                return;
            }
            if(args.Length == 2)
                System.Console.WriteLine(holder.GetPossibleFunctions(key));
            else
            {
                try
                {
                    System.Console.WriteLine(holder.CallFunction(key, args[2]));
                }
                catch (ArgumentException e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }
        else 
            System.Console.WriteLine("Entry key should be a positive integer.");
    }
    
    private InteractionsHolder holder;

    private readonly FunctionInfo[] CommandsList;
    
    private delegate void Command(string[] args);
    private struct FunctionInfo
    {
        public FunctionInfo(string name, Command function)
        {
            Name = name;
            Function = function;
        }
        
        public string Name { get; set; }
        public Command Function { get; set; }
    }
}