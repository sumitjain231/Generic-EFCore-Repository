using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericEFRepositoryFramework.Manager
{
    public class RepositoryManager<T> where T : class
    {
        private static T repository = default(T)!;
        public static T GetInstance()
        {
            if (repository == null)
            {
                repository = Activator.CreateInstance<T>();
            }
            return repository;
        }
    }   
}
