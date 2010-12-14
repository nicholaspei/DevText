using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevText.Framework.Widget
{
    public interface IWidgetManager
    {
         IEnumerable<IWidget> RegisteredWidgets { get; set; }
    }
}
