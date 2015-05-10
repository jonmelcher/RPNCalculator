using System;
using System.Collections.Generic;

namespace RPNCalculator
{
    class RPNParse
    {
        protected static void GetRPNExpression(RPNStack stk)
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

            stk.Push(addition);
        }

        protected static List<RPNToken> TryParseRPN(string line)
        {
            List<RPNToken> parsed = new List<RPNToken>();

            foreach (string s in line.Split(' '))
            {
                RPNToken temp = new RPNToken(s);
                parsed.Add(temp);

                if (!(temp.TokenType == RPNStack.Outputs.Double
                    || temp.TokenType == RPNStack.Outputs.Operator))
                {
                    Console.WriteLine("line could not be parsed: {0}", line);
                    parsed.Clear();
                    break;
                }
            }

            return parsed;
        }

    }
}
