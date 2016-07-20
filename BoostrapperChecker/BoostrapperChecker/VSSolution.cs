using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostrapperChecker
{
    public class VSSolution
    {
        public VSSolution(String solutionNickname, String solutionDir)
        {
            // Solution nickname is because solution/overarching project name can all be slightly different than the filenames
            m_solutionNickname = solutionNickname;
            m_solutionDir = solutionDir;
        }

        public bool ParseSolution()
        {
            if (Directory.Exists(m_solutionDir))
            {
                String sourceDir = Path.Combine(m_solutionDir, "source");
                String[] solutionFiles = Directory.GetFiles(sourceDir, "*.sln");

                if (solutionFiles != null)
                {
                    if (solutionFiles.Length == 1)
                    {
                        Debug.WriteLine($"Solution file located for {m_solutionNickname}: {solutionFiles[0]}");

                        // Parse solution file for solution projects

                        // Parse project files and their references
                        return true;
                    }
                }
            }

            return false;
        }

        String m_solutionNickname;
        String m_solutionDir;
    }
}
