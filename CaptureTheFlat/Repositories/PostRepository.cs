using System;
using System.Collections.Generic;
using System.Linq;
using CaptureTheFlat.Helpers;
using CaptureTheFlat.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace CaptureTheFlat.Repositories
{
    public class PostRepository : IPostRepository
    {
        private IConfiguration _config;

        private readonly HtmlNodeHelper _htmlNodeHelper;

        private HtmlDocument Doc { get; }

        private string[] BadWordList => _config.GetSection("Badwords").Get<string[]>();

        private List<HtmlNode> PostNodes => Doc.DocumentNode
            .SelectNodes("//div[@id='eintraege']/ul/li[not(contains(@class, 'gewerblich'))]/a")
            .ToList();

        private readonly APIContext _db;

        public PostRepository(IConfiguration configuration, APIContext context)
        {
            _config = configuration;
            _db = context;
            var url = _config["PostsUrl"];
            var web = new HtmlWeb();
            Doc = web.Load(url);
            _htmlNodeHelper = new HtmlNodeHelper(_config);
        }

        private List<Post> Posts
        {
            get
            {
                List<Post> posts = new List<Post>();
                foreach (var htmlPost in PostNodes)
                {
                    Post post = _htmlNodeHelper.NodeToPostObject(htmlPost);
                    if (post is Post)
                    {
                        posts.Add(post);
                    }
                }

                return posts;
            }
        }

        public override void Migrate()
        {
            foreach (var post in Posts)
            {
                _db.Posts.Add(post);
                _db.SaveChanges();
            }
        }

        public override IEnumerable<Post> GetAllPosts()
        {
            if (_db.Posts.Any())
            {
                if (_db.Posts.OrderByDescending(p => p.FoundAt).FirstOrDefault().FoundAt < DateTime.Now.AddMinutes(-5))
                {
                    IEnumerable<Post> newPosts = Posts.Where(p => p.PublishDate == DateTime.Today);
                    foreach (var post in newPosts)
                    {
                        if (_db.Posts.Find(post.PostId) == null)
                        {
                            _db.Posts.Add(post);
                        }
                    }

                    _db.SaveChanges();
                }                
            }
            else
            {
                Migrate();
            }


            return _db.Posts;
        }
    }
}