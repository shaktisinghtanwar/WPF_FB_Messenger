using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Fb_InstaWpf.Helper;
using Fb_InstaWpf.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using HtmlAgilityPack;

namespace Fb_InstaWpf.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Field
        private ObservableCollection<FbpageInboxUserInfo> userListInfo;
        private ObservableCollection<FacebookPageInboxmember> fbPageInboxmember;
        private ObservableCollection<InstaInboxmember> instaInboxmember;
        private FbpageInboxUserInfo selectedUserInfo;
        private FacebookPageInboxmember SelectedFbPageInboxmember;
        private ObservableCollection<FbUserMessageInfo> messagingListInfo { get; set; }

        public ChromeDriver ChromeWebDriver;
        readonly ChromeOptions _options = new ChromeOptions();
        public string LstItemUserName { get; set; }
        private readonly Queue<string> myqueue = new Queue<string>();
        private readonly Queue<string> _queueUrl = new Queue<string>();

        public string textBxValue { get; set; }
        public string getimgurl { get; set; }
        private int messageIdount;
        public string urlName;
        private string textcommet = string.Empty;
        private readonly BackgroundWorker worker = new BackgroundWorker();



        #endregion

        #region Contructor
        public MainWindowViewModel()
        {
            InstaListmembers = new ObservableCollection<InstaInboxmember>();
            FbPageListmembers=new ObservableCollection<FacebookPageInboxmember>();
            //FbPageInboxmember = new ObservableCollection<FacebookPageInboxmember>();
            UserListInfo = new ObservableCollection<FbpageInboxUserInfo>();
            MessagingListInfo = new ObservableCollection<FbUserMessageInfo>();
             BIndFbPageData();
            BIndInstData();
           //BindUserInfo();
            SendMessageCommand = new DelegateCommand(SendMessageCommandHandler, null);
            LoginCommand = new DelegateCommand(LoginCommandHandler, null);
            IntaInboxCommand = new DelegateCommand(IntaInboxCommandHandler,null);
            
        }

        private void IntaInboxCommandHandler(object obj)
        {

           // string commentinsta = (new TextRange(RichTextBoxinsta.Document.ContentStart, RichTextBoxinsta.Document.ContentEnd).Text).Trim();
            string url = "https://www.facebook.com/TP-1996120520653285/inbox/";
            ChromeWebDriver.Navigate().GoToUrl(url);
            Thread.Sleep(3000);

            ReadOnlyCollection<IWebElement> collection = ChromeWebDriver.FindElements(By.ClassName("_32wr"));
            {
                if (collection.Count > 0)
                {
                    collection[2].Click();
                    Thread.Sleep(3000);
                }
            }

            ReadOnlyCollection<IWebElement> commentpost = ChromeWebDriver.FindElements(By.XPath(".//*[@class='_11eg _5aj7']"));
            if (commentpost.Count > 0)
            {
                for (int i = 0; i < commentpost.Count; i++)
                {
                    var data = commentpost[i].Text;
                    FbPageListmembers.Add(new FacebookPageInboxmember { FbPageName = data, FbPageImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
                    //{
                    //    commentpost[i].Click();
                    //    Thread.Sleep(3000);
                    //    ReadOnlyCollection<IWebElement> sendcomment = ChromeWebDriver.FindElements(By.XPath(".//*[@class='_58al']"));
                    //    if (sendcomment.Count > 0)
                    //    {
                    //        //sendcomment[0].SendKeys(textBxValue);
                    //        Thread.Sleep(2000);
                    //        sendcomment[0].SendKeys(OpenQA.Selenium.Keys.Enter);


                    //    }
                    //}
                }
            }

            //
        }

        private void BIndFbPageData()
        {
            FbPageListmembers.Add(new FacebookPageInboxmember { FbPageName = "Facebook Page1 Post", FbPageImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
            FbPageListmembers.Add(new FacebookPageInboxmember { FbPageName = "Facebook Page2 Post", FbPageImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
        }

        private void BIndInstData()
        {
            InstaListmembers.Add(new InstaInboxmember { InstaInboxUserName = "Instagram Page1 Post", InstaInboxUserImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
            InstaListmembers.Add(new InstaInboxmember { InstaInboxUserName = "Instagram Page2 Post", InstaInboxUserImage = "E:\\RAHUL_WORK\\WPF_Examples\\Fb_InstaWpf12052018\\Fb_InstaWpf\\Fb_InstaWpf\\Images\\download.jpg" });
        }


        private void LoginCommandHandler(object obj)
        {
            //string url1 =
            //     "https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002948674558";
            //// string url = "https://go.developer.ebay.com/api-call-limits";
            //var Client = new HttpClient();
            //Task<string> t = Client.GetStringAsync(url1);
            //string s = t.Result;
            //Console.WriteLine(s);



            try
            {
                string appStartupPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // string appStartupPath = Path.Combine(Environment.CurrentDirectory);
                const string url = "https://en-gb.facebook.com/login/";

                _options.AddArgument("--disable-notifications");
                _options.AddArgument("--disable-extensions");
                _options.AddArgument("--test-type");
                //options.AddArgument("--headless");
                _options.AddArgument("--log-level=3");
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(appStartupPath);
                chromeDriverService.HideCommandPromptWindow = true;
                ChromeWebDriver = new ChromeDriver(chromeDriverService, _options);
                ChromeWebDriver.Manage().Window.Maximize();
                ChromeWebDriver.Navigate().GoToUrl(url);
                try
                {
                    ((IJavaScriptExecutor)ChromeWebDriver).ExecuteScript("window.onbeforeunload = function(e){};");
                }
                catch (Exception)
                {

                }

                ReadOnlyCollection<IWebElement> emailElement = ChromeWebDriver.FindElements(By.Id("email"));
                if (emailElement.Count > 0)
                {

                    emailElement[0].SendKeys("rishusingh77777@gmail.com");
                    //emailElement[0].SendKeys(TextBoxUserEmail.Text);

                }
                ReadOnlyCollection<IWebElement> passwordElement = ChromeWebDriver.FindElements(By.Id("pass"));
                if (passwordElement.Count > 0)
                {
                    passwordElement[0].SendKeys("1234567#rk");
                    //passwordElement[0].SendKeys(TextBox_Password.Text);

                }
                ReadOnlyCollection<IWebElement> signInElement = ChromeWebDriver.FindElements(By.Id("loginbutton"));
                if (signInElement.Count > 0)
                {
                    signInElement[0].Click();
                   
                    ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
                    ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002948674558");
                    Thread.Sleep(2000);
                    string pageSource = ChromeWebDriver.PageSource;
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(pageSource);
                    HtmlNodeCollection imgNode = htmlDocument.DocumentNode.SelectNodes("//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[2]/div/div[1]/div/div/div/div/div/div/img");
                    if (imgNode!=null)
                    {
                        foreach (var imgNodeItem in imgNode)
                        {
                            getimgurl = imgNodeItem.Attributes["src"].Value.Replace(";", "&");

                             myqueue.Enqueue(getimgurl);
                        }
                    }


                    var listNodeElements =
                        htmlDocument.DocumentNode.SelectNodes(
                            "//div[@class='_4ik4 _4ik5']");
                    if (listNodeElements != null)
                    {
                        for (int i = 0; i < listNodeElements.Count; i++)
                        {
                            if (i % 2 == 0)
                            {
                                LstItemUserName = listNodeElements[i].ChildNodes[0].InnerText;
                                var imgUrl = myqueue.Dequeue();

                                UserListInfo.Add(new FbpageInboxUserInfo() { InboxUserName = LstItemUserName, InboxUserImage = imgUrl });




                               
                            }


                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
       

        #region Property

        #region User Info Details
      
        public ObservableCollection<FbpageInboxUserInfo> UserListInfo
        {

            get { return userListInfo; }
            set
            {
                userListInfo = value;
                OnPropertyChanged("UserListInfo");

            }
        }
        public FbpageInboxUserInfo SelectedUserInfo
        {

            get { return selectedUserInfo; }
            set
            {
                selectedUserInfo = value;
                BindUserMessage(SelectedUserInfo);
                OnPropertyChanged("SelectedUserInfo");

            }
        }


        #endregion



        #region FbPageListmembers Details
        public ObservableCollection<FacebookPageInboxmember> FbPageListmembers
        {
            get { return fbPageInboxmember; }
            set { fbPageInboxmember = value; }
        }

        #endregion

        #region FbPageListmembers Details
        public ObservableCollection<InstaInboxmember> InstaListmembers
        {
            get { return instaInboxmember; }
            set { instaInboxmember = value; }
        }

        #endregion



        #region Messaging Info

        public ObservableCollection<FbUserMessageInfo> MessagingListInfo
        {

            get { return messagingListInfo; }
            set
            {
                messagingListInfo = value;
                OnPropertyChanged("MessagingListInfo");

            }
        }
        
        #endregion

        #endregion

        #region Commands
     
        public DelegateCommand SendMessageCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand FbPageInboxCommand { get; set; }
        public DelegateCommand IntaInboxCommand { get; set; }

        #endregion
        #region Method
        private void BindUserInfo()
        {
            //BindUserInfoByApi();
            UserListInfo.Add(new FbpageInboxUserInfo { InboxUserName = "rahul baba" });
            UserListInfo.Add(new FbpageInboxUserInfo { InboxUserName = "YoYO baba" });
            UserListInfo.Add(new FbpageInboxUserInfo { InboxUserName = "Tiger baba" });

        }


        private void BindUserMessage1(FbpageInboxUserInfo fbpageInboxUserInfo)
        {
            //Data Retrive
            MessagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            if (fbpageInboxUserInfo.InboxUserName.Equals("YoYO baba"))
            {
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "rahul baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = "YoYO baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "Tiger baba" });
            }
            else if (fbpageInboxUserInfo.InboxUserName.Equals("Tiger baba"))
            {
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "Gaurav baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = "Sonam baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "kajal baba" });
            }
            else
            {
                ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
                ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002948674558");



            }
        }



        private void BindUserMessage(FbpageInboxUserInfo fbpageInboxUserInfo)
        {
            //Data Retrive
            MessagingListInfo = new ObservableCollection<FbUserMessageInfo>();
            if (fbpageInboxUserInfo.InboxUserName.Equals("YoYO baba"))
            {
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "rahul baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = "YoYO baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "Tiger baba" });
            }
            else if (fbpageInboxUserInfo.InboxUserName.Equals("Tiger baba"))
            {
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "Gaurav baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = "Sonam baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "kajal baba" });
            }
            else
            {
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "Deepak baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 1, Message = "Saurab baba" });
                MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = "abi baba" });

                GetUserChatBoxHistory();
            }
        }

        private void GetUserChatBoxHistory()
        {
            ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
            ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/TP-1996120520653285/inbox/?selected_item_id=100002948674558");
            ReadOnlyCollection<IWebElement> yourPageMainNode = ChromeWebDriver.FindElements(By.ClassName("uiScrollableAreaContent"));
            if (yourPageMainNode.Count>0)
            {
                var uiScrollableAreaContent = ChromeWebDriver.FindElements(By.ClassName("uiScrollableAreaContent"));
                if (uiScrollableAreaContent.Count>0)
                {
                    
                }
            }

        }

        public void SendMessageCommandHandler(object j)
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();

           MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = textBxValue });
            //if (textBxValue!=null)
            //{
            //    textBxValue = "";
            //}
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           // throw new NotImplementedException();
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //MessagingListInfo.Add(new FbUserMessageInfo { UserType = 0, Message = textBxValue });
           // string textcommet = (new TextRange(RichTextBoxmsngr.Document.ContentStart, RichTextBoxmsngr.Document.ContentEnd).Text).Trim();
            //string textcommet = TextBox2.Text;
            try
            {

                //_chromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/messages/t/");
                ChromeWebDriver.Navigate().GoToUrl("https://www.facebook.com/pages/?category=your_pages");
                string pageSource = ChromeWebDriver.PageSource;

                ReadOnlyCollection<IWebElement> yourPageMainNode = ChromeWebDriver.FindElements(By.Id("page_browser_your_pages"));
                if (yourPageMainNode.Count > 0)
                {
                    ReadOnlyCollection<IWebElement> abcd =
                        ChromeWebDriver.FindElements(By.XPath(".//*[@id='page_browser_your_pages']/div/div/div/div[1]/a"));
                    if (abcd.Count > 0)
                    {
                        foreach (var abcdtemp in abcd)
                        {
                            var hrefString = abcdtemp.GetAttribute("href");
                            _queueUrl.Enqueue(hrefString);
                        }
                        try
                        {
                            for (int i = 0; i <= _queueUrl.Count; i++)
                            {

                                if (_queueUrl.Count > 0)
                                {
                                    messageIdount++;
                                    urlName = _queueUrl.Dequeue();
                                    ChromeWebDriver.Navigate().GoToUrl(urlName);
                                    Thread.Sleep(3000);
                                    ReadOnlyCollection<IWebElement> pageInbox = ChromeWebDriver.FindElements(By.XPath("//*[@role='tablist']/li[2]/a"));
                                    if (pageInbox.Count > 0)
                                    {
                                        pageInbox[0].Click();
                                        Thread.Sleep(3000);
                                    }

                                    ReadOnlyCollection<IWebElement> pageInboxFbMsngr = ChromeWebDriver.FindElements(By.XPath("//*[@id='u_0_t']/div/div/div/table/tbody/tr/td[1]/div/div[1]/div[1]/div[2]/div"));
                                    if (pageInboxFbMsngr.Count > 0)
                                    {
                                        Thread.Sleep(2000);
                                        pageInboxFbMsngr[0].Click();
                                        Thread.Sleep(2000);
                                    }
                                    ReadOnlyCollection<IWebElement> writeNode =
                                           ChromeWebDriver.FindElements(By.XPath("//*[@placeholder='Write a reply...']"));
                                    if (writeNode.Count > 0)
                                    {
                                        Thread.Sleep(2000);
                                        writeNode[0].SendKeys(textBxValue);
                                        Thread.Sleep(2000);
                                        //writeNode[0].SendKeys(msgtxtbox.Text);

                                    }
                                    ReadOnlyCollection<IWebElement> submitnode =
                                           ChromeWebDriver.FindElements(By.XPath("//*[@type='submit']"));
                                    if (submitnode.Count > 0)
                                    {
                                        Thread.Sleep(2000);
                                        submitnode[1].Click();
                                        Thread.Sleep(2000);
                                       
                                    }

                                    //var textstr = 




                                }
                    
                            }


                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion


    }
}
