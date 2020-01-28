using System;
using System.Threading;

namespace TreadTest
{
    class AsyncCaller
    {
        private EventHandler eventHandler;

        public AsyncCaller(EventHandler handler)
        {
            eventHandler = handler;

        }

        public bool Invoke(int duration, object sender, EventArgs e)
        {
            IAsyncResult res = eventHandler.BeginInvoke(sender, e, null, null);
            Thread.Sleep(duration);
            return res.IsCompleted;
        }
    }
}
