using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Blogs;
using Sabio.Data;
using static System.Net.Mime.MediaTypeNames;
using Sabio.Models.Domain;
using System.Reflection.Metadata;
using Sabio.Services.Interfaces;
using System.Collections;
using System.Reflection.PortableExecutable;
using Sabio.Models.Requests.Blogs;
using Sabio.Models;

namespace Sabio.Services
{
    public class BlogService : IBlogService
    {
        IDataProvider _data = null;
        public BlogService(IDataProvider data)
        {
            _data = data;
        }
        public int Add(BlogAddRequest model, int userId)
        {
        int id = 0;
        string procName = "[dbo].[Blogs_Insert]";

        _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
        {
            AddCommonParams(model, col);
            col.AddWithValue("@AuthorId", userId);
            SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
            idOut.Direction = ParameterDirection.Output;
            col.Add(idOut);
        },
        returnParameters: delegate (SqlParameterCollection returnCollection)
        {
            object oldId = returnCollection["@Id"].Value;
            int.TryParse(oldId.ToString(), out id);
          
        });
        return id;
         }

        public Paged<Blog> Pagination(int pageIndex, int pageSize)
            {
            Paged<Blog> pagedList = null;
            List<Blog> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("dbo.Blogs_SelectAll_Paginated",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    int idx = 0;
                    Blog blog = MapSingleBlog(reader, ref idx);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(idx++);
                    }

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
            }

        public Paged<Blog> PaginationCreatedBy(int pageIndex, int pageSize, int authorId)
        {
            Paged<Blog> pagedList = null;
            List<Blog> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("dbo.Blogs_SelectBy_CreatedBy",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@AuthorId", authorId);
                },
                (reader, recordSetIndex) =>
                {
                    int idx = 0;
                    Blog blog = MapSingleBlog(reader, ref idx);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(idx++);

                    }

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Blog Get(int id)
        {
            string procName = "[dbo].[Blogs_SelectById]";
            Blog blog = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int idx = 0;
                blog = MapSingleBlog(reader, ref idx);
            }
            );
            return blog;
        }

        public void Delete(int Id)
        {
            string procName = "[dbo].[Blogs_Delete]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection col)
            {           
                col.AddWithValue("@Id", Id);
            },
         returnParameters: null);
        }

        public void Update(BlogUpdateRequest model, int id, int userId)
        {         
            string procName = "[dbo].[Blogs_Update]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@AuthorId", userId);
                col.AddWithValue("@Id", id);

            },
         returnParameters: null);
        }

        public List<Blog> SelectAllByBlogType(int blogType)
        {
            List<Blog> list = null;
            string procName = "[dbo].[Blogs_Select_BlogType]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@BlogType", blogType);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
           {
               int idx = 0;
               Blog aBlog = new Blog();
               aBlog = MapSingleBlog(reader, ref idx);
               
               if (list == null)
               {
                   list = new List<Blog>();
               }
               list.Add(aBlog);
           }
          );
            return list;
        }

        public Paged<Blog> PaginationBlogType(int pageIndex, int pageSize, int blogType)
        {
            Paged<Blog> pagedList = null;
            List<Blog> list = null;
            int totalCount = 0;

            _data.ExecuteCmd("[dbo].[Blogs_SelectByBlogType_Paginated]",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@BlogType", blogType);
                },
                (reader, recordSetIndex) =>
                {
                    int idx = 0;
                    Blog blog = MapSingleBlog(reader, ref idx);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(idx++);

                    }

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Blog> SearchFiltered(int pageIndex, int pageSize, string query, int blogTypeId, DateTime startDate, DateTime endDate)
        {
            Paged<Blog> pagedList = null;
            List<Blog> list = null;
            int totalCount = 0;
            DateTime checkDate = new DateTime(01/01/0001);

            _data.ExecuteCmd("[dbo].[Blogs_SearchFiltered_Paginated]",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@Query", query);

                    if (blogTypeId == 0){param.AddWithValue("@BlogType", null);}
                    else { param.AddWithValue("@BlogType", blogTypeId);  };

                    if (startDate.Date == checkDate.Date){ param.AddWithValue("@StartDate", null);}
                    else { param.AddWithValue("@StartDate", startDate); };

                    if (endDate.Date == checkDate.Date) { param.AddWithValue("@EndDate", null); }
                    else { param.AddWithValue("@EndDate", endDate); }
                    
                },
                (reader, recordSetIndex) =>
                {
                    int idx = 0;
                    Blog blog = MapSingleBlog(reader, ref idx);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(idx++);

                    }

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        private static Blog MapSingleBlog(IDataReader reader, ref int startingIndex)
        {
            Blog aBlog = new Blog();

            LookUp aBlogType = new LookUp();

            BaseUser aBaseUser = new BaseUser();

            aBlog.Id = reader.GetSafeInt32(startingIndex++);
            aBlogType.Id = reader.GetSafeInt32(startingIndex++);           
            aBlogType.Name = reader.GetSafeString(startingIndex++);
            aBlog.BlogType = aBlogType;
            aBlog.AuthorId = reader.GetSafeInt32(startingIndex++);
            aBaseUser.FirstName = reader.GetSafeString(startingIndex++);
            aBaseUser.LastName = reader.GetSafeString(startingIndex++);
            aBaseUser.AvatarUrl = reader.GetSafeString(startingIndex++);
            aBlog.Author = aBaseUser;
            aBlog.Title = reader.GetSafeString(startingIndex++);
            aBlog.Subject = reader.GetSafeString(startingIndex++);
            aBlog.Content = reader.GetSafeString(startingIndex++);
            aBlog.IsPublished = reader.GetSafeBool(startingIndex++);
            aBlog.ImageUrl = reader.GetSafeString(startingIndex++);
            aBlog.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aBlog.DateModified = reader.GetSafeDateTime(startingIndex++);
            aBlog.DatePublished = reader.GetSafeDateTime(startingIndex++);
            aBlog.IsDeleted = reader.GetSafeBool(startingIndex++);
          
            return aBlog;
        }

        private static void AddCommonParams(BlogAddRequest model, SqlParameterCollection col)
                {
                    col.AddWithValue("@BlogTypeId", model.BlogTypeId);
                    col.AddWithValue("@Title", model.Title);
                    col.AddWithValue("@Subject", model.Subject);
                  
            if (model.Content == null)
                {
                    col.AddWithValue("@Content", DBNull.Value);
                }
                else
                {
                    col.AddWithValue("@content", model.Content);
                }
                col.AddWithValue("@IsPublished", model.IsPublished);
                   
            if (model.ImageUrl == null)
                        {
                            col.AddWithValue("@ImageUrl", DBNull.Value);
                        }
                        else
                        {
                            col.AddWithValue("@ImageUrl", model.ImageUrl);
                        }
            if (model.DatePublished == null)
                    {
                        col.AddWithValue("@DatePublished", DBNull.Value);
                    }
                    else
                    {
                        col.AddWithValue("@DatePublished", model.DatePublished);
                    }


        }
    }
}
