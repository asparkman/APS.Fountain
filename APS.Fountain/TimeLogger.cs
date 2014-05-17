using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace APS.Fountain
{
    public class TimeLogger
    {
        private DateTime _LastExecStart { get; set; }
        private DateTime _ExecStart { get; set; }

        private DateTime _LastExecEnd { get; set; }
        private DateTime _ExecEnd { get; set; }

        private TimeSpan _TimeSinceLastExec { get; set; }
        private TimeSpan _TimeForThisExec { get; set; }

        private TimeSpan _MaxTimeSinceLastExec { get; set; }
        private TimeSpan _MinTimeSinceLastExec { get; set; }

        private TimeSpan _MaxTimeForThisExec { get; set; }
        private TimeSpan _MinTimeForThisExec { get; set; }

        public virtual DateTime LastExecStart
        {
            get {  return _LastExecStart; }
            set { _LastExecStart = value; }
        }
        public virtual DateTime ExecStart
        {
            get { return _ExecStart; }
            set
            {
                _ExecStart = value;
                _TimeSinceLastExec = _ExecStart - _LastExecStart;
                if (_TimeSinceLastExec > _MaxTimeSinceLastExec)
                    _MaxTimeSinceLastExec = _TimeSinceLastExec;
                if (_TimeSinceLastExec < _MinTimeSinceLastExec)
                    _MinTimeSinceLastExec = _TimeSinceLastExec;
            }
        }

        public virtual DateTime LastExecEnd
        {
            get { return _LastExecEnd; }
            set { _LastExecEnd = value; }
        }
        public virtual DateTime ExecEnd
        {
            get { return _ExecEnd; }
            set
            {
                _ExecEnd = value;
                _TimeForThisExec = _ExecEnd - _ExecStart;
                if (_TimeForThisExec > _MaxTimeForThisExec)
                    _MaxTimeForThisExec = _TimeForThisExec;
                if (_TimeForThisExec < _MinTimeForThisExec)
                    _MinTimeForThisExec = _TimeForThisExec;
            }
        }

        public virtual TimeSpan TimeSinceLastExec { get { return _TimeSinceLastExec; } }
        public virtual TimeSpan TimeForThisExec { get { return _TimeForThisExec; } }

        public virtual TimeSpan MinTimeSinceLastExec { get { return _MinTimeSinceLastExec; } }
        public virtual TimeSpan MaxTimeSinceLastExec { get { return _MaxTimeSinceLastExec; } }

        public virtual TimeSpan MinTimeForThisExec { get { return _MinTimeForThisExec; } }
        public virtual TimeSpan MaxTimeForThisExec { get { return _MaxTimeForThisExec; } }

        public override string ToString()
        {
            var result = "";
            result += string.Format(
                    "Time Since Last Exec: ({0}, {1}, {2})\r\n", 
                    TimeSinceLastExec.Milliseconds,
                    MinTimeSinceLastExec.Milliseconds,
                    MaxTimeSinceLastExec.Milliseconds
                );
            result += string.Format(
                    "Time For This Exec: ({0}, {1}, {2})",
                    TimeForThisExec.Milliseconds,
                    MinTimeForThisExec.Milliseconds,
                    MaxTimeForThisExec.Milliseconds
                );
            return result;
        }
    }
}
