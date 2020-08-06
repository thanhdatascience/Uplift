using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _db;

        public FrequencyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetFrequencyListForDropDown()
        {
            return _db.Frequency.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Frequency frequency)
        {
            var objFromDb = _db.Frequency.FirstOrDefault(i => i.Id == frequency.Id);

            if(objFromDb != null)
            {
                objFromDb.Name = frequency.Name;
                objFromDb.FrequencyCount = frequency.FrequencyCount;
            }

            _db.SaveChanges();
        }
    }
}
