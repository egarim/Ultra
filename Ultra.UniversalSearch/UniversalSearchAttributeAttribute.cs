using System;
using System.Linq;

namespace Ultra.UniversalSearch.BusinessObjects
{
    public class UniversalSearchAttributeAttribute : Attribute
    {
        public string DisplayProperties { get; set; }

        public UniversalSearchAttributeAttribute(string displayProperties)
        {
            DisplayProperties = displayProperties;
        }
    }
}