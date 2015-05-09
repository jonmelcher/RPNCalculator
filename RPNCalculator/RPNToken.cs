// *******************************************************************
//  RPNToken - A token struct for used when working with RPNCalculator
//           - supports doubles as strings, and "+","-","*", "/","^"
//
//  Written by Jonathan Melcher
//  Last updated May 09, 2015
// *******************************************************************

using System;

namespace RPNCalculator
{
    struct RPNToken
    {
        public string token;
        public double value;
        public bool isDouble;
        public bool isOperation;
        
        public RPNToken(string token)
        {
            this.token = token;
            this.isDouble = double.TryParse(token, out this.value);
            this.isOperation = (token.Length == 1 && "-+*/^".Contains(token));
        }

        public override string ToString()
        {
            return isDouble ? value.ToString("f2") : token;
        }
    }
}
