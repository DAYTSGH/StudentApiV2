using StudentApiV2.DtoParameters;
using StudentApiV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Interface
{
    public interface INoticeRepository
    {
        Task<IEnumerable<Notice>> GetNoticesAsync(NoticeDtoParameters noticeDtoParameters);
        Task<Notice> GetNoticeAsync(Guid NoticeId);
        void AddNotice(Notice notice);
        void UpdateNotice(Notice notice);
        void DeleteNotice(Notice notice);
        Task<bool> NoticeExistAsync(Guid noticeId);
        Task<bool> SaveAsync();
    }
}
