/*
Name: Mikel Skreen = 11390873
Date: 2/18/2016
*/
using ExpTree_MSkreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTree_Mskreen
{
    class Program
    {

        static void Main(string[] args)
        {
            int i = 0, flag = 0;
            string expression = "A2+(10+B5)";
            ExpTree newExpTree = new ExpTree(expression);
            
            while (flag != 1)
            {
                i = 0;
                while((i < 1) || (i > 4))
                {
                    Console.WriteLine("================== MENU ==================");
                    Console.WriteLine("Current Expression: " + expression);
                    Console.WriteLine(" 1 = Enter a new expression");
                    Console.WriteLine(" 2 = Set a variable value");
                    Console.WriteLine(" 3 = Evaluate tree");
                    Console.WriteLine(" 4 = Quit");
                    Console.WriteLine("==========================================");
                    string input = Console.ReadLine();
                    i = Int32.Parse(input);
                }
                if (i == 1) //create new exptree
                {
                    Console.WriteLine("Enter a new expression: ");
                    expression = Console.ReadLine();
                    ExpTree temp = new ExpTree(expression);
                    newExpTree = temp;
                    
                }
                else if (i == 2) //set exptree variables
                {
                    Console.Write("Enter variable name: ");
                    string variable = Console.ReadLine();
                    Console.Write("Enter variable value: ");
                    double value = Int32.Parse(Console.ReadLine());
                    newExpTree.SetVar(variable, value);
                }
                else if (i == 3) //evaluate exptree
                {
                    Console.WriteLine(newExpTree.Eval());
                }
                else if (i == 4) //quit
                {
                    flag = 1;
                    Console.WriteLine("Done");
                }

            }
        }
    }
}
