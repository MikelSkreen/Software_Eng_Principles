/*
    Name: Mikel Skreen
    ID:   11390873
    Date: 4-14-16
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;

namespace HW12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "http://yahoo.com";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "Sorting...";

            textBox3.Enabled = false;
            button1.Enabled = false;

            Thread t1 = new Thread(GenerateLists);
            t1.Start();
        }

        private void GenerateLists()
        {
            Random r = new Random();
            Stopwatch stopWatch = new Stopwatch();


            List<List<int>> singleT = new List<List<int>>();
            List<List<int>> multiT = new List<List<int>>();

            for(int i = 0; i < 8; i++)
            {
                List<int> s = new List<int>();
                //List<int> m = new List<int>;

                for(int j = 0; j < 1000000; j++)
                {
                    int num = r.Next();
                    s.Add(num);
                    //m.Add(num);
                }
                singleT.Add(s);
                multiT.Add(s);
            }


            // sort 8 lists using a single thread
            stopWatch.Start();

            foreach(List<int> l in singleT)
            {
                l.Sort();
            }

            stopWatch.Stop();
            TimeSpan ts1 = stopWatch.Elapsed;
            // end of part 1

            stopWatch.Reset();

            Thread[] threads = new Thread[8];


            // sort 8 lists using 8 separate threads
            stopWatch.Start();

            for(int i=0; i<8; i++)
            {
                int x = i;
                threads[i] = new Thread(() => multiT[x].Sort());
                threads[i].Start();
            }

            foreach (Thread t in threads)
                t.Join();

            stopWatch.Stop();
            TimeSpan ts2 = stopWatch.Elapsed;

            string results = "";
            results = "Single threaded time: " + ts1.Milliseconds + "ms\r\n";
            results += "Multi-threaded time:  " + ts2.Milliseconds + "ms";

            this.Invoke(new Action<string>(SorterDone), results);
        }

        private void SorterDone(string result)
        {
            textBox3.Text = result;
            textBox3.Enabled = true;
            button1.Enabled = true;
        }

        private void generateSortList(int Size, int min, int max)
        {
            Random r = new Random();
            List<int> tempList = new List<int>();

            for (int i = 0; i < Size; i++)
                tempList.Add(r.Next(min, max));

            tempList.Sort();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "Downloading...";

            button2.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            


            Thread t2 = new Thread(dlURL);
            
            t2.Start();
        }

        private void dlURL()
        {
            string url = textBox1.Text;
            string result = "";

            if (url != "")
            { 
                using (var wc = new System.Net.WebClient())
                    result = wc.DownloadString(url);
            }
            this.Invoke(new Action<string>(dlDone), result);
        }

        private void dlDone(string result)
        {
            textBox2.Text = result;

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button2.Enabled = true;
        }
    }
}
