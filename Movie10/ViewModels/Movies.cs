using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Movie10.ViewModels
{
    [DataContract]
    public class Movies
    {
        [DataMember]
        public string MaxQuality { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string VideoDetailUrl { get; set; }
        [DataMember]
        public string Rating { get; set; }
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public string Desc { get; set; }
        public BitmapImage ImageSource { get; set; }
        [DataMember]
        public string Time { get; set; }
    }
}
