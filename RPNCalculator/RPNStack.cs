// ***************************************************************************
//  RPNStack - A wrapper for using and maintaining a Stack<RPNToken> structure
//           - Supports Count, Push(RPNToken), Push(List<RPNToken), Pop(),
//             Peek(), Clear(), and Display()
//           - In RPNCalculator namespace, inherited by RPNArithmetic
//
//  Written by Jonathan Melcher
//  Last updated May 11, 2015
// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;


namespace RPNCalculator
{
    public class RPNStack
    {
        protected Stack<RPNToken> stk;

        public RPNStack()
        {
            stk = new Stack<RPNToken>();
        }

        public int Count
        {
            get { return stk.Count; }
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
