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
    public class NoticeRepository:INoticeRepository
    {
        private readonly ManageDbContext _context;
        public NoticeRepository(ManageDbContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        //查询Notice
        public async Task<IEnumerable<Notice>> GetNoticesAsync(NoticeDtoParameters noticeDtoParameters)
        {
            if (noticeDtoParameters == null)
            {
                throw new ArgumentNullException(nameof(noticeDtoParameters));
            }
            //如果 筛选条件 和 查询字符串 都为空的话
            if (string.IsNullOrWhiteSpace(noticeDtoParameters.NoticeName) && string.IsNullOrWhiteSpace(noticeDtoParameters.SearchTerm))
            {
                return await _context.Notices.ToListAsync();
            }

            var noticeItems = _context.Notices as IQueryable<Notice>;
            //筛选条件
            if (!string.IsNullOrWhiteSpace(noticeDtoParameters.NoticeName))
            {
                noticeDtoParameters.NoticeName = noticeDtoParameters.NoticeName.Trim();

                noticeItems = noticeItems.Where(x => x.Title == noticeDtoParameters.NoticeName);
            }
            //查询条件
            if (!string.IsNullOrWhiteSpace(noticeDtoParameters.SearchTerm))
            {
                noticeDtoParameters.SearchTerm = noticeDtoParameters.SearchTerm.Trim();

                noticeItems = noticeItems.Where(x => x.Title.Contains(noticeDtoParameters.SearchTerm) ||
                x.Content.Contains(noticeDtoParameters.SearchTerm));
            }

            return await noticeItems.ToListAsync();
        }
        public async Task<Notice> GetNoticeAsync(Guid noticeId)
        {
            if (noticeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(noticeId));
            }
            return await _context.Notices.FirstOrDefaultAsync(x => x.NoticeId == noticeId);
        }
        public async Task<IEnumerable<Notice>> GetNoticesAsync(IEnumerable<Guid> noticeIds)
        {
            if (noticeIds == null)
            {
                throw new ArgumentNullException(nameof(noticeIds));
            }
            return await _context.Notices.
                Where(x => noticeIds.Contains(x.NoticeId)).
                OrderBy(x => x.NoticeId).
                ToListAsync();
        }
        //增删改Notice
        public void AddNotice(Notice notice)
        {
            if (notice == null)
            {
                throw new ArgumentNullException(nameof(notice));
            }
            notice.NoticeId = Guid.NewGuid();

            _context.Notices.Add(notice);

        }
        public void UpdateNotice(Notice notice)
        {
            //_context.Entry(notice).State = EntityState.Modified;
        }
        public void DeleteNotice(Notice notice)
        {
            if (notice == null)
            {
                throw new ArgumentNullException(nameof(notice));
            }
            _context.Notices.Remove(notice);
        }
        public async Task<bool> NoticeExistAsync(Guid noticeid)
        {
            if (noticeid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(noticeid));
            }
            return await _context.Notices.AnyAsync(x => x.NoticeId == noticeid);
        }
        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
