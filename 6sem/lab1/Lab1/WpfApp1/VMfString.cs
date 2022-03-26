using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;

namespace WpfApp1
{
    public class VMfString
    {
        public VMF FuncType { get; set; }
        public string FuncName { get; set; }
        public VMfString(VMF func_type, string name)
        {
            FuncType = func_type; FuncName = name;
        }
        public VMfString() { }
    }
}
