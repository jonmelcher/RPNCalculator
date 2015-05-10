// *************************************************************************
//  RPNCalculator - A Reverse Polish Notation Calculator Console Application
//                - Supports -, +, *, /, ^ operations
//
//  Written by Jonathan Melcher
//  Last updated May 09, 2015
// *************************************************************************

using System;


namespace RPNCalculator
{
    // *****************************************************************************************
    //  class RPNCalculator - contains methods for parsing strings and populating the RPN Stack,
    //                        as well as a recursive reduction method for calculating the expre-
    //                        ssion of the Stack.  Also contains methods for handling user inte-
    //                        raction with the console application.
    // *****************************************************************************************
    class RPNCalculator : RPNParse
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
            RPNStack stk = new RPNStack();
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
                        GetRPNExpression(stk);
                        break;
                    case ConsoleKey.L:
                        Console.WriteLine(
                            "\n\nThe previous calculation was {0}.", previousCalculation);
                        break;
                    case ConsoleKey.C:
                        CalculateAndDisplay(stk, out previousCalculation);
                        break;
                    case ConsoleKey.S:
                        stk.Display();
                        break;
                    case ConsoleKey.X:
                        stk.Clear();
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

        private static void CalculateAndDisplay(RPNStack stk, out RPNToken previousCalculation)
        {
            previousCalculation = stk.ReduceStack();
            Console.WriteLine("\n\nThe calculated value is {0}.", previousCalculation);
        }

        #endregion
    }
}
