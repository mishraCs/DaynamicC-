using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Dcrud.Collections;

namespace Dcrud
{
    internal class Collections
    {
        public delegate void CollectionDelegate();

        // System.Collections.Generic, System.Collection, System.Collections.ConCurrent;
        public string MethodGeneric()
        {
            List<int> intsGeneric = new List<int>();
            pushIndex(intsGeneric);
            List<int>.Enumerator intEnumerator = intsGeneric.GetEnumerator();
            int capGen = intsGeneric.Capacity;
            int counGen = intsGeneric.Count;
            string retString = counGen.ToString() + capGen.ToString();
            return retString;

            void pushIndex(List<int> intsGeneric)
            {
                intsGeneric.Add(1);
                intsGeneric.Add(2);
                intsGeneric.Add(3);
                intsGeneric.Add(4);
            }
        }


        public string CollectionWithDelegates()
        {
            // Delegates is a reference to a method, in case of static here only declared that which class of which method to do execute
            // but in case of instanse method, delegates have also information that which object from this method reference;
            // As like class, we can also declare delegates and create their object and access methods by there objects.
            // VoidDelegates del = VoidDelegates(string s){Console.WriteLine(s)}; del("abce");
            CollectionDelegate contDele = Morning;
            contDele += Afternoon;
            contDele += Evening;
            ToCallDelegates.ToCallDelegate(contDele);
            return "Welcome";
        }

        public string FuncDeleg()
        {
            Func<string> returnFunc = MakeFuncMessage;
            string message = returnFunc();
             Console.WriteLine(message);
            return message;
        }

        public string MakeFuncMessage()
        {
            return "Hello team, Come back and take rest to do something big";
        }


        private void Morning()
        {
            Console.WriteLine("Good Morning"); 
        }

        private void Afternoon()
        {
            Console.WriteLine("Good afternoon");
        }

        private void Evening()
        {
            Console.WriteLine("Good Evening");
        }
    }
}

class ToCallDelegates
{
    public static void ToCallDelegate(CollectionDelegate FuncDeleg)
    {
        FuncDeleg();
    }
}
