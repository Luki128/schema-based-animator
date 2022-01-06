using System;
using System.Collections.Generic;
using System.Text;

namespace LewyDiagnostic
{
    static class dbg
    {
        public static bool errorsEnabled = true;
        public static bool waringsEnabled = true;
        public static bool infoEnabled = true;
        public static bool testInfoEnabled = true;
        public static Random rng = new Random();//324343 -- 13864



        public static int succesCounter = 0;
        public static int failCounter = 0;
        public static int warningCounter = 0;


        public static void ResetTestInf()
        {
            succesCounter = 0;
            failCounter = 0;
            warningCounter = 0;
        }
        public static void NewLine()
        {
            Console.WriteLine("");
        }
        public static void WriteLine(string str, ConsoleColor textColor)
        {
            var tem = Console.ForegroundColor;

            Console.ForegroundColor = textColor;
            Console.WriteLine(str);
            Console.ForegroundColor = tem;
        }
        public static void Write(string str, ConsoleColor textColor)
        {
            var tem = Console.ForegroundColor;

            Console.ForegroundColor = textColor;
            Console.Write(str);
            Console.ForegroundColor = tem;
        }
        public static void Title(string str, int size, char paddnigChar, ConsoleColor textColor)
        {
            Title(str, size, paddnigChar, textColor, textColor);
        }
        public static void Centred(string str, int size, ConsoleColor textColor)
        {
            Title(str, size, ' ', textColor, textColor);
        }
        public static void PutCentred(string str, int size, ConsoleColor textColor)
        {
            var tem = Console.ForegroundColor;
            int padding = (size / 2) - (str.Length / 2);
            int correction = size - (2 * padding) - str.Length;

            Console.ForegroundColor = textColor;
            Console.Write("".PadLeft(padding, ' '));
            Console.Write(str);
            Console.Write("".PadLeft(padding + correction, ' '));
            Console.ForegroundColor = tem;
        }
        public static void Title(string str, int size, char paddnigChar, ConsoleColor tileColor, ConsoleColor paddnigColor)
        {
            var tem = Console.ForegroundColor;
            int padding = (size / 2) - (str.Length / 2);
            int correction = size - (2 * padding) - str.Length;

            Console.ForegroundColor = paddnigColor;
            Console.Write("".PadLeft(padding, paddnigChar));
            Console.ForegroundColor = tileColor;
            Console.Write(str);
            Console.ForegroundColor = paddnigColor;
            Console.WriteLine("".PadLeft(padding + correction, paddnigChar));
            Console.ForegroundColor = tem;
        }
        public static void WriteProperty(string propName, ConsoleColor nameColor, string prpValue, ConsoleColor valueColor)
        {
            var tem = Console.ForegroundColor;

            Console.ForegroundColor = nameColor;
            Console.Write(propName + "\t: ");
            Console.ForegroundColor = valueColor;
            Console.WriteLine(prpValue);
            Console.ForegroundColor = tem;
        }

        public static void Error(string message)
        {
            if (!errorsEnabled) return;

            WriteLine(message, ConsoleColor.Red);
        }
        public static void Info(string message)
        {
            if (!infoEnabled) return;
            WriteLine(message, ConsoleColor.Blue);
        }
        public static void Wraning(string message)
        {
            if (!waringsEnabled) return;
            warningCounter++;
            WriteLine(message, ConsoleColor.Yellow);
        }
        public static void Succes(string message)
        {
            succesCounter++;
            if (!testInfoEnabled) return;

            WriteLine(message, ConsoleColor.Green);
        }
        public static void Fail(string message)
        {
            failCounter++;
            if (!testInfoEnabled) return;

            WriteLine(message, ConsoleColor.Red);
        }

        public static void Test(bool condytion, string debugData, string failMsg = "fail", string succecMsg = "succes")
        {
            if (condytion)
                Succes($"[{succecMsg}]: {debugData}");
            else
                Fail($"[{failMsg}]: {debugData}");
        }
        public static bool Raport(bool rstCounters = true)
        {
            //WriteLine("---------------Tests for system report---------------\n", ConsoleColor.Yellow);
            Title("Tests for system report", 50, '-', ConsoleColor.Yellow, ConsoleColor.Cyan);

            WriteProperty("\nFails", ConsoleColor.Red, $"{failCounter}", ConsoleColor.DarkRed);
            WriteProperty("Succes", ConsoleColor.Green, $"{succesCounter}", ConsoleColor.DarkGreen);
            WriteProperty("Total", ConsoleColor.Blue, $"{failCounter + succesCounter} \n", ConsoleColor.DarkBlue);

            Title("Conclusion", 50, '-', ConsoleColor.Yellow, ConsoleColor.Cyan);

            if (failCounter > 0)
                Centred("System failed, not all test passed", 50, ConsoleColor.Red);
            else
                Centred("System pass tests successfully ", 50, ConsoleColor.Green);
            Title("", 50, '-', ConsoleColor.Cyan);

            bool failed = (failCounter == 0);

            if (rstCounters)
            {
                failCounter = 0;
                succesCounter = 0;
            }
            return failed;
        }
        public static bool ClassicRaport(bool rstCounters = true)
        {
            //WriteLine("---------------Tests for system report---------------\n", ConsoleColor.Yellow);
            Title("Work Done", 50, '-', ConsoleColor.Yellow, ConsoleColor.Cyan);

            WriteProperty("\nErrors    ", ConsoleColor.Red, $"{failCounter}", ConsoleColor.DarkRed);
            WriteProperty("\nWarnings   ", ConsoleColor.Red, $"{warningCounter}", ConsoleColor.DarkYellow);
           

            Title("Conclusion", 50, '-', ConsoleColor.Yellow, ConsoleColor.Cyan);

            if (failCounter > 0)
                Centred("System failed", 50, ConsoleColor.Red);
            else
                Centred("System pass successfully ", 50, ConsoleColor.Green);
            Title("", 50, '-', ConsoleColor.Cyan);

            bool failed = (failCounter == 0);

            if (rstCounters)
            {
                failCounter = 0;
                succesCounter = 0;
            }
            return failed;
        }
        public static void CRT_STOP(int exitCode = -20)
        {
            Wraning("Appliaction has been halted press any key to end apllication...");
            Console.Read();
            Environment.Exit(exitCode);
        }
    }
}