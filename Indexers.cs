using System;

namespace Dcrud
{
    internal class Indexers
    {
        public string[] propIndexer = new string[6];
        public string paraString;

        public Indexers()
        {
            Console.WriteLine("default Constructor");
            paraString = "null";
        }

        public Indexers(string user)
        {
            this.paraString = $"Welcome in programming with {user}";
            Console.WriteLine(paraString);
        }

        public string this[int index]
        {
            get { return propIndexer[index]; }
            set { propIndexer[index] = value; }
        }

        public string makeString(string[] str)
        {
            try
            {
                this[0] = "Hello";
                this[1] = "body,";
                this[2] = "Think";
                this[3] = "beyond";
                this[4] = "and";
                this[5] = "grow";

                for (int i = 0; i < propIndexer.Length; i++)
                {
                    Console.WriteLine(propIndexer[i]);
                }
                return "User Constructor";
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Invalid argument {e}");
            }
        }
    }
}
