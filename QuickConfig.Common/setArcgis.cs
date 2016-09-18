using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickConfig
{
   public class setArcgis
    {
        public static void init()
        {
            try
            {
                ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine);
            }
            catch (Exception eg)
            {
                throw eg;
            }
        }
    }
}
