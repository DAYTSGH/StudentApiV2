using Microsoft.EntityFrameworkCore;
using StudentApiV2.Data;
using StudentApiV2.DtoParameters;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class ProfessionRepository:IProfessionRepository
    {
        private readonly ManageDbContext _context;
        public ProfessionRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        //查询Profession
        public async Task<IEnumerable<Profession>> GetProfessionsAsync(ProfessionDtoParameters professionDtoParameters)
        {
            if (professionDtoParameters == null)
            {
                throw new ArgumentNullException(nameof(professionDtoParameters));
            }
            //如果 筛选条件 和 查询字符串 都为空的话
            if (string.IsNullOrWhiteSpace(professionDtoParameters.ProfessionName) && string.IsNullOrWhiteSpace(professionDtoParameters.SearchTerm))
            {
                return await _context.Professions.ToListAsync();
            }

            var professionItems = _context.Professions as IQueryable<Profession>;
            //筛选条件
            if (!string.IsNullOrWhiteSpace(professionDtoParameters.ProfessionName))
            {
                professionDtoParameters.ProfessionName = professionDtoParameters.ProfessionName.Trim();

                professionItems = professionItems.Where(x => x.ProfessionName == professionDtoParameters.ProfessionName);
            }
            //查询条件
            if (!string.IsNullOrWhiteSpace(professionDtoParameters.SearchTerm))
            {
                professionDtoParameters.SearchTerm = professionDtoParameters.SearchTerm.Trim();

                professionItems = professionItems.Where(x => x.ProfessionName.Contains(professionDtoParameters.SearchTerm) ||
                x.ProfessionCode.Contains(professionDtoParameters.SearchTerm));
            }

            return await professionItems.ToListAsync();
        }
        public async Task<Profession> GetProfessionAsync(Guid professionId)
        {
            if (professionId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(professionId));
            }
            return await _context.Professions.FirstOrDefaultAsync(x => x.ProfessionId == professionId);
        }
        public async Task<IEnumerable<Profession>> GetProfessionsAsync(IEnumerable<Guid> professionIds)
        {
            if (professionIds == null)
            {
                throw new ArgumentNullException(nameof(professionIds));
            }
            return await _context.Professions.
                Where(x => professionIds.Contains(x.ProfessionId)).
                OrderBy(x => x.ProfessionId).
                ToListAsync();
        }
        //增删改Profession
        public void AddProfession(Profession profession)
        {
            if (profession == null)
            {
                throw new ArgumentNullException(nameof(profession));
            }
            profession.ProfessionId = Guid.NewGuid();

            _context.Professions.Add(profession);

        }
        public void UpdateProfession(Profession profession)
        {
            //_context.Entry(profession).State = EntityState.Modified;
        }
        public void DeleteProfession(Profession profession)
        {
            if (profession == null)
            {
                throw new ArgumentNullException(nameof(profession));
            }
            _context.Professions.Remove(profession);
        }
        public async Task<bool> ProfessionExistAsync(Guid professionid)
        {
            if (professionid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(professionid));
            }
            return await _context.Professions.AnyAsync(x => x.ProfessionId == professionid);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<IEnumerable<Profession>> GetProfessionForAcademyAsync(Guid academyId)
        {
            if (academyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(academyId));
            }
            if (!_context.Academies.Any(x => x.AcademyId == academyId))
            {
                throw new ArgumentNullException(nameof(academyId));
            }

            return await _context.Professions.Where(x => x.AcademyId == academyId).ToListAsync();
        }
    }
}
