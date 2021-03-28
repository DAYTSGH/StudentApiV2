using Microsoft.EntityFrameworkCore;
using StudentApiV2.Data;
using StudentApiV2.Entities;
using StudentApiV2.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class AdminRepository:IAdminRepository
    {
        private readonly ManageDbContext _context;
        public AdminRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        //查询Admin
        public async Task<IEnumerable<Admin>> GetAdminsAsync()
        {
            return await _context.Admins.ToListAsync();
        }
        public async Task<Admin> GetAdminAsync(Guid adminId)
        {
            if (adminId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(adminId));
            }
            return await _context.Admins.FirstOrDefaultAsync(x => x.AdminId == adminId);
        }
        public async Task<IEnumerable<Admin>> GetAdminsAsync(IEnumerable<Guid> adminIds)
        {
            if (adminIds == null)
            {
                throw new ArgumentNullException(nameof(adminIds));
            }
            return await _context.Admins.
                Where(x => adminIds.Contains(x.AdminId)).
                OrderBy(x => x.AdminId).
                ToListAsync();
        }
        //增删改Admin
        public void AddAdmin(Admin admin)
        {
            if (admin == null)
            {
                throw new ArgumentNullException(nameof(admin));
            }
            admin.AdminId = Guid.NewGuid();

            _context.Admins.Add(admin);

        }
        public void UpdateAdmin(Admin admin)
        {
            //_context.Entry(admin).State = EntityState.Modified;
        }
        public void UpdatePassword(Admin admin)
        {
            //_context.Entry(admin).State = EntityState.Modified;
        }
        public void DeleteAdmin(Admin admin)
        {
            if (admin == null)
            {
                throw new ArgumentNullException(nameof(admin));
            }
            _context.Admins.Remove(admin);
        }
        public async Task<bool> AdminExistAsync(Guid adminid)
        {
            if (adminid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(adminid));
            }
            return await _context.Admins.AnyAsync(x => x.AdminId == adminid);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public Admin GetAdminByCodeAsync(string adminCode)
        {
            if (string.IsNullOrWhiteSpace(adminCode))
            {
                throw new ArgumentNullException(nameof(adminCode));
            }
            return _context.Admins.FirstOrDefault(x => x.AdminCode == adminCode);
        }
    }
}
