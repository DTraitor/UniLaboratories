namespace DataAccessLayer.Abilities;

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

public class CanNotStudy
{
    public string Study(IStudy.StudyHandler studyHandler)
    {
        studyHandler(false);
        return "I can not study";
    }
}