using StudentApi.DtoParameters;
using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface IAcademyRepository
    {
        //查询Academy
        Task<IEnumerable<Academy>> GetAcademiesAsync(AcademyDtoParameters academyDtoParameters);
        Task<Academy> GetAcademyAsync(Guid academyId);
        Task<IEnumerable<Academy>> GetAcademiesAsync(IEnumerable<Guid> academyIds);
        //增删改Academy
        void AddAcademy(Academy academy);
        void UpdateAcademy(Academy academy);
        void DeleteAcademy(Academy academy);
        Task<bool> AcademyExistAsync(Guid academyId);
        Task<bool> SaveAsync();
    }
}
