using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Model.Interface
{
    public interface IDocumentMemento
    {
        DateTime SavedAt { get; }
        string Description { get; }
    }
}
