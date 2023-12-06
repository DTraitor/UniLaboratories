using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using DataAccessLayer.Abilities;
using DataAccessLayer;

namespace DataAccessLayer.Entities;

[Serializable]
public class Student : Entity
{
    public Student() : base() {}

    public Student(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Course = (int)info.GetValue("Course", typeof(int));
        StudentId = (string)info.GetValue("StudentID", typeof(string));
        Sex = (string)info.GetValue("Sex", typeof(string));
        Residence = (string)info.GetValue("Residence", typeof(string));
        GradeBook = (string)info.GetValue("GradeBook", typeof(string));
        var type = (string)info.GetValue("StudyAbility", typeof(string));
        var typeToSet = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IStudy).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .FirstOrDefault(x => x.Name == type);
        StudyAbility = (IStudy)Activator.CreateInstance(typeToSet);
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Course", Course);
        info.AddValue("StudentID", StudentId);
        info.AddValue("Sex", Sex);
        info.AddValue("Residence", Residence);
        info.AddValue("GradeBook", GradeBook);
        info.AddValue("StudyAbility", StudyAbility.GetType().Name);
    }

    public override string GetData()
    {
        return base.GetData() + $"Course: {Course}\nStudent ID: {StudentId}\nSex: {Sex}\nResidence: {Residence}\nGrade book: {GradeBook}\n";
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
                    this.Course = course;
                else
                    throw new CustomException("Should be an integer!");
                break;
            case "StudentID":
                if (studentIdRegex.IsMatch(value))
                    StudentId = value;
                else
                    throw new CustomException("Should be in format XX-YYYYYYYY!");
                break;
            case "Sex":
                if (value == "M" || value == "F")
                    Sex = value;
                else
                    throw new CustomException("Should be M or F!");
                break;
            case "Residence":
                Residence = value;
                break;
            case "GradeBook":
                if (gradeBookRegex.IsMatch(value))
                    GradeBook = value;
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
                return StudyAbility.Study((success) =>
                {
                    if (success)
                        Course++;
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
                StudyAbility = (IStudy)Activator.CreateInstance(typeToSet);
                return;
        }
        base.SetAbilityType(ability, type);
    }

    public override void ReadXml(XmlReader reader)
    {
        base.ReadXml(reader);
        Course = int.Parse(reader.GetAttribute("Course"));
        StudentId = reader.GetAttribute("StudentID");
        Sex = reader.GetAttribute("Sex");
        Residence = reader.GetAttribute("Residence");
        GradeBook = reader.GetAttribute("GradeBook");
        var type = reader.GetAttribute("StudyAbility");
        var typeToSet = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IStudy).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .FirstOrDefault(x => x.Name == type);
        if (typeToSet == null)
            throw new CustomException("This type does not exist!");
        StudyAbility = (IStudy)Activator.CreateInstance(typeToSet);
    }

    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);
        writer.WriteAttributeString("Course", Course.ToString());
        writer.WriteAttributeString("StudentID", StudentId);
        writer.WriteAttributeString("Sex", Sex);
        writer.WriteAttributeString("Residence", Residence);
        writer.WriteAttributeString("GradeBook", GradeBook);
        writer.WriteAttributeString("StudyAbility", StudyAbility.GetType().FullName);
    }

    public int Course { get; set; }
    public string StudentId { get; set; }
    public string Sex { get; set; }
    public string Residence { get; set; }
    public string GradeBook { get; set; }
    public IStudy StudyAbility { get; set; }
    private static readonly Regex studentIdRegex = new(@"^[А-Я]{2}-\d{8}$");
    private static readonly Regex gradeBookRegex = new(@"^\d{8}$");
}