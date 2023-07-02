using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SampleApplication.Repository.Entities;
using SampleApplication.Repository.Repos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IblogRepository _service;
        private readonly ILogger<BlogController> _logger;
        public BlogController(IblogRepository service, ILogger<BlogController> logger)
        {
            _service = service;
            _logger = logger;
        }
        

       
        //https://localhost:7272/api/Blog/GetuserBlogs/1
        // GET api/<BlogController>/5
        [HttpGet("GetuserBlogs/{id}")]
        //[Authorize]
        public ActionResult<IEnumerable<Blog>> GetuserspecificBlogs(int id)
        {
            try
            {
                _logger.LogInformation("Retrieve All Blogs");
                var blogs = _service.GetUserBlogs(id);

                if (blogs == null)
                {
                    return NotFound(new { message = "No Blogs found" });
                }

                _logger.LogDebug($"The response for the get Blogs is {JsonConvert.SerializeObject(blogs)}");
                return Ok(blogs);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError($"Issue at controller: {ex.Message}");
                //return StatusCode(500, ex.Message);
                //return StatusCode(HttpContext.Response.StatusCode, ex.Message);
                throw new Exception(ex.Message);

            }
        }

        //https://localhost:7113/api/user/DeleteBlog/2
        //Delete blog
        [Route("DeleteBlog/{id}")]
        [HttpPut]
        public async Task<ActionResult> DeleteBlog(int id)
        {
            _logger.LogInformation("Delete Blog");
            if (!_service.BlogExists(id))
            {
                _logger.LogError($"Blog does not exist");
                return NotFound(new { message = "Blog does not exist" });
            }
            try
            {
                var isAdded = await _service.DeleteBlog(id);

                if (isAdded == false)
                {
                    _logger.LogError($"Error Occured");
                    return NotFound(new { message = "Error Occured" });
                }
                else
                {
                    _logger.LogDebug($"Deleted Successfully");
                    return Ok(new { message = "Deleted Successfully" });

                }
            }
            catch (DbUpdateConcurrencyException ex)
            {

                _logger.LogError($"Something went wrong: {ex.Message}");
                //return StatusCode(500, ex.Message);
                throw new Exception(ex.Message);

            }


        }

        //https://localhost:7272/api/Blog/addBlog
        // POST api/<BlogController>
        [HttpPost("addBlog")]
        public async Task<ActionResult> AddBlog(Blog blogModel)
        {
            _logger.LogInformation("Add Blog");
            if (_service.BlogExists(blogModel.BlogId))
            {
                _logger.LogError($"Blog already exists");
                return NotFound(new { message = "Blog already exists" });
            }
            
            try
            {
                var blogs = await _service.AddBlog(blogModel);

                if (blogs == 0)
                {
                    _logger.LogError($"Error occurred while adding Blog");
                    return NotFound(new { message = "Error occurred while adding Blog" });
                }
                else
                {
                    _logger.LogDebug($"The response for Adding blog is {JsonConvert.SerializeObject(blogs)}");
                    return Ok(new { message = "Added Successfully" });
                    //return Ok(blogs);// for unit test uncomment this code and comment above code
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {

                _logger.LogError($"Something went wrong: {ex.Message}");
                //return StatusCode(500, ex.Message);
                throw new Exception(ex.Message);

            }


        }

        //// PUT api/<BlogController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<BlogController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
