namespace DataAccessLayer.Abilities;

public interface ICalculateBigNumbers
{
    public string Calculate();
}

public class CanCalculate : ICalculateBigNumbers
{
    public string Calculate()
    {
        return "I can calculate big numbers";
    }
}

public class CanNotCalculate : ICalculateBigNumbers
{
    public string Calculate()
    {
        return "I can not calculate big numbers";
    }
}
