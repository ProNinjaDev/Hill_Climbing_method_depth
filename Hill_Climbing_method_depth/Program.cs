using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hill_Climbing_method_depth
{
    public class Encoding
    {
        public string Data { get; set; }
        public int Adaptation { get; set; }

        public Encoding(string data, int adaptation)
        {
            Data = data;
            Adaptation = adaptation;
        }

        public override string ToString()
        {
            return $"{Data} - {Adaptation}";
        }

        public override bool Equals(object obj) // todo: Перепроверить несколько раз на повторения
        {
            if(obj is Encoding otherEncoding)
            {
                return Data == otherEncoding.Data;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }


    internal class Program
    {
        static List<Encoding> FormationNeighborhood(Encoding encoding, List<Encoding> searchSpace)
        {
            List<Encoding> neighborhood = new List<Encoding>();

            foreach(var neighbor in searchSpace)
            {
                if(HemmingwayDistance(neighbor.Data, encoding.Data) == 1)
                {
                    neighborhood.Add(neighbor);
                }
            }
            return neighborhood;

        }

        static int HemmingwayDistance(string s1, string s2)
        {
            int distance = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] != s2[i])
                {
                    distance++;
                }
            }
            return distance;
        }

        // Второй вариант
        public static int BinaryToDecimal(string binary)
        {
            int _decimal = 0;
            for (int i = 0; i < 5; ++i)
            {
                _decimal += int.Parse(binary[i].ToString()) * Convert.ToInt32(Math.Pow(2, 4 - i));
            }
            return _decimal;
        }

        // Третий вариант
        public static int QuadraticAdaptationFunction(string binary)
        {
            int decimalVal = BinaryToDecimal(binary);
            return Convert.ToInt32(Math.Pow((decimalVal - Convert.ToInt32(Math.Pow(2, 15 - 1))), 2));
        }

        static void Main(string[] args)
        {
            Random rnd = new Random();
            List<Encoding> searchSpace = new List<Encoding>();

            int numEncodings = Convert.ToInt32(Math.Pow(2, 5));

            while (true)
            {
                Console.WriteLine("1 - Generate encoding list");
                Console.WriteLine("2 - Run the algorithm");
                Console.WriteLine("3 - Exit the program");

                int path = int.Parse(Console.ReadLine());

                switch (path)
                {
                    case 1:
                        searchSpace.Clear();
                        HashSet<Encoding> uniqueEncodings = new HashSet<Encoding>();
                        // Генерация кодировок
                        while (uniqueEncodings.Count < numEncodings)
                        {
                            string data = "";

                            for (int j = 0; j < 5; ++j)
                            {
                                data += rnd.Next(0, 2).ToString();
                            }

                            int adaptation = BinaryToDecimal(data);
                            Encoding encoding = new Encoding(data, adaptation);

                            if (uniqueEncodings.Add(encoding))
                            {
                                searchSpace.Add(encoding);
                            }

                        }

                        Console.WriteLine("The list has been successfully generated!");
                        break;

                    case 2:
                        if (searchSpace.Count > 0)
                        {
                            HashSet<int> randIndexes = new HashSet<int>();
                            while (randIndexes.Count < 32)
                            {
                                randIndexes.Add(rnd.Next(0, numEncodings));
                            }

                            List<int> indexes = randIndexes.ToList();

                            for (int i = 0; i < 32; ++i)
                            {
                                Encoding encoding = searchSpace[indexes[i]];
                                Console.WriteLine($"[{indexes[i]}] {encoding}"); // Не рекомендует использовать ToString()
                            }

                            Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+\n");

                            // Метод восхождения на холм

                            Console.Write("Enter the number of steps: ");
                            int numSteps = int.Parse(Console.ReadLine());

                            Encoding currentEncoding = searchSpace[rnd.Next(0, searchSpace.Count)];

                            List<Encoding> checkedEncodings = new List<Encoding>();

                            for (int i = 0; i < numSteps; i++)
                            {
                                Console.WriteLine("\n=========================================================");
                                Console.WriteLine($"Step {i + 1}");
                                Console.WriteLine("---------------------------------------------------------");

                                Console.WriteLine($"Current encoding: {currentEncoding.Data} | Adaptation: {currentEncoding.Adaptation}");

                                List<Encoding> neighborhood = FormationNeighborhood(currentEncoding, searchSpace);
                                neighborhood = neighborhood.Except(checkedEncodings).ToList();

                                Console.WriteLine("\nNeighborhood:");
                                foreach (var neighbor in neighborhood)
                                {
                                    Console.WriteLine($"Encoding: {neighbor.Data} | Adaptation: {neighbor.Adaptation}");
                                }

                                if (neighborhood.Count > 0)
                                {
                                    Encoding randEncoding = neighborhood[rnd.Next(0, neighborhood.Count)];
                                    checkedEncodings.Add(randEncoding);

                                    Console.WriteLine($"\nNew encoding selected: {randEncoding.Data} | Adaptation: {randEncoding.Adaptation}");

                                    if (currentEncoding.Adaptation < randEncoding.Adaptation)
                                    {
                                        Console.WriteLine("Updating current encoding!");
                                        currentEncoding = randEncoding;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Current encoding remains unchanged.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Neighborhood is empty, algorithm completed.");
                                    break;
                                }

                                Console.WriteLine("\nCurrent maximum adaptation: " + currentEncoding.Adaptation);
                                Console.WriteLine("Current best encoding: " + currentEncoding.Data);
                            }

                            Console.WriteLine("\n*********************************************************");
                            Console.WriteLine("FINAL BEST SOLUTION:");
                            Console.WriteLine("---------------------------------------------------------");
                            Console.WriteLine($"Best encoding: {currentEncoding.Data}");
                            Console.WriteLine($"Maximum adaptation: {currentEncoding.Adaptation}");
                            Console.WriteLine("*********************************************************");

                        }
                        else
                        {
                            Console.WriteLine("The list is empty!");
                        }
                        break;

                    case 3:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Incorrect input!");
                        break;
                }
            }
        }
    }
}
