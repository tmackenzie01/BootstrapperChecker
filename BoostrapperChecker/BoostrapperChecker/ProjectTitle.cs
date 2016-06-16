using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostrapperChecker
{
    public class ProjectTitle : IComparable, IEquatable<ProjectTitle>
    {
        public ProjectTitle(String name)
        {
            Name = name;
            CorrectSpacing = !name.StartsWith(" ");
        }

        // For equality
        public bool Equals(ProjectTitle other)
        {
            if (other != null)
            {
                // Name is the only thing that matters
                return Name.Equals(other.Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        // For ordering
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            ProjectTitle other = obj as ProjectTitle;
            if (other != null)
            {
                return Name.CompareTo(other.Name);
            }
            else
            {
                throw new ArgumentException("Object is not a ProjectTitle");
            }
        }

        public bool CorrectSpacing { get; set; }
        public String Name { get; set; }
    }
}
