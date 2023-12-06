using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DataAccessLayer.Abilities;

[XmlInclude(typeof(CanStudy))]
[XmlInclude(typeof(CanNotStudy))]
[JsonDerivedType(typeof(CanStudy), "CanStudy")]
[JsonDerivedType(typeof(CanNotStudy), "CanNotStudy")]
public interface IStudy
{
    public delegate void StudyHandler(bool studied);
    public string Study(StudyHandler studyHandler);
}

public class CanStudy : IStudy
{
    public string Study(IStudy.StudyHandler studyHandler)
    {
        studyHandler(true);
        return "I can study";
    }
}

public class CanNotStudy : IStudy
{
    public string Study(IStudy.StudyHandler studyHandler)
    {
        studyHandler(false);
        return "I can not study";
    }
}