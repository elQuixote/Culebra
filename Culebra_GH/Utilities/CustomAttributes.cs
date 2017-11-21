using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using GH_IO;
using GH_IO.Serialization;
using Grasshopper.GUI.Canvas;
using System.Drawing;

namespace Culebra_GH.Utilities
{
    public class CustomAttributes : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {
        public int type = new int();
        public CustomAttributes(IGH_Component component, int type) : base(component)
        {
            this.type = type;
        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            Grasshopper.GUI.Canvas.GH_PaletteStyle styleStandard = null;
            Grasshopper.GUI.Canvas.GH_PaletteStyle styleSelected = null;

            Grasshopper.GUI.Canvas.GH_PaletteStyle styleStandard_Hidden = null;
            Grasshopper.GUI.Canvas.GH_PaletteStyle styleSelected_Hidden = null;
            if (channel == GH_CanvasChannel.Objects)
            {
                // Cache the current styles.
                styleStandard = GH_Skin.palette_normal_standard;
                styleSelected = GH_Skin.palette_normal_selected;

                styleStandard_Hidden = GH_Skin.palette_hidden_standard;
                styleSelected_Hidden = GH_Skin.palette_hidden_selected;
                if (this.type == 0)
                {
                    GH_Skin.palette_normal_standard = new GH_PaletteStyle(Color.Black, Color.Black, Color.Chartreuse);
                    GH_Skin.palette_normal_selected = new GH_PaletteStyle(Color.Chartreuse, Color.Red, Color.Black);
                    GH_Skin.palette_hidden_standard = new GH_PaletteStyle(Color.Black, Color.Black, Color.Chartreuse);
                    GH_Skin.palette_hidden_selected = new GH_PaletteStyle(Color.Chartreuse, Color.Red, Color.Black);
                }
                if (this.type == 1)
                {
                    GH_Skin.palette_normal_standard = new GH_PaletteStyle(Color.Black, Color.Black, Color.DarkGray);
                    GH_Skin.palette_normal_selected = new GH_PaletteStyle(Color.DarkGray, Color.Red, Color.Black);
                    GH_Skin.palette_hidden_standard = new GH_PaletteStyle(Color.Black, Color.Black, Color.DarkGray);
                    GH_Skin.palette_hidden_selected = new GH_PaletteStyle(Color.DarkGray, Color.Red, Color.Black);
                }
                if (this.type == 2)
                {
                    GH_Skin.palette_normal_standard = new GH_PaletteStyle(Color.Black, Color.Black, Color.HotPink);
                    GH_Skin.palette_normal_selected = new GH_PaletteStyle(Color.HotPink, Color.Red, Color.Black);
                    GH_Skin.palette_hidden_standard = new GH_PaletteStyle(Color.Black, Color.Black, Color.HotPink);
                    GH_Skin.palette_hidden_selected = new GH_PaletteStyle(Color.HotPink, Color.Red, Color.Black);
                }
            }
            // Allow the base class to render itself.
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                // Restore the cached styles.
                GH_Skin.palette_normal_standard = styleStandard;
                GH_Skin.palette_normal_selected = styleSelected;
                GH_Skin.palette_hidden_standard = styleStandard_Hidden;
                GH_Skin.palette_hidden_selected = styleSelected_Hidden;
            }
        }
    }
}
