using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostrapperChecker
{
    public interface IXmlLoader
    {
        String LoadFromFile(String filename);
    }

    public class XmlLoader : IXmlLoader
    {
        public String LoadFromFile(String filename)
        {
            return File.ReadAllText(filename);
        }
    }
}
