using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_ucheb_2
{
    public partial class Form1 : Form
    {

        Person person;

        Transit transit;

        Alarm alarm;

        Fireman fireman;

        List<Person> persons = new List<Person>();

        string[] mas_room = new string[13] { "R1", "R2", "R3", "R4", "R5", "R6", "R7", "R8", "R9", "R10", "R11", "R12", "R13" };

        bool fireman_ex = false;


        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            R1.Click += Room_Click;
            R2.Click += Room_Click;
            R3.Click += Room_Click;
            R4.Click += Room_Click;
            R5.Click += Room_Click;
            R6.Click += Room_Click;
            R7.Click += Room_Click;
            R8.Click += Room_Click;
            R9.Click += Room_Click;
            R10.Click += Room_Click;
            R11.Click += Room_Click;
            R12.Click += Room_Click;
            R13.Click += Room_Click;

            C2.Click += Room_Click;
            C3.Click += Room_Click;
            C4.Click += Room_Click;

            transit = new Transit(R1, 85, 45, C3, 0, 46);
            transit = new Transit(R2, 85, 45, C3, 0, 146);
            transit = new Transit(R3, 0, 45, C3, 87, 46);
            transit = new Transit(R4, 0, 45, C3, 87, 146);
            transit = new Transit(R5, 85, 45, C4, 0, 46);
            transit = new Transit(R6, 85, 45, C4, 0, 146);
            transit = new Transit(R7, 0, 45, C4, 87, 46);
            transit = new Transit(R8, 0, 45, C4, 87, 146);

            transit = new Transit(R9, 42, 2, C2, 43, 61);
            transit = new Transit(R10, 42, 2, C2, 143, 61);
            transit = new Transit(R11, 42, 2, C2, 243, 61);
            transit = new Transit(R12, 42, 2, C2, 343, 61);
            transit = new Transit(R13, 42, 2, C2, 543, 61);

            transit = new Transit(C2, 443, 61, C1, 42, 2);
            transit = new Transit(C2, 145, 3, C3, 45, 161);
            transit = new Transit(C2, 445, 4, C4, 45, 161);

            alarm = new Alarm(C2, 250, 0);

            person = new Person(R9);
            persons.Add(person);
            person = new Person(R11);
            persons.Add(person);
            person = new Person(R1);
            persons.Add(person);
            person = new Person(R7);
            persons.Add(person);
            person = new Person(R13);
            persons.Add(person);
        }


        private void Room_Click(object sender, EventArgs e)
        {
            Panel room = (Panel)sender;
            if (!room.Contains(room.Controls["f"]))
            {
                Label fire = new Label();
                fire.BackColor = Color.Orange;
                fire.Name = "f";
                fire.Text = "f";
                fire.AutoSize = true;
                room.Controls.Add(fire);
            } else
            {
                room.Controls.Remove(room.Controls["f"]);
            }
        }

        private void Job()
        {
            foreach(Person person in persons)
            {
                if(person.status == "safe")
                {
                    person.status = "goal";
                    person.goal = mas_room[random.Next(mas_room.Length)];
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                button1.Text = "START";
                timer1.Stop();
            }
            else
            {
                button1.Text = "STOP";
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Job();
            foreach(Person person in persons)
            {
                if (person.transfer)
                {
                    string curr_loc = person.Loc;
                    string next_loc = person.path;

                    Panel panel_curr = (Panel)panel2.Controls[curr_loc];
                    panel_curr.Controls.Remove(person.body);

                    Panel panel_next = (Panel)panel2.Controls[next_loc];
                    panel_next.Controls.Add(person.body);
                    person.body.BringToFront();
                    Label point = (Label)panel2.Controls[person.path].Controls[person.Loc];

                    int point_corr_x = panel_next.Width - point.Location.X;
                    int point_corr_y = panel_next.Height - point.Location.Y;

                    if (point_corr_x < person.body.Width)
                    {
                        point_corr_x = person.body.Width / 2;
                    }
                    else
                    {
                        point_corr_x = 0;
                    }

                    if (point_corr_y < person.body.Height)
                    {
                        point_corr_y = person.body.Height - point.Height / 2;
                    }
                    else
                    {
                        point_corr_y = 0;
                    }

                    person.body.Location = new Point(point.Location.X - point_corr_x, point.Location.Y - point_corr_y);
                    person.Loc = panel_next.Name;

                    if (person.Loc == person.goal)
                    {
                        if (person.status == "goal")
                        {
                            person.StartJob(panel_next);
                        }
                        else if (person.status == "fire")
                        {

                        }
                    }

                    person.transfer = false;
                }

                Panel panel = (Panel)panel2.Controls[person.Loc];
                person.Action(panel);
            }
            AfterFire();
            
        }


        public void AfterFire()
        {
            bool allin = true;
            bool work = false;
            foreach(Person person in persons)
            {
                if(person.Loc != "C1")
                {
                    allin = false;
                }
                if (person.fireman_work == true)
                {
                    work = true;
                }
            }
            if (allin && !fireman_ex && work == false)
            {
                fireman = new Fireman(C1);
                fireman_ex = true;
            }
            if (allin && fireman_ex)
            {
                Panel panel = (Panel)panel2.Controls[fireman.Loc];
                fireman.Action(panel);

                if (fireman.transfer)
                {
                    string curr_loc = fireman.Loc;
                    string next_loc = fireman.path;

                    Panel panel_curr = (Panel)panel2.Controls[curr_loc];
                    panel_curr.Controls.Remove(fireman.body);

                    Panel panel_next = (Panel)panel2.Controls[next_loc];
                    panel_next.Controls.Add(fireman.body);
                    fireman.body.BringToFront();
                    Label point = (Label)panel2.Controls[fireman.path].Controls[fireman.Loc];

                    int point_corr_x = panel_next.Width - point.Location.X;
                    int point_corr_y = panel_next.Height - point.Location.Y;

                    if (point_corr_x < fireman.body.Width)
                    {
                        point_corr_x = fireman.body.Width / 2;
                    }
                    else
                    {
                        point_corr_x = 0;
                    }

                    if (point_corr_y < fireman.body.Height)
                    {
                        point_corr_y = fireman.body.Height - point.Height / 2;
                    }
                    else
                    {
                        point_corr_y = 0;
                    }

                    fireman.body.Location = new Point(point.Location.X - point_corr_x, point.Location.Y - point_corr_y);
                    fireman.Loc = panel_next.Name;

                    if (fireman.Loc == fireman.goal)
                    {
                        fireman.NextGoal();
                    }

                    fireman.transfer = false;
                }

                if (fireman.done)
                {
                    fireman_ex = false;
                    foreach (Person person in persons)
                    {
                        person.fireman_work = true; 
                    }
                    Person.alarm = "safe";
                    C1.Controls.Remove(fireman.body);
                    Job();
                }
            }
            
        }
    }
}
