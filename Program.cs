using System;

namespace ConsoleApplication1
{
    internal static class Program
    {
        public static void Main()
        {
            Console.WriteLine(Calculation(Console.ReadLine()));
        }

        private static double Calculation(string s) //Калькулятор
        {
            s = '(' + s + ')';
            int pos = 0;
            object sign;
            object lastSign = ' '; 
            do
            {
                sign = StacksWork.GetSign(s, ref pos);
                if (sign is char c && lastSign is char && (char) lastSign == '(' && (c == '+' || c == '-'))
                    StacksWork.MyStackE.Push(0);

                switch (sign)
                {
                    case double d:
                        StacksWork.MyStackE.Push(d);
                        break;
                    case char _
                        :
                        if ((char) sign == ')')
                        {
                            while (StacksWork.MyStackT.Count > 0 && StacksWork.MyStackT.Peek() != '(')
                                StacksWork.PopDe(StacksWork.MyStackE, StacksWork.MyStackT);
                            StacksWork.MyStackT.Pop();
                        }
                        else
                        {
                            while (StacksWork.CanPop((char) sign, StacksWork.MyStackT))
                                StacksWork.PopDe(StacksWork.MyStackE, StacksWork.MyStackT);

                            StacksWork.MyStackT.Push((char) sign);
                        }

                        break;
                }

                lastSign = sign;
            } while (sign != null);

            if (StacksWork.MyStackE.Count > 1 || StacksWork.MyStackT.Count > 0)
                throw new Exception("Parsing expression");
            return StacksWork.MyStackE.Pop();
        }

        public static char ReadDe(string s, ref int pos) //Считывание строки
        {
            return s[pos++];
        }

        public static string ReadValue(string s, ref int pos) //Считывание числа
        {
            string res = "";
            while (pos < s.Length && (char.IsDigit(s[pos]) || s[pos] == '.'))
                res += s[pos++];

            return res;
        }
    }

    internal static class StacksWork //Функции, работающие со стеками
    {
        public static readonly MyStack<double> MyStackE = new MyStack<double>();
        public static readonly MyStack<char> MyStackT = new MyStack<char>();

        public static void PopDe(MyStack<double> se, MyStack<char> st) //Функция выталкивания
        {
            double b = se.Pop();
            double a = se.Pop();
            switch (st.Pop())
            {
                case '+':
                    se.Push(a + b);
                    break;
                case '-':
                    se.Push(a - b);
                    break;
                case '*':
                    se.Push(a * b);
                    break;
                case '/':
                    if (Math.Abs(b) < 1)
                    {
                        throw new Exception("Деление на 0");
                    }
                    else
                    {
                        se.Push(a / b);
                        break;
                    }
            }
        }

        public static bool CanPop(char op1, MyStack<char> de1) //Проверка, можно ли вытолкунть операцию
        {
            if (MyStackT.Count == 0)
                return false;
            int p1 = GetPriority(op1);
            int p2 = GetPriority(de1.Peek());

            return p1 >= 0 && p2 >= 0 && p1 >= p2;
        }

        private static int GetPriority(char op) //Расставление приоритетов операций
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
                    throw new Exception("Wrong operation");
            }
        }

        public static object GetSign(string s, ref int pos) //Получение символа
        {
            Space(s, ref pos);
            if (pos == s.Length)
                return null;
            if (char.IsDigit(s[pos])) //Проверка, не является ли символ - числом
                return Convert.ToDouble(Program.ReadValue(s, ref pos));
            return Program.ReadDe(s, ref pos);
        }

        private static void Space(string s, ref int pos) //Считывание всех пробелов 
        {
            while (pos < s.Length && char.IsWhiteSpace(s[pos]))
                pos++;
        }
    }

    public sealed class MyStack<T> //Стек, реализованный с помощью массива
    {
        private T[] _array; //массив для хранения данных типа T
        private const int DefaultSize = 10; //вместимость по умолчанию

        public MyStack()
        {
            Count = 0;
            _array = new T [DefaultSize];
        }
        
        public int Count //вывод размера 
        {
            get;
            private set;
        }

        public T Pop() //метод взятия с вершины
        {
            if (Count == 0)
            {
                throw new Exception("Stack is empty");
            }

            return _array[--Count];
        }

        public void Push(T newElement)
        {
            if (Count == _array.Length)
            {
                T[] newArray = new T[2 * _array.Length];
                Array.Copy(_array, 0, newArray, 0, Count);
                _array = newArray;
            }

            _array[Count++] = newElement; //вставляем элемент
        }

        public T Peek()
        {
            if (Count == 0)
            {
                throw new Exception("Stack is empty");
            }

            return _array[Count - 1];
        }
    }
}
