using System;
using System.Collections;
using System.Collections.Generic;

namespace Dcrud
{
    internal class Dictionary
    {
        public static ArrayList sweetFruit = new ArrayList(); // Ordered collection

        private Dictionary<int, string> wordDictionary = new Dictionary<int, string>(); // Generic collection, key unique

        public string CollectionDictinary()
        {
            sweetFruit.Add("Papaya");
            sweetFruit.Add("Peach"); // Corrected spelling
            sweetFruit.Add("Pineapple");
            sweetFruit.Add("Guava"); // Corrected spelling
            sweetFruit.Add("Banana");
            sweetFruit.Add("Grapes");
            int fruitCount = sweetFruit.Count;
            int fruitCapacity = sweetFruit.Capacity;
            int arraylistInIndexof = sweetFruit.IndexOf("Banana");
            return $"Ordered collection using ArrayList, Where Count is {fruitCount} and Capacity is {fruitCapacity}" +
                   $" and the index of Banana is {arraylistInIndexof}";
        }

        public void iterationOfArrayList()
        {
            foreach (string fruit in sweetFruit)
            {
                Console.WriteLine(fruit);
            }
        }

        public string IEnumeratorfun()
        {
            IEnumerator enumeratorFruit = sweetFruit.GetEnumerator();
            string result = "";
            while (enumeratorFruit.MoveNext())
            {
                result += enumeratorFruit.Current + "\n";
            }
            return result;
        }

        public string dictionaryTech()
        {
            wordDictionary.Add(1, "C");
            wordDictionary.Add(2, "C++");
            wordDictionary.Add(3, "Java");
            wordDictionary.Add(4, "Python");
            wordDictionary.Add(5, "C#");
            wordDictionary.Add(6, "HTML");

            foreach (KeyValuePair<int, string> tech in wordDictionary)
            {
                Console.WriteLine($"{tech.Key}: {tech.Value}");
            }
            return "welcome";
        }
    }
}
