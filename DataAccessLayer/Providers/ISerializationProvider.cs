namespace DataAccessLayer.Providers;

internal interface ISerializationProvider<T> where T : class
{
    public List<T> Read();
    public void Write(List<T> entities);
}