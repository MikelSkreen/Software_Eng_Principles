//Name: Mikel Skreen - 11390873
//Date: 2-3-2016

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Numerics;
using System.IO;

namespace HW3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
       
        // Function saves what is currently stored visually in the textbox to a file
        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                FileStream fs = new FileStream(path, FileMode.Create);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (string line in textBox1.Lines)
                        sw.Write(line + sw.NewLine);
                }
            }
        }

        // Function loads contents of file into textbox
        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                StreamReader sr = new StreamReader(fileName);
                LoadText(sr);
                   // textBox1.Text = File.ReadAllText(fileName);
                 sr.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Function uses the FibonacciTextReader class to generate a Fibonacci sequence of 50
        private void loadFibonacciNumbersfirst50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            FibonacciTextReader ftr = new FibonacciTextReader(50);
            textBox1.AppendText(ftr.ReadToEnd());
        }

        // Function uses the FibonacciTextReader class to generate a Fibonacci sequence of 100
        private void loadFibonacciNumbersfirst100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            FibonacciTextReader ftr = new FibonacciTextReader(100);
            textBox1.AppendText(ftr.ReadToEnd());
        }

        // Funciton will load the contents of the TextReader object into the textbox
        private void LoadText(TextReader sr)
        {
            textBox1.Clear();
            textBox1.AppendText(sr.ReadToEnd());
        }
    }

    // FibonacciTextReader class is used to generate and return fibonacci sequence of choosing
    public class FibonacciTextReader : TextReader
    {
        BigInteger curSeq = 1;
        BigInteger prevSeq = 0;
        BigInteger temp = 0;
        int curSequence = 0;
        int maxSequence;
        StringBuilder str = new StringBuilder();

        public FibonacciTextReader(int max)
        {
            maxSequence = max;
        }

        // Function returns the current Fib sequence
        public override string ReadLine()
        {
            string readStr = String.Empty;
            if (curSequence > 1 && curSequence < maxSequence)
            {
                temp = curSeq + prevSeq;
                prevSeq = curSeq;
                curSeq = temp;
                return readStr += "\r\n" + (++curSequence) + ": " + (curSeq);
            }
            else if(curSequence == 0)
                return readStr += (++curSequence) + ": 0";
            else if (curSequence == 1)
                return readStr += "\r\n" + (++curSequence) + ": 1";
            return null;
        }

        // Uses ReadLine to populate the Fib sequence up to the max value specified in constructor
        public override string ReadToEnd()
        {
            while(curSequence < maxSequence)
            {
                str.Append(ReadLine());
            }
            return str.ToString();
        }
    }
}
