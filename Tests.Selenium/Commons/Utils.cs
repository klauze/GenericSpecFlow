using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace Tests.Selenium.ToscaObstacleTests.Commons
{
    public static class Utils
    {
        /// <summary>
        /// this is to process string containing special pattern: 
        /// 
        /// if match [key=Random]  :
        ///  it will replace Random with current datetime in yyyyMMddHHmmss format and put value into context could be retreived later by "key"
        /// if match [key] :
        ///   it will just retrieve key in Scenario Context
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// 

        static System.Configuration.Configuration config =
ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;
        public static int waitsec = Int32.Parse(ConfigurationManager.AppSettings.Get("WaitSec"));
        private static IWebDriver Webdriver;



        public static void elementHighlight(IWebElement element, IWebDriver d)
        {
            if (element.TagName != "a")
            {
                var jsDriver = (IJavaScriptExecutor)d;
                string highlightJavascript = @"$(arguments[0]).css({ ""border-width"" : ""2px"", ""border-style"" : ""solid"", ""border-color"" : ""red"" });";
                jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
            }
        }
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



        public static string ContextInject(this string str)
        {
            var m = Regex.Match(str.Trim(), @"^\[(.*)\]$");
            if (m.Success)
            {
                var value = m.Groups[1].Value.Trim();

                var m1 = Regex.Match(value, @"(.*)=(.*)");
                if (m1.Success)
                {
                    var cxtValue = m1.Groups[2].Value.Trim().ContextInject().Replace("Random", DateTime.Now.ToString("%MddHHmmss"));
                    var cxtKey = m1.Groups[1].Value.Trim().ContextInject();
                    ScenarioContext.Current[cxtKey] = cxtValue;

                    Console.WriteLine(string.Format("[{0}={1}]", cxtKey, cxtValue));
                    return cxtValue;
                }
                else
                {
                    try
                    {
                        var cxtValue = ScenarioContext.Current[value] as string;
                        Console.WriteLine(string.Format("[{0}={1}]", value, cxtValue));
                        return cxtValue;
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new KeyNotFoundException("can't find key:" + value + " in scenario");
                    }
                }
            }
            else
            {
                return str;
            }

        }

        public static string[] ContextInject(this string[] array)
        {
            return array.Select(s => s.ContextInject()).ToArray();

        }


        public static Table ContextInject(this Table table)
        {

            foreach (var row in table.Rows)
            {
                row.ContextInject();
            }
            return table;
        }

        public static TableRow ContextInject(this TableRow row)
        {

            foreach (var k in row.Keys)
                row[k] = row[k].ContextInject();
            return row;
        }
        /// <summary>
        /// Trim and convert string to lower case for comparison
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ForCompare(this string str)
        {
            return str.Trim().ToLower();
        }

        public static bool SameString(this string str, string target)
        {
            return str.Trim().ToLower() == target.Trim().ToLower();
        }



    }
}
