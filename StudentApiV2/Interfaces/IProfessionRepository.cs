using StudentApiV2.DtoParameters;
using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface IProfessionRepository
    {
        Task<IEnumerable<Profession>> GetProfessionsAsync(ProfessionDtoParameters professionDtoParameters);
        Task<Profession> GetProfessionAsync(Guid professionId);
        Task<IEnumerable<Profession>> GetProfessionForAcademyAsync(Guid academyId);
        Task<IEnumerable<Profession>> GetProfessionsAsync(IEnumerable<Guid> professionIds);
        //增删改Profession
        void AddProfession(Profession profession);
        void UpdateProfession(Profession profession);
        void DeleteProfession(Profession profession);
        Task<bool> ProfessionExistAsync(Guid professionId);
        Task<bool> SaveAsync();
    }
}
