// Name: Mikel Skreen
// ID#:  11390873
// Date: 4-29-16

using System;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW14
{
    public partial class Form1 : Form
    {
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        List<Gravity> fieldColl;
        int Cursx;
        int Cursy;

        int circCenterx;
        int circCentery;

        int gravSize;
        Bitmap DrawArea;

        SolidBrush gravField = new SolidBrush(Color.Gray);
        SolidBrush gravCent = new SolidBrush(Color.Black);

        //planet colors
        SolidBrush Rplanet = new SolidBrush(Color.Red);
        SolidBrush Oplanet = new SolidBrush(Color.Orange);
        SolidBrush Yplanet = new SolidBrush(Color.Yellow);
        SolidBrush Gplanet = new SolidBrush(Color.YellowGreen);
        SolidBrush Bplanet = new SolidBrush(Color.Blue);
        SolidBrush Iplanet = new SolidBrush(Color.Indigo);
        SolidBrush Vplanet = new SolidBrush(Color.Violet);

        public Form1()
        {
            InitializeComponent();

            fieldColl = new List<Gravity>();
            cGrav.Select();
            PColor.Text = "Red";

            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
            myTimer.Tick += new EventHandler(TimerEventProcessor);

            myTimer.Interval = 60;
            myTimer.Start();
        }

        private void TimerEventProcessor(Object myObject,
                                            EventArgs myEventArgs)
        {
            UpdateGalaxy();
        }

        private  void UpdateGalaxy()
        {
            Graphics graph = Graphics.FromImage(DrawArea);
            foreach (Gravity g in fieldColl)
            {
                foreach(Planet p in g.planets)
                {
                    UpdatePos(p, g, graph);
                }
            }
            pictureBox1.Image = DrawArea;

            graph.Dispose();
        }

        private void UpdatePos(Planet p, Gravity g, Graphics graph)
        {
            graph.FillEllipse(gravField, p.x - 4, p.y - 4, 8, 8);

            double angleInDegrees = 5;
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double radius = Math.Sqrt((p.x - g.X) * (p.x - g.X) + (p.y - g.Y) * (p.y - g.Y));
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            int X = (int) (cosTheta * (p.x - g.X) - sinTheta * (p.y - g.Y) + g.X);
            int Y = (int) (sinTheta * (p.x - g.X) + cosTheta * (p.y - g.Y) + g.Y);

            p.x = X;
            p.y = Y;

            graph.FillEllipse(p.color, p.x-4, p.y-4, 8, 8);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            gravSize = (int)numericUpDown1.Value;
            MouseEventArgs eM = (MouseEventArgs)e;
            Cursx = eM.X;//Cursor.Position.X;//pictureBox1.Width / 4;
            Cursy = eM.Y;//pictureBox1.Height / 4;

            Graphics g = Graphics.FromImage(DrawArea);

            if (cGrav.Checked == true)
            {
                if (collisionDet(Cursx, Cursy) == false)
                {
                    //draw gravity field center
                    circCenterx = Cursx - (gravSize / 2);
                    circCentery = Cursy - (gravSize / 2);
                    g.FillEllipse(gravField, circCenterx, circCentery, gravSize, gravSize);
                    //add gravity field position into temp;
                    Gravity tempG = new Gravity(Cursx-4, Cursy+4, gravSize / 2);
                    fieldColl.Add(tempG);

                    //draw gravity core center
                    int gravCentSz = gravSize / 10;
                    circCenterx = Cursx - (gravCentSz / 2);
                    circCentery = Cursy - (gravCentSz / 2);
                    g.FillEllipse(gravCent, circCenterx, circCentery, gravCentSz, gravCentSz);
                }
            }
            else
            {
                //decide what color the planet needs to be
                if (PColor.Text != null)
                {
                    SolidBrush planet;
                    if (PColor.Text == "Red")
                        planet = Rplanet;
                    else if (PColor.Text == "Orange")
                        planet = Oplanet;
                    else if (PColor.Text == "Yellow")
                        planet = Yplanet;
                    else if (PColor.Text == "Green")
                        planet = Gplanet;
                    else if (PColor.Text == "Blue")
                        planet = Bplanet;
                    else if (PColor.Text == "Indigo")
                        planet = Iplanet;
                    else
                        planet = Vplanet;

                    circCenterx = Cursx - 4;
                    circCentery = Cursy - 4;
                    Planet tempP = new Planet(Cursx, Cursy, planet);

                    g.FillEllipse(planet, circCenterx, circCentery, 8, 8);
                    insideGrav(tempP);
                }
            }
            pictureBox1.Image = DrawArea;

            g.Dispose();
        }

        private void insideGrav(Planet p)
        {
            foreach(Gravity g in fieldColl)
            {
                //check if planet is inside of gravity field radius
                if(Math.Sqrt(((g.X - p.x) * (g.X - p.x)) + ((g.Y - p.y) * (g.Y - p.y))) < g.R)
                {
                    g.planets.Add(p);
                    return;
                }
            }
            return;
        }

        //check for gravity collisions
        private bool collisionDet(int x, int y)
        {
            foreach(Gravity g in fieldColl)
            {   //check if gravity fields overlap. If they do, do not place field
                if (Math.Sqrt(((g.X - x)*(g.X - x)) + ((g.Y - y)*(g.Y - y))) < (g.R * 2))
                    return true;
            }
            return false;
        }

    }

    public class Gravity
    {
        private int x;
        private int y;
        private int r;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }    
        public int R
        {
            get
            {
                return r;
            }
            set
            {
                r = value;
            }
        }

        public List<Planet> planets;

        public Gravity(int x, int y, int r)
        {
            X = x;
            Y = y;
            R = r;
            planets = new List<Planet>();
        }
    }

    public class Planet
    {
        public int x;
        public int y;
        public SolidBrush color;

        public Planet(int X, int Y, SolidBrush c)
        {
            x = X;
            y = Y;
            color = c;
        }
    }

}
