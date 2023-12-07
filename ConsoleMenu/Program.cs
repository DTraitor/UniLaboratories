using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;
using ConsoleMenu;

//Disable obsolete warning
#pragma warning disable SYSLIB0011

List<CustomString> stringsListExample = new List<CustomString>
{
    new ("Change", 1),
    new ("da world...", -1),
    new ("My", 2),
    new ("final", -2),
    new ("message.", 3),
    new ("Goodbye!", -3),
};
List<CustomString> stringsList;

var stream = new FileStream("databaseFile", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

//Example of JSON serialization
stream.SetLength(0);
using (StreamWriter writer = new StreamWriter(stream, leaveOpen: true))
{
    writer.Write(JsonSerializer.Serialize(stringsListExample));
}

using (StreamReader reader = new StreamReader(stream, leaveOpen: true))
{
    stringsList = JsonSerializer.Deserialize<List<CustomString>>(reader.ReadToEnd());
}

//Example of binary serialization
stream.SetLength(0);
BinaryFormatter formatter = new BinaryFormatter();
using (StreamWriter writer = new StreamWriter(stream, leaveOpen: true))
{
    formatter.Serialize(writer.BaseStream, stringsListExample);
}

using (StreamReader reader = new StreamReader(stream, leaveOpen: true))
{
    stringsList = (List<CustomString>)formatter.Deserialize(reader.BaseStream);
}

//Example of XML serialization
stream.SetLength(0);
XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<CustomString>));
using (StreamWriter writer = new StreamWriter(stream, leaveOpen: true))
{
    xmlSerializer.Serialize(writer.BaseStream, stringsListExample);
}

using (StreamReader reader = new StreamReader(stream, leaveOpen: true))
{
    stringsList = (List<CustomString>)xmlSerializer.Deserialize(reader.BaseStream);
}
