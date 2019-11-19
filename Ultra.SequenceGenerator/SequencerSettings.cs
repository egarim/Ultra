using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.SequenceGenerator
{
    public static class SequencerSettings
    {
        public static int MaxIdGenerationAttempts = 7;
        public static int MinConflictDelay = 50;
        public static int MaxConflictDelay = 500;
    }
}
