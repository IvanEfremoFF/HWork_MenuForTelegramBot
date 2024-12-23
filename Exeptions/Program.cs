﻿using System.Collections;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Exceptions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 0, b = 0, c = 0;

            Console.CursorVisible = false;

            while (true)
            {
                try
                {
                    var pressedKey = InputABC(ref a, ref b, ref c);
                    if  (pressedKey == ConsoleKey.Escape)
                        break;

                    if (pressedKey == ConsoleKey.Enter)
                        CalculateEquation(a, b, c);
                }

                catch (InputDataException e)
                {
                    FormatData(e.Message, Severity.Error, e.Data);
                }
                
                catch (NoRootsException e)
                {
                    FormatData(e.Message, Severity.Warning, e.Data);
                }
            }
        }

        private static void FormatData(string message, Severity severity, IDictionary data) 
        {
            if (severity == Severity.Error)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;
            }

            if (severity == Severity.Warning)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Yellow;
            }

            Console.SetCursorPosition(0, 13);
            Console.WriteLine( new string('-', 50));
            Console.Write(message + ": ");
            foreach (DictionaryEntry item in data)
                Console.Write(item.Key + " ");
            Console.WriteLine("\n" + new string('-', 50));

            foreach (DictionaryEntry item in data)
                Console.WriteLine(item.Key + " = " + item.Value);

            Console.Write("Press any key...");
            Console.ReadKey();
            Console.SetCursorPosition(0, 0);
            Console.ResetColor();
        }

        private static void CalculateEquation(int a, int b, int c)
        {
            double x1, x2;
            double discriminant;

            Console.SetCursorPosition(0, 13);
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("List of roots: ");

            discriminant = Math.Pow(b, 2) - 4 * a * c;

            switch (discriminant) 
            { 
                case 0:
                    if (a != 0)
                        x1 = ((-1 * b) + Math.Sqrt(discriminant)) / (2 * a);
                    else
                        x1 = -1 * b;

                    Console.WriteLine($"Equation has only ONE root: {x1, 0:f2}");
                    break;

                case < 0:
                    var ex = new NoRootsException("No roots found");
                    throw ex;

                case > 0:
                    if (a != 0)
                    {
                        x1 = ((-1 * b) + Math.Sqrt(discriminant)) / (2 * a);
                        x2 = ((-1 * b) - Math.Sqrt(discriminant)) / (2 * a);
                    }
                    else 
                    {
                        x1 = -1 * b;
                        x2 = b;
                    }

                    Console.WriteLine($"Root X1: {x1, 0:f2}");
                    Console.WriteLine($"Root X2: {x2, 0:f2}");
                    break;
            }

            Console.Write("\nPress any key...");
            Console.ReadKey();
            Console.SetCursorPosition(0, 0);

        }

        private static ConsoleKey InputABC(ref int a, ref int b, ref int c)
        {
            string _a = "", _b = "",  _c = "";
            byte menuItem = 1;


            while (true)
            { 
                Console.Clear();
                Console.WriteLine("*************************************");
                Console.WriteLine(" Finding roots of quadratic equation");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine(" Up/Down to go through the menu. \n ENTER to confirm input. \n ESC to quit the program.");
                Console.WriteLine("*************************************");
                Console.Write("\n Input values: ");
                Console.Write($"{(_a.Length > 0 ? _a : "a")} * x^2 ");
                Console.Write($"{(_b.Length > 0 ? (_b[0] == '-' ? "- " + _b.Substring(1) : "+ " + _b) : "+ b")} * x ");
                Console.Write($"{(_c.Length > 0 ? (_c[0] == '-' ? "- " + _c.Substring(1) : "+ " + _c) : "+ c")} = 0 \n");

                switch (menuItem) 
                {
                    case 1:
                        Console.WriteLine($"-> a: {_a}\n   b: {_b}\n   c: {_c}");
                        break;

                    case 2:
                        Console.WriteLine($"   a: {_a}\n-> b: {_b}\n   c: {_c}");
                        break;

                    case 3:
                        Console.WriteLine($"   a: {_a}\n   b: {_b}\n-> c: {_c}");
                        break;

                    default:
                        menuItem = 1;
                        break;
                }

                var pressedKey = Console.ReadKey();
                switch (pressedKey.Key) 
                {
                    case ConsoleKey.DownArrow:
                        if (menuItem == 3)
                            menuItem = 1;
                        else
                            menuItem++;
                        break;

                    case ConsoleKey.UpArrow:
                        if (menuItem == 1)
                            menuItem = 3;
                        else
                            menuItem--;
                        break;

                    case ConsoleKey n when (n == ConsoleKey.OemMinus || n == ConsoleKey.Subtract || n <= ConsoleKey.NumPad9 && n >= ConsoleKey.NumPad0) 
                                            || (n <= ConsoleKey.D9 && n >= ConsoleKey.D0):
                        switch (menuItem)
                        {
                            case 1:
                                if ((pressedKey.Key == ConsoleKey.OemMinus || pressedKey.Key == ConsoleKey.Subtract) && _a.Length == 0)
                                    _a = "-";
                                else if (pressedKey.Key == ConsoleKey.OemMinus || pressedKey.Key == ConsoleKey.Subtract)
                                    break;
                                else _a += pressedKey.KeyChar;
                                break;

                            case 2:
                                if ((pressedKey.Key == ConsoleKey.OemMinus || pressedKey.Key == ConsoleKey.Subtract) && _b.Length == 0)
                                    _b = "-";
                                else if (pressedKey.Key == ConsoleKey.OemMinus || pressedKey.Key == ConsoleKey.Subtract)
                                    break;
                                else
                                    _b += pressedKey.KeyChar;
                                break;

                            case 3:
                                if ((pressedKey.Key == ConsoleKey.OemMinus || pressedKey.Key == ConsoleKey.Subtract) && _c.Length == 0)
                                    _c = "-";
                                else if (pressedKey.Key == ConsoleKey.OemMinus || pressedKey.Key == ConsoleKey.Subtract)
                                    break;
                                else
                                    _c += pressedKey.KeyChar;
                                break;
                        }
                        break;

                    case ConsoleKey.Backspace:
                        switch (menuItem)
                        {
                            case 1:
                                _a = _a.Length > 0 ? _a.Substring(0, _a.Length - 1) : _a;
                                break;

                            case 2:
                                _b = _b.Length > 0 ? _b.Substring(0, _b.Length - 1) : _b;
                                break;

                            case 3:
                                _c = _c.Length > 0 ? _c.Substring(0, _c.Length - 1) : _c;
                                break;
                        }
                        break;
                    
                    case ConsoleKey.Enter:
                        var ex = new InputDataException("Incorrect input");
                        string availablePeriod = $" (value should be from {Int32.MinValue} to {Int32.MaxValue})";

                        if (_a.Length == 0)  _a = "0";
                        if (_b.Length == 0)  _b = "0";
                        if (_c.Length == 0)  _c = "0";

                        if (!Int32.TryParse(_a, out a))
                            ex.Data.Add("a",_a + availablePeriod);
                        if (!Int32.TryParse(_b, out b))
                            ex.Data.Add("b", _b + availablePeriod );
                        if (!Int32.TryParse(_c, out c))
                            ex.Data.Add("c", _c + availablePeriod);
                        if (ex.Data.Count > 0)
                            throw ex;
                        return pressedKey.Key;

                    case ConsoleKey.Escape:
                        return pressedKey.Key;

                    default:
                        break;
                }
            }
        }

    }
}
