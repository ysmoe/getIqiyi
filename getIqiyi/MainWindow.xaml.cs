using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Web;
using System.IO;
using System.Net.Cache;

namespace getIqiyi
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class MainWindow : Window
    {
        public string playUrlString;
        public static byte[] playUrlByte;
        public static string recvString;
        public static string flashUrlString;
        public Stream getStream;
        public Stream recvStream;
        public StreamReader getResponseStreamReader;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void getClick(object sender, RoutedEventArgs e)
        {
            playUrlString = playUrl.Text;
            if (String.IsNullOrEmpty(playUrl.Text))
            {
                MessageBox.Show("请填写URL", "不要恶意卖萌的说~", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else if(chooseIqiyi.IsChecked == true)
            {
                sendGetIqiyi();
            }
            else if(choosePptv.IsChecked == true)
            {
                sendGetPptv();
            }
            else
            {
                MessageBox.Show("请选择视频源", "不要恶意卖萌的说~", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void copyButtonClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(flashUrl.Text,true);
            MessageBox.Show("复制完成了喵~拿去玩吧~");
        }

        private void clearButtonClick(object sender, RoutedEventArgs e)
        {
            flashUrl.Text = "";
        }
        public void sendGetIqiyi()
        {
            try
            {
                playUrlByte = Encoding.UTF8.GetBytes(playUrlString);
                HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(new Uri(playUrlString));
                getRequest.CookieContainer = new CookieContainer();
                getRequest.Method = "GET";
                getRequest.Accept = "text/html, application/xhtml+xml, */*";
                getRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
                HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
                getResponseStreamReader = new StreamReader(getResponse.GetResponseStream(), Encoding.UTF8);
                recvString = getResponseStreamReader.ReadToEnd();
                getResponseStreamReader.Close();
                getResponse.Close();
                Match videoId = Regex.Match(recvString, @"data-player-videoid=.(.*)..");
                string vid = videoId.Groups[1].Value;
                Match videoTvid = Regex.Match(recvString, @"data-player-tvid=.(.*)..");
                string tvid = videoTvid.Groups[1].Value;
                Match albumId = Regex.Match(recvString, @"data-player-albumid=.(.*)..");
                string aid = albumId.Groups[1].Value;
                Match swf = Regex.Match(playUrlString, @"http://www.iqiyi.com/(.*).....#");
                string swfName = swf.Groups[1].Value;
                flashUrlString = "http://player.video.qiyi.com/" + vid + "/0/0/" + swfName + ".swf-ismember=false-albumId=" + aid + "-tvId=" + tvid + "-isPurchase=0-cnId=4";
                flashUrl.Text = flashUrlString;
            }
            catch (Exception netException)
            {
                MessageBox.Show(netException.Message);
            }
        }

        public void sendGetPptv()
        {
            try
            {
                playUrlByte = Encoding.UTF8.GetBytes(playUrlString);
                HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(new Uri(playUrlString));
                getRequest.CookieContainer = new CookieContainer();
                getRequest.Method = "GET";
                getRequest.Accept = "text/html, application/xhtml+xml, */*";
                getRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
                HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
                getResponseStreamReader = new StreamReader(getResponse.GetResponseStream(), Encoding.UTF8);
                recvString = getResponseStreamReader.ReadToEnd();
                getResponseStreamReader.Close();
                getResponse.Close();
                Match videoId = Regex.Match(recvString, @"\""swf"":"".............................(.*)....""}]");
                string vid = videoId.Groups[1].Value;
                flashUrlString = "http://player.pptv.com/v/" + vid + ".swf";
                flashUrl.Text = flashUrlString;
            }
            catch (Exception netException)
            {
                MessageBox.Show(netException.Message);
            }
        }
        private void aboutButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("作者 : imi415@U2 \nTwitter : @imi415_ \n软件更新 : https://github.com/imi415/getIqiyi \n软件主页 : https://blog.imi.moe/?p=251 ", "关于", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
