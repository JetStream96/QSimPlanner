using System;

namespace QSP.RouteFinding.Tracks.Actions
{
    public class ActionSequence
    {
        public Action Before { get; }
        public Action After { get; }

        public ActionSequence(Action Before, Action After)
        {
            this.Before = Before;
            this.After = After;
        }

        public static ActionSequence Empty => new ActionSequence(() => { }, () => { });
    }
}