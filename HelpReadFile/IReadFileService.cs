using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpReadFile
{
    public interface IReadFileService
    {
        
        List<T> ConvertXmlToObject<T>(string xmlString) where T : class, new();

        
        string GenerateSampleXml<T>(T sampleObject) where T : class, new();
    }
} 