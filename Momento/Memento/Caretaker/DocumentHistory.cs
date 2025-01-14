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
            /// Wenn max size untere entfernen
            if (history.Count >= maxHistorySize)
            {
                DocumentMemento[] tempArray = history.ToArray();
                history.Clear();
                /// ältesten state überspringen
                for (int i = 0; i < tempArray.Length - 1; i++)
                {
                    history.Push(tempArray[tempArray.Length - 2 - i]);
                }
            }

            /// neuen state hinzufügen
            history.Push(document.CreateMemento(description));
        }

        public bool Undo()
        {
            if (history.Count == 0)
                return false;

            ///jetziger state weg
            history.Pop();

            if (history.Count == 0)
            {
                /// komplett leeren dokument state erstellen
                document.UpdateContent(string.Empty);
                document.RestoreFromMemento(new DocumentMemento(
                    string.Empty,
                    new Dictionary<string, string>(),
                    new Dictionary<string, string>(),
                    "Initial empty state"
                ));
                return true;
            }

            /// vorherigen state wiederherstellen
            var previousState = history.Peek();
            document.RestoreFromMemento(previousState);
            return true;
        }

        public List<IDocumentMemento> GetHistory()
        {
            return new List<IDocumentMemento>(history);
        }
    }

    // tag::DocumentHistory[]
    //public class DocumentHistory
    //{
    //    private readonly Stack<DocumentMemento> history;
    //    private readonly Document document;
    //    private readonly int maxHistorySize;

    //    public DocumentHistory(Document document, int maxHistorySize = 10)
        

    //    public void SaveState(string description)
       

    //    public bool Undo()
        

    //    public List<IDocumentMemento> GetHistory()
        
    //}
    // end::DocumentHistory[]
}
