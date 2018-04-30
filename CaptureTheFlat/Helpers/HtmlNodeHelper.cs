using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using CaptureTheFlat.Models;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Extensions.Configuration;

namespace CaptureTheFlat.Helpers
{
    public class HtmlNodeHelper
    {
        private readonly IConfiguration _config;

        public HtmlNodeHelper(IConfiguration config)
        {
            _config = config;
        }


        private string BaseUrl => _config["BaseUrl"];

        public Post NodeToPostObject(HtmlNode postNode)
        {
            string postDateString = CleanString(postNode.QuerySelector(".list_date").InnerText).Trim();
            DateTime postDate = DateTime.ParseExact(postDateString, "dd.MM.yy", CultureInfo.CurrentCulture);
            string postTitle = RemoveDateFromTitle(CleanString(postNode.InnerText), postDateString).Trim();
            string postLink = BaseUrl + postNode.Attributes["href"].Value;
            bool postHasPhoto = postNode.QuerySelector(".fa-camera") != null ? true : false;
            string postId = "flat_"+Sha256(postNode.InnerText);
            DateTime postFound = DateTime.Now;

            Post postObj = new Post()
            {
                HasPhoto = postHasPhoto,
                PostId = postId,
                PublishDate = postDate,
                Title = postTitle,
                Link = postLink,
                FoundAt = postFound
            };
            return postObj;
        }

        private string CleanString(string dirtyString)
        {
            string cleanString = Regex.Replace(dirtyString, @"\t|\n|\r", "");
            return cleanString;
        }

        private string RemoveDateFromTitle(string title, string date)
        {
            return title.Replace(date, "");
        }

        static string Sha256(string stringToHash)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(stringToHash));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }

            return hash;
        }
    }
}