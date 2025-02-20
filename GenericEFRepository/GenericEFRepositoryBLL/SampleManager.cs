using EFDomainData.Constant;
using EFDomainData.Models;
using EFDomainModel;
using GenericEFRepositoryDAL.Manager;
using GenericEFRepositoryFramework.Enums;
using GenericEFRepositoryFramework.Interface;
using GenericEFRepositoryFramework.Repository;
using System.Linq.Expressions;
using Utility;

namespace GenericEFRepositoryBLL
{
    public static class SampleManager
    {
        public static async Task<SampleTableDTO> GetSampleTableById(long id)
        {
            SampleTableDTO sampleTableDTO = null!;
            var result = await GenericEFDataManager<SampleTable>.FirstOrDefaultAsync(Databases.MySampleDatabase, x => x.Id == id);
            if (result != null)
                sampleTableDTO = new SampleTableDTO
                {
                    Id = result.Id,
                    Name = result.Name
                };
            return sampleTableDTO;
        }

        public static async Task<SampleTableDTO> GetSampleTableWithDetailById(long id)
        {
            SampleTableDTO sampleTableDTO = null!;
            var result = await GenericEFDataManager<SampleTable>.FirstOrDefaultAsync(Databases.MySampleDatabase, x => x.Id == id,
                includes: new Expression<Func<SampleTable, object>>[] { x => x.SampleTableDetails });
            if (result != null)
            {
                sampleTableDTO = new SampleTableDTO
                {
                    Id = result.Id,
                    Name = result.Name
                };
                if (result.SampleTableDetails != null)
                {
                    sampleTableDTO.SampleTableDetail = ObjectTranslator.FromListType<SampleTableDetailDTO, SampleTableDetail>(result.SampleTableDetails);
                    return sampleTableDTO;
                }
            }
            return sampleTableDTO;
        }

        public static async Task<List<SampleTableDTO>> GetAllSampleTable(string name)
        {
            var result = await GenericEFDataManager<SampleTable>.FindAsync(Databases.MySampleDatabase, x => x.Name.StartsWith(name));
            if (result != null)
                return ObjectTranslator.FromListType<SampleTableDTO, SampleTable>(result);
            return null!;
        }

        public static async Task<List<SampleTableDTO>> GetAllSampleTableOrderByName(string name)
        {
            IOrderByClause<SampleTable>[] orderBy = new IOrderByClause<SampleTable>[] {
                    new OrderByClause<SampleTable,string>(p=> p.Name,SortDirection.Descending)
                };
            var result = await GenericEFDataManager<SampleTable>.FindAsync(Databases.MySampleDatabase, x => x.Name.StartsWith(name), orderBy: orderBy);
            if (result != null)
                return ObjectTranslator.FromListType<SampleTableDTO, SampleTable>(result);
            return null!;
        }

        public static async Task<bool> DeleteSampleTableDataById(long id, string userId)
        {
            var result = await GenericEFDataManager<SampleTable>.FirstOrDefaultAsync(Databases.MySampleDatabase, x => x.Id == id);
            if (result != null)
                return await GenericEFDataManager<SampleTable>.SaveAsync(Databases.MySampleDatabase, userId, deletedEntity: result) == 1;
            return false;
        }

        public static async Task<bool> SaveSampleTableData(SampleTableDTO sampleTableDtoObject, string userId)
        {
            if (sampleTableDtoObject == null)
                return false;
            var result = await GenericEFDataManager<SampleTable>.FirstOrDefaultAsync(Databases.MySampleDatabase, x => x.Id == sampleTableDtoObject.Id);
            if (result != null)
            {
                result.Name = sampleTableDtoObject.Name;
                return await GenericEFDataManager<SampleTable>.SaveAsync(Databases.MySampleDatabase, userId, modifiedEntity: result) == 1;
            }
            else
            {
                var sampleTable = new SampleTable
                {
                    Id = sampleTableDtoObject.Id,
                    Name = sampleTableDtoObject.Name
                };
                return await GenericEFDataManager<SampleTable>.SaveAsync(Databases.MySampleDatabase, userId, sampleTable) == 1;
            }
        }

        public static async Task<List<PartialSampleTableDTO>> GetAllPartialSampleTable(string name)
        {
            var result = await GenericEFSelectDataManager<SampleTable, PartialSampleTableDTO>.FindAsync(Databases.MySampleDatabase,
                x => new PartialSampleTableDTO { Name = x.Name }, x => x.Name.StartsWith(name));
            if (result != null)
                return result.ToList();
            return null!;
        }

        public static async Task<IEnumerable<SampleTableDTO>> GetAllSampleTableByNameFromProc(string name)
        {
            object[] param = new object[] { name };
            return await GenericEFDataManager<SampleTableDTO>.ExecuteSqlQueryAsync(Databases.MySampleDatabase, ProcedureConstants.Usp_GetSampleTableData, param);
        }
    }
}