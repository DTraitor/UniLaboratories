using BusinessLogicLayer;
using DataAccessLayer;

namespace PresentationLayer;

public class ConsoleMenu
{
    public void Start()
    {
        Console.WriteLine("Choose serializer type:");
        foreach (DataProvider.SerializerType type in Enum.GetValues<DataProvider.SerializerType>())
        {
            Console.WriteLine($"{(int)type}. {type}");
        }
        DataProvider.SerializerType serializerType;
        while (!Enum.TryParse(Console.ReadLine(), out serializerType))
        {
            Console.WriteLine("Invalid input!");
        }
        Console.WriteLine("Enter file Name:");
        string file = Console.ReadLine();

        entityService = new EntityService(file, serializerType);

        while (true)
        {
            Console.WriteLine($"Number of entities: {entityService.GetEntityCount()}");
            Console.WriteLine("Choose action:");
            Console.WriteLine("1. Create entity");
            Console.WriteLine("2. Edit entity");
            Console.WriteLine("3. Delete entity");
            Console.WriteLine("4. Get entity data");
            Console.WriteLine("5. Use ability");
            Console.WriteLine("6. Calculate number of 5th course female students that live in Kyiv");
            Console.WriteLine("7. Exit");
            int action;
            while (!int.TryParse(Console.ReadLine(), out action) || action < 1 || action > 7)
            {
                Console.WriteLine("Invalid input!");
            }
            switch (action)
            {
                case 1:
                    CreateEntity();
                    break;
                case 2:
                    EditEntity();
                    break;
                case 3:
                    DeleteEntity();
                    break;
                case 4:
                    GetEntityData();
                    break;
                case 5:
                    UseAbility();
                    break;
                case 6:
                    SpecialTask();
                    break;
                case 7:
                    return;
            }
        }
    }

    private void CreateEntity()
    {
        Console.WriteLine("Choose entity type:");
        var entityTypes = entityService.GetEntityTypes();
        for (int i = 0; i < entityTypes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {entityTypes[i]}");
        }
        int entityType;
        while (!int.TryParse(Console.ReadLine(), out entityType) || entityType < 1 || entityType > entityTypes.Count)
        {
            Console.WriteLine("Invalid input!");
        }
        entityService.CreateEntity(entityTypes[entityType - 1]);

        List<string> editableData = entityService.GetEditableData(entityService.GetEntityCount() - 1);
        foreach (string data in editableData)
        {
            Console.WriteLine($"Enter {data}:");
            bool changedValue = false;
            while (!changedValue)
            {
                try
                {
                    entityService.SetValue(entityService.GetEntityCount() - 1, data, Console.ReadLine());
                    changedValue = true;
                }
                catch (CustomException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        List<string> editableAbilities = entityService.GetAbilities(entityService.GetEntityCount() - 1);
        foreach (string ability in editableAbilities)
        {
            Console.WriteLine($"Choose {ability} type:");
            List<string> abilityTypes = entityService.GetAbilityTypes(entityService.GetEntityCount() - 1, ability);
            for (int j = 0; j < abilityTypes.Count; j++)
            {
                Console.WriteLine($"{j + 1}. {abilityTypes[j]}");
            }
            int abilityType;
            while (!int.TryParse(Console.ReadLine(), out abilityType) || abilityType < 1 || abilityType > abilityTypes.Count)
            {
                Console.WriteLine("Invalid input!");
            }
            entityService.SetAbilityType(entityService.GetEntityCount() - 1, ability, abilityTypes[abilityType - 1]);
        }

        entityService.SaveChanges();
        Console.WriteLine("Entity created!");
    }

    private void EditEntity()
    {
        Console.WriteLine($"Choose entity (1-{entityService.GetEntityCount()}):");
        int entityIndex;
        while (!int.TryParse(Console.ReadLine(), out entityIndex) || entityIndex < 1 || entityIndex > entityService.GetEntityCount())
        {
            Console.WriteLine("Invalid input!");
        }

        Console.WriteLine("Choose data to edit:");
        List<string> editableData = entityService.GetEditableData(entityIndex - 1);
        for (int i = 0; i < editableData.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {editableData[i]}");
        }
        int dataIndex;
        while (!int.TryParse(Console.ReadLine(), out dataIndex) || dataIndex < 1 || dataIndex > editableData.Count)
        {
            Console.WriteLine("Invalid input!");
        }
        Console.WriteLine("Enter new value:");

        bool changedValue = false;
        while (!changedValue)
        {
            try
            {
                entityService.SetValue(entityIndex - 1, editableData[dataIndex - 1], Console.ReadLine());
                changedValue = true;
            }
            catch (CustomException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        entityService.SaveChanges();
    }

    private void DeleteEntity()
    {
        Console.WriteLine($"Choose entity (1-{entityService.GetEntityCount()}):");
        int entityIndex;
        while (!int.TryParse(Console.ReadLine(), out entityIndex) || entityIndex < 1 || entityIndex > entityService.GetEntityCount())
        {
            Console.WriteLine("Invalid input!");
        }
        entityService.DeleteEntity(entityIndex - 1);
        entityService.SaveChanges();
        Console.WriteLine("Entity deleted!");
    }

    private void GetEntityData()
    {
        Console.WriteLine($"Choose entity (1-{entityService.GetEntityCount()}):");
        int entityIndex;
        while (!int.TryParse(Console.ReadLine(), out entityIndex) || entityIndex < 1 || entityIndex > entityService.GetEntityCount())
        {
            Console.WriteLine("Invalid input!");
        }
        Console.Write(entityService.GetData(entityIndex - 1));
    }

    private void UseAbility()
    {
        Console.WriteLine($"Choose entity (1-{entityService.GetEntityCount()}):");
        int entityIndex;
        while (!int.TryParse(Console.ReadLine(), out entityIndex) || entityIndex < 1 || entityIndex > entityService.GetEntityCount())
        {
            Console.WriteLine("Invalid input!");
        }
        Console.WriteLine("Choose ability:");
        List<string> abilities = entityService.GetAbilities(entityIndex - 1);
        for (int i = 0; i < abilities.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {abilities[i]}");
        }
        int abilityIndex;
        while (!int.TryParse(Console.ReadLine(), out abilityIndex) || abilityIndex < 1 || abilityIndex > abilities.Count)
        {
            Console.WriteLine("Invalid input!");
        }
        Console.WriteLine(entityService.UseAbility(entityIndex - 1, abilities[abilityIndex - 1]));
        entityService.SaveChanges();
    }

    private void SpecialTask()
    {
        Console.WriteLine($"Number of 5th course female students that live in Kyiv: {entityService.CalculateSpecialTask()}");
    }

    private EntityService entityService;
}