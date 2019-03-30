using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.SharedCode.Contracts
{
    internal interface IObfuscate
    {
        string Obfuscate(string ClearText);

        string Deobfuscate(string ObfuscatedText);
    }
}