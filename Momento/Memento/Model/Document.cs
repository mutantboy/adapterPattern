using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Model
{
    public class Document(string id)
    {
        private string content = string.Empty;
        private Dictionary<string, string> formatting = new Dictionary<string, string>();
        private Dictionary<string, string> metadata = new Dictionary<string, string>();
        private readonly string documentId = id;

        // Content operations
        public void UpdateContent(string newContent)
        {
            content = newContent;
        }

        public void ApplyFormatting(string element, string style)
        {
            formatting[element] = style;
        }

        public void UpdateMetadata(string key, string value)
        {
            metadata[key] = value;
        }

        /// Memento erstellen
        public DocumentMemento CreateMemento(string description)
        {
            return new DocumentMemento(content, formatting, metadata, description);
        }

        /// <summary>
        /// Von Memento wiederherstellen
        /// </summary>
        /// <param name="memento"></param>
        public void RestoreFromMemento(DocumentMemento memento)
        {
            content = memento.GetContent();
            formatting = memento.GetFormatting();
            metadata = memento.GetMetadata();
        }

        public override string ToString()
        {
            return $"Document {documentId}:\nContent: {content}\n" +
                   $"Formatting: {string.Join(", ", formatting)}\n" +
                   $"Metadata: {string.Join(", ", metadata)}";
        }
    }
}
