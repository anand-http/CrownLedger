using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace fintech.Data
{
    public static class XMLHelper
    {
        public static string ConvertObjectToXml<T>(this T dataToSerialize, string RootElment)
        {
            try
            {
                //var stringwriter = new System.IO.StringWriter();
                //var serializer = new XmlSerializer(typeof(T));
                //serializer.Serialize(stringwriter, dataToSerialize);
                //return stringwriter.ToString();

                //if (String.IsNullOrEmpty(RootElment))
                //    RootElment = typeof(T).Name;
                //if (dataToSerialize == null)
                //    return "";
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T), new XmlRootAttribute(RootElment));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                // exclude xsi and xsd namespaces by adding the following:
                ns.Add(string.Empty, string.Empty);

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, dataToSerialize, ns);
                    }
                    return textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string ConvertDatatableToXML(DataTable dtTemp)
        {
            MemoryStream str = new MemoryStream();
            dtTemp.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }
        public static T ConvertXmlToObject<T>(this string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}