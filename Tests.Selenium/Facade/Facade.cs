using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Selenium.Environments;
using Tests.Selenium.PageModels;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Tests.Selenium.Facade
{
    public class InTheDrink
    {
        static public IWebDriver WebDriver { get; set; }


        static public HomePage HomePage { get { return new HomePage(WebDriver); } }

        //App.config settings
        static public string environment;
        static public string protocol;
        static public BrowserType browser;
        static public int waitsec;
        static public string driverLocation;


        public InTheDrink()
        {
      
            System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;


            environment = ConfigurationManager.AppSettings.Get("Environment");
            browser = (BrowserType)Enum.Parse(typeof(BrowserType), ConfigurationManager.AppSettings.Get("Browser"));
            protocol = ConfigurationManager.AppSettings.Get("Protocol");
            waitsec = Int32.Parse(ConfigurationManager.AppSettings.Get("WaitSec"));

            string codeBase = typeof(InTheDrink).Assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);


            driverLocation = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;

            Console.WriteLine(driverLocation);
            // Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
            //driverLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "AppData";
            ///driverLocation = System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "AppData";

        }



        public void OpenNewSession()
        {


            ChromeOptions _ChromeOptions = new ChromeOptions();
            _ChromeOptions.AddArguments("disable-infobars");      //disable the information bar in chrome
            _ChromeOptions.AddArguments("--start-maximized");
            //DesiredCapabilities capability;
            DesiredCapabilities capabilities = new DesiredCapabilities();

            var obj = _ChromeOptions.ToCapabilities().GetCapability("recreateChromeDriverSessions");
            obj = true;
            switch (browser)//Properties.Settings.Default.Browser)
            {

                //case BrowserType.EDGE:
                //    WebDriver = new EdgeDriver();
                //    WebDriver.Navigate().GoToUrl(protocol + environment);
                //    break;
                case BrowserType.Chrome:
                    WebDriver = new ChromeDriver(driverLocation, _ChromeOptions, TimeSpan.FromMinutes(100));
                    WebDriver.Navigate().GoToUrl(protocol + environment);
                    break;

                default:
                    throw new ArgumentException("Browser Type Invalid");


            }
        }

        public static void CloseSession()
        {
            if (WebDriver != null)
            {
                try
                {
                    WebDriver.Dispose();
                }
                catch (Exception) { }
            }
            WebDriver = null;

            switch (browser)//Properties.Settings.Default.Browser)
            {
                case BrowserType.Chrome:
                    KillProcess("chrome.exe");
                    KillProcess("chromedriver.exe");
                    break;

                default:
                    throw new ArgumentException("Browser Type Invalid");

            }
        }


        public static void KillProcess(string processName)
        {
            foreach (var process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName)))
            {
                try
                {
                    process.Kill();
                }
                catch
                { }

            }
        }

        static public bool IsToscaObstacleOpen()
        {
            return WebDriver != null;
        }




    }



}