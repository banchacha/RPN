using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Введите математическую операцию ");
        string input = Console.ReadLine();
        Console.WriteLine();
        List<object> rpn = RPN(input.Replace(" ", string.Empty));
        Console.WriteLine("ОПЗ: " + string.Join(" ", rpn) + "\n");
        Console.WriteLine("Числа: " + string.Join(" ", GetNums(rpn)) + "\n");
        Console.WriteLine("Операторы: " + string.Join(" ", GetOperators(rpn)) + "\n");
        Console.WriteLine("Ответ: " + Calculate(rpn)[0]);
    }
    static int Priority(char operators)
    {
        if (operators == '(' || operators == ')') return 0;
        else if (operators == '+' || operators == '-') return 1;
        else return 2;
    }   
    static bool IsOperator(char symbol) 
    {
        string operators = "+-*/()";
        if (operators.Contains(symbol)) return true;
        else return false;
    }

    static double TheOperation(char oper, int numer1, int number2)
    {
        if (oper == '+') return numer1 + number2;
        else if (oper == '-') return numer1 - number2;
        else if (oper == '*') return numer1 * number2;
        else return numer1 / number2;
    }  
    static List<int> GetNums(List<object> expression) 
    {
        List<int> res = new List<int>();
        foreach (var i in expression)
        {
            if (!(i is char))
                res.Add(Convert.ToInt32(i));
        }
        return res;
    }

    static List<char> GetOperators(List<object> expression) 
    {
        List<char> res = new List<char>();
        foreach (var  i in expression)
        {
            if (i is char)
                res.Add(Convert.ToChar(i));
        }
        return res;
    }
    public static List<object> RPN(string input)
    {
        List<object> output = new List<object>();
        Stack<char> operators = new Stack<char>();
        string number = string.Empty;

        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]))
            {
                number += input[i];
            }

            if (IsOperator(input[i]))
            {
                output.Add(number);
                number = string.Empty;
                if (input[i] == '(')
                {
                    operators.Push(input[i]);
                }
                else if (input[i] == ')')
                {
                    while (operators.Peek() != '(')
                    {
                        output.Add(operators.Peek());
                        operators.Pop();
                    }
                    operators.Pop();
                }
                else
                {
                    if (operators.Count > 0)
                    {
                        if (Priority(input[i]) <= Priority(operators.Peek()))
                        {
                            output.Add(operators.Peek());
                            operators.Pop();
                        }

                        operators.Push(input[i]);
                    }
                    else
                    {
                        operators.Push(input[i]);
                    }
                }
            }
        }

        output.Add(number);

        while (operators.Count != 0)
        {
            output.Add(operators.Peek());
            operators.Pop();
        }

        while (output.Contains(string.Empty)) output.Remove(string.Empty);
        return output;
    }
    static List<object> Calculate(List<object> expression)
    {
        int i = 0;
        while (expression.Count > 1)
        {
            if (i > expression.Count)
                i = 0;
            if (expression[i] is char && IsOperator((char)expression[i]))
            {
                expression[i - 2] = TheOperation((char)expression[i], Convert.ToInt32(expression[i - 2]), Convert.ToInt32(expression[i - 1]));
                expression.RemoveAt(i);
                expression.RemoveAt(i - 1);
                i = 0;
            }
            i++;
        }
        return expression;
    }
}