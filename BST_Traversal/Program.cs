/*
Name: Mikel Skreen
ID:   11390873
Date: 4/5/2016
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11
{
    class Node
    {
        public int data;
        public Node left;
        public Node right;

        public Node(int data)
        {
            this.data = data;
            left = null;
            right = null;
        }
    }

    class BST
    {
        Node root;
        static int count = 0;

        public BST()
        {
            root = null;
        }

        public Node addNode(int data)
        {
            Node newNode = new Node(data);

            if (root == null)
            {
                root = newNode;
            }
            return newNode;
        }


        public bool insertNode(Node root, Node newNode)
        {
            Node temp;
            temp = root;

            if (newNode.data == temp.data)
            {
                System.Console.WriteLine("Duplicate data " + newNode.data + ". Returning...");
                return false;
            }

            else if (newNode.data < temp.data)
            {
                if (temp.left == null)
                {
                    temp.left = newNode;
                    count++;
                }

                else
                {
                    temp = temp.left;
                    insertNode(temp, newNode);

                }
            }
            else if (newNode.data > temp.data)
            {
                if (temp.right == null)
                {
                    temp.right = newNode;
                    count++;
                }

                else
                {
                    temp = temp.right;
                    insertNode(temp, newNode);
                }
            }
            return true;
        }

        // displays contents of tree using recursion
        public void displayTreeRec(Node n)
        {
            Node temp;
            temp = n;

            if (temp == null)
                return;
            displayTreeRec(temp.left);
            System.Console.Write(temp.data + " ");
            displayTreeRec(temp.right);
        }

        // displays contents of tree using a stack non recursively
        public void displayTreeStack(Node n)
        {
            Stack<Node> s = new Stack<Node>();
            Node temp = n;
            //s.Push(temp);


            do
            {
                if (temp != null)
                {
                    s.Push(temp);
                    temp = temp.left;
                }

                else//(temp == null)
                {
                    temp = s.Pop();
                    Console.Write(temp.data + " ");
                    temp = temp.right;
                }
                if (s.Count == 0 && temp == null)
                    return;
            } while (s.Count >= 0);
        }

        // function prints the contents of the BST by creating/checking for links. Uses the Link helper function
        public void displayTreeLoop(Node n)
        {
            while(n != null)
            {
                if (Link(n))
                    n = n.left;
                else
                {
                    Console.Write(n.data + " ");
                    n = n.right;
                }
            }
        }

        public bool Link(Node n)
        {
            if (n.left == null)
                return false;

            Node temp = n.left;

            while (temp.right != null && temp.right != n)
                temp = temp.right;

            if(temp.right == n)
            {
                temp.right = null;
                return false;
            }
            else
            {
                temp.right = n;
                return true;
            }
        }

        public int height(Node n)
        {
            if (n == null)
                return 0;
            return 1 + Math.Max(height(n.left), height(n.right));
        }


        static void Main(string[] args)
        {
            string again = "y";

            while (again == "y" || again == "Y")
            {
                System.Console.Clear();

                BST BST = new BST();

                Random temp = new Random();
                Node root = BST.addNode(temp.Next(0, 100));
                int i = 0;
                while (i < 20)
                {
                    if (BST.insertNode(BST.root, BST.addNode(temp.Next(0, 100))) == true)
                        i++;
                }

                System.Console.Write("\nIn-order traversal without using a stack or recursion.");
                System.Console.Write("\nTree contents: ");
                BST.displayTreeLoop(root);

                System.Console.Write("\n\nIn-order traversal using stack and no recursion.");
                System.Console.Write("\nTree contents: ");
                BST.displayTreeStack(root);

                System.Console.Write("\n\nIn-order traversal using recursive calls.");
                System.Console.Write("\nTree contents: ");
                BST.displayTreeRec(root);
                
                //System.Console.Write("\nDone.");
                System.Console.Write("\n\nAgain (y/n)\n: ");
                again = Console.ReadLine();
            }
        }
    }
}

