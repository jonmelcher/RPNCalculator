// *************************************************************************
//  RPNCalculator - A Reverse Polish Notation Calculator Console Application
//                - Supports -, +, *, /, ^ operations
//
//  Written by Jonathan Melcher
//  Last updated May 09, 2015
// *************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace RPNCalculator
{
    // *****************************************************************************************
    //  class RPNCalculator - contains methods for parsing strings and populating the RPN Stack,
    //                        as well as a recursive reduction method for calculating the expre-
    //                        ssion of the Stack.  Also contains methods for handling user inte-
    //                        raction with the console application.
    // *****************************************************************************************
    class RPNCalculator
    {
        #region entrypoint

        // **********
        // Entrypoint
        // **********
        static void Main(string[] args)
        {
            // *********************************************************************
            // Fields :
            // rpnStack - Stack<RPNToken> containing RPN expression to be calculated
            // previousCalculation - RPNToken storing previously calculated value
            // userInput - ConsoleKey storing current userInput
            // *********************************************************************
            Stack<RPNToken> rpnStack = new Stack<RPNToken>();
            RPNToken previousCalculation = new RPNToken("0");
            ConsoleKey userInput;

            // **********************
            // Console User Interface
            // **********************
            do
            {
                DisplayMenu();
                userInput = Console.ReadKey().Key;
                switch (userInput)
                {
                    case ConsoleKey.A:
                        Console.WriteLine();
                        GetAdditionToRPNStack(rpnStack);
                        break;
                    case ConsoleKey.L:
                        Console.WriteLine(
                            "\n\nThe previous calculation was {0}.", previousCalculation);
                        break;
                    case ConsoleKey.C:
                        DisplayCalculation(rpnStack, out previousCalculation);
                        break;
                    case ConsoleKey.S:
                        DisplayStack(rpnStack);
                        break;
                    case ConsoleKey.X:
                        rpnStack.Clear();
                        break;
                }
                Console.Write("\nPress any key to continue...  ");
                Console.ReadKey();
            }
            while (userInput != ConsoleKey.Q);
        }

        #endregion

        #region display methods

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("\t\tWelcome to RPN Calculator!\n\n");
            Console.WriteLine("A.\tAdd to RPN Stack");
            Console.WriteLine("L.\tLook at previous calculation");
            Console.WriteLine("C.\tCalculate current RPN Stack");
            Console.WriteLine("S.\tSee entire RPN Stack");
            Console.WriteLine("X.\tClear entire RPN Stack");
            Console.WriteLine("Q.\tQuit program\n\n");
            Console.Write("Please select an input:  ");
        }

        private static void DisplayStack(Stack<RPNToken> rpnStack)
        {
            if (rpnStack.Count == 0)
                Console.WriteLine("\n\nThe stack is empty.");
            else
            {
                // .ToList() on a stack prints top as first element, so must reverse
                List<RPNToken> temp = rpnStack.ToList();
                temp.Reverse();
                Console.WriteLine("\n\nThe entire stack is {0}.", string.Join(" ", temp));
            }
        }

        private static void DisplayCalculation(
            Stack<RPNToken> rpnStack, out RPNToken previousCalculation)
        {
            previousCalculation = Reduce(rpnStack);
            Console.WriteLine("\n\nThe calculated value is {0}.", previousCalculation);
        }

        #endregion
        #region user get methods

        private static void GetAdditionToRPNStack(Stack<RPNToken> rpnStack)
        {
            List<RPNToken> addition = new List<RPNToken>();
            string input;

            // Invalid inputs cause addition to be empty
            while (addition.Count == 0)
            {
                Console.Write("\nPlease enter your addition to the RPN Stack:  ");
                input = Console.ReadLine().Trim();
                addition = TryParseRPN(input);
            }

            PushTokens(addition, rpnStack);
            Console.WriteLine("Addition successful.");
        }

        #endregion
        #region rpn stack methods

        private static RPNToken Reduce(Stack<RPNToken> rpnStack)
        {
            Stack<RPNToken> operations = new Stack<RPNToken>();
            Stack<RPNToken> values = new Stack<RPNToken>();

            // Proceed through stack, separating operations and values as they appear
            // While there are no values, operations can be stacked indefinitely
            // If next token is an operation when there are some values, must call function
            // recursively, since an operation after a value can not operate on it so must
            // act on two different values appearing later
            while (rpnStack.Count > 0)
            {
                if (rpnStack.Peek().isOperation && values.Count == 0)
                    operations.Push(rpnStack.Pop());
                else if (rpnStack.Peek().isOperation)
                    values.Push(Reduce(rpnStack));
                else
                    values.Push(rpnStack.Pop());

                // Since we only accept binary operations, once values reaches two there must be
                // an operation to act on them, otherwise the RPN expression was invalid
                if (values.Count == 2 && operations.Count == 0)
                {
                    rpnStack.Clear();
                    values.Clear();
                }
                else if (values.Count == 2)
                    rpnStack.Push(GetResultantRPNToken(values.Pop(), values.Pop(), operations.Pop()));
            }
            // After stack is used up, if there are operations that have not been accounted for,
            // the RPN expression was invalid.  If values is empty, there was no calculation done
            if (operations.Count != 0)
                values.Push(new RPNToken("Error"));
            else if (values.Count == 0)
                values.Push(new RPNToken("0"));

            return values.Pop();
        }

        private static RPNToken GetResultantRPNToken(RPNToken x, RPNToken y, RPNToken fn)
        {
            RPNToken result;

            if (!(x.isDouble && y.isDouble && fn.isOperation))
                return new RPNToken("ERROR");

            switch (fn.token)
            {
                case "-":
                    result = new RPNToken((x.value - y.value).ToString());
                    break;
                case "+":
                    result = new RPNToken((x.value + y.value).ToString());
                    break;
                case "*":
                    result = new RPNToken((x.value * y.value).ToString());
                    break;
                case "/":
                    result = new RPNToken((x.value / y.value).ToString());
                    break;
                default:
                    result = new RPNToken(Math.Pow(x.value, y.value).ToString());
                    break;
            }

            return result;
        }

        #endregion

        #region helper methods

        private static List<RPNToken> TryParseRPN(string line)
        {
            List<RPNToken> parsed = new List<RPNToken>();

            foreach (string s in line.Split(' '))
            {
                RPNToken temp = new RPNToken(s);
                parsed.Add(temp);

                if (!(temp.isDouble || temp.isOperation))
                {
                    Console.WriteLine("line could not be parsed: {0}", line);
                    parsed.Clear();
                    break;
                }
            }

            return parsed;
        }

        private static void PushTokens(List<RPNToken> tokens, Stack<RPNToken> rpnStack)
        {
            if (rpnStack.Count != 0 && rpnStack.Peek().token == "ERROR")
                rpnStack.Clear();

            tokens.ForEach(x => rpnStack.Push(x));
        }

        #endregion
    }
}
