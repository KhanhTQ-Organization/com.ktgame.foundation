using System.Collections.Generic;

namespace com.ktgame.foundation.patterns.behavioral.command
{
    /// <summary> Asks the command to carry out the request. </summary>
    public class CommandInvoker
    {
        private readonly List<ICommand> undo = new List<ICommand>();
        private readonly List<ICommand> redo = new List<ICommand>();

        /// <summary> Maximum number of commands to keep in history to prevent memory leaks. </summary>
        public int MaxHistory { get; set; } = 50;

        /// <summary> Execute a command and add it to the undo redo stack if success. </summary>
        /// <param name="command"></param>
        /// <returns>True if the execution was successful.</returns>
        public bool Execute(ICommand command)
        {
            if (command.OnExecute() == true)
            {
                undo.Add(command);
                if (undo.Count > MaxHistory)
                {
                    undo.RemoveAt(0); // Remove oldest command to free memory
                }
                redo.Clear();
                return true;
            }

            return false;
        }

        /// <summary> Reverse the last command executed. </summary>
        public void Undo()
        {
            if (undo.Count > 0)
            {
                ICommand executed = undo[undo.Count - 1];
                undo.RemoveAt(undo.Count - 1);
                
                executed.OnUndo();
                redo.Add(executed);
            }
            else
            {
                Log.Warning("Undo stack empty");
            }
        }

        /// <summary> Replay the last undone command. </summary>
        public void Redo()
        {
            if (redo.Count > 0)
            {
                ICommand undone = redo[redo.Count - 1];
                redo.RemoveAt(redo.Count - 1);
                
                if (undone.OnExecute() == true)
                {
                    undo.Add(undone);
                }
            }
            else
            {
                Log.Warning("Redo stack empty");
            }
        }
    }
}