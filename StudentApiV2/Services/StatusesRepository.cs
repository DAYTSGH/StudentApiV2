using Microsoft.EntityFrameworkCore;
using StudentApiV2.Data;
using StudentApiV2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Services
{
    public class StatusesRepository : IStatusesRepository
    {
        private readonly ManageDbContext _context;
        public StatusesRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public bool GetStudentChooseAsync()
        {
            var status = _context.Statuses.FirstOrDefault();
            return status.StudentChoose;
        }

        public bool GetTeacherGradeAsync()
        {
            var status = _context.Statuses.FirstOrDefault();
            return status.TeacherGrade;
        }

        public void SwitchStudentChooseAsync()
        {
            var status = _context.Statuses.First();

            status.StudentChoose = !status.StudentChoose;

            //_context.Entry(status).State = EntityState.Modified;
        }

        public void SwitchTeacherGradeAsync()
        {
            var status = _context.Statuses.First();

            status.TeacherGrade = !status.TeacherGrade;

            //_context.Entry(status).State = EntityState.Modified;
        }


        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
