using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostrapperChecker
{
    public class BuildProject
    {
        public BuildProject(String name)
        {
            m_name = name;
        }

        public override string ToString()
        {
            return m_name;
        }

        String m_name;
    }
}
