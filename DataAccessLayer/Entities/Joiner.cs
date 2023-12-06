using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace DataAccessLayer.Entities;

[Serializable]
public class Joiner : Entity
{
    public Joiner() : base() {}

    public Joiner(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Price = (int)info.GetValue("Price", typeof(int));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Price", Price);
    }

    public override string GetData()
    {
        return base.GetData() + $"Price: {Price}\n";
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
                    this.Price = price;
                else
                    throw new CustomException("Should be an integer!");
                break;
        }
    }

    public override void ReadXml(XmlReader reader)
    {
        base.ReadXml(reader);
        if (reader.MoveToAttribute("Price") && reader.ReadAttributeValue())
            Price = int.Parse(reader.Value);
    }

    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);
        writer.WriteAttributeString("Price", Price.ToString());
    }

    public int Price;
}