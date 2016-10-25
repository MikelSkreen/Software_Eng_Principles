using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BST
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


    public void insertNode(Node root, Node newNode)
    {
        Node temp;
        temp = root;

        if (newNode.data == temp.data)
        {
            System.Console.WriteLine("Duplicate data " + newNode.data + ". Returning...");
            return;
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
    }


    public void displayTree(Node root)
    {
        Node temp;
        temp = root;

        if (temp == null)
            return;
        displayTree(temp.left);
        System.Console.Write(temp.data + " ");
        displayTree(temp.right);
    }

    public int height(Node n)
    {
        if (n == null)
            return 0;
        return 1 + Math.Max(height(n.left), height(n.right));

    }


    static void Main(string[] args)
    {
        BST BST = new BST();

        System.Console.Write("Enter a collection of numbers in the range [0, 100], separated by spaces:\n");
        string input = Console.ReadLine();
        string[] individual = input.Split(' ');

        int temp = Int32.Parse(individual[0]);
        Node root = BST.addNode(temp);

        for (int i = 1; i < individual.Length; i++)
        {
            temp = Int32.Parse(individual[i]);
            BST.insertNode(BST.root, BST.addNode(temp));

        }

        int possibleTH = (int)Math.Log((BST.count + 1), 2);
        System.Console.Write("\nTree contents: ");
        BST.displayTree(root);
        System.Console.Write("\nNumber of nodes:  " + (BST.count + 1));
        System.Console.Write("\nNumber of levels: " + BST.height(root));
        System.Console.Write("\nMinimum number of levels that a tree with " + (BST.count + 1) + " nodes could have is " + (possibleTH + 1));
        System.Console.Write("\nDone.");
        Console.ReadKey();
    }
}
}
