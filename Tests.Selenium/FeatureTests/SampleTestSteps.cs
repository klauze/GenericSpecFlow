using Panviva.SPUI.Selenium.Project.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Tests.Selenium.Facade;
using Tests.Selenium.ToscaObstacleTests.Commons;

namespace Tests.Selenium.FeatureTests
{
    [Binding]
    public sealed class SampleTestSteps
    {


        //Note Step Definition includes all GIVEN WHEN THEN phrases. Alternatively you can add 3 lines


        [StepDefinition(@"I am on the (.+) page")]
        public void GivenIAmOnTheHomePage(String URL)
        {
             InTheDrink.HomePage.AssertCurrentURL(URL);
        }

        [StepDefinition(@"I navigate to (.+) page")]
        public void GivenINavigatetoPage(String Page)
        {
             InTheDrink.HomePage.NavigateToPage(Page);
        }

        [StepDefinition(@"I search for (.+) search term")]
        public void GivenISearchForTest(String SearchTerm)
        {
            SearchTerm = SearchTerm.ContextInject();
            InTheDrink.HomePage.SearchForTerm(SearchTerm);
        }

        [StepDefinition(@"I verify searched term (.+) is displayed")]
        public void GivenIVerifySearchedTermTestIsDisplayed(String Term)
        {
            Term = Term.ContextInject();
            InTheDrink.HomePage.VerifySearchedTermDisplayed(Term);
        }


    }
}
