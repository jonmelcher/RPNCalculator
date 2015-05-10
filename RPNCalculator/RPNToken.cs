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
    public struct RPNToken
    {
        private string token;
        private double value;
        private RPNStack.Outputs tokenType;

        public RPNToken(string token)
        {
            this.token = token;
            this.tokenType = double.TryParse(token, out this.value) ? RPNStack.Outputs.Double :
                   (token.Length == 1 && "-+*/^".Contains(token)) ? RPNStack.Outputs.Operator :
                                                                        RPNStack.Outputs.Error;
        }

        public string Token
        {
            get { return token; }
        }

        public double Value
        {
            get { return value; }
        }

        public RPNStack.Outputs TokenType
        {
            get { return tokenType; }
        }

        public override string ToString()
        {
            if (tokenType == RPNStack.Outputs.Double)
                return value.ToString("f2");
            return token;
        }
    }
}
