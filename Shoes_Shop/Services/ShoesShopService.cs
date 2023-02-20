﻿using Microsoft.EntityFrameworkCore;

using Shoes_Shop.Models;

namespace Shoes_Shop.Services {
    public class ShoesShopService : IDatabaseService {
        private readonly ShoesShopContext _db;

        public ShoesShopService(ShoesShopContext db)
        {
            _db = db;
        }

        public void Add<T>(T entity) where T : class
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            _db.Entry(entity).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public T GetById<T>(int id) where T : class
        {
            return _db.Set<T>().Find(id);
        }

        public void Update<T>(T entity) where T : class
        {
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
