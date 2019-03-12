using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteElement
{
    class IllegalBridgeException : Exception
    {
        public IllegalBridgeException(string message) :
            base(message)
        { }
    }
}
