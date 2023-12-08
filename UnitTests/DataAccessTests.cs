

using System.Reflection;
using System.Text.Json;

namespace UnitTests;

public class DataAccessTests
{
    [Test]
    public void ReadWriteTest()
    {
        File.WriteAllText("db_test.json", string.Empty);
        DataProvider _dataProvider = new DataProvider("db_test.json");
        _dataProvider.Entities = new List<Entity>
        {
            new Entity()
            {
                Name = "Joe",
                Surname = "Mama",
                BigNumbers = new CanCalculate()
            }
        };
        _dataProvider.Save();
        _dataProvider.stream.Position = 0;
        using StreamReader reader = new StreamReader(_dataProvider.stream, leaveOpen: true);
        Assert.That(reader.ReadToEnd(), Is.EqualTo("[{\"Name\":\"Joe\",\"Surname\":\"Mama\",\"BigNumbers\":{\"$type\":\"CanCalculate\"}}]"));
        _dataProvider.Read();
        for (int i = 0; i < _dataProvider.Entities.Count; i++)
        {
            Assert.That(_dataProvider.Entities[i].Name, Is.EqualTo("Joe"));
            Assert.That(_dataProvider.Entities[i].Surname, Is.EqualTo("Mama"));
            Assert.That(_dataProvider.Entities[i].BigNumbers, Is.TypeOf<CanCalculate>());
        }
        _dataProvider.stream.SetLength(0);
        using StreamWriter writer = new StreamWriter(_dataProvider.stream, leaveOpen: true);
        writer.Write("Something unexpected");
        _dataProvider.Read();
        Assert.That(_dataProvider.Entities.Count, Is.EqualTo(0));
    }

    [Test]
    public void EntityTests()
    {
        Entity _entity = Entity.Create("Entity");
        _entity.SetData("Name", "Joe");
        _entity.SetData("Surname", "Mama");
        _entity.SetAbilityType("Calculate Big Numbers", "CanCalculate");
        Assert.That(_entity.Name, Is.EqualTo("Joe"));
        Assert.That(_entity.Surname, Is.EqualTo("Mama"));
        Assert.That(_entity.BigNumbers, Is.TypeOf<CanCalculate>());
        try
        {
            _entity.SetAbilityType("Calculate Big Numbers", "Something clearly wrong");
            Assert.Fail();
        }
        catch (CustomException e)
        {
            Assert.That(e.Message, Is.EqualTo("This type does not exist!"));
        }
        Assert.That(_entity.UseAbility("Calculate Big Numbers"), Is.EqualTo("I can calculate big numbers"));
        try
        {
            _entity.UseAbility("Something clearly wrong");
            Assert.Fail();
        }
        catch (CustomException e)
        {
            Assert.That(e.Message, Is.EqualTo("This ability does not exist!"));
        }
        Assert.That(_entity.GetData(), Is.EqualTo("Name: Joe\nSurname: Mama\n"));
        Assert.That(_entity.GetEditableData().Count, Is.EqualTo(2));
        Assert.That(_entity.GetAbilities().Count, Is.EqualTo(1));
    }

    [Test]
    public void JoinerVariables()
    {
        Joiner _joiner = Entity.Create("Joiner") as Joiner;
        _joiner.SetData("Name", "Joe");
        _joiner.SetData("Surname", "Mama");
        _joiner.SetData("Price", "100");
        _joiner.SetAbilityType("Calculate Big Numbers", "CanCalculate");
        Assert.That(_joiner.Name, Is.EqualTo("Joe"));
        Assert.That(_joiner.Surname, Is.EqualTo("Mama"));
        Assert.That(_joiner.Price, Is.EqualTo(100));
        Assert.That(_joiner.BigNumbers, Is.TypeOf<CanCalculate>());
        Assert.That(_joiner.GetData(), Is.EqualTo("Name: Joe\nSurname: Mama\nPrice: 100\n"));
        Assert.That(_joiner.GetEditableData().Count, Is.EqualTo(3));
        Assert.That(_joiner.GetAbilities().Count, Is.EqualTo(1));
    }

