
/*
Author: Mikel Skreen - 11390873
*/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ExpTree_MSkreen
{
    public class Spreadsheet
    {
        private int columns, rows;
        public sCell[,] Grid;

        public event PropertyChangedEventHandler CellPropertyChanged;
        private void cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CellPropertyChanged(sender, e);
        }

        public Spreadsheet(int Col, int Row)
        {
            columns = Col;
            rows = Row;
            Grid = new sCell[Row, Col];

            int i, j;

            for (i = 0; i < rows; i++)
            {
                for (j = 0; j < Col; j++)
                {
                    sCell newCell = sCell.createCell(i, j);
                    Grid[i, j] = newCell;
                    newCell.PropertyChanged += cell_PropertyChanged;//handler;
                                                                    //OnCellPropertyChanged(newCell, "Grid");
                }
            }
        }

        private int RowCount
        {
            get { return rows; }
        }

        private int ColumnCount
        {
            get { return columns; }
        }

        public Cell GetCell(int Col, int Row)
        {
            if (Grid[Row, Col] != null)
                return Grid[Row, Col];
            else
                return null;
        }

        // inheriting class of Cell
        public class sCell : Cell
        {
            public sCell(int ri, int ci) : base(ri, ci)
            {
            }

            public static sCell createCell(int ri, int ci)
            {
                return new sCell(ri, ci);
            }
            public void SetsCellText(string setThis)
            {
                Text = setThis;
            }
            public void setValue(sCell[,] cellGrid)
            {
                string thisText = this.Text;

                if (thisText[0] == '=')
                {
                    byte[] asciiBytes = ASCIIEncoding.ASCII.GetBytes(thisText);

                    this.value = cellGrid[Int32.Parse(Regex.Replace(thisText, "[^0-9]+", string.Empty)), asciiBytes[1] - 65].value;
                }
                else
                {
                    this.value = thisText;
                }

            }

        }
    }

    public abstract class Cell : INotifyPropertyChanged
    {
        readonly int columnindex, rowindex;
        protected string text;
        protected string value;

        public Cell(int column, int row)
        {
            columnindex = column;
            rowindex = row;
        }

        public int ColumnIndex
        {
            get { return columnindex; }
            /*set
            {
                ColumnIndex = value;
                OnPropertyChanged("ColumnIndex");
            }*/
        }
        public int RowIndex
        {
            get { return rowindex; }
            /*set
            {
                RowIndex = value;
                OnPropertyChanged("RowIndex");
            }*/
        }

        public string Value
        {
            get { return value; }
        }

        public string Text
        {
            get { return text; }
            protected set
            {
                if (text == value)
                    return;
                else
                {
                    text = value;
                    //OnPropertyChanged("text");
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        /*private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void handler(object sender, PropertyChangedEventArgs e)
        {
            Cell c = sender as Cell;

            OnPropertyChanged("text");
        }*/


    }

    public class ExpTree
    {
        Op_Node root;

        private Dictionary<string, double> m_t = new Dictionary<string, double>();

        public ExpTree(string expression)
        {
            string[] infix = ParseString(expression);
            string[] postfix = new string[expression.Length];
            postfix = InfixToPostfix(infix);
            var Stack = new Stack<Node>();
            root = new Op_Node();

            int i=0;
            while (postfix.GetUpperBound(0) > i++){; }

            for(int j=0; j < i; j++)
            {
                if(!("*/+-".Contains(postfix[j])))
                {
                    if (Regex.IsMatch(postfix[j], @"^\d+$") == true) //check if string is a constant
                    {
                        Const_Node tempconst = new Const_Node();
                        tempconst.value = Int32.Parse(postfix[j]);
                        Stack.Push(tempconst);
                    }
                    else //must be a variable
                    {
                        m_t[postfix[j]] = 0;
                        Var_Node tempvar = new Var_Node();
                        tempvar.name = postfix[j];
                        Stack.Push(tempvar);
                    }
                }
                else //must be an operator
                {
                    Op_Node tempop = new Op_Node();
                    if (postfix[j] == "*")
                        tempop.op = '*';
                    else if (postfix[j] == "/")
                        tempop.op = '/';
                    else if (postfix[j] == "+")
                        tempop.op = '+';
                    else if (postfix[j] == "-")
                        tempop.op = '-';

                    tempop.Right = Stack.Pop();
                    tempop.Left = Stack.Pop();
                    
                    
                    Stack.Push(tempop);
                }
            }
            root = (Op_Node)Stack.Pop();
        }

        public void SetVar(string varName, double varValue)
        {
            m_t[varName] = varValue;
        }

        public double Eval()
        {
            return root.Eval(m_t);
        }

        public static string[] InfixToPostfix(string[] infixArray)
        {
            var stack = new Stack<string>();
            var postfix = new Stack<string>();

            int k = 0;
            while ((infixArray.GetUpperBound(0) > k++) && infixArray[k] != null) { }

            string st;

            for (int i = 0; i < k; i++) 
            {
                if (!("()*/+-".Contains(infixArray[i])))
                {
                    postfix.Push(infixArray[i]);
                }
                else
                {
                    if (infixArray[i].Equals("("))
                    {
                        stack.Push("(");
                    }
                    else if (infixArray[i].Equals(")"))
                    {
                        st = stack.Pop();
                        while (!(st.Equals("(")))
                        {
                            postfix.Push(st);
                            st = stack.Pop();
                        }
                    }
                    else
                    {
                        while (stack.Count > 0)
                        {
                            st = stack.Pop();
                            if (infixArray[i] == ")" && st == "(") {; }

                            else if (PriorityChck(st) >= PriorityChck(infixArray[i]) && st != "(")
                            {
                                postfix.Push(st);
                            }
                            else
                            {
                                stack.Push(st);
                                break;
                            }
                        }
                        stack.Push(infixArray[i]);
                    }
                }
            }
            while (stack.Count > 0)
            {
                postfix.Push(stack.Pop());
            }

            return postfix.Reverse().ToArray();
        }

        public static int PriorityChck(string op)
        {
            if (op == "(" || op == ")")
                return 3;
            if (op == "*" || op == "/")
                return 2;
            if (op == "+" || op == "-")
                return 1;
            return -1;

        }

        public string[] ParseString(string str)
        {
            string[] parsed = new string[str.Length];
            
            int i=0, j = 0;
            char c;
            c = str.ElementAt(i);
            while (i < str.Length)
            {
                
                Char.IsNumber(c);
                if((c == '/') || (c == '*') || 
                   (c == '+') || (c == '-') || 
                   (c == '(') || (c == ')'))
                {
                    parsed[j] = str.ElementAt(i++).ToString();
                    if (i >= str.Length)
                        break;
                    c = str.ElementAt(i);
                }

                else
                {
                    string temp = null;
                    temp += str.ElementAt(i++);
                    if (i < str.Length)
                    {
                        c = str.ElementAt(i);
                        while ((c != '/') && (c != '*') &&
                               (c != '+') && (c != '-') &&
                               (c != '(') && (c != ')') && (i < str.Length))
                        {
                            temp += str.ElementAt(i++).ToString();
                            if (i < str.Length)
                                c = str.ElementAt(i);
                        }
                    }
                    parsed[j] = temp;
                }

                j++;
            }

            return parsed;
        }

    }
    public abstract class Node
    {
        public double value;
        public abstract double Eval(Dictionary<string, double> m_table);
    }

    public class Const_Node : Node
    {
        public new double value;
        public override double Eval(Dictionary<string, double> m_table)
        {
            return value;
        }
    }

    public class Var_Node : Node
    {
        public string name;
        //private Dictionary<string, double> m_table;
        public override double Eval(Dictionary<string, double> m_table)
        {
            return m_table[name];
        }
    }

    public class Op_Node : Node
    {
        public Node Left, Right;
        public char op;

        public override double Eval(Dictionary<string, double> m_table)
        {
            if (op == '+')
                return Left.Eval(m_table) + Right.Eval(m_table);
            if (op == '-')
                return Left.Eval(m_table) - Right.Eval(m_table);
            if (op == '*')
                return Left.Eval(m_table) * Right.Eval(m_table);
            if (op == '/')
                return Left.Eval(m_table) / Right.Eval(m_table);
            return 0;
        }
    }
}
