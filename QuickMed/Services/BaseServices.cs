using QuickMed.DB;
using QuickMed.Interface;
using SQLite;
using System.Linq.Expressions;
using System.Reflection;

namespace QuickMed.Services
{
    public class BaseServices : IBase 
    {
        public readonly ApplicationDbContext _context;
        public BaseServices(ApplicationDbContext context)
        {
            _context = context;
        }
        
    }

}
