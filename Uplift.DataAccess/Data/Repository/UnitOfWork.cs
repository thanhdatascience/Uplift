﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Frequency = new FrequencyRepository(_db);
            Service = new ServiceRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            User = new UserRepository(_db);
        }

        public ICategoryRepository Category { get; private set; }

        public IFrequencyRepository Frequency { get; private set; }

        public IServiceRepository Service { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; set; }

        public IOrderDetailsRepository OrderDetails { get; set; }

        public IUserRepository User { get; set; }

    

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
