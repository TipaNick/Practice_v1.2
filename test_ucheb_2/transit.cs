using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace test_ucheb_2
{
    internal class Transit
    {
        public string in_pan;
        public string out_pan;

        public Transit(Panel frst, int x1, int y1, Panel scnd, int x2, int y2)
        {
            in_pan = frst.Name;
            out_pan = scnd.Name;

            Label in_id = new Label();
            Label out_id = new Label();

            in_id.Visible = false;
            out_id.Visible = false;

            ToolTip toolTip = new ToolTip();
            ToolTip toolTip2 = new ToolTip();

            in_id.Text = "E"; 
            out_id.Text = "E";

            in_id.AutoSize = true;
            out_id.AutoSize = true;

            in_id.Name = scnd.Name;
            out_id.Name = frst.Name;

            in_id.Location = new Point(x1, y1);
            out_id.Location = new Point(x2, y2);

            frst.Controls.Add(in_id);
            scnd.Controls.Add(out_id);

            toolTip.SetToolTip(in_id, "Name: " + in_id.Name);
            toolTip2.SetToolTip(out_id, "Name: " + out_id.Name);

        }
    }
}