    [Test]
    public void PhotographerVariables()
    {
        Photographer _photographer = Entity.Create("Photographer") as Photographer;
        _photographer.SetData("Name", "Joe");
        _photographer.SetData("Surname", "Mama");
        _photographer.SetData("Camera model", "Canon");
        _photographer.SetAbilityType("Calculate Big Numbers", "CanCalculate");
        Assert.That(_photographer.Name, Is.EqualTo("Joe"));
        Assert.That(_photographer.Surname, Is.EqualTo("Mama"));
        Assert.That(_photographer.CameraModel, Is.EqualTo("Canon"));
        Assert.That(_photographer.BigNumbers, Is.TypeOf<CanCalculate>());
        Assert.That(_photographer.GetData(), Is.EqualTo("Name: Joe\nSurname: Mama\nCamera model: Canon\n"));
        Assert.That(_photographer.GetEditableData().Count, Is.EqualTo(3));
        Assert.That(_photographer.GetAbilities().Count, Is.EqualTo(1));
    }

    [Test]
    public void StudentVariables()
    {
        Student _student = Entity.Create("Student") as Student;
        _student.SetData("Name", "Joe");
        _student.SetData("Surname", "Mama");
        _student.SetData("Course", "5");
        _student.SetData("StudentID", "КВ-00002222");
        _student.SetData("Sex", "F");
        _student.SetData("Residence", "Kyiv");
        _student.SetData("GradeBook", "00002222");
        _student.SetAbilityType("Calculate Big Numbers", "CanCalculate");
        _student.SetAbilityType("Study", "CanStudy");
        Assert.That(_student.Name, Is.EqualTo("Joe"));
        Assert.That(_student.Surname, Is.EqualTo("Mama"));
        Assert.That(_student.Course, Is.EqualTo(5));
        Assert.That(_student.StudentId, Is.EqualTo("КВ-00002222"));
        Assert.That(_student.Sex, Is.EqualTo("F"));
        Assert.That(_student.Residence, Is.EqualTo("Kyiv"));
        Assert.That(_student.GradeBook, Is.EqualTo("00002222"));
        Assert.That(_student.BigNumbers, Is.TypeOf<CanCalculate>());
        Assert.That(_student.StudyAbility, Is.TypeOf<CanStudy>());
        try
        {
            _student.SetAbilityType("Study", "Something clearly wrong");
            Assert.Fail();
        }
        catch (CustomException e)
        {
            Assert.That(e.Message, Is.EqualTo("This type does not exist!"));
        }
        Assert.That(_student.UseAbility("Study"), Is.EqualTo("I can study"));
        try
        {
            _student.UseAbility("Something clearly wrong");
            Assert.Fail();
        }
        catch (CustomException e)
        {
            Assert.That(e.Message, Is.EqualTo("This ability does not exist!"));
        }

        Assert.That(_student.GetData(), Is.EqualTo("Name: Joe\nSurname: Mama\nCourse: 6\nStudent ID: КВ-00002222\nSex: F\nResidence: Kyiv\nGrade book: 00002222\n"));
        Assert.That(_student.GetEditableData().Count, Is.EqualTo(7));
        Assert.That(_student.GetAbilities().Count, Is.EqualTo(2));
    }

    [Test]
    public void AbilitiesReturnTest()
    {
        ICalculateBigNumbers _calculateBigNumbers = new CanCalculate();
        Assert.That(_calculateBigNumbers.Calculate(), Is.EqualTo("I can calculate big numbers"));
        _calculateBigNumbers = new CanNotCalculate();
        Assert.That(_calculateBigNumbers.Calculate(), Is.EqualTo("I can not calculate big numbers"));
        IStudy _study = new CanStudy();
        Assert.That(_study.Study((success) => { }), Is.EqualTo("I can study"));
        _study = new CanNotStudy();
        Assert.That(_study.Study((success) => { }), Is.EqualTo("I can not study"));
    }
}
