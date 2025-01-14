using Memento.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Model
{
    public class DocumentMemento : IDocumentMemento
    {
        private readonly string content;
        private readonly Dictionary<string, string> formatting;
        private readonly Dictionary<string, string> metadata;
        private readonly DateTime savedAt;
        private readonly string description;

        public DateTime SavedAt => savedAt;
        public string Description => description;

        public DocumentMemento(string content, Dictionary<string, string> formatting,
            Dictionary<string, string> metadata, string description)
        {
            this.content = content;
            this.formatting = new Dictionary<string, string>(formatting);
            this.metadata = new Dictionary<string, string>(metadata);
            this.savedAt = DateTime.Now;
            this.description = description;
        }

        /// <summary>
        /// Nur Originator kann ganzen State accessen
        /// </summary>
        /// <returns></returns>
        internal string GetContent() => content;
        internal Dictionary<string, string> GetFormatting() => new Dictionary<string, string>(formatting);
        internal Dictionary<string, string> GetMetadata() => new Dictionary<string, string>(metadata);
    }
}
