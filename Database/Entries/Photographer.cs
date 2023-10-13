using Database.Entries.Abilities;

namespace Database.Entries;

public class Photographer : Entry
{
    [DatabaseAbilityFunction("Take Photo")]
    public string TakePhotoAbility() => Photo.TakePhoto();
    [DatabaseAbilityVariable("Take Photo")]
    public IPhoto Photo { get; set; }
}