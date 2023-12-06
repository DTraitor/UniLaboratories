using System.Xml.Serialization;
using DataAccessLayer.Entities;

namespace DataAccessLayer;

public class EntityList
{
    [XmlArray("Entities")]
    [XmlArrayItem("Photographer", typeof(Photographer))]
    [XmlArrayItem("Joiner", typeof(Joiner))]
    [XmlArrayItem("Student", typeof(Student))]
    public List<Entity> Entities { get; set; } = new List<Entity>();
}