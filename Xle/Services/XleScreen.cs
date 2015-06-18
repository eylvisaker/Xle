using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

using ERY.Xle.Rendering;
using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Services
{
    public class XleScreen : IXleScreen
    {
        private IXleRenderer renderer;
        private XleSystemState systemState;
        private IXleLegacyCore legacyCore;

        public XleScreen(
            IXleLegacyCore legacyCore,
            IXleRenderer renderer,
            XleSystemState systemState)
        {
            this.legacyCore = legacyCore;
            this.renderer = renderer;
            this.systemState = systemState;

            InitializeScreenSize();
        }

        private void InitializeScreenSize()
        {
            Rectangle coords = renderer.Coordinates;
            int height = coords.Height - systemState.WindowBorderSize.Height * 2;
            int width = (int)(320 / 200.0 * height);

            systemState.WindowBorderSize = new Size(
                (coords.Width - width) / 2,
                systemState.WindowBorderSize.Height);
        }

        public void RunRedrawLoop()
        {
            while (Display.CurrentWindow.IsClosed == false && systemState.ReturnToTitle == false)
            {
                Redraw();
            }
        }

        private void Redraw()
        {
            legacyCore.Redraw();
        }
    }
}
