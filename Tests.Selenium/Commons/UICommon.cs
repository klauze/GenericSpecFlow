using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Configuration;
using OpenQA.Selenium.Internal;

using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace Panviva.SPUI.Selenium.Project.Commons
{
    public static class UICommon
    {
        //public static int waitsec = Properties.Settings.Default.WaitTime;
        static System.Configuration.Configuration config =
        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;
        public static int waitsec = Int32.Parse(ConfigurationManager.AppSettings.Get("WaitSec"));
        private static IWebDriver Webdriver;

        /// <summary>
        /// Wait until element is present or visible on page depending on whether expectVisible is true
        /// By default, expectVisible is true which means expect element to be visible.
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="d"></param>
        /// <param name="expectVisible"></param>
        /// <returns></returns>
        public static IWebElement GetElement(By searchType, IWebDriver d, bool expectVisible = true, bool highlight = true)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
                IWebElement elem = expectVisible ?
                                        wait.Until(ExpectedConditions.ElementIsVisible(searchType))
                                        : wait.Until(ExpectedConditions.ElementExists(searchType));

                if (highlight == true)
                {
                    elementHighlight(elem, d);
                }


                return elem;
            }
            catch (WebDriverTimeoutException e)
            {
                // generate better messsages for error debug
                throw new WebDriverTimeoutException("Can't find web element:" + searchType + " on page "
                                                     + " {title=" + d.Title + ", url=" + d.Url + "}"
                                                      , e);
            }

        }

        ///// <summary>
        ///// Wait until element is present or visible on page depending on whether expectVisible is true
        ///// By default, expectVisible is true which means expect element to be visible
        ///// </summary>
        ///// <param name="searchType"></param>
        ///// <param name="expectVisible"></param>
        ///// <returns></returns>
        //public static IWebElement GetElement(this IWebDriver d, By searchType, bool expectVisible = true, bool highlight = true)
        //{
        //    return GetElement(searchType, d, expectVisible, highlight);
        //}

        //public static IWebElement GetElement(By searchType, IWebDriver d, int secondsToWait, bool expectVisible = true)
        //{
        //    try
        //    {
        //        WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(secondsToWait));
        //        IWebElement elem = expectVisible ?
        //                                wait.Until(ExpectedConditions.ElementIsVisible(searchType))
        //                                : wait.Until(ExpectedConditions.ElementExists(searchType));


        //        elementHighlight(elem, d);

        //        return elem;
        //    }
        //    catch (WebDriverTimeoutException e)
        //    {
        //        // generate better messsages for error debug
        //        throw new WebDriverTimeoutException("Can't find web element:" + searchType + " on page "
        //                                             + " {title=" + d.Title + ", url=" + d.Url + "}"
        //                                              , e);
        //    }

        //}





        //public static ReadOnlyCollection<IWebElement> GetElements(this IWebDriver d, By searchType, int timeoutInSeconds = 5, bool expectVisible = true)
        //{
        //    try
        //    {
        //        return d.WaitFor(drv =>
        //        {
        //            var es = d.FindElements(searchType);
        //            var rst = expectVisible ? es.Where(e => e.Displayed) : es;
        //            if (rst.Count() == 0) return null;
        //            return rst.ToList().AsReadOnly();
        //        }, timeoutInSeconds);
        //    }
        //    catch (WebDriverTimeoutException)
        //    {
        //        return d.FindElements(searchType);
        //    }
        //}

        //public static ReadOnlyCollection<IWebElement> GetElements(this IWebElement element, By searchType, int waitForSeconds = -1, bool expectVisible = true)
        //{
        //    try
        //    {
        //        return element.GetWebDriver().WaitFor(drv =>
        //        {
        //            var es = element.FindElements(searchType);
        //            var rst = expectVisible ? es.Where(e => e.Displayed) : es;
        //            if (rst.Count() == 0) return null;
        //            return rst.ToList().AsReadOnly();
        //        }, waitForSeconds);
        //    }
        //    catch (WebDriverTimeoutException)
        //    {
        //        return element.FindElements(searchType).Where(e1 => expectVisible ? e1.Displayed : true).ToList().AsReadOnly();
        //    }
        //}
        ///// <summary>
        ///// Extension method for FindElement that takes an explicit timeout
        ///// </summary>
        ///// <param name="caller">
        ///// </param>
        ///// <param name="by">
        ///// </param>
        ///// <param name="timeoutInSeconds">
        ///// </param>
        ///// <returns>
        ///// The <see cref="IWebElement"/>.
        ///// </returns>
        //public static IWebElement GetElement(this IWebElement caller, By by, bool expectVisible = true, int timeoutInSeconds = -1)
        //{
        //    IWebDriver driver = caller.GetWebDriver();

        //    try
        //    {
        //        var elem = driver.WaitFor(d =>
        //        {
        //            var e = caller.FindElement(by);
        //            if (expectVisible && !e.Displayed) return null;
        //            return e;
        //        }, timeoutInSeconds);

        //        elementHighlight(elem, driver);
        //        return elem;
        //    }
        //    catch (WebDriverTimeoutException)
        //    {
        //        // generate better messsages for error debug
        //        throw new WebDriverTimeoutException(
        //            string.Format("Can't find web element by locator: {0} within element({1}) on page (title={2}, url={3}) ",
        //             by.ToString(), caller.GetAttribute("outerHTML"), driver.Title, driver.Url));
        //    }
        //}
        ///// <summary>
        ///// wait for something to happen
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="driver"></param>
        ///// <param name="cond"></param>
        ///// <param name="msg">msg to raise if time out</param>
        ///// <param name="seconds"></param>
        ///// <returns></returns>
        //public static T WaitFor<T>(this IWebDriver driver, Func<IWebDriver, T> cond, string msg, int seconds = -1)
        //{
        //    if (seconds < 0) seconds = waitsec;
        //    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        //    if (!string.IsNullOrEmpty(msg)) wait.Message = msg;
        //    Exception lastException = null;

        //    try
        //    {
        //        return wait.Until(d =>
        //       {
        //           try
        //           {
        //               return cond(d);
        //           }
        //           catch (Exception e)   //caught exceptions
        //            {
        //               lastException = e;
        //               return default(T);
        //           }
        //       });
        //    }
        //    catch (WebDriverTimeoutException)
        //    {
        //        throw new WebDriverTimeoutException(
        //            string.Format("Timeout waiting for condition:{0} on page (title={1}, url={2})",
        //                          cond.Method.GetMethodBody().ToString(),
        //                          driver.Title,
        //                          driver.Url),
        //            lastException);
        //    }

        //}

        //public static T WaitFor<T>(this IWebDriver driver, Func<IWebDriver, T> cond, int seconds = -1)
        //{
        //    return WaitFor(driver, cond, null, seconds);
        //}

        //public static IReadOnlyCollection<IWebElement> GetElements(By searchType, IWebDriver d)
        //{
        //    return d.FindElements(searchType);
        //}

        ///// <summary>
        ///// ClickButton by Click hold and release with 500ms wait in between. Optional delay
        ///// </summary>
        ///// <param name="searchType"></param>
        ///// /// <param name="delayMilliSec"></param>
        ///// /// <param name="d"></param>
        public static void ClickButton(By searchType, IWebDriver d, int delayMilliSec = 0)
        {
            IWebElement elem = GetElement(searchType, d);
            WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));

            elem = wait.Until(c => c.FindElement(searchType));
   
            Thread.Sleep(delayMilliSec);
            Actions action = new Actions(d);
            action.MoveToElement(elem).Click().Build().Perform();
            Thread.Sleep(500);
            // action.MoveToElement(elem).Release().Build().Perform();
            // Thread.Sleep(200);
        }
       
        ///// <summary>
        ///// fluent way of ClickButton
        ///// </summary>
        ///// <param name="d"></param>
        ///// <param name="searchType"></param>
        //public static void ClickButton(this IWebDriver d, By searchType, int timeoutInSeconds = -1)
        //{
        //    Exception ex = null;

        //    IWebElement elem = GetElement(searchType, d);
        //    elem.GetWebDriver().WaitFor(drv =>
        //        {
        //            try
        //            {
        //                //drv.FindElements(searchType).First(e => e.Displayed).Click();
        //                //Updated to be compatible with IE
        //                drv.FindElements(searchType).First(e => e.Displayed).SendKeys(OpenQA.Selenium.Keys.Enter);

        //                return true;
        //            }
        //            catch (Exception e1)
        //            {
        //                ex = e1; return false;
        //            }
        //        }, string.Format("Failed to click element: {0} due to {1}", searchType, ex == null ? "unknown" : ex.ToString()), timeoutInSeconds
        //    );
        //}

        ///// <summary>
        ///// fluent way of ClickButton
        ///// </summary>
        ///// <param name="searchType"></param>
        //public static void ClickElement(this IWebElement element)
        //{
        //    Exception e = null;

        //    element.GetWebDriver().WaitFor(
        //        d =>
        //        {
        //            try
        //            {
        //                Thread.Sleep(1000);
        //                WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //                wait.Until(ExpectedConditions.ElementToBeClickable(element)).Click();
        //                return true;
        //            }
        //            catch (Exception e1)
        //            { e = e1; return false; }
        //        },
        //        string.Format("Failed to click element: {0} due to {1}", GetDebugInfo(element), e == null ? "unknown" : e.ToString()));

        //}

        //public static string GetDebugInfo(IWebElement el)
        //{
        //    return el.GetAttribute("outerHtml");
        //}

        //private static string CreateScreenShot(IWebDriver d)
        //{
        //    var randomName = "ScreenShot" + Guid.NewGuid().ToString().Substring(0, 8);
        //    //Screenshot ss = ((ITakesScreenshot)d).GetScreenshot();
        //    //ss.SaveAsFile(randomName, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return randomName;
        //}

        //public static void DoubleClickButton(By searchType, IWebDriver d)
        //{
        //    IWebElement elem = GetElement(searchType, d);
        //    Actions action = new Actions(d);
        //    action.DoubleClick(elem);
        //    action.Perform();
        //}

        //public static void SetValue(By searchType, string value, IWebDriver d, int timerInMillSecondsAfterClear = 0, bool clearText = true)
        //{
        //    IWebElement elem = GetElement(searchType, d, true);

        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    elem = wait.Until(ExpectedConditions.ElementToBeClickable(searchType));
        //    Thread.Sleep(1000);
        //    elem.Click();
        //    if (clearText) { elem.Clear(); }
        //    //strangly some inputbox takes time to start accepting chars after clear
        //    if (timerInMillSecondsAfterClear > 0) Thread.Sleep(timerInMillSecondsAfterClear);

        //    if (value != null)
        //    {
        //        foreach (char c in value)
        //        {
        //            elem.SendKeys(c.ToString());
        //            Thread.Sleep(100);
        //        }
        //    }
        //}
        //public static void SetValueNumeric(By searchType1, By searchType2, string value, IWebDriver d, int timerInMillSecondsAfterClear = 0, bool clearText = true)
        //{
        //    IWebElement elem = GetElement(searchType1, d, true);
        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));

        //    IWebElement elem2 = GetElement(searchType2, d, false);

        //    if (clearText) { elem.Clear(); }
        //    elem.ClickElement();
        //    //strangly some inputbox takes time to start accepting chars after clear
        //    if (timerInMillSecondsAfterClear > 0) Thread.Sleep(timerInMillSecondsAfterClear);

        //    if (value != null)
        //    {
        //        foreach (char c in value)
        //        {
        //            elem2.SendKeys(c.ToString());
        //            Thread.Sleep(30);
        //        }
        //    }
        //}
        //public static void ExecuteJSScript(IWebDriver d, string jsScript)
        //{
        //    IJavaScriptExecutor _javaScriptExecutor = (IJavaScriptExecutor)d;
        //    _javaScriptExecutor.ExecuteScript(jsScript);
        //    Thread.Sleep(2000);
        //}
        //public static void ClickMultipleSelection(By searchType, IWebDriver d)
        //{
        //    IWebElement elem = GetElement(searchType, d);
        //    elem.Click();
        //}

        //public static void SelectListValue(By searchType, string value, IWebDriver d)
        //{
        //    IWebElement elem = GetElement(searchType, d);
        //    //elem.Click();
        //    SelectElement selector = new SelectElement(elem);
        //    selector.SelectByText(value);


        //    //try

        //    //{
        //    //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    //    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(string.Format("//select/option[text()='{0}']", value)))).Click();
        //    //    Assert.IsTrue(elem.GetText().Equals(value));

        //    //}

        //    //catch (Exception e)
        //    //{ }


        //}

        public static void elementHighlight(IWebElement element, IWebDriver d)
        {
            if (element.TagName != "a")
            {
                var jsDriver = (IJavaScriptExecutor)d;
                string highlightJavascript = @"$(arguments[0]).css({ ""border-width"" : ""2px"", ""border-style"" : ""solid"", ""border-color"" : ""red"" });";
                jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
            }
        }

        //public static void Highlight(this IWebElement element)
        //{
        //    elementHighlight(element, element.GetWebDriver());
        //}

        //internal static void ClickLink(By by, IWebDriver d)
        //{
        //    try
        //    {
        //        IWebElement elem = GetElement(by, d);
        //        elem.ClickElement();
        //        Thread.Sleep(500);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e + ": " + by);
        //    }
        //}

        ///// <summary>
        ///// Clicks the switch.
        ///// </summary>
        ///// <param name="by">The by.</param>
        ///// <param name="d">The d.</param>
        ///// <exception cref="System.Exception">
        ///// ClickSwitch: Cannot find switch container.
        ///// or
        ///// ClickSwitch: Cannot find switch element.
        ///// </exception>
        //internal static void ClickSwitch(By by, IWebDriver d)
        //{
        //    var container = GetElement(by, d);
        //    if (container == null)
        //        throw new Exception("ClickSwitch: Cannot find switch container.");

        //    var sw = container.FindElement(By.ClassName("bool-slider"));
        //    if (sw == null)
        //        throw new Exception("ClickSwitch: Cannot find switch element.");

        //    sw.Click();
        //}

        ///// <summary>
        ///// The switch to new window.
        ///// </summary>
        ///// <param name="driver">
        ///// The driver.
        ///// </param>
        ///// <param name="titleSwitchTo">
        ///// The title switch to.
        ///// </param>
        ///// <returns>
        ///// The <see cref="string"/>.
        ///// </returns>
        ///// <exception cref="NoSuchElementException">
        ///// </exception>
        //public static string SwitchToNewWindow(this IWebDriver driver, string titleSwitchTo = null)
        //{
        //    // Store the current window handle
        //    var winHandleBefore = driver.CurrentWindowHandle;

        //    // Perform the click operation that opens new window
        //    // switch window is current window
        //    if (titleSwitchTo != null && driver.Title.Contains(titleSwitchTo))
        //        return winHandleBefore;

        //    // Switch to new window opened
        //    var newWinHandle = driver.WaitFor(d =>
        //    {
        //        var handles = d.WindowHandles;
        //        return handles.FirstOrDefault(s =>
        //        {
        //            d.SwitchTo().Window(s);
        //            return (!s.Equals(winHandleBefore)) &&
        //                   (titleSwitchTo == null || d.Title.Contains(titleSwitchTo));
        //        });
        //    },
        //        "Can't switch to new window :" + titleSwitchTo ?? string.Empty);

        //    driver.SwitchTo().Window(newWinHandle);
        //    return newWinHandle;
        //}

        ///// <summary>
        ///// Aquire new window.
        ///// </summary>
        ///// <param name="driver">
        ///// The driver.
        ///// </param>
        ///// <param name="titleToAquire">
        ///// The title to aquire.
        ///// </param>
        ///// <returns>
        ///// The <see cref="string"/>.
        ///// </returns>
        ///// <exception cref="NoSuchElementException">
        ///// </exception>
        //public static string AquireNewWindow(this IWebDriver driver, string titleToAquire = null)
        //{
        //    // Switch to new window opened
        //    var newWinHandle = driver.WaitFor(d =>
        //    {
        //        var handles = d.WindowHandles;
        //        return handles.FirstOrDefault(s =>
        //        {
        //            d.SwitchTo().Window(s);
        //            return (d.Title.Contains(titleToAquire));
        //        });
        //    },
        //        "Can't switch to new window :" + titleToAquire ?? string.Empty);

        //    driver.SwitchTo().Window(newWinHandle);
        //    return newWinHandle;
        //}
        ////  [System.Obsolete("this method is deprecated as it's slow, please use SwitchToNewWindow instead.")]

        ///// <summary>
        ///// Switches to new browser with URL.
        ///// </summary>
        ///// <param name="d">The webdriver.</param>
        ///// <param name="url">The URL.</param>
        ///// <param name="isRegexp">if set to <c>true</c> [url passed in is regexp].</param>







        ///// <summary>
        ///// Switches to new page with title.
        ///// </summary>
        ///// <param name="d">The driver.</param>
        ///// <param name="Title">The title.</param>
        //public static void SwitchToNewPageWithTitle(IWebDriver d, string Title)
        //{
        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    wait.Until((driver) => { return d.Title.Contains(Title); });

        //}
        //public static IWebElement GetSearchResultTable(By searchTableBy, IWebDriver d)
        //{
        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    IWebElement webElementBody = wait.Until(ExpectedConditions.ElementIsVisible(searchTableBy));
        //    //Check that the grid loading image has gone
        //    Assert.IsTrue(ObjectNotExists(By.XPath("//div[@class='k-loading-mask']"), d), "Table did not complete loading");
        //    return webElementBody;
        //}

        //public static bool ObjectNotExists(By searchType, IWebDriver d, int timeout = 10)
        //{
        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    wait.Until((driver) => { return d.FindElements(searchType).Count == 0; });
        //    return true;
        //}

        //public static string GetElementAttribute(By searchType, string attribute, IWebDriver d)
        //{

        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    IWebElement elem = wait.Until(ExpectedConditions.ElementExists(searchType));
        //    elementHighlight(elem, d);
        //    return elem.GetAttribute(attribute);

        //}

        //public static IWebElement GetFolderTree(By searchBy, IWebDriver d)
        //{
        //    IWebElement elem = UICommon.GetElement(searchBy, d);
        //    return elem;
        //}





        //public static void DeselectCheckbox(By searchType, IWebDriver d)
        //{
        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    IWebElement elem = GetElement(searchType, d);

        //    // gets the current checked state of the checkbox
        //    bool ischecked = elem.FindElement(searchType).Selected;

        //    // disables the checkbox if it is currently selected
        //    if (ischecked)
        //    {
        //        Actions action = new Actions(d);
        //        action.MoveToElement(elem).Click().Build().Perform();
        //        Thread.Sleep(500);
        //    }
        //}
        //public static void TurnSliderOn(By searchType, IWebDriver d)
        //{
        //    IWebElement elem = d.GetElement(searchType);

        //    bool isOn = elem.FindElement(searchType).GetAttribute("class").Contains("bootstrap-switch-on")
        //        && !elem.FindElement(searchType).GetAttribute("class").Contains("bootstrap-switch-off");
        //    // turn slider on
        //    if (!isOn)
        //    {
        //        Actions action = new Actions(d);
        //        action.MoveToElement(elem).ClickAndHold().Build().Perform();
        //        Thread.Sleep(500);
        //        action.MoveToElement(elem).Release().Build().Perform();
        //        Thread.Sleep(500);
        //    }
        //    Assert.IsTrue(elem.FindElement(searchType).GetAttribute("class").Contains("bootstrap-switch-on")
        //        , "slider is not on");
        //}
        //public static void TurnSliderOff(By searchType, IWebDriver d)
        //{
        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    IWebElement elem = wait.Until(ExpectedConditions.ElementToBeClickable(searchType));

        //    bool isOff = elem.FindElement(searchType).GetAttribute("class").Contains("bootstrap-switch-off")
        //        && !elem.FindElement(searchType).GetAttribute("class").Contains("bootstrap-switch-on");
        //    // turn slider off
        //    if (!isOff)
        //    {
        //        Actions action = new Actions(d);
        //        action.MoveToElement(elem).ClickAndHold().Build().Perform();
        //        Thread.Sleep(500);
        //        action.MoveToElement(elem).Release().Build().Perform();



        //        Thread.Sleep(500);
        //    }
        //    Assert.IsTrue(elem.FindElement(searchType).GetAttribute("class").Contains("bootstrap-switch-off")
        //        , "slider is not off");
        //}
        //public static void ConfirmSliderOff(By searchType, IWebDriver d)
        //{
        //    IWebElement elem = GetElement(searchType, d);

        //    bool isOff = elem.GetAttribute("class").Contains("bootstrap-switch-off")
        //        && !elem.GetAttribute("class").Contains("bootstrap-switch-on");
        //    Assert.IsTrue(isOff, "slider is not off");
        //}
        //public static void ConfirmSliderOn(By searchType, IWebDriver d)
        //{
        //    IWebElement elem = GetElement(searchType, d);

        //    bool isOn = elem.GetAttribute("class").Contains("bootstrap-switch-on")
        //        && !elem.GetAttribute("class").Contains("bootstrap-switch-off");
        //    Assert.IsTrue(isOn, "slider is not on");
        //}


        //public static void ConfirmSliderDisabled(By searchType, IWebDriver d)
        //{
        //    IWebElement elem = GetElement(searchType, d);

        //    bool isDisabled = elem.GetAttribute("class").Contains("bootstrap-switch-readonly");
        //    Assert.IsTrue(isDisabled, "slider is not disabled");
        //}


        //public static void ConfirmSliderEnabled(By searchType, IWebDriver d)
        //{
        //    IWebElement elem = GetElement(searchType, d);

        //    bool isEnabled = !elem.GetAttribute("class").Contains("bootstrap-switch-readonly");
        //    Assert.IsTrue(isEnabled, "slider is disabled");
        //}



        //public static void ConfirmCheckboxIsChecked(By searchType, IWebDriver d)
        //{
        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    IWebElement elem = GetElement(searchType, d);

        //    // gets the current checked state of the checkbox
        //    bool ischecked = elem.Selected;

        //    Assert.IsTrue(ischecked, searchType.ToString() + " is not checked");
        //}
        //public static void ConfirmCheckboxIsUnchecked(By searchType, IWebDriver d)
        //{
        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(waitsec));
        //    IWebElement elem = GetElement(searchType, d);

        //    // gets the current checked state of the checkbox
        //    bool ischecked = elem.Selected;

        //    Assert.IsFalse(ischecked, searchType.ToString() + " is still checked");
        //}
        //public static void ClickOnFolder(string Page, string[] Folders, By FolderTree, IWebDriver d)
        //{
        //    //get folder tree element
        //    IWebElement folderTree;
        //    folderTree = GetFolderTree(FolderTree, d);
        //    //count the number of indexes
        //    int folderTreeDepth = Folders.Count();

        //    for (int i = 0; i < folderTreeDepth; i++)
        //    {
        //        //get the name of the current folder
        //        bool folderFound = false;
        //        string FolderName = Folders[i];
        //        //get the number of available parent folders in docexplorertree

        //        d.WaitFor(drv => folderTree.FindElement(By.XPath("./ul/li")));  //this is to wait for tree node load
        //        IReadOnlyCollection<IWebElement> folders = folderTree.FindElements(By.XPath("./ul/li"));


        //        //check each parent folder in tree. Go to exception if non are found         
        //        foreach (IWebElement folder in folders)
        //        {
        //            IWebElement folderText = folder.FindElement(By.XPath(".//div/span/span"));

        //            if (folderText.Text == FolderName)
        //            {
        //                //Expand the folder
        //                if ((folder.GetAttribute("aria-expanded") != "true") && (folderTreeDepth - 1 != i))
        //                {
        //                    Actions action = new Actions(d);
        //                    var stuff = d.GetElement(By.XPath(string.Format(".//div/span/span[contains(text(),'{0}')]", FolderName)));
        //                    action.DoubleClick(folderText).Build().Perform();
        //                    //wait for loading to finish
        //                    UICommon.ObjectNotExists(By.XPath("//div/span[@role='presentation'][contains(@class,'k-loading')]"), d);
        //                    folderTree = folder;
        //                    folderFound = true;
        //                    break;
        //                }
        //                else
        //                {
        //                    Actions action = new Actions(d);
        //                    action.MoveToElement(folderText).Click().Build().Perform();
        //                    //wait for loading to finish
        //                    UICommon.ObjectNotExists(By.XPath("//div/span[@role='presentation'][contains(@class,'k-loading')]"), d);
        //                    Thread.Sleep(1000);
        //                    folderTree = folder;
        //                    folderFound = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (folderFound == false)
        //        {
        //            throw new Exception("Folder " + FolderName + " could not be found");
        //        }
        //    }
        //}


        //public static void ClickOnFolderKendo(string Page, string[] Folders, By FolderTree, IWebDriver d)
        //{
        //    //get folder tree element
        //    IWebElement folderTree;
        //    folderTree = GetFolderTree(FolderTree, d);
        //    //count the number of indexes
        //    int folderTreeDepth = Folders.Count();

        //    for (int i = 0; i < folderTreeDepth; i++)
        //    {
        //        //get the name of the current folder
        //        bool folderFound = false;
        //        string FolderName = Folders[i];
        //        //get the number of available parent folders in docexplorertree

        //        d.WaitFor(drv => folderTree.FindElement(By.XPath(".//ul//li")));  //this is to wait for tree node load
        //        IReadOnlyCollection<IWebElement> folders = folderTree.FindElements(By.XPath(".//ul//li"));


        //        //check each parent folder in tree. Go to exception if non are found         
        //        foreach (IWebElement folder in folders)
        //        {
        //            IWebElement folderText = folder.FindElement(By.XPath(".//div/span/span"));

        //            if (folderText.Text == FolderName)
        //            {
        //                //Expand the folder
        //                if (folder.GetAttribute("aria-expanded") != "true")
        //                {
        //                    Actions action = new Actions(d);
        //                    action.DoubleClick(folderText).Build().Perform();
        //                    //wait for loading to finish
        //                    UICommon.ObjectNotExists(By.XPath("//div/span[@role='presentation'][contains(@class,'k-loading')]"), d);
        //                    folderTree = folder;
        //                    folderFound = true;
        //                    break;
        //                }
        //                else
        //                {
        //                    Actions action = new Actions(d);
        //                    action.MoveToElement(folderText).Click().Build().Perform();
        //                    //wait for loading to finish
        //                    UICommon.ObjectNotExists(By.XPath("//div/span[@role='presentation'][contains(@class,'k-loading')]"), d);
        //                    Thread.Sleep(1000);
        //                    folderTree = folder;
        //                    folderFound = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (folderFound == false)
        //        {
        //            throw new Exception("Folder " + FolderName + " could not be found");
        //        }
        //    }
        //}

        //public static void ClickOnViewerFolder(string Page, string[] Folders, By FolderTree, IWebDriver d)
        //{
        //    //get folder tree element
        //    IWebElement folderTree;
        //    folderTree = GetFolderTree(FolderTree, d);

        //    //count the number of indexes
        //    int folderTreeDepth = Folders.Count();

        //    for (int i = 0; i < folderTreeDepth; i++)
        //    {
        //        //get the name of the current folder
        //        bool folderFound = false;
        //        string FolderName = Folders[i];
        //        //get the number of available parent folders in docexplorertree

        //        d.WaitFor(drv =>
        //        {
        //            try
        //            {
        //                return folderTree.FindElements(By.XPath("./ul/li")).Where(e => e.Displayed).Count() > 0
        //                 && folderTree.GetElement(By.XPath("./ul/li")).Text.Trim().ToLower() != "loading...";
        //            }
        //            catch
        //            { return false; }
        //        }); // this is just to wait for tree node load

        //        IReadOnlyCollection<IWebElement> folders = folderTree.FindElements(By.XPath("./ul/li"));


        //        //check each parent folder in tree. Go to exception if non are found         
        //        foreach (IWebElement folder in folders)
        //        {
        //            IWebElement folderText = folder.FindElement(By.XPath(".//a"));
        //            // Console.WriteLine("folder:" + folderText.Text.Trim());
        //            if (folderText.Text.Trim() == FolderName.Trim())
        //            {
        //                //Expand the folder
        //                if (folder.GetAttribute("class").Contains("jstree-open")
        //                    || folder.GetAttribute("class").Contains("jstree - open"))

        //                {
        //                    folderText.ClickElement();
        //                    folderText.ClickElement();

        //                    d.WaitFor(drv => folder.GetAttribute("class").Contains("jstree-open") || folder.GetAttribute("class").Contains("jstree - open"));
        //                    //Actions action = new Actions(d);
        //                    //action.DoubleClick(folderText).Build().Perform();
        //                    folderTree = folder;
        //                    folderFound = true;
        //                    break;
        //                }
        //                else
        //                {
        //                    //Actions action = new Actions(d);
        //                    //action.MoveToElement(folderText).Click().Build().Perform();
        //                    //Thread.Sleep(1000);
        //                    folderText.ClickElement();
        //                    folderTree = folder;
        //                    folderFound = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (folderFound == false)
        //        {
        //            throw new Exception("Folder could not be found: " + FolderName);
        //        }
        //    }
        //}

        //public static bool FindGridRecord(string Record, By Grid, IWebDriver d)
        //{
        //    //get folder tree element
        //    IWebElement grid;
        //    grid = GetFolderTree(Grid, d);

        //    //get the number of available parent folders in docexplorertree

        //    IReadOnlyCollection<IWebElement> records = grid.FindElements(By.XPath("./ul/li"));

        //    //check each parent folder in tree. Go to exception if non are found         
        //    foreach (IWebElement record in records)
        //    {
        //        if (record.Text.Contains(Record))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}


        //public static string getRandomName(string name)
        //{
        //    string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        //    return name + currentTime;
        //}

        //public static bool confirmInterruptMessage(IWebDriver d)
        //{

        //    WebDriverWait wait = new WebDriverWait(d, TimeSpan.FromSeconds(5));
        //    wait.Until(c => c.FindElement(By.Id("cancelTitle")));
        //    // wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("cancelTitle")));
        //    d.FindElement(By.Id("cancelTitle")).Click();
        //    //UICommon.ClickButton(By.Id("cancelTitle"), d);

        //    //Read Interrupt Message Later
        //    By interruptMessage = By.Id("qtip-interruptMsgModal");
        //    IWebElement elem = d.FindElement(interruptMessage);
        //    try
        //    {
        //        elem.FindElement(By.Id("cancelTitle")).Click();
        //    }
        //    catch { }
        //    try
        //    {
        //        elem.FindElement(By.Id("alertMesgOkTitle")).Click();
        //    }
        //    catch { }

        //    return true;
        //}

        //public static bool confirmToastInfoMessage(string message, IWebDriver d)
        //{
        //    //wait until toast message is exists
        //    By infoToast = By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-info')]");
        //    IWebElement elem = GetElement(infoToast, d);

        //    //confirm message is correct

        //    //String toastText = elem.FindElement(By.XPath("//div[@class='toast-title']")).Text;
        //    StringAssert.Contains(elem.FindElement(By.XPath(".//div[@class='toast-message']")).Text, message, "Info message is invalid");
        //    elem.FindElement(By.XPath(".//button[@class='toast-close-button']")).Click();
        //    //wait until toast message does not exist
        //    Assert.IsTrue(ObjectNotExists(infoToast, d), "Object still exists");
        //    return true;
        //}

        //public static bool confirmToastInfoTitle(string title, IWebDriver d)
        //{
        //    //wait until toast message is exists
        //    By infoToast = By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-info')]");
        //    IWebElement elem = GetElement(infoToast, d, 5);

        //    //confirm message is correct

        //    //String toastText = elem.FindElement(By.XPath("//div[@class='toast-title']")).Text;
        //    StringAssert.Contains(elem.FindElement(By.XPath(".//div[@class='toast-title']")).Text, title, "Info title is invalid");
        //    //   elem.FindElement(By.XPath(".//button[@class='toast-close-button']")).Click();
        //    //wait until toast message does not exist
        //    Assert.IsTrue(ObjectNotExists(infoToast, d), "Object still exists");
        //    return true;
        //}

        //public static bool confirmToastInProgress(string message, IWebDriver d)
        //{
        //    //wait until toast message is exists
        //    By infoToast = By.XPath("//div[@id='toast-container']//div[@ng-repeat='toaster in toasters']");
        //    IWebElement elem = GetElement(infoToast, d, 5);

        //    //confirm message is correct

        //    //String toastText = elem.FindElement(By.XPath("//div[@class='toast-title']")).Text;
        //    StringAssert.Contains(elem.FindElement(By.XPath(".//div[@ng-class='config.message']")).Text, message, "Info title is invalid");
        //    //   elem.FindElement(By.XPath(".//button[@class='toast-close-button']")).Click();
        //    //wait until toast message does not exist
        //    Assert.IsTrue(ObjectNotExists(infoToast, d), "Object still exists");
        //    return true;
        //}


        //public static bool verifyToastTitle(string title, IWebDriver d)
        //{
        //    // var Toast = d.GetElement(By.XPath(string.Format(".//div[@class='toast-title'][text()='{0}']", title)));
        //    var Toast = d.GetElement(By.XPath(".//div[@class='toast-title']")).Text;
        //    Assert.AreEqual(title, Toast, "Expected toast text does not match");
        //    return true;
        //}
        //public static bool confirmToastSuccessMessage(string message, IWebDriver d)
        //{
        //    //wait until toast message is exists
        //    By infoToast = By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-success')]");
        //    IWebElement elem = GetElement(infoToast, d);
        //    // Wait for message to load
        //    d.WaitFor(driver => driver.FindElement(By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-success')]//div[@class='toast-message']")).Text.Contains(message));
        //    //confirm message is correct
        //    StringAssert.Contains(elem.FindElement(By.XPath(".//div[@class='toast-message']")).Text, message, "Info message is invalid");
        //    //elem.FindElement(By.XPath(".//button[@class='toast-close-button']")).Click();
        //    //wait until toast message does not exist
        //    Assert.IsTrue(ObjectNotExists(infoToast, d), "Object still exists");
        //    return true;
        //}


        ////Added this for contains rather than exact text

        //public static bool confirmToastSuccessMessage(string message, IWebDriver d, int watTime)
        //{
        //    //wait until toast message is exists
        //    By infoToast = By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-success')]");
        //    IWebElement elem = GetElement(infoToast, d, watTime);
        //    // Wait for message to load
        //    d.WaitFor(driver => driver.FindElement(By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-success')]//div[@class='toast-message']")).Text.Contains(message));
        //    //confirm message is correct
        //    StringAssert.Contains(elem.FindElement(By.XPath(".//div[@class='toast-message']")).Text, message, "Info message is invalid");
        //    elem.FindElement(By.XPath(".//button[@class='toast-close-button']")).Click();
        //    //wait until toast message does not exist
        //    Assert.IsTrue(ObjectNotExists(infoToast, d), "Object still exists");
        //    return true;
        //}

        //public static bool confirmEditorToastSuccessMessage(string message, IWebDriver d)
        //{
        //    //wait until toast message is exists
        //    By infoToast = By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-success')]");
        //    IWebElement elem = GetElement(infoToast, d);
        //    //confirm message is correct
        //    StringAssert.Contains(elem.FindElement(By.XPath(".//div[@class='toast-title']")).Text, message, "Info message is invalid");
        //    //wait until toast message does not exist
        //    Assert.IsTrue(ObjectNotExists(infoToast, d), "Object still exists");
        //    return true;
        //}

        //public static bool confirmToastErrorMessage(string message, IWebDriver d)
        //{
        //    //wait until toast message is exists
        //    By infoToast = By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-error')]");
        //    IWebElement elem = GetElement(infoToast, d);
        //    //confirm message is correct
        //    StringAssert.Contains(elem.FindElement(By.XPath(".//div[@class='toast-message']")).Text, message, "Info message is invalid");
        //    //wait until toast message does not exist
        //    Assert.IsTrue(ObjectNotExists(infoToast, d), "Object still exists");
        //    return true;
        //}
        ///// <summary>
        ///// Detects error toast messages and closes toasts message.
        ///// Then confirms closure of element
        ///// </summary>
        ///// <param name="message"></param>
        ///// <param name="d"></param>
        ///// <returns></returns>
        //public static bool confirmAndCloseToastErrorMessage(string message, IWebDriver d)
        //{
        //    //wait until toast message is exists
        //    By infoToast = By.XPath("//div[@id='toast-container']//div[contains(@class,'toast-error')]");
        //    IWebElement elem = GetElement(infoToast, d);
        //    //confirm message is correct
        //    StringAssert.Contains(elem.FindElement(By.XPath(".//div[@class='toast-message']")).Text, message, "Info message is invalid");
        //    elem.FindElement(By.XPath(".//button[@class='toast-close-button']")).Click();
        //    //wait until toast message does not exist
        //    Assert.IsTrue(ObjectNotExists(infoToast, d), "Object still exists");
        //    return true;
        //}

        ///// <summary>
        ///// Get webdriver from a webelement
        ///// </summary>
        ///// <param name="element"></param>
        ///// <returns></returns>
        //public static IWebDriver GetWebDriver(this IWebElement element)
        //{
        //    var e = element;

        //    // unwrap element till to IWebElement
        //    while (e is IWrapsElement && !(e is IWrapsDriver))
        //    {
        //        e = ((IWrapsElement)e).WrappedElement;
        //    }

        //    return ((IWrapsDriver)e).WrappedDriver;
        //}

        ///// <summary>
        ///// Get webdriver from a webdriver instance
        ///// </summary>
        ///// <param name="driver">
        ///// The driver.
        ///// </param>
        ///// <returns>
        /////  Unwrapped IWebDriver object
        ///// </returns>
        //public static IWebDriver GetWebDriver(this IWebDriver driver)
        //{
        //    var d = driver;
        //    while (d is IWrapsDriver)
        //    {
        //        d = ((IWrapsDriver)d).WrappedDriver;
        //    }

        //    return d;
        //}

        ///// <summary>
        ///// set a value to an web element:
        /////  it can be a SelectElement, a InputBox
        ///// </summary>
        ///// <param name="element">
        ///// </param>
        ///// <param name="value">
        ///// </param>
        ///// <param name="timeoutInSeconds">
        ///// The timeout In Seconds.
        ///// </param>
        ///// <returns>
        ///// The <see cref="IWebElement"/>.
        ///// </returns>
        //public static IWebElement SetText(this IWebElement element, string value, int timeoutInSeconds = -1)
        //{

        //    switch (element.TagName.ToLower())
        //    {
        //        case "textarea":
        //        case "input":
        //            element.GetWebDriver().WaitFor(ExpectedConditions.ElementToBeClickable(element));
        //            element.Clear();
        //            foreach (char c in value)
        //            {
        //                element.SendKeys(c.ToString());
        //            }

        //            break;
        //        case "select":

        //            if (element.GetText() == value) break;
        //            // as select options may be loaded dynamically, so add wait here
        //            try
        //            {
        //                element.GetWebDriver().WaitFor(
        //                    e =>
        //                    {
        //                        try
        //                        {
        //                            new SelectElement(element).SelectByText(value);
        //                            return true;
        //                        }
        //                        //catch (NoSuchElementException)
        //                        catch (Exception)
        //                        {
        //                            return false;
        //                        }
        //                    }, timeoutInSeconds);
        //            }
        //            catch (WebDriverTimeoutException e)
        //            {
        //                throw new WebDriverTimeoutException("Can't find " + value + " to select", e);
        //            }

        //            break;
        //        default:
        //            throw new ArgumentException("SetValue for " + element.TagName + " is currently not supported");
        //    }

        //    return element;
        //}

        ///// <summary>
        ///// return displayed text for a web element:
        /////    - Input : get value
        /////    - Single selection : get selected text
        /////    - Label /Others : get text
        ///// </summary>
        ///// <param name="element">
        ///// </param>
        ///// <returns>
        ///// The <see cref="string"/>.
        ///// </returns>
        public static string GetText(this IWebElement element)
        {
            switch (element.TagName.ToLower())
            {
                case "input":
                case "textarea":
                    return element.GetAttribute("value");
                case "select":
                    return new SelectElement(element).SelectedOption.Text;
                default:
                    return element.Text;
            }

            }



        }
}
