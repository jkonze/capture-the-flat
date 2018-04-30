using System;
using System.Collections.Generic;
using System.Linq;
using CaptureTheFlat.Helpers;
using CaptureTheFlat.Models;
using CaptureTheFlat.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CaptureTheFlat.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        private IEnumerable<Post> Posts => _postRepository.GetAllPosts();

        [HttpGet]
        public OkObjectResult GetAllPosts()
        {
            IEnumerable<Post> posts = Posts;
            return Ok(posts);
        }


        [HttpGet("days={days}")]
        public IActionResult GetByDaysBack(int days)
        {
            DateTime minDate = DateTime.Now.AddDays(-days);

            IEnumerable<Post> postsWithDateBefore = Posts.Where(x => x.PublishDate >= minDate);
            return Ok(postsWithDateBefore);
        }

        [HttpGet("NoCache")]
        public IActionResult Cache()
        {
            return Ok(_postRepository.GetAllPostsWithoutCache());
        }
    }
}