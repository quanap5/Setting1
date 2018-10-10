using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2.Commands
{
    public class RelayCommand2 : ICommand
    {
        #region Fields

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        private string _displayText;

        public static List<string> Log = new List<string>();
        private Action<object> action;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand2(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand2(Action<object> execute, Predicate<object> canExecute)
            : this(execute, canExecute, "")
        {
        }

        public RelayCommand2(Action<object> execute, Predicate<object> canExecute, string displayText)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
            _displayText = displayText;
        }


        public string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }

        #endregion // Constructors

        #region ICommand Members

        
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {

            _execute(parameter);
        }

        #endregion // ICommand Members
    }

}
