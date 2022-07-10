using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace test_ucheb_2
{
    internal class Fireman
    {
        public string Loc;
        public string goal;
        public string path;
        public string last_corr;

        int check = 0;

        public bool done = false;

        public bool transfer;

        string[] mas_room = new string[13] { "R13", "R8", "R7", "R5", "R6", "R4", "R3", "R1", "R2", "R9", "R10", "R11", "R12" };

        public Label body;

        public Fireman(Panel panel)
        {
            Loc = "C1";
            goal = mas_room[check];
            path = "C2";

            body = new Label();
            body.Name = "Fireman";
            body.Text = " f" +
                "\n/|\\\n" +
                "/ \\";
            body.AutoSize = true;
            panel.Controls.Add(body);
            body.BringToFront();
        }

        public void Action(Panel room)
        {
            if (check == 13 && Loc == "C1")
            {
                done = true;
            } 
            else
            {
                FindFire(room);
                FindPath(room);
                MoveTo(room);
            }
        }

        private void FindFire(Panel room)
        {
            foreach(Label label in room.Controls)
            {
                if(label.Name == "f")
                {
                    room.Controls.Remove(label);
                }
            }
        }

        private void FindPath(Panel room)
        {
            bool find_path = false;
            if (Loc[0] == 'R')
            {
                foreach (Label path in room.Controls)
                {
                    if (path.Name[0] == 'C')
                    {
                        this.path = path.Name;
                    }
                }
            }
            else if (Loc[0] == 'C')
            {
                foreach (Label path in room.Controls)
                {
                    if (path.Name == this.goal)
                    {
                        this.path = path.Name;
                        find_path = true;
                    }
                }

                if (!find_path)
                {
                    switch (this.Loc)
                    {
                        case "C2":
                            if (last_corr == null)
                            {
                                path = "C4";
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
        


        public void NextGoal()
        {
            if (check == 13)
            {
                goal = "C1";
            }
            else
            {
                goal = mas_room[check];
                check += 1;
            }
                
        }

        private void MoveTo(Panel room)
        {
            Label exit = (Label)room.Controls[path];
            if (exit.Location.X == body.Location.X && exit.Location.Y == body.Location.Y)
            {
                transfer = true;
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
        private void Move(int x, int y, Panel room)
        {
            int x_body = body.Location.X;
            int y_body = body.Location.Y;

            if (x > 0)
            {
                if (x_body < room.Width - body.Width / 2)
                    body.Location = new Point(x_body += x, y_body);
            }
            else
            {
                if (x_body > 0 - body.Width / 2)
                    body.Location = new Point(x_body += x, y_body);
            }

            if (y > 0)
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
    }
}
