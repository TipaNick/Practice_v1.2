using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace test_ucheb_2
{
    internal class Alarm
    {
        public static string state = "safe";

        Label body;

        public Alarm(Panel panel, int x, int y)
        {
            body = new Label();
            body.Location = new Point(x, y);
            body.AutoSize = true;
            body.Name = "alarm";
            body.Text = "A";
            body.BackColor = Color.LightGreen;
            panel.Controls.Add(body);
        }
    }
}
