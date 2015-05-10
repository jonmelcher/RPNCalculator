using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNCalculator
{
    public class RPNStack : RPNArithmetic
    {
        public enum Outputs { Double, Operator, Error };
        private Stack<RPNToken> stk;

        public RPNStack()
        {
            stk = new Stack<RPNToken>();
        }

        public void Push(RPNToken token)
        {
            stk.Push(token);
        }

        public void Push(List<RPNToken> tokens)
        {
            tokens.ForEach(x => Push(x));
        }

        public RPNToken Pop()
        {
            return stk.Pop();
        }

        public RPNToken Peek()
        {
            if (stk.Count == 0)
                return new RPNToken("ERROR");
            return stk.Peek();
        }

        public void Clear()
        {
            stk.Clear();
        }

        public int Count
        {
            get { return stk.Count; }
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
                if (operandStk.Count == 2 && operatorStk.Count == 0)
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

        public void Display()
        {
            if (Count == 0)
                Console.WriteLine("\n\nThe stack is empty.");
            else
            {
                // .ToList() on a stack prints top as first element, so must reverse
                List<RPNToken> temp = stk.ToList();
                temp.Reverse();
                Console.WriteLine("\n\n{0}.", string.Join(" ", temp));
            }
        }

    }
}
