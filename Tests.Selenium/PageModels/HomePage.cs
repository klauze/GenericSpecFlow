using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using OpenQA.Selenium.Interactions;
using Tests.Selenium.ToscaObstacleTests.Commons;
using Panviva.SPUI.Selenium.Project.Commons;

namespace Tests.Selenium.PageModels
{
    public class HomePage
    {
        //Get time to wait from app config
        System.Configuration.Configuration config =
        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;
        public static int waitsec = Int32.Parse(ConfigurationManager.AppSettings.Get("WaitSec"));


        static protected IWebDriver d;


        public IWebDriver WebDriver
        {
            get
            {
                return d;
            }
        }

        //IWebDriver d;
        public HomePage(IWebDriver driver)
            {
            d = driver;
            //Wait for title to be displayed 
            System.Diagnostics.Debug.WriteLine("wait4title");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitsec));
            wait.Until((D) =>
            {
                System.Diagnostics.Debug.WriteLine("testTitle:" + d.Title);
                return d.Title.Contains("In the Drink");
            });
            System.Diagnostics.Debug.WriteLine("done");
        }

        //Navigation Elements
        By Home = By.XPath("//a[@href='https://inthedrink.org.uk/']");
        By AboutUs = By.XPath("//ul[@id='primary-menu']/li/a[contains(text(),'About Us')]");
        By Problem = By.XPath("//ul[@id='primary-menu']/li/a[contains(text(),'Problem')]");

        By SearchButton = By.XPath("//li[@class='menu-item menu-item-search']/a");
        By SearchButtonExpanded = By.XPath("//input[@id='searchform-submit']");
        By SearchTextBox = By.XPath("//input[@id='searchform-input']");
        By SearchResultHeader = By.XPath("//span[text()='Search Results']/following-sibling::span");


        public void AssertCurrentURL(string expectedURL)
        {
            switch (expectedURL)
            {
                
                case "home":
                    Assert.IsTrue(d.Url.Equals("https://inthedrink.org.uk/"));
             
                    break;

                case "about us":
                    Assert.IsTrue(d.Url.Equals("https://inthedrink.org.uk/about-us/"));
                    break;

                case "problem":
                    Assert.IsTrue(d.Url.Equals("https://inthedrink.org.uk/problem/"));
                    break;


                default:
                    throw new ArgumentException("Invalid page");


            }

            
        }

        public void NavigateToPage(string page)
        {
            switch (page)
            {

                case "home":
                    UICommon.ClickButton(Home, d);
                    break;

                case "about us":
                    UICommon.GetElement(AboutUs, d).Click();
                    break;

                case "problem":
                    UICommon.GetElement(Problem, d).Click();
                    break;


                default:
                    throw new ArgumentException("Invalid page");


            }


        }


        public void SearchForTerm(String Term)
        {
            UICommon.ClickButton(SearchButton, d);

            UICommon.GetElement(SearchTextBox, d,true,false).SendKeys(Term);

            UICommon.GetElement(SearchButtonExpanded, d, true, false).Click();
        

        }

        public void VerifySearchedTermDisplayed(String Term)
        {
            var SearchedText = UICommon.GetElement(SearchResultHeader, d,true,false).GetText();
            var FormattedTerm = "\""+Term+"\"";
            Assert.AreEqual((FormattedTerm),SearchedText, true);
        }

    }



}





