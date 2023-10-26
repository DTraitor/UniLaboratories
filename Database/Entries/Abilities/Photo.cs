using System.Text.Json.Serialization;

namespace Database.Entries.Abilities;

[JsonDerivedType(typeof(CanPhoto), "canPhoto")]
[JsonDerivedType(typeof(CanNotPhoto), "cantPhoto")]
public abstract class PhotoAbility : EntryAbility
{
    public abstract string TakePhoto();
}

[DatabaseAbilityDescription("Can take photos.")]
public class CanPhoto : PhotoAbility
{
    public override string TakePhoto() => "They took a photo.";
}

[DatabaseAbilityDescription("Can't take photos.")]
public class CanNotPhoto : PhotoAbility
{
    public override string TakePhoto() => "They are unable to take photos.";
}
