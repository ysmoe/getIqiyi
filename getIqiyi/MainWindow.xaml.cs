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
                IsGetButtonClicked(true);
                sendGetIqiyi();
            }
            else if(choosePptv.IsChecked == true)
            {
                IsGetButtonClicked(true);
                sendGetPptv();
            }
            else if(chooseBiliBili.IsChecked == true)
            {
                IsGetButtonClicked(true);
                sendGetBiliBili();
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

        public void sendGetBiliBili()
        {
            Match animeId = Regex.Match(playUrlString, @"av(.*)");
            string aid = animeId.Groups[1].Value;
            flashUrlString = "http://static.hdslb.com/play.swf?&aid=" + aid;
            flashUrl.Text = flashUrlString;
        }

        private void aboutButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("作者 : imi415@U2 \nTwitter : @imi415_ \n软件更新 : https://github.com/imi415/getIqiyi \n软件主页 : https://blog.imi.moe/?p=251 \n本工具以MIT协议开源 \nBackground : © GMO INTERNET GROUP", "关于", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void chooseBiliBiliChecked(object sender, RoutedEventArgs e)
        {
            IsvideoSourceChecked(true);
            IsGetButtonClicked(false);
            urlLabel.Content = "请输入B站AV号（包含“av”）";
        }

        private void chooseIqiyiChecked(object sender, RoutedEventArgs e)
        {
            IsvideoSourceChecked(true);
            IsGetButtonClicked(false);
            urlLabel.Content = "视频播放地址输进这里哟~";
        }

        private void choosePptvChecked(object sender, RoutedEventArgs e)
        {
            IsvideoSourceChecked(true);
            IsGetButtonClicked(false);
            urlLabel.Content = "视频播放地址输进这里哟~";
        }
        public void IsvideoSourceChecked(bool IsChecked)
        {
            if (IsChecked == true)
            {
                playUrl.Visibility = System.Windows.Visibility.Visible;
                urlLabel.Visibility = System.Windows.Visibility.Visible;
                getButton.Visibility = System.Windows.Visibility.Visible;
            }
        }
        public void IsGetButtonClicked(bool IsClicked)
        {
            if (IsClicked == true)
            {
                flashUrl.Visibility = System.Windows.Visibility.Visible;
                flashLabel.Visibility = System.Windows.Visibility.Visible;
                copyButton.Visibility = System.Windows.Visibility.Visible;
                clearButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                flashUrl.Visibility = System.Windows.Visibility.Hidden;
                flashLabel.Visibility = System.Windows.Visibility.Hidden;
                copyButton.Visibility = System.Windows.Visibility.Hidden;
                clearButton.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
