using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.Blogs;
using Sabio.Models.Requests.Blogs;
using Sabio.Models;
using Microsoft.AspNetCore.Authorization;

namespace Sabio.Web.Api.Controllers
{
        [Route("api/blogs")]
        [ApiController]
        public class BlogApiController : BaseApiController
        {
            private IBlogService _service = null;
            private IAuthenticationService<int> _authService = null;

            public BlogApiController(IBlogService service
                , ILogger<BlogApiController> logger
                , IAuthenticationService<int> authService) : base(logger)
            {
                _service = service;
                _authService = authService;
            }
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(BlogAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };


                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;

        }


        [HttpGet("paginate")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Paged<Blog>>> Pagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Blog> blogPaged = _service.Pagination(pageIndex, pageSize);

                if (blogPaged == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resourse Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = blogPaged };

                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message.ToString());
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Blog>>> SearchFiltered(int pageIndex, int pageSize, string query, int blogTypeId, DateTime startDate, DateTime endDate)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Blog> blogPaged = _service.SearchFiltered(pageIndex, pageSize, query, blogTypeId,startDate,endDate);

                if (blogPaged == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resourse Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = blogPaged };

                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message.ToString());
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate/{id:int}")]
        public ActionResult<ItemResponse<Paged<Blog>>> PaginationCreatedBy(int pageIndex, int pageSize, int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Blog> blogPaged = _service.PaginationCreatedBy(pageIndex, pageSize, id);

                if (blogPaged == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resourse Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = blogPaged };

                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message.ToString());
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate/blogType/{id:int}")]
        public ActionResult<ItemResponse<Paged<Blog>>> PaginationBlogType(int pageIndex, int pageSize, int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Blog> blogPaged = _service.PaginationBlogType(pageIndex, pageSize, id);

                if (blogPaged == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resourse Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = blogPaged };

                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message.ToString());
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Blog>> Get(int id)
            {

                int iCode = 200;
                BaseResponse response = null;

                try
                {
                    Blog blog = _service.Get(id);


                    if (blog == null)
                    {
                        iCode = 404;
                        response = new ErrorResponse("Blog not found.");

                    }
                    else
                    {
                        response = new ItemResponse<Blog> { Item = blog };
                    }

                }

                catch (Exception ex)
                {
                    iCode = 500;
                    base.Logger.LogError(ex.ToString());
                    response = new ErrorResponse($"Generic Errors: ${ex.Message}");
                }

                return StatusCode(iCode, response);

            }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(BlogUpdateRequest model, int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, id, userId);

                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }

            return StatusCode(code, response);
        }

        [HttpPut("delete/{id:int}")]
        public ActionResult<ItemResponse<int>> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {

                _service.Delete(id);

                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());

            }

            return StatusCode(code, response);
        }



        [HttpGet("blogType/{id:int}")]
        public ActionResult<ItemsResponse<Blog>> SelectAllByBlogType(int id)
        {
            int code = 200;
            BaseResponse response = null;// do not declare an instance

            try
            {
                List<Blog> list = _service.SelectAllByBlogType(id);


                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resorce not found.");
                }
                else
                {
                    response = new ItemsResponse<Blog> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }


    }
    }
