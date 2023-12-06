namespace DataAccessLayer.Providers;

internal interface ISerializationProvider
{
    public EntityList Read();
    public void Write(EntityList entities);
}