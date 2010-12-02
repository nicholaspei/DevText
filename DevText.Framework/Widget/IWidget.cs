using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevText.Framework.Widget
{
    public interface IWidget
    {
        string Name { get; }
        string Slug { get; }
        string Version { get; } 
        string  Author {get; }
        string LicenseName { get; }
        IEnumerable<string> AllowedRoles { get; }
    }
}
