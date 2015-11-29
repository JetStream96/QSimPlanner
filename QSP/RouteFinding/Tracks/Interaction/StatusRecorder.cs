using System.Collections.Generic;
using System.Collections.ObjectModel;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Interaction
{

    public class StatusRecorder
    {


        private List<Entry> _records;
        public ReadOnlyCollection<Entry> Records
        {
            get { return _records.AsReadOnly(); }
        }

        public StatusRecorder()
        {
            _records = new List<Entry>();
        }

        public void AddEntry(Severity Severity, string Message, TrackType Type)
        {
            _records.Add(new Entry(Severity, Message, Type));
        }

        public void Clear()
        {
            _records.Clear();
        }


        public void Clear(TrackType type)
        {

            for (int i = _records.Count - 1; i >= 0; i --)
            {
                if (_records[i].Type == type)
                {
                    _records.RemoveAt(i);
                }

            }

        }

        public enum Severity
        {

            Advisory = 0,
            Caution = 1,
            Critical = 2

        }

        public class Entry
        {

            private Severity _severity;
            private string _message;

            private TrackType _type;
            public Severity Severity
            {
                get { return _severity; }
            }

            public string Message
            {
                get { return _message; }
            }

            public TrackType Type
            {
                get { return _type; }
            }


            public Entry(Severity Severity, string Message, TrackType Type)
            {
                _severity = Severity;
                _message = Message;
                _type = Type;

            }

        }

    }

}
