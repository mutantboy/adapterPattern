using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Model
{
    
    public partial class Document
    {
        private string content;
        private Dictionary<string, string> formatting;
        private Dictionary<string, string> metadata;
        private readonly string documentId;
        public Document(string id)
        {
            documentId = id;
            content = string.Empty;
            formatting = new Dictionary<string, string>();
            metadata = new Dictionary<string, string>();
        }

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

        public DocumentMemento CreateMemento(string description)
        {
            var formattingCopy = new Dictionary<string, string>(formatting);
            var metadataCopy = new Dictionary<string, string>(metadata);

            return new DocumentMemento(content, formattingCopy, metadataCopy, description);
        }

        // Restore complete state from memento
        public void RestoreFromMemento(DocumentMemento memento)
        {
            // Clear and recreate collections instead of just clearing
            content = memento.GetContent();
            formatting = memento.GetFormatting(); 
            metadata = memento.GetMetadata(); 
        }

        public override string ToString()
        {
            var formattingStr = formatting.Count > 0
                ? $"[{string.Join(", ", formatting.Select(f => $"{f.Key}, {f.Value}"))}]"
                : "";

            var metadataStr = metadata.Count > 0
                ? $"[{string.Join(", ", metadata.Select(m => $"{m.Key}: {m.Value}"))}]"
                : "";

            return $"Document {documentId}:\n" +
                   $"Content: {content}\n" +
                   $"Formatting: {formattingStr}\n" +
                   $"Metadata: {metadataStr}";
        }
    }

    // tag::document[]
    //public class Document
    //{
    //    private string content;
    //    private Dictionary<string, string> formatting;
    //    private Dictionary<string, string> metadata;
    //    private readonly string documentId;

    //    public Document(string id);

    //    public void UpdateContent(string newContent);

    //    public void ApplyFormatting(string element, string style);

    //    public void UpdateMetadata(string key, string value);

    //    public DocumentMemento CreateMemento(string description);

    //    public void RestoreFromMemento(DocumentMemento memento);

    //    public override string ToString();
    //}
    // end::document[]
}
