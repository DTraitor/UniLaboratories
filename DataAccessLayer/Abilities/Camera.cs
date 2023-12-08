using System.Text.Json.Serialization;

namespace DataAccessLayer.Abilities;

[JsonDerivedType(typeof(CanCalculate), "CanCalculate")]
[JsonDerivedType(typeof(CanNotCalculate), "CanNotCalculate")]
public interface ICamera
{
    public string Photo();
}

public class ProfessionalCamera : ICamera
{
    public string Photo()
    {
        return "I use professional camera";
    }
}

public class PhoneCamera : ICamera
{
    public string Photo()
    {
        return "I use camera in my phone";
    }
}