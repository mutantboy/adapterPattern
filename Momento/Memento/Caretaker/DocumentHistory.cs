using Memento.Model.Interface;
using Memento.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Caretaker
{
    public class DocumentHistory
    {
        private readonly Stack<DocumentMemento> history;
        private readonly Document document;
        private readonly int maxHistorySize;

        public DocumentHistory(Document document, int maxHistorySize = 10)
        {
            this.document = document;
            this.maxHistorySize = maxHistorySize;
            history = new Stack<DocumentMemento>();
        }

        public void SaveState(string description)
        {
            // When max size reached, remove oldest (bottom of stack)
            if (history.Count >= maxHistorySize)
            {
                DocumentMemento[] tempArray = history.ToArray();
                history.Clear();
                // Skip the oldest state and keep the rest
                for (int i = 0; i < tempArray.Length - 1; i++)
                {
                    history.Push(tempArray[tempArray.Length - 2 - i]);
                }
            }

            // Add new state
            history.Push(document.CreateMemento(description));
        }

        public bool Undo()
        {
            if (history.Count == 0)
                return false;

            // Pop the current state
            history.Pop();

            // If there are no more states, return to empty state
            if (history.Count == 0)
            {
                document.RestoreFromMemento(document.CreateMemento("Empty state"));
                return true;
            }

            // Get the previous state (without removing it)
            var previousState = history.Peek();
            document.RestoreFromMemento(previousState);
            return true;
        }

        public List<IDocumentMemento> GetHistory()
        {
            return new List<IDocumentMemento>(history);
        }
    }
}
