using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms
{
    public class AsyncCommand : Command
    {
        public AsyncCommand(Func<Task> execute) : base(() => execute())
        {
        }
        public AsyncCommand(Func<object, Task> execute) : base((o) => execute(o))
        {
        }
    }

    public class AsyncCommand<T> : Command
    {
        public AsyncCommand(Func<T, Task> execute) : base((o) => execute((T)o))
        {
        }

        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute) : base((o) => execute((T)o), (o) => canExecute((T)o))
        {
        }

        public AsyncCommand(Func<Task> execute, Func<T, bool> canExecute) : base((o) => execute(), (o) => canExecute((T)o))
        {
        }
    }
}
