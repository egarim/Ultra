using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB.Exceptions;

namespace Ultra.SequenceGenerator
{
    public sealed class Sequencer : XPBaseObject {
        // use GUID keys to prepare your database for replication
        [Key(true)]
        public Guid Oid;
        
        [Size(254), Indexed(Unique = true)]
        public string SequenceId;
        public int CurrentSequence;
        public Sequencer(Session session) : base(session) { }
       
        public static int GetNextValue(IDataLayer dataLayer, string SequenceId) {
            if(dataLayer == null)
                throw new ArgumentNullException("dataLayer");
            if(SequenceId == null)
                SequenceId = string.Empty;

            int attempt = 1;
            while(true) {
                try {
                    using(Session generatorSession = new Session(dataLayer)) {
                        Sequencer generator = generatorSession.FindObject<Sequencer>(new OperandProperty(nameof(Sequencer.SequenceId)) == SequenceId);
                        if(generator == null) {
                            generator = new Sequencer(generatorSession);
                            generator.SequenceId = SequenceId;
                        }
                        generator.CurrentSequence++;
                        generator.Save();
                        return generator.CurrentSequence;
                    }
                }
                catch(LockingException) {
                    if(attempt >= SequencerSettings.MaxIdGenerationAttempts)
                        throw;
                }
                if(attempt > SequencerSettings.MaxIdGenerationAttempts / 2)
                    Thread.Sleep(new Random().Next(SequencerSettings.MinConflictDelay, SequencerSettings.MaxConflictDelay));

                attempt++;
            }
        }
        public static int GetNextValue(string sequencePrefix) {
            return GetNextValue(XpoDefault.DataLayer, sequencePrefix);
        }
    }
}
