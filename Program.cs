using System;

namespace ConsoleApplication1
{
    internal static class Program
    {
        public static void Main()
        {
            var s = Console.ReadLine();
            Console.WriteLine(Calculation(s));
        }

        private static double Calculation(string s)
        {
            s = '(' + s + ')';
            var position = 0;
            object sign;
            object lastSign = ' ';
            do
            {
                sign = WorkStacks.GetSign(s, ref position);
                if (sign is char c && lastSign is char && (char) lastSign == '(' && (c == '+' || c == '-'))
                {
                    WorkStacks.StackE.Push(0);
                }

                switch (sign)
                {
                    case double d:
                        WorkStacks.StackE.Push(d);
                        break;
                    case char _
                        :
                        if ((char) sign == ')')
                        {
                            while (WorkStacks.StackT.Count > 0 && WorkStacks.StackT.Peek() != '(')
                                WorkStacks.PopDe(WorkStacks.StackE, WorkStacks.StackT);
                            WorkStacks.StackT.Pop();
                        }
                        else
                        {
                            while (WorkStacks.CanBeenPop((char) sign, WorkStacks.StackT))
                                WorkStacks.PopDe(WorkStacks.StackE, WorkStacks.StackT);

                            WorkStacks.StackT.Push((char) sign);
                        }

                        break;
                }

                lastSign = sign;
            } while (sign != null);

            if (WorkStacks.StackE.Count > 1 || WorkStacks.StackT.Count > 0)
                throw new Exception("Ошибка в разборе выражения");
            return WorkStacks.StackE.Pop();
        }

        public static char Read(string s, ref int pos)
        {
            return s[pos++];
        }

        public static string ReadValue(string s, ref int pos)
        {
            var res = "";
            while (pos < s.Length && (char.IsDigit(s[pos]) || s[pos] == '.'))
                res += s[pos++];

            return res;
        }
    }

    internal static class WorkStacks
    {
        public static readonly Stack<double> StackE = new Stack<double>();
        public static readonly Stack<char> StackT = new Stack<char>();

        public static void PopDe(Stack<double> stackE, Stack<char> stackT)
        {
            var b = stackE.Pop();
            var a = stackE.Pop();
            if (stackT.Pop() == '+')
                stackE.Push(a + b);
            else if (stackT.Pop() == '-')
            {
                stackE.Push(a - b);
            }
            else if (stackT.Pop() == '*')
            {
                stackE.Push(a * b);
            }
            else if (stackT.Pop() == '/')
            {
                if (Math.Abs(b) < 1)
                {
                    throw new Exception("Деление на 0");
                }

                stackE.Push(a / b);
            }
        }

        public static bool CanBeenPop(char op1, Stack<char> de1)
        {
            if (StackT.Count == 0)
                return false;
            var p1 = GetPriority(op1);
            var p2 = GetPriority(de1.Peek());

            return p1 >= 0 && p2 >= 0 && p1 >= p2;
        }

        private static int GetPriority(char op)
        {
            switch (op)
            {
                case '(':
                    return -1;
                case '*':
                case '/':
                    return 1;
                case '+':
                case '-':
                    return 2;
                default:
                    throw new Exception("Недопустимая операция");
            }
        }

        public static object GetSign(string s, ref int pos)
        {
            Probeli(s, ref pos);
            if (pos == s.Length)
                return null;
            if (!char.IsDigit(s[pos])) return Program.Read(s, ref pos);
            return Convert.ToDouble(Program.ReadValue(s, ref pos));
        }

        private static void Probeli(string s, ref int pos)
        {
            while (pos < s.Length && char.IsWhiteSpace(s[pos]))
                pos++;
        }
    }

    public sealed class Stack<T> //Cтек, реализованный с помощью массива
    {
        private T[] _array;
        private const int DefaultSize = 10;

        public Stack()
        {
            Count = 0;
            _array = new T [DefaultSize];
        }

        public int Count { get; private set; }

        public T Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException();
            return _array[--Count];
        }

        public void Push(T newElement)
        {
            if (Count == _array.Length)
            {
                var newArray = new T[2 * _array.Length];
                Array.Copy(_array, 0, newArray, 0, Count);
                _array = newArray;
            }

            _array[Count++] = newElement;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException();
            return _array[Count - 1];
        }
    }
}