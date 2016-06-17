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
        String Load();
        String LoadFromFile(String filename);
    }

    public class XmlLoader : IXmlLoader
    {
        public XmlLoader()
        {
        }

        public XmlLoader(String xmlText)
        {
            m_xmlText = xmlText;
        }

        public String Load()
        {
            return m_xmlText;
        }

        public String LoadFromFile(String filename)
        {
            m_xmlText = File.ReadAllText(filename);
            return m_xmlText;
        }

        String m_xmlText;
    }
}
