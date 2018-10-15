using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Tests.Selenium.Facade;

namespace Tests.Selenium.ToscaObstacleTests.Commons
{
    [Binding]
    class GlobalHook : Steps
    {


        [BeforeScenario]
        public void BeforeScenarioHook()
        {

            //DONT COMMENT ANY OF THIS OUT. EVER!


            GivenSiteIsOpened();

        }


        [AfterScenario]
        public void AfterScenarioHook()
        {

            TestCleanup();
        }


        [Given(@"The site is opened")]
        [When(@"The site is opened")]
        [Then(@"The site is opened")]
        public void GivenSiteIsOpened()
        {

            InTheDrink session = new InTheDrink();
            session.OpenNewSession();



        }


        [Given(@"I close the site")]
        [When(@"I close the site")]
        [Then(@"I close the site")]
        public void TestCleanup()
        {

            InTheDrink.CloseSession();


        }

    }
}



