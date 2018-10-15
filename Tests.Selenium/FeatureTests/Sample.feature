@EnterFeatureTagHere
Feature: Sample Feature
Enter your Feature purpose here

##Enter Scenarios here##

@EnterScenarioTagHere


	Scenario: Navigate from home to acout us and do a search
	Given I am on the home page
	And I navigate to problem page
	And I am on the problem page
	And I navigate to about us page
	And I am on the about us page
	And I navigate to home page

	And I search for [STerm=SearchRandom] search term
	And I verify searched term [STerm] is displayed

	Scenario Outline: Navigate Using Examples
	Given I am on the home page
	And I navigate to <page> page
	And I am on the <onpage> page

	And I search for <term> search term
	And I verify searched term [term] is displayed

	Examples: 
        | page     | onpage   | term             |
        | problem  | problem  | [term=Random]    |
        | about us | about us | [term=abcRandom] |