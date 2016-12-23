using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;

namespace QSP.RouteFinding.Tracks.Interaction
{
    // Exceptions occurring at different stage are categorized into 
    // 3 severity levels:
    // (1) Advisory: Very minor error.
    // (2) Causion: Small error such as failed to read/add a single track.
    // (3) Critical: Failed to download the entire NATs, or an exception 
    //     making all tracks of PACOTs unusable.

    /// <summary>
    /// Records the status of downloading/processing tracks. Exceptions
    /// are recorded and some are displayed to the user.
    /// </summary>
    public class StatusRecorder
    {
        private List<Entry> _records;

        public IReadOnlyList<Entry> Records => _records;

        public StatusRecorder()
        {
            _records = new List<Entry>();
        }

        public void AddEntry(Severity Severity, string Message, TrackType Type)
        {
            _records.Add(new Entry(Severity, Message, Type));
        }

        public void AddEntries(IEnumerable<Entry> entries)
        {
            _records.AddRange(entries);
        }

        public void Clear()
        {
            _records.Clear();
        }

        public void Clear(TrackType type)
        {
            for (int i = _records.Count - 1; i >= 0; i--)
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
            public Severity Severity { get; private set; }
            public string Message { get; private set; }
            public TrackType Type { get; private set; }

            public Entry(Severity Severity, string Message, TrackType Type)
            {
                this.Severity = Severity;
                this.Message = Message;
                this.Type = Type;
            }
        }
    }
}
