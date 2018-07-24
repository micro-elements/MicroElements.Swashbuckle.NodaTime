using System;

namespace MicroElements.Swashbuckle.NodaTime
{
    public class Calculator
    {
        public static int Sum(int a, int b) => a + b;
        public static int Sub(int a, int b) => a - b;
        public static int Mul(int a, int b) => a * b;
        public static int Div(int a, int b)
        {
            if(b != 0)
                return a / b;
            throw new NotSupportedException("Can not divide by zero!");
        } 
    }
}
