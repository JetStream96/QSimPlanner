using System.Collections.Generic;
using System.Threading;

namespace QSP.LibraryExtension
{

    public class STATaskRunner
    {
        // As soon as a task is added, this handler empties its queue by 
        // executing all added tasks sequentially, on seperate threads in STA.
        // The fact that tasks are executed in STA means this cannot 
        // be done simply with Async/Await. 

        private Queue<Subroutine> tasks;
        public delegate void Subroutine();
        private object _lock;

        public STATaskRunner()
        {
            tasks = new Queue<Subroutine>();
            _lock = new object();
        }

        public void AddTask(Subroutine item)
        {
            tasks.Enqueue(item);

            Thread thread1 = new Thread(DoAll);
            thread1.SetApartmentState(ApartmentState.STA);
            thread1.Start();
        }

        private void DoAll()
        {
            lock (_lock)
            {
                Subroutine runTask = null;
                while (tasks.Count > 0)
                {
                    runTask = tasks.Dequeue();
                    runTask();
                }
            }
        }

        public bool IsBusy()
        {
            if (Monitor.TryEnter(_lock))
            {
                if (tasks.Count == 0)
                {
                    Monitor.Exit(_lock);
                    return false;
                }
                Monitor.Exit(_lock);
            }
            return true;
        }
    }
}
