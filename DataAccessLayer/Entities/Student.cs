using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using DataAccessLayer.Abilities;
using DataAccessLayer;

namespace DataAccessLayer.Entities;

internal class Student : Entity
{
    public Student() : base() {}

    public Student(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        course = (int)info.GetValue("Course", typeof(int));
        studentId = (string)info.GetValue("StudentID", typeof(string));
        sex = (string)info.GetValue("Sex", typeof(string));
        residence = (string)info.GetValue("Residence", typeof(string));
        gradeBook = (string)info.GetValue("GradeBook", typeof(string));
        var type = (string)info.GetValue("StudyAbility", typeof(string));
        var typeToSet = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IStudy).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .FirstOrDefault(x => x.Name == type);
        studyAbility = (IStudy)Activator.CreateInstance(typeToSet);
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Course", course);
        info.AddValue("StudentID", studentId);
        info.AddValue("Sex", sex);
        info.AddValue("Residence", residence);
        info.AddValue("GradeBook", gradeBook);
        info.AddValue("StudyAbility", studyAbility.GetType().Name);
    }

    public override string GetData()
    {
        return base.GetData() + $"Course: {course}\nStudent ID: {studentId}\nSex: {sex}\nResidence: {residence}\nGrade book: {gradeBook}\n";
    }

    public override List<string> GetEditableData()
    {
        return base.GetEditableData().Append("Course").Append("StudentID").Append("Sex").Append("Residence").Append("GradeBook").ToList();
    }

    public override void SetData(string data, string value)
    {
        base.SetData(data, value);
        switch (data)
        {
            case "Course":
                if (int.TryParse(value, out var course))
                    this.course = course;
                else
                    throw new CustomException("Should be an integer!");
                break;
            case "Student ID":
                if (studentIdRegex.IsMatch(value))
                    studentId = value;
                else
                    throw new CustomException("Should be in format XX-YYYYYYYY!");
                break;
            case "Sex":
                if (value == "M" || value == "F")
                    sex = value;
                else
                    throw new CustomException("Should be M or F!");
                break;
            case "Residence":
                residence = value;
                break;
            case "GradeBook":
                if (gradeBookRegex.IsMatch(value))
                    gradeBook = value;
                else
                    throw new CustomException("Should be exactly 8 digits!");
                break;
        }
    }

    public override List<string> GetAbilities()
    {
        return base.GetAbilities().Append("Study").ToList();
    }

    public override string UseAbility(string ability)
    {
        switch (ability)
        {
            case "Study":
                return studyAbility.Study((success) =>
                {
                    if (success)
                        course++;
                });
        }
        return base.UseAbility(ability);
    }

    public override List<string> GetAbilityTypes(string ability)
    {
        switch (ability)
        {
            case "Study":
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => typeof(IStudy).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(x => x.Name)
                    .ToList();
        }
        return base.GetAbilityTypes(ability);
    }

    public override void SetAbilityType(string ability, string type)
    {
        switch (ability)
        {
            case "Study":
                var typeToSet = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => typeof(IStudy).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .FirstOrDefault(x => x.Name == type);
                if (typeToSet == null)
                    throw new CustomException("This type does not exist!");
                studyAbility = (IStudy)Activator.CreateInstance(typeToSet);
                break;
        }
        base.SetAbilityType(ability, type);
    }

    private int course;
    private string studentId;
    private string sex;
    private string residence;
    private string gradeBook;
    private IStudy studyAbility;
    private static readonly Regex studentIdRegex = new(@"^[А-Я]{2}-\d{8}$");
    private static readonly Regex gradeBookRegex = new(@"^\d{8}$");
}