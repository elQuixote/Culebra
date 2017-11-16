using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Culebra_GH
{
    public class Culebra_GHInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Culebra_GH";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Culebra is a live agent based plugin for Grasshopper.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("e13fa471-7357-4f05-90cf-a17f5109d37f");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Luis Quinones";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "luis@complicitMatter.com";
            }
        }
    }
}
