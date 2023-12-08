namespace UnitTests;

public class BusinessLogicTests
{
    [Test]
    public void EntityServiceTests()
    {
        File.WriteAllText("entityservice_test.json", string.Empty);

        EntityService _entityService = new EntityService("entityservice_test.json");
        _entityService.CreateEntity("Student");
        _entityService.SetValue(0, "Name", "Joe");
        _entityService.SetValue(0, "Surname", "Mama");
        _entityService.SetValue(0, "StudentID", "КВ-12345678");
        _entityService.SetValue(0, "Sex", "F");
        _entityService.SetValue(0, "Course", "5");
        _entityService.SetValue(0, "GradeBook", "12345678");
        _entityService.SetValue(0, "Residence", "Kyiv");
        _entityService.SetAbilityType(0, "Calculate Big Numbers", "CanCalculate");
        _entityService.SetAbilityType(0, "Study", "CanStudy");
        Assert.That(_entityService.GetData(0), Is.EqualTo("Name: Joe\nSurname: Mama\nCourse: 5\nStudent ID: КВ-12345678\nSex: F\nResidence: Kyiv\nGrade book: 12345678\n"));
        Assert.That(_entityService.GetEditableData(0), Is.EqualTo(new List<string> { "Name", "Surname", "Course", "StudentID", "Sex", "Residence", "GradeBook"}));
        Assert.That(_entityService.GetAbilities(0), Is.EqualTo(new List<string>() { "Calculate Big Numbers", "Study" }));
        Assert.That(_entityService.UseAbility(0, "Calculate Big Numbers"), Is.EqualTo("I can calculate big numbers"));
        try
        {
            _entityService.UseAbility(0, "Something clearly wrong");
            Assert.Fail();
        }
        catch (CustomException e)
        {
            Assert.That(e.Message, Is.EqualTo("This ability does not exist!"));
        }
        try
        {
            _entityService.SetAbilityType(0, "Calculate Big Numbers", "Something clearly wrong");
            Assert.Fail();
        }
        catch (CustomException e)
        {
            Assert.That(e.Message, Is.EqualTo("This type does not exist!"));
        }

        Assert.That(_entityService.GetEntityCount(), Is.EqualTo(1));
        Assert.That(_entityService.CalculateSpecialTask(), Is.EqualTo(1));
        _entityService.SaveChanges();

        _entityService.Close();
        _entityService = new EntityService("entityservice_test.json");
        Assert.That(_entityService.GetEntityCount(), Is.EqualTo(1));

        _entityService.DeleteEntity(0);
        Assert.That(_entityService.GetEntityCount(), Is.EqualTo(0));
    }
}