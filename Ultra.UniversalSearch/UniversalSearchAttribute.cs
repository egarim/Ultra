using DevExpress.ExpressApp.Utils;
using System;
using System.Linq;

namespace Ultra.UniversalSearch
{
    public class UniversalSearchAttribute : Attribute
    {
        public string DisplayProperties { get; set; }
        public string DisplayPropertiesStringFormat { get; set; }

        public UniversalSearchAttribute(string DisplayProperties, string DisplayPropertiesStringFormat)
        {
            Guard.ArgumentNotNullOrEmpty(DisplayProperties, nameof(DisplayProperties));
            Guard.ArgumentNotNullOrEmpty(DisplayPropertiesStringFormat, nameof(DisplayPropertiesStringFormat));

            this.DisplayProperties = DisplayProperties;
            this.DisplayPropertiesStringFormat = DisplayPropertiesStringFormat;
        }
    }
}