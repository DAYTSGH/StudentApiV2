using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interfaces
{
    public interface IStatusesRepository
    {
        bool GetStudentChooseAsync();
        bool GetTeacherGradeAsync();
        Task<bool> SaveAsync();
        void SwitchStudentChooseAsync();
        void SwitchTeacherGradeAsync();
    }
}
