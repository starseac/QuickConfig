using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace QuickConfig
{
    public class setMessage
    {
      public  static void MessageShow(string title,string message,Control control){
            ToolTip tooltip = new ToolTip();           
            tooltip.UseFading = true;
            tooltip.ShowAlways = true;
            tooltip.IsBalloon = true;
            tooltip.ToolTipTitle = title;

            tooltip.ForeColor = Color.Blue;
            tooltip.BackColor = Color.Chocolate;
            
            tooltip.Show(message, control, 20, -50, 3000);
           // tooltip.Dispose();
        }
    }
}
