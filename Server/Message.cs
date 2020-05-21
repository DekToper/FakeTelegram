using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    [XmlRoot(ElementName = "Message")]
    public class Message
    {
        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }
        [XmlElement(ElementName = "Time")]
        public string Time { get; set; }
        [XmlElement(ElementName = "fromUser")]
        public string fromUser { get; set; }
    }

    [XmlRoot(ElementName = "Messages")]
    public class Messages
    {
        [XmlElement(ElementName = "Message")]
        public List<Message> Message { get; set; }
    }

    public static class Serializer
    {
        public static T Deserialize<T>(string path) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            TextReader tr = new System.IO.StreamReader(path);
            using (XmlReader xr = new XmlTextReader(tr))
            {
                return (T)ser.Deserialize(xr);
            }
        }



        public static string Serialize(Task objectToSerialize)
        {
            XmlSerializer ser = new XmlSerializer(objectToSerialize.GetType());
            using (StringWriter sw = new StringWriter())
            {
                ser.Serialize(sw, objectToSerialize);
                return sw.ToString();
            }
        }
    }
}