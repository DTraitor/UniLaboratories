using System.Runtime.Serialization;

namespace DataAccessLayer.Entities;

internal class Joiner : Entity
{
    public Joiner() : base() {}

    public Joiner(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        price = (int)info.GetValue("Price", typeof(int));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Price", price);
    }

    public override string GetData()
    {
        return base.GetData() + $"Price: {price}\n";
    }

    public override List<string> GetEditableData()
    {
        return base.GetEditableData().Append("Price").ToList();
    }

    public override void SetData(string data, string value)
    {
        base.SetData(data, value);
        switch (data)
        {
            case "Price":
                if (int.TryParse(value, out var price))
                    this.price = price;
                else
                    throw new CustomException("Should be an integer!");
                break;
        }
    }

    private int price;
}