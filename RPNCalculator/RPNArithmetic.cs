// *******************************************************************************
//  RPNArithmetic - derived class from RPNStack containing methods for calculating
//                  and reducing RPN Expressions contained in said RPNStack
//                - Supports -, +, *, /, ^ operations
//
//  Written by Jonathan Melcher
//  Last updated May 11, 2015
// *******************************************************************************

using System;
using System.Collections.Generic;


namespace RPNCalculator
{
    public class RPNArithmetic : RPNStack
    {
        public RPNToken Calculate(RPNToken x, RPNToken y, RPNToken fn)
        {
            RPNToken result;

            if (!(x.TokenType == Outputs.Double && y.TokenType == Outputs.Double
                                           && fn.TokenType == Outputs.Operator))
                return new RPNToken("ERROR");

            switch (fn.Token)
            {
                case "-":
                    result = new RPNToken((x.Value - y.Value).ToString());
                    break;
                case "+":
                    result = new RPNToken((x.Value + y.Value).ToString());
                    break;
                case "*":
                    result = new RPNToken((x.Value * y.Value).ToString());
                    break;
                case "/":
                    result = new RPNToken((x.Value / y.Value).ToString());
                    break;
                default:
                    result = new RPNToken(Math.Pow(x.Value, y.Value).ToString());
                    break;
            }

            return result;
        }

        public RPNToken ReduceStack()
        {
            // Proceed through stack, separating operations and values as they appear
            // While there are no values, operations can be stacked indefinitely
            // If next token is an operation when there are some values, must call function
            // recursively, since an operation after a value can not operate on it so must
            // act on two different values appearing later

            Stack<RPNToken> operatorStk = new Stack<RPNToken>();
            Stack<RPNToken> operandStk = new Stack<RPNToken>();
            while (Count > 0)
            {
                switch (Peek().TokenType)
                {
                    case Outputs.Operator:
                        if (operandStk.Count == 0)
                            operatorStk.Push(Pop());
                        else
                            operandStk.Push(ReduceStack());
                        break;
                    case Outputs.Double:
                        operandStk.Push(Pop());
                        break;
                    default:
                        return Pop();
                }

                // Since we only accept binary operations, once values reaches two there must be
                // an operation to act on them, otherwise the RPN expression was invalid
                if (operandStk.Count == 2)
                {
                    if (operatorStk.Count == 0)
                    {
                        Clear();
                        return new RPNToken("ERROR");
                    }
                    Push(Calculate(operandStk.Pop(), operandStk.Pop(), operatorStk.Pop()));
                }
            }

            // After stack is used up, if there are operations that have not been accounted for,
            // the RPN expression was invalid.  If values is empty, there was no calculation done
            if (operatorStk.Count != 0)
                return new RPNToken("ERROR");
            else if (operandStk.Count == 0)
                return new RPNToken("0");

            return operandStk.Pop();
        }
    }
}
