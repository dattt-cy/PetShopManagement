﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopPetManagement.DAL
{
    public interface ICrudRepository<T> where T : class
    {
        List<T> GetAll();      
        T GetById(int id);           
        void Add(T entity);           
        void Update(T entity);     
        void Delete(int id);          
    }
}
