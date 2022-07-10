using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace test_ucheb_2
{
    internal class Person
    {
        static int id = 0;

        public string Loc;

        public string status;
        public string goal;
        public string path;
        public string last_corr;
        public static string alarm = "safe";

        public bool transfer;

        public bool fireman_work = false;

        public Label body;

        public Timer timer;

        ToolTip info = new ToolTip();

        public Person(Panel panel)
        {
            body = new Label();
            body.AutoSize = true;
            body.Text = " o\n/|\\\n/ \\";
            body.Name = "person" + id.ToString();
            Loc = panel.Name;
            panel.Controls.Add(body);
            body.BringToFront();
            body.Location = new Point(panel.Width / 2 - body.Width / 2, panel.Height / 2 - body.Height / 2);
            transfer = false;
            status = "safe";
            id++;
            timer = new Timer();
            timer.Interval = 4000;
            timer.Tick += Timer_Tick;
            Update();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (status == "job")
            {
                timer.Stop();
                status = "safe";
                
            }
        }

        public void Action(Panel room)
        {
            FindFire(room);
            switch (status)
            {
                case "safe":
                    break;
                case "job":
                    fireman_work = false;
                    break;
                case "fire":
                case "goal":
                    Find_Goal(room);
                    MoveTo(room);
                    break;
            }
            Update();
        }
        
        private void FindFire(Panel room)
        {
            foreach (Label label in room.Controls)
            {
                if (label.Text == "f")
                {
                    status = "fire";
                    goal = "C1";
                }
            }
            if (alarm == "fire")
            {
                status = "fire";
                goal = "C1";
            }
        }

        private void Find_Goal(Panel room)
        {
            bool find_goal = false;
            if(status == "fire" && alarm != "fire")
            {
                foreach(Label goal in room.Controls)
                {
                    if(goal.Name == "alarm")
                    {
                        path = goal.Name;
                        find_goal = true;
                    }
                }
            }
            if (!find_goal)
            {
                if (Loc == "C1" && fireman_work == false)
                {
                    status = "safe";
                }
                else
                {
                    if (Loc[0] == 'R')
                    {
                        foreach (Label goal in room.Controls)
                        {
                            if (goal.Name[0] == 'C')
                            {
                                path = goal.Name;
                            }
                        }
                    }
                    else if (Loc[0] == 'C')
                    {
                        foreach (Label goal in room.Controls)
                        {
                            if (goal.Name == this.goal)
                            {
                                path = goal.Name;
                                find_goal = true;
                            }
                        }
                        if (!find_goal)
                        {
                            switch (this.Loc)
                            {
                                case "C1":
                                    path = "C2";
                                    break;
                                case "C2":
                                    if (last_corr == null && path != "C3" && path != "C4")
                                    {
                                        int C3_X = 0;
                                        int C4_X = 0;
                                        foreach (Label goal in room.Controls)
                                        {
                                            if (goal.Name == "C3")
                                            {
                                                C3_X = goal.Location.X;
                                            }
                                            if (goal.Name == "C4")
                                            {
                                                C4_X = goal.Location.X;
                                            }
                                        }

                                        if (Math.Abs(C3_X - body.Location.X) < Math.Abs(C4_X - body.Location.X))
                                        {
                                            path = "C3";
                                        }
                                        else
                                        {
                                            path = "C4";
                                        }
                                    }
                                    if (last_corr == "C3")
                                    {
                                        path = "C4";
                                    }

                                    if (last_corr == "C4")
                                    {
                                        path = "C3";
                                    }
                                    break;
                                case "C3":
                                    path = "C2";
                                    last_corr = "C3";
                                    break;

                                case "C4":
                                    path = "C2";
                                    last_corr = "C4";
                                    break;
                            }
                        }
                    }
                }
            }
        }
        
        private void MoveTo(Panel room)
        {
            if(status != "safe")
            {
                Label exit = (Label)room.Controls[path];
                if (exit.Location.X == body.Location.X && exit.Location.Y == body.Location.Y)
                {
                    if(exit.Name == "alarm")
                    {
                        alarm = "fire";
                    } 
                    else
                    {
                        transfer = true;
                    }
                }
                else
                {
                    if (exit.Location.X > body.Location.X)
                    {
                        Move(1, 0, room);
                    }
                    else if (exit.Location.X < body.Location.X)
                    {
                        Move(-1, 0, room);
                    }

                    if (exit.Location.Y > body.Location.Y)
                    {
                        Move(0, -1, room);
                    }
                    else if (exit.Location.Y < body.Location.Y)
                    {
                        Move(0, 1, room);
                    }
                }
            }
        }

        private void Move(int x, int y, Panel room)
        {
            int x_body = body.Location.X;
            int y_body = body.Location.Y;

            if(x > 0)
            {
                if (x_body < room.Width - body.Width / 2)
                    body.Location = new Point(x_body += x, y_body);
            }
            else
            {
                if (x_body > 0 - body.Width / 2)
                    body.Location = new Point(x_body += x, y_body);
            }

            if(y > 0)
            {
                if (y_body > 0 - body.Height / 2)
                    body.Location = new Point(x_body, y_body -= y);
            } 
            else
            {
                if (y_body < room.Height - body.Height)
                    body.Location = new Point(x_body, y_body -= y);
            }
        }

        public void StartJob(Panel room)
        {
            status = "job";
            Random rand = new Random();
            timer.Interval = rand.Next(2, 5) * 1000;
            timer.Start();
        }

        private void Update()
        {
            info.SetToolTip(body,
                "id: " + body.Name +
                "\nLoc: " + Loc +
                "\nStatus: " + status +
                "\nX: " + Convert.ToString(body.Location.X) +
                "\nY: " + Convert.ToString(body.Location.Y) +
                "\nGoal: " + goal +
                "\nPath: " + path +
                "\nLast: " + last_corr
                );
        }
    }
}
