using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

namespace Dcrud
{
    class Crud
    {
        interface IEncapMethod
        {
            public string CreateInfo();
        }

        class UseTupple
        {
            string serInfoName = "";
            Tuple<int, string> serInfo = new Tuple<int, string>(4, "John"); //Using Constructor of Tuple Class
            public void CreateTuple()
            {
                var personInfo = Tuple.Create("seeHongaty", "London"); // Uing Create method;
                serInfoName = personInfo.Item1;
                displayTuple(personInfo); //Tuple as a Method Parameter


                void displayTuple(Tuple<string, string> aTuple)
                {
                    Console.WriteLine(serInfoName);
                    Console.WriteLine(aTuple.Item2);
                }

            }
            static Tuple<string, int, bool> methodTuple() //Tuple as a Return Type
            {
                return Tuple.Create("bunty", 25, true);
            }
            public Tuple<string, int, bool> tupleByMethod = methodTuple();

        }

        class InheritInterface : IEncapMethod
        {
            public string CreateInfo()
            {
                return "This is method that show the use way of Interface";
            }
        }
        static void Main(string[] args)
        {
            string[] fruit = { "Mango", "Apple", "Orange" };
            /* Tuple, Delegates, Exception handling, Collection & Generics
             * char to int to long to float to double can use with explicit casting */

            InheritInterface InterfaceCreateInfo = new InheritInterface();
            string userObject = InterfaceCreateInfo.CreateInfo();
            //Console.WriteLine(userObject);

            UseTupple useTupple = new UseTupple();
            //useTupple.CreateTuple();
            //Console.WriteLine(useTupple.tupleByMethod);

            DcrudJoin Join = new DcrudJoin();
            //Console.WriteLine(Join.Message());

            string[] fruitRevarse = Join.ReverseArray(fruit);
            //foreach (string f in fruitRevarse) { Console.WriteLine(f); }

            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var (evenNumbers, oddNumbers) = evenOrOdd(numbers);
            //Console.WriteLine("Even numbers: " + string.Join(", ", evenNumbers));
            //Console.WriteLine("Odd numbers: " + string.Join(", ", oddNumbers));

            Collections delegatesCol = new Collections();
            string come = delegatesCol.CollectionWithDelegates();
            //Console.WriteLine(come);
            string FuncCome = delegatesCol.FuncDeleg();

            // Indexer
            string user = "Jhon";
            Indexers consIndexer = new Indexers(user);
            string[] IndMess = new string[6];
            string returnconsIndexer = consIndexer.makeString(IndMess);
            //Console.WriteLine(returnconsIndexer);

            // 
            Dictionary arrayDictionary = new Dictionary();
            string fruitOrdered = arrayDictionary.CollectionDictinary();
            //Console.WriteLine(fruitOrdered);
            //arrayDictionary.iterationOfArrayList();
            string enumearatorArray = arrayDictionary.IEnumeratorfun();
            //foreach(char enumearator in enumearatorArray)
            //{
            //    Console.WriteLine(enumearator);
            //}
            arrayDictionary.dictionaryTech();

        }

        public static (int[], int[]) evenOrOdd(int[] intArray)
        {
            List<int> evenElements = new List<int>();
            List<int> oddElements = new List<int>();

            for (int i = 0; i < intArray.Length; i++)
            {
                if (intArray[i] % 2 == 0)
                {
                    evenElements.Add(intArray[i]);
                }
                else
                {
                    oddElements.Add(intArray[i]);
                }
            }

            return (evenElements.ToArray(), oddElements.ToArray());
        }

    } 
}
