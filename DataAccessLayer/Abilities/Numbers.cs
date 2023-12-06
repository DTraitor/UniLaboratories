using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DataAccessLayer.Abilities;

[JsonDerivedType(typeof(CanCalculate), "CanCalculate")]
[JsonDerivedType(typeof(CanNotCalculate), "CanNotCalculate")]
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
