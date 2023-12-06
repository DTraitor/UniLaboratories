using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace DataAccessLayer.Entities;

[Serializable]
public class Photographer : Entity
{
    public Photographer() : base() { }

    public Photographer(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        CameraModel = (string)info.GetValue("CameraModel", typeof(string));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("CameraModel", CameraModel);
    }

    public override string GetData()
    {
        return base.GetData() + $"Camera model: {CameraModel}\n";
    }

    public override List<string> GetEditableData()
    {
        return base.GetEditableData().Append("Camera model").ToList();
    }

    public override void SetData(string data, string value)
    {
        base.SetData(data, value);
        switch (data)
        {
            case "Camera model":
                CameraModel = value;
                break;
        }
    }

    public override void ReadXml(XmlReader reader)
    {
        base.ReadXml(reader);
        if (reader.MoveToAttribute("CameraModel") && reader.ReadAttributeValue())
            CameraModel = reader.Value;
    }

    public override void WriteXml(XmlWriter writer)
    {
        base.WriteXml(writer);
        writer.WriteAttributeString("CameraModel", CameraModel);
    }

    public string CameraModel;
}