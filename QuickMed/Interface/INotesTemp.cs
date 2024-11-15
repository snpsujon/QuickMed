using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMed.Interface
{
    public interface INotesTemp : IBase
    {
        Task<dynamic> GetAsync();
    }
}
