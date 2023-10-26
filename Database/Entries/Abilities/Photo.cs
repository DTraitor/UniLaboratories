using System.Text.Json.Serialization;

namespace Database.Entries.Abilities;

[JsonDerivedType(typeof(CanPhoto), "canPhoto")]
[JsonDerivedType(typeof(CanNotPhoto), "cantPhoto")]
public interface IPhoto : IAbility
{
    public string TakePhoto();
}

[DatabaseAbilityDescription("Can take photos.")]
public class CanPhoto : IPhoto
{
    public string TakePhoto() => "They took a photo.";
}

[DatabaseAbilityDescription("Can't take photos.")]
public class CanNotPhoto : IPhoto
{
    public string TakePhoto() => "They are unable to take photos.";
}
