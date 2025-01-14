using Memento.Model.Interface;
using Memento.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Caretaker
{
    public class DocumentHistory(Document document, int maxHistorySize = 10)
    {
        private readonly Stack<DocumentMemento> history = new Stack<DocumentMemento>();
        private readonly Document document = document;
        private readonly int maxHistorySize = maxHistorySize;

        public void SaveState(string description)
        {
            if (history.Count >= maxHistorySize)
            {
                /// Wenn max größe erreicht --> ältestes memento entfernen
                var tempStack = new Stack<DocumentMemento>();
                while (history.Count > maxHistorySize - 1)
                {
                    history.Pop();
                }
                while (tempStack.Count > 0)
                {
                    history.Push(tempStack.Pop());
                }
            }
            history.Push(document.CreateMemento(description));
        }

        public bool Undo()
        {
            if (history.Count == 0)
                return false;

            var memento = history.Pop();
            document.RestoreFromMemento(memento);
            return true;
        }

        public List<IDocumentMemento> GetHistory()
        {
            return new List<IDocumentMemento>(history);
        }
    }
}
