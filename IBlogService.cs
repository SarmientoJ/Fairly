using Sabio.Models;
using Sabio.Models.Domain.Blogs;
using Sabio.Models.Requests.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IBlogService
    {

        int Add(BlogAddRequest model, int userId);

        Paged<Blog> Pagination(int pageIndex, int pageSize);

        Paged<Blog> PaginationCreatedBy(int pageIndex, int pageSize, int authorId);
        Paged<Blog> PaginationBlogType(int pageIndex, int pageSize, int blogType);
        Paged<Blog> SearchFiltered(int pageIndex, int pageSize, string query, int blogTypeId, DateTime startDate, DateTime endDate);
        Blog Get(int id);

        void Update(BlogUpdateRequest model, int Id, int userId);
        void Delete(int Id);
        List<Blog> SelectAllByBlogType(int blogType);
  
    }
}
