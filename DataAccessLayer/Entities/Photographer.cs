using System.Runtime.Serialization;

namespace DataAccessLayer.Entities;

internal class Photographer : Entity
{
    public Photographer() : base() { }

    public Photographer(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        cameraModel = (string)info.GetValue("CameraModel", typeof(string));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("CameraModel", cameraModel);
    }

    public override string GetData()
    {
        return base.GetData() + $"Camera model: {cameraModel}\n";
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
                cameraModel = value;
                break;
        }
    }

    private string cameraModel;
}