/*
    Name: Mikel Skreen - 11390873
    Assignment 13
    Date: 4-18-2016
*/

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW13
{
    public partial class Form1 : Form
    {
        Trie T;
        public Form1()
        {
            InitializeComponent();
            T = new Trie();

            if (File.Exists("wordsEn.txt"))
            {
                /*using (StreamReader sr = new StreamReader("wordsEn.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        // Read the stream to a string, and write the string to the console.
                        String line = sr.ReadLine();
                        T.Add(line);
                    }
                }*/
                string[] archive = File.ReadAllLines("wordsEn.txt");
                foreach (string s in archive)
                    T.Add(s);
            }
        }

        public class Node
        {
            public char c;
            public Dictionary<char, Node> children;
            public Node()
            {
                children = new Dictionary<char, Node>();
            }
        }

        public class AutoCompleteResult
        {
            public string id;
            public string label;
            public string value;
        }

        private class Trie
        {
            public Node m_root;

            public Trie()
            {
                m_root = new Node();
                m_root.children = new Dictionary<char, Node>();
            }

            // function adds a single string to the trie
            public void Add(string s)
            {
                s = s.ToLower();
                Node n = m_root;

                foreach (char ch in s)
                {
                    if (n.children.Count != 0 && n.children.ContainsKey(ch) == true && ch != '\0')
                        n = n.children[ch];
                    else
                    {
                        Node temp = new Node();
                        temp.c = ch;
                        n.children.Add(ch, temp);
                        n = n.children[ch];
                    }
                }
                Node endNode = new Node();
                endNode.c = '\0';
                n.children.Add('\0', endNode);
            }

            // function finds all words that start with given substring
            public List<string> subStringReturn(string sub)
            {
                Node n = m_root;
                List<string> Completed = new List<string>();
                // traverse to the end of Trie substring
                foreach (char c in sub)
                {
                    if (n.children.ContainsKey(c) == true)
                        n = n.children[c];
                    else
                        return Completed;
                }
                string temp = sub;
                if(temp.Length > 0)
                    temp = temp.Remove(temp.Length - 1); // trim the last character from the string
                
                return finString(n, temp, Completed);
            }

            // helper function to subStringReturn
            public List<string> finString(Node n, string temp, List<string> c)
            {
                if(n.c == '\0')
                {
                    c.Add(temp);
                    return c;
                }

                temp += n.c;

                if(n.children.ContainsKey('\0'))
                {
                    finString(n.children['\0'], temp, c);
                }

                for(char i='a'; (int)(i-97) < 27 && n.children.Count != 0; i++)
                {
                    if (n.children.ContainsKey(i) == true)
                    {
                        finString(n.children[i], temp, c);
                    }
                }

                return c;
            }
        }

        private void PrefixText_TextChanged(object sender, EventArgs e)
        {
            ResultsText.Items.Clear();

            ResultsText.Text = "";
            List<string> output = T.subStringReturn(PrefixText.Text);

            ResultsText.Items.AddRange(output.ToArray());
        }

        private void ResultsText_TextChanged(object sender, EventArgs e)
        {

        }

        private void ResultsText_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
