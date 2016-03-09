using HtmlAgilityPack;
using Movie10.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VungleSDK;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Movie10.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<MovieCategory> _movieCategorys;
        private List<MovieCategory> _movieYears;
        public VungleAd sdkInstance;
        public ObservableCollection<Movies> NewMovies { get; set; }
        public ObservableCollection<MoviesItems> Items { get; set; }
        public MovieDetail movieDetails { get; set; }
        public bool readyPlay = false;
        public MainViewModel()
        {
            this.NewMovies = new ObservableCollection<Movies>();
            this.Items = new ObservableCollection<MoviesItems>();
            this.movieDetails = new MovieDetail();
        }
        private async void SdkInstance_OnAdPlayableChanged(object sender, AdPlayableEventArgs e)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            new DispatchedHandler(() =>
            {
                Debug.WriteLine(e.AdPlayable);
                this.readyPlay = e.AdPlayable;
            }));

        }
        public bool IsDataLoaded
        {
            get;
            private set;
        }
        public void LoadData()
        {
            //sdkInstance = AdFactory.GetInstance("56547b0ebc3d01137b00000f");
            //sdkInstance.OnAdPlayableChanged += SdkInstance_OnAdPlayableChanged;

            this.IsDataLoaded = true;
        }
        public List<MovieCategory> MovieCategorys
        {
            get
            {
                if (_movieCategorys == null)
                    _movieCategorys = createCategorys();
                return _movieCategorys;
            }
        }
        public List<MovieCategory> MovieYears
        {
            get
            {
                if (_movieYears == null)
                    _movieYears = createYears();
                return _movieYears;
            }
        }

        private List<MovieCategory> createYears()
        {
            List<MovieCategory> list = new List<MovieCategory>();
            for(int i=2010;i<2016;i++)
            {
                MovieCategory movieCategory1 = new MovieCategory()
                {
                    Url = "/tag/year",
                    Title = i.ToString(),
                    RootCateory = "YEAR"
                };
                list.Add(movieCategory1);
            }
            return list;
        }

        public async void getPhim(int page, string url, string type)
        {
            if (page == 0)
                App.ViewModel.Items.Clear();
            else { }
            IsLoading = true;
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                var response = await httpClient.GetAsync(string.Format("{0}?pagenum={1}&type={2}&max=30", url, page*30, type), HttpCompletionOption.ResponseContentRead);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                List<MoviesItems> temp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MoviesItems>>(content);
                foreach(var item in temp)
                {
                    Debug.WriteLine(item.genres);
                    if (!App.ViewModel.Items.Any(p => p.title == item.title))
                    {
                        App.ViewModel.Items.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            IsLoading = false;
        }
        
        public Task<string> getStream(string Url)
        {
            string st = "";
            var request = HttpWebRequest.Create(new Uri(Url)) as HttpWebRequest;
            request.Method = "HEAD";
            var taskComplete = new TaskCompletionSource<string>();
            request.BeginGetResponse(ar =>
            {
                try
                {
                    HttpWebRequest req2 = (HttpWebRequest)ar.AsyncState;
                    using (var response = (HttpWebResponse)req2.EndGetResponse(ar))
                    {
                        st = response.ResponseUri.ToString();
                        //  Debug.WriteLine(st);
                        taskComplete.TrySetResult(st);
                    }
                }
                catch
                {

                }
            }, request);

            return taskComplete.Task;
        }
        public static MovieDetail ParseDetailPhimmoi(string htmlString, string url, string Title)
        {
            MovieDetail movieDetail = new MovieDetail();
            try
            {
                movieDetail.Title = Title;
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(WebUtility.HtmlDecode(htmlString));
                List<MoviePart> list0 = new List<MoviePart>();

                if (htmlString.Contains("target=\"EZWebPlayer\""))
                {
                    var nodesepisode = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "a" && x.GetAttributeValue("target", "") == "EZWebPlayer");
                    if (nodesepisode.ToList().Count > 0)
                    {
                        foreach (var item in nodesepisode)
                        {
                            HtmlDocument html2 = new HtmlDocument();
                            html2.LoadHtml(WebUtility.HtmlDecode(item.OuterHtml));
                            MoviePart moviepart = new MoviePart();
                            if (html2.DocumentNode.Descendants().Where(x => x.Name == "a").FirstOrDefault().Attributes["href"].Value.Length > 10)
                            {

                                moviepart.Title = html2.DocumentNode.Descendants().Where(x => x.Name == "a").FirstOrDefault().Descendants().Where(z => z.Name == "input").FirstOrDefault().Attributes["value"].Value;
                                moviepart.Url = html2.DocumentNode.Descendants().Where(x => x.Name == "a").FirstOrDefault().Attributes["href"].Value;
                                list0.Add(moviepart);
                            }
                            else
                            {
                                moviepart.Title = html2.DocumentNode.Descendants().Where(x => x.Name == "a").FirstOrDefault().Descendants().Where(z => z.Name == "input").FirstOrDefault().Attributes["value"].Value;
                                moviepart.Url = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "iframe" && x.GetAttributeValue("name", "") == "EZWebPlayer").FirstOrDefault().Attributes["src"].Value;
                                list0.Add(moviepart);
                            }
                        }
                    }
                    movieDetail.CurrUrl = list0[0].Url;
                }
                else
                {
                    movieDetail.CurrUrl = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "iframe" && x.GetAttributeValue("name", "") == "EZWebPlayer").FirstOrDefault().Attributes["src"].Value;
                    list0.Add(new MoviePart() { Title = "Full Movie", Url = movieDetail.CurrUrl });
                }
                movieDetail.MovieParts = list0;

                List<Movies> list1 = new List<Movies>();
                var nodes = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "div" && x.GetAttributeValue("class", "") == "related-item");
                ObservableCollection<Movies> listResult = new ObservableCollection<Movies>();
                foreach (var item in nodes)
                {
                    Movies movie = new Movies();
                    movie.VideoDetailUrl = item.Descendants().Where(x => x.Name == "h3" && x.GetAttributeValue("class", "") == "post-box-title").FirstOrDefault().Descendants().Where(x => x.Name == "a").FirstOrDefault().Attributes["href"].Value;
                    movie.Title = item.Descendants().Where(x => x.Name == "h3" && x.GetAttributeValue("class", "") == "post-box-title").FirstOrDefault().Descendants().Where(x => x.Name == "a").FirstOrDefault().InnerText.Trim();
                    movie.Image = item.Descendants().Where(x => x.Name == "div" && x.GetAttributeValue("class", "") == "post-thumbnail").FirstOrDefault().Descendants().Where(x => x.Name == "a").FirstOrDefault().Descendants().Where(y => y.Name == "img").FirstOrDefault().Attributes["src"].Value;
                    //movie.ImageSource = new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute));
                    movie.MaxQuality = item.Descendants().Where(x => x.Name == "div" && x.GetAttributeValue("class", "") == "f_tag").FirstOrDefault().InnerText.Trim();
                    list1.Add(movie);
                    GC.Collect();
                }
                try
                {
                    movieDetail.Storyline = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "div" && x.GetAttributeValue("class", "") == "kno-rdesc").FirstOrDefault().InnerText;
                }
                catch { }
                movieDetail.BigImage = htmlDocument.DocumentNode.Descendants().Where(x => x.Name == "div" && x.GetAttributeValue("dir", "") == "ltr").FirstOrDefault().Descendants().Where(y => y.Name == "img").FirstOrDefault().Attributes["src"].Value;
                movieDetail.ReMovies = list1;
            }
            catch
            {
            }
            return movieDetail;
        }
        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");

            }
        }
        private List<MovieCategory> createCategorys()
        {
            List<MovieCategory> list = new List<MovieCategory>();
            MovieCategory movieCategory1 = new MovieCategory()
            {
                Url = "/tag/action",
                Title = "Action",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory1);
            MovieCategory movieCategory2 = new MovieCategory()
            {
                Url = "/tag/adventure",
                Title = "Adventure",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory2);
            MovieCategory movieCategory3 = new MovieCategory()
            {
                Url = "/tag/animation",
                Title = "Animation",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory3);
            MovieCategory movieCategory4 = new MovieCategory()
            {
                Url = "/tag/biography",
                Title = "Biography",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory4);
            MovieCategory movieCategory5 = new MovieCategory()
            {
                Url = "/tag/comedy",
                Title = "Comedy",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory5);
            MovieCategory movieCategory6 = new MovieCategory()
            {
                Url = "/tag/crime",
                Title = "Crime",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory6);
            MovieCategory movieCategory7 = new MovieCategory()
            {
                Url = "/tag/DOCUMENTARY",
                Title = "Documentary",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory7);
            MovieCategory movieCategory8 = new MovieCategory()
            {
                Url = "/tag/drama",
                Title = "Drama",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory8);
            MovieCategory movieCategory9 = new MovieCategory()
            {
                Url = "/tag/FAMILY",
                Title = "Family",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory9);
            MovieCategory movieCategory10 = new MovieCategory()
            {
                Url = "/tag/FANTASY",
                Title = "Fantasy",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory10);
            MovieCategory movieCategory11 = new MovieCategory()
            {
                Url = "/tag/HISTORY",
                Title = "History",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory11);
            MovieCategory movieCategory12 = new MovieCategory()
            {
                Url = "/tag/HORROR",
                Title = "Horror",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory12);
            MovieCategory movieCategory13 = new MovieCategory()
            {
                Url = "/tag/PSYCHOLOGY",
                Title = "Psychology",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory13);
            MovieCategory movieCategory14 = new MovieCategory()
            {
                Url = "/musical_movies",
                Title = "Musical",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory14);

            MovieCategory movieCategory17 = new MovieCategory()
            {
                Url = "/tag/ROMANCE",
                Title = "Romance",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory17);
            MovieCategory movieCategory18 = new MovieCategory()
            {
                Url = "/tag/SCI-FI",
                Title = "Sci-Fi",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory18);
            MovieCategory movieCategory19 = new MovieCategory()
            {
                Url = "/tag/sport",
                Title = "Sport",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory19);
            MovieCategory movieCategory20 = new MovieCategory()
            {
                Url = "/tag/THRILLER",
                Title = "Thriller",
                RootCateory = "GENRE"
            };
            list.Add(movieCategory20);


            return list;
        }
        public async void Search(string key)
        {
            IsLoading = true;
            try
            {
                this.Items.Clear();
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                var response = await httpClient.GetAsync("http://quyen23.cloudapp.net/quyenpro/search.php?key=" + key, HttpCompletionOption.ResponseContentRead);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                List<MoviesItems> temp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MoviesItems>>(content);
                foreach (var item in temp)
                {
                    Debug.WriteLine(item.genres);
                    if (!App.ViewModel.Items.Any(p => p.title == item.title))
                    {
                        App.ViewModel.Items.Add(item);
                    }
                }
            }
            catch { }
            IsLoading = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
