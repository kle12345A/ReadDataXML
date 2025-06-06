using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Linq;

namespace HelpReadFile
{
    public class ReadFileService : IReadFileService
    {
        public List<T> ConvertXmlToObject<T>(string xmlString) where T : class, new()
        {
            try
            {
                var result = new List<T>();
                var doc = new XmlDocument();
                doc.LoadXml(xmlString);

                // Lấy tất cả các node con của root
                var nodes = doc.DocumentElement.ChildNodes;
                
                foreach (XmlNode node in nodes)
                {
                    var instance = new T();
                    var properties = typeof(T).GetProperties();

                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                       
                        var property = properties.FirstOrDefault(p => p.Name.Equals(childNode.Name, StringComparison.OrdinalIgnoreCase));
                        
                        if (property != null)
                        {
                            try
                            {
                                var value = childNode.InnerText;
                                
                                if (property.PropertyType == typeof(string))
                                {
                                    property.SetValue(instance, value);
                                }
                                else if (property.PropertyType == typeof(int))
                                {
                                    if (int.TryParse(value, out int intValue))
                                    {
                                        property.SetValue(instance, intValue);
                                    }
                                }
                                else if (property.PropertyType == typeof(double) )
                                {
                                    if (double.TryParse(value, out double doubleValue))
                                    {
                                        property.SetValue(instance, doubleValue);
                                    }
                                }
                                else if (property.PropertyType == typeof(DateTime))
                                {
                                    if (DateTime.TryParse(value, out DateTime dateValue))
                                    {
                                        property.SetValue(instance, dateValue);
                                    }
                                }
                                else if (property.PropertyType == typeof(bool))
                                {
                                    if (bool.TryParse(value, out bool boolValue))
                                    {
                                        property.SetValue(instance, boolValue);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                               
                            }
                        }
                    }
                    result.Add(instance);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error converting XML to object: {ex.Message}", ex);
            }
        }

        public string GenerateSampleXml<T>(T sampleObject) where T : class, new()
        {
            try
            {
               
                var obj = sampleObject ?? new T();
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();
                XmlDocument doc = new XmlDocument();
                
                // Thêm XML declaration
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(xmlDeclaration);
                XmlElement objectElement = doc.CreateElement(type.Name);
                doc.AppendChild(objectElement);
                foreach (PropertyInfo pr in properties)
                {
                    XmlElement propElement = doc.CreateElement(pr.Name);
                    if (pr.PropertyType == typeof(string) || pr.PropertyType == typeof(DateTime?) || Nullable.GetUnderlyingType(pr.PropertyType) != null)
                    {
                        propElement.InnerText = "";
                    }
                    else
                    {
                        
                        propElement.InnerText = "0";
                    }

                    objectElement.AppendChild(propElement);
                }
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(sw))
                    {
                        writer.Formatting = Formatting.Indented;
                        writer.Indentation = 2;
                        doc.WriteTo(writer);
                    }
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo XML mẫu: {ex.Message}", ex);
            }
        }

        
    }
} 