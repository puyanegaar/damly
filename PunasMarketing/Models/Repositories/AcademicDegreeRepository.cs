using System;
using System.Collections.Generic;
using System.Linq;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.Repositories
{
    public class AcademicDegreeRepository
    {
        private readonly List<AcademicDegreeMetaData> _collection = new List<AcademicDegreeMetaData>()
        {
            new AcademicDegreeMetaData{Id = 1, Name = "بی سواد" },
            new AcademicDegreeMetaData{Id = 2, Name = "زیردیپلم" },
            new AcademicDegreeMetaData{Id = 3, Name = "دیپلم" },
            new AcademicDegreeMetaData{Id = 4, Name = "فوق دیپلم" },
            new AcademicDegreeMetaData{Id = 5, Name = "لیسانس" },
            new AcademicDegreeMetaData{Id = 6, Name = "فوق لیسانس" },
            new AcademicDegreeMetaData{Id = 7, Name = "دکترا" },
            new AcademicDegreeMetaData{Id = 8, Name = "فوق دکترا" }
        };

        public IEnumerable<AcademicDegreeMetaData> Select()
        {
            try
            {
                return _collection;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<TResult> Select<TResult>(Func<AcademicDegreeMetaData, TResult> selector)
        {
            try
            {
                return _collection.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<AcademicDegreeMetaData> Where(Func<AcademicDegreeMetaData, bool> predicate)
        {
            try
            {
                return _collection.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public AcademicDegreeMetaData Find(byte id)
        {
            try
            {
                return _collection.FirstOrDefault(i => i.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<AcademicDegreeMetaData> Find(string search)
        {
            try
            {
                return _collection.Where(i => i.Name.Contains(search));
            }
            catch
            {
                return null;
            }
        }
    }
}