using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSP.LibraryExtension
{
    // Runs the added tasks sequentially.
    // Add any task and it will be run automatically.
    // 
    public class SequentialTaskRunner
    {
        private object _lock = new object();
        private Queue<Task> _tasks = new Queue<Task>();

        public SequentialTaskRunner() { }

        public void AddTask(Task task)
        {
            lock (_lock)
            {
                _tasks.Enqueue(task);
            }

            runAll();
        }

        private void runAll()
        {
            while (true)
            {
                Task task = null;

                lock (_lock)
                {
                    if (_tasks.Count == 0)
                    {
                        return;
                    }
                    task = _tasks.Dequeue();
                }

                task.Start();
                task.Wait();
            }
        }

        public bool IsBusy()
        {
            lock (_lock)
            {
                return _tasks.Count != 0;
            }
        }
    }
}
