using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB.Exceptions;
using DevExpress.Xpo.Helpers;

namespace Ultra.SequenceGenerator
{
    public sealed class XpoServerId : XPLiteObject {
        static object SyncRoot = new object();
        [Key(false)]
        public int Zero {
            get { return 0; }
            set {
                if(value != 0)
                    throw new ArgumentException("0 expected");
            }
        }
        public string SequencePrefix;
        public XpoServerId(Session session) : base(session) { }
        static string cachedSequencePrefix = null;
        //static WeakReference dataLayerForCachedServerPrefix = new WeakReference(null);
        static IDataLayer dataLayerForCachedServerPrefix = null;
        public static void ResetCache() {
            dataLayerForCachedServerPrefix = null;
            // dataLayerForCachedServerPrefix.Target = null; <<< if WeakReference
        }
        public static string GetSequencePrefix(IDataLayer dataLayer) {
            if(dataLayer == null)
                throw new ArgumentNullException("dataLayer");
            lock(SyncRoot) {
                if(dataLayerForCachedServerPrefix/*.Target*/ != dataLayer) {
                    using(Session session = new Session(dataLayer)) {
                        XpoServerId sid = session.GetObjectByKey<XpoServerId>(0);
                        if(sid == null) {
                            // we can throw exception here instead of creating random prefix
                            sid = new XpoServerId(session);
                            sid.SequencePrefix = XpoDefault.NewGuid().ToString();
                            try {
                                sid.Save();
                            }
                            catch {
                                sid = session.GetObjectByKey<XpoServerId>(0, true);
                                if(sid == null)
                                    throw;
                            }
                        }
                        cachedSequencePrefix = sid.SequencePrefix;
                        dataLayerForCachedServerPrefix = dataLayer;
                        // dataLayerForCachedServerPrefix.Target = dataLayer; <<< if WeakReference
                    }
                }
                return cachedSequencePrefix;
            }
        }
       
        public static int GetNextUniqueValue(ISessionProvider SessionProvider, string sequencePrefix)
        {
            return GetNextUniqueValue(SessionProvider.Session.DataLayer, sequencePrefix);
        }
        public static int GetNextUniqueValue(XPBaseObject Session, string sequencePrefix)
        {
            return GetNextUniqueValue(Session.Session.DataLayer, sequencePrefix);
        }
        public static int GetNextUniqueValue(Session Session, string sequencePrefix)
        {
            return GetNextUniqueValue(Session.DataLayer, sequencePrefix);
        }
        public static int GetNextUniqueValue(IDataLayer dataLayer, string sequencePrefix) {
            if(dataLayer == null)
                throw new ArgumentNullException("dataLayer");
            //we don't need to include the suffix
            string realSeqPrefix = sequencePrefix; //+ '@' + GetSequencePrefix(dataLayer);
            
            return Sequencer.GetNextValue(dataLayer, realSeqPrefix);
        }
        public static int GetNextUniqueValue(string sequencePrefix) {
            return GetNextUniqueValue(XpoDefault.DataLayer, sequencePrefix);
        }
    }
}
