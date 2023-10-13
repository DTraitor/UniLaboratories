namespace Database.Entries.Abilities;

public interface IPhoto
{
    public string TakePhoto();
}

[DatabaseAbilityDescription("Can take photos.")]
public class CanPhoto
{
    public string TakePhoto() => "They took a photo.";
}

[DatabaseAbilityDescription("Can't take photos.")]
public class CanNotPhoto
{
    public string TakePhoto() => "They are unable to take photos.";
}
