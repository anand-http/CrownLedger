using System.Collections.Generic;
using System.Linq;
using fintech.Data;

namespace fintech.DAL
{
    public class CompanyRepository
    {
        private readonly FintechDbContext _db = new FintechDbContext();

        public IEnumerable<Company> GetAll()
        {
            return _db.Companies.ToList();
        }

        public Company Get(int id)
        {
            return _db.Companies.Find(id);
        }

        public void Add(Company c)
        {
            _db.Companies.Add(c);
            _db.SaveChanges();
        }
    }
}