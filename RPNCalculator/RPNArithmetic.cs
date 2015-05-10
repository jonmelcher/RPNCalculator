

namespace RPNCalculator
{
    interface RPNArithmetic
    {
        RPNToken Calculate(RPNToken x, RPNToken y, RPNToken fn);
        RPNToken ReduceStack();
    }
}
