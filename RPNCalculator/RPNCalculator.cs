// *************************************************************************
//  RPNCalculator - A Reverse Polish Notation Calculator Console Application
//                - Supports -, +, *, /, ^ operations
//
//  Written by Jonathan Melcher
//  Last updated May 11, 2015
// *************************************************************************

using System;


namespace RPNCalculator
{
    // ***************************************************************************************
    //  class RPNCalculator - console UI for combining RPNArithmetic and RPNParse into an RPN
    //                        calculator.  Contains methods for handling user interaction with
    //                        the console application.
    // ***************************************************************************************
    class RPNCalculator : RPNParse
    {
        #region entrypoint

        // **********
        // Entrypoint
        // **********
        static void Main(string[] args)
        {
            // ***********************************************************************************
            // Fields :
            // calculator - RPNArithmetic handles math / management of the stored RPN expressions
            // previousCalculation - RPNToken storing previously the previously reduced expression
            // userInput - ConsoleKey storing current userInput
            // ***********************************************************************************
            RPNArithmetic calculator = new RPNArithmetic();
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
                        GetRPNExpression(calculator);
                        break;
                    case ConsoleKey.L:
                        Console.WriteLine(
                            "\n\nThe previous calculation was {0}.", previousCalculation);
                        break;
                    case ConsoleKey.C:
                        CalculateAndDisplay(calculator, out previousCalculation);
                        break;
                    case ConsoleKey.S:
                        calculator.Display();
                        break;
                    case ConsoleKey.X:
                        calculator.Clear();
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

        private static void CalculateAndDisplay(RPNArithmetic calculator, out RPNToken previousCalculation)
        {
            previousCalculation = calculator.ReduceStack();
            Console.WriteLine("\n\nThe calculated value is {0}.", previousCalculation);
        }

        #endregion
    }
}
