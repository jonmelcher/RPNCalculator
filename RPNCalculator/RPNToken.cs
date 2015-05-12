// *****************************************************************
//  RPNToken - A token struct for used when working with an RPNStack
//           - supports doubles as strings, and "+","-","*", "/","^"
//
//  Written by Jonathan Melcher
//  Last updated May 11, 2015
// *****************************************************************

using System;


namespace RPNCalculator
{
    public enum Outputs { Double, Operator, Error };

    public struct RPNToken
    {
        private string token;
        private double value;
        private Outputs tokenType;

        public RPNToken(string token)
        {
            this.token = token;
            this.tokenType = double.TryParse(token, out this.value) ? Outputs.Double :
                   (token.Length == 1 && "-+*/^".Contains(token)) ? Outputs.Operator :
                                                                        Outputs.Error;
        }

        public string Token
        {
            get { return token; }
        }

        public double Value
        {
            get { return value; }
        }

        public Outputs TokenType
        {
            get { return tokenType; }
        }

        public override string ToString()
        {
            if (tokenType == Outputs.Double)
                return value.ToString("f2");
            return token;
        }
    }
}
