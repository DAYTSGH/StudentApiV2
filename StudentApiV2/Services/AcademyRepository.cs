using Microsoft.EntityFrameworkCore;
using StudentApi.DtoParameters;
using StudentApiV2.Data;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class AcademyRepository : IAcademyRepository
    {
        private readonly ManageDbContext _context;
        public AcademyRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        //查询Academy
        public async Task<IEnumerable<Academy>> GetAcademiesAsync(AcademyDtoParameters academyDtoParameters)
        {
            if (academyDtoParameters == null)
            {
                throw new ArgumentNullException(nameof(academyDtoParameters));
            }
            //如果 筛选条件 和 查询字符串 都为空的话
            if (string.IsNullOrWhiteSpace(academyDtoParameters.AcademyName) && string.IsNullOrWhiteSpace(academyDtoParameters.SearchTerm))
            {
                return await _context.Academies.ToListAsync();
            }

            var academyItems = _context.Academies as IQueryable<Academy>;
            //筛选条件
            if (!string.IsNullOrWhiteSpace(academyDtoParameters.AcademyName))
            {
                academyDtoParameters.AcademyName = academyDtoParameters.AcademyName.Trim();

                academyItems = academyItems.Where(x => x.AcademyName == academyDtoParameters.AcademyName);
            }
            //查询条件
            if (!string.IsNullOrWhiteSpace(academyDtoParameters.SearchTerm))
            {
                academyDtoParameters.SearchTerm = academyDtoParameters.SearchTerm.Trim();

                academyItems = academyItems.Where(x => x.AcademyName.Contains(academyDtoParameters.SearchTerm) ||
                x.AcademyCode.Contains(academyDtoParameters.SearchTerm));
            }

            return await academyItems.ToListAsync();
        }
        public async Task<Academy> GetAcademyAsync(Guid academyId)
        {
            if (academyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(academyId));
            }
            return await _context.Academies.FirstOrDefaultAsync(x => x.AcademyId == academyId);
        }
        public async Task<IEnumerable<Academy>> GetAcademiesAsync(IEnumerable<Guid> academyIds)
        {
            if (academyIds == null)
            {
                throw new ArgumentNullException(nameof(academyIds));
            }
            return await _context.Academies.
                Where(x => academyIds.Contains(x.AcademyId)).
                OrderBy(x => x.AcademyId).
                ToListAsync();
        }
        //增删改Academy
        public void AddAcademy(Academy academy)
        {
            if (academy == null)
            {
                throw new ArgumentNullException(nameof(academy));
            }
            academy.AcademyId = Guid.NewGuid();

            _context.Academies.Add(academy);

        }
        public void UpdateAcademy(Academy academy)
        {
            //_context.Entry(academy).State = EntityState.Modified;
        }
        public void DeleteAcademy(Academy academy)
        {
            if (academy == null)
            {
                throw new ArgumentNullException(nameof(academy));
            }
            _context.Academies.Remove(academy);
        }
        public async Task<bool> AcademyExistAsync(Guid academyid)
        {
            if (academyid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(academyid));
            }
            return await _context.Academies.AnyAsync(x => x.AcademyId == academyid);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
