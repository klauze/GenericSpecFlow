//using OpenQA.Selenium;
//using System;
//using System.Configuration;

//namespace Tests.Selenium.PageModels
//{
//    public abstract class BasePage
//    {
//        static protected IWebDriver d;
//        public IWebDriver WebDriver
//        {
//            get
//            {
//                return d;
//            }
//        }
//        public BasePage(IWebDriver driver)
//        {
//            //  d = driver;
//            // d.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(waitsec));
//        }

//        public BasePage(IWebDriver driver, By locator) : this(driver)
//        {
           
//        }

//        public BasePage(IWebDriver driver, Predicate<IWebDriver> cond) : this(driver)
//        {

//        }
//    }
//}
