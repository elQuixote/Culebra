using System;
using System.Collections.Generic;
using System.Linq;

namespace CulebraData.Utilities
{
    /// <summary>
    /// Provides a set of general Utilities
    /// </summary>
    public static class General
    {
        /// <summary>
        /// Sets the viewport in rhino
        /// </summary>
        /// <param name="view">the name of the view</param>
        /// <param name="displayType">the display type, wireframe, shaded etc</param>
        public static void setViewport(String view, String displayType)
        {
            //-------Set viewport to specific view and maximize it & set display type--------
            Rhino.DocObjects.Tables.NamedViewTable nvt = Rhino.RhinoDoc.ActiveDoc.NamedViews;
            Rhino.DocObjects.Tables.ViewTable vt = Rhino.RhinoDoc.ActiveDoc.Views;
            Rhino.Display.RhinoView[] rvs = vt.GetViewList(true, false);

            List<Rhino.Display.RhinoView> st = rvs.ToList();
            List<String> viewList = new List<String>();
            int viewCount = 0;
            foreach (Rhino.Display.RhinoView v in st)
            {
                if (v.ActiveViewport.Name == view)
                {
                    Rhino.Display.RhinoViewport rvp = rvs[viewCount].ActiveViewport;
                    rvs[viewCount].Maximized = true;
                }
                viewCount++;
            }
            var type = Rhino.Display.DisplayModeDescription.FindByName(displayType);
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.DisplayMode = type;
        }
    }
}
