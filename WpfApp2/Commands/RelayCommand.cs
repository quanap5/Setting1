using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2.Commands
{
    /// <summary>
    /// This is RelayCommand to solve the problem of Individual Commands
    /// Following https://codeproject.com/Articles/1052346/ICommand-Interface-in-WPF for more details
    /// </summary>
    public class RelayCommand: ICommand
    {
        private Action commandTask;
        private Action<object> commandTask2;

        public RelayCommand(Action workToDo)
        {
            commandTask = workToDo;
        }
        public RelayCommand(Action<object> workTodDo2)
        {
            commandTask2 = workTodDo2;
        }



        public event EventHandler CanExecuteChanged;
        //{

        //    //add { CommandManager.RequerySuggested += value; }
        //    //remove { CommandManager.RequerySuggested -= value; }
        //}

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                commandTask();
                Console.WriteLine("command");
            }
            else
            {
                commandTask2(parameter);
                Console.WriteLine("command2");
            }
                


        }
    }
}
