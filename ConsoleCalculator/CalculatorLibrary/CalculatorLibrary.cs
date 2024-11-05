using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonTextWriter writer;
        public int TimesUsed { get; private set; }
        public Calculator()
        {
            TimesUsed = 0;
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");

            switch (op)
            {
                case "add":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;

                case "sub":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;

                case "mult":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;

                case "div":
                    while (num2 == 0)
                    {
                        Console.WriteLine("Cannot divide by 0. Please enter another number.");
                        num2 = Convert.ToDouble(Console.ReadLine());
                    }

                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        writer.WriteValue("Divide");
                    }
                    break;

                case "sq":
                    result = Math.Sqrt(num1);
                    writer.WriteValue("Square Root");
                    break;

                case "pow":
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Raised to the Power of");
                    break;

                case "10x":
                    result = num1 * 10;
                    writer.WriteValue("10X");
                    break;

                case "sin":
                    result = Math.Sin(num1);
                    writer.WriteValue("Sin");
                    break;

                case "cos":
                    result = Math.Cos(num1);
                    writer.WriteValue("Cos");
                    break;

                case "tan":
                    result = Math.Tan(num1);
                    writer.WriteValue("Tan");
                    break;

                default:
                    Console.WriteLine("I'm sorry, I didn't understand that operation.");
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            TimesUsed++;

            return result;
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
