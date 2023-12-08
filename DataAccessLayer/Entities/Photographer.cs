using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using DataAccessLayer.Abilities;

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

    public override List<string> GetAbilities()
    {
        return base.GetAbilities().Append("Camera").ToList();
    }

    public override string UseAbility(string ability)
    {
        switch (ability)
        {
            case "Camera":
                return Camera.Photo();
            default:
                return base.UseAbility(ability);
        }
    }

    public override List<string> GetAbilityTypes(string ability)
    {
        switch (ability)
        {
            case "Camera":
                return new List<string>() { "ProfessionalCamera", "PhoneCamera" };
            default:
                return base.GetAbilityTypes(ability);
        }
    }

    public override void SetAbilityType(string ability, string type)
    {
        switch (ability)
        {
            case "Camera":
                switch (type)
                {
                    case "ProfessionalCamera":
                        Camera = new ProfessionalCamera();
                        break;
                    case "PhoneCamera":
                        Camera = new PhoneCamera();
                        break;
                    default:
                        base.SetAbilityType(ability, type);
                        break;
                }
                break;
            default:
                base.SetAbilityType(ability, type);
                break;
        }
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
    public ICamera Camera;
}