using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuickConfig
{
    public class setMessage
    {
      public  static void MessageShow(string title,string message,Control control){
            ToolTip tooltip = new ToolTip();           
            tooltip.UseFading = true;
            tooltip.ShowAlways = true;
            //tooltip.IsBalloon = true;
            tooltip.ToolTipTitle = title;
            tooltip.Show(message, control, control.Bounds.X, control.Bounds.Y, 4000);
           // tooltip.Dispose();
        }
    }
}
