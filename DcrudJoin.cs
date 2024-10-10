using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcrud
{
    internal class DcrudJoin
    {
        public string Message()
        {
            return "This is another file of class DcrudJoin";
        }

        public string[] ReverseArray(string[] stringArray)
        {
            string[] updateArray = new string[stringArray.Length];
            int arrayLength = stringArray.Length;
            for(int i = 0; i < stringArray.Length; i++)
            {
                updateArray[i] = stringArray[arrayLength - i - 1];
            }
            return updateArray;
        }
    }
}
