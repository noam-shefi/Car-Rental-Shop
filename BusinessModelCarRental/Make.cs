using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModelCarRental
{
    // Make
    public class Make
    {
        // MakeName
        private int MakeID;
        public int MyMakeID { get { return this.MakeID; } set { this.MakeID = value; } }

        // MakeName
        private string MakeName;
        public string MyMakeName { get { return this.MakeName; } set { this.MakeName = value; } }

        // CountryOfOrigin
        private string CountryOfOrigin;
        public string MyCountryOfOrigin { get { return this.CountryOfOrigin; } set { this.CountryOfOrigin = value; } }

        // WebsiteURL
        private string WebsiteURL;
        public string MyWebsiteURL { get { return this.WebsiteURL; } set { this.WebsiteURL = value; } }

        // Description
        private string Description;
        public string MyDescription { get { return this.Description; } set { this.Description = value; } }
    }
}
