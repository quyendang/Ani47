using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie10.ViewModels
{
    public class MovieDetail
    {
        
        // public MovieDetail();
        public string AlbumUrl { get; set; }
        public string BigImage { get; set; }
        public string CurrUrl { get; set; }
        public List<DownloadUrlAndResolution> DownloadList { get; set; }
        public string Image { get; set; }
        public string Lenght { get; set; }
        public List<MoviePart> MovieParts { get; set; }
        public string PlayID { get; set; }
        public string PlayUrl { get; set; }
        public string ReleaseDate { get; set; }
        public List<Movies> ReMovies { get; set; }
        public string Ring { get; set; }
        public string Storyline { get; set; }
        public string Title { get; set; }
    }
    public class DownloadUrlAndResolution
    {
        public string VideoResolution { get; set; }

        public string Height { get; set; }

        public string width { get; set; }

        public string VideoUrl { get; set; }

        public string FormatReslution { get; set; }
    }
    public class MoviePart
    {
        //public MoviePart();

        public bool IsSel { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
    public class JsonResponse
    {
        public List<Content> content { get; set; }
        public string description { get; set; }
        public List<string> text { get; set; }
        public List<Thumbnail> thumbnail { get; set; }
    }
    public class Content
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string type { get; set; }
    }

    public class Thumbnail
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
}
