using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAdminsAsync();
        Task<Admin> GetAdminAsync(Guid adminId);
        Admin GetAdminByCodeAsync(string adminCode);
        void AddAdmin(Admin admin);
        void UpdateAdmin(Admin admin);
        void UpdatePassword(Admin admin);
        void DeleteAdmin(Admin admin);
        Task<bool> AdminExistAsync(Guid adminId);
        Task<bool> SaveAsync();
    }
}
