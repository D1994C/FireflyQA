using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Firefly
{
    public class FireflyQATests
    {
        //Setting the objects to be used in the test file
        FireflyXpaths xpaths = new FireflyXpaths();
        FireflyConfig config = new FireflyConfig();
        IWebDriver driver;

        [SetUp]
        public void fireflySetup()
        {
            //In setup we set the driver to be a chromedriver, send it to the login page and set an impliciate wait of 5 seconds
            driver = new ChromeDriver();
            driver.Url = "https://fireflyautotest.staging.fireflysolutions.co.uk";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        }

        [TearDown]
        public void fireflyTeardown()
        {
            //In the teardown we close the driver
            driver.Close();
        }

        [Test]
        public void exampleTest()
        {
            //Test to execute the steps required for this task.
            login(driver, config.getUname(), config.getPwd());
            Assert.IsTrue(confirmCorrectUserLoggedIn(driver, "Terry Teacher"));
            Assert.IsTrue(confirmElementIsPresent(driver, xpaths.newtaskbtn));
            navigateToTasksPage(driver);
            selectTasksFilter(driver, "All");
            Assert.IsTrue(confirmTaskInTable(driver, "Automation Test Task"));
            editTaskTitleAndConfirm(driver, "Automation Test Task-Edited", "Automation Test Task");

        }

        public void login(IWebDriver driver, string uName, string passwd)
        {
            //Login method which uses the loging vars from the config to login and assert we are at the correct URL
            IWebElement unamebox = driver.FindElement(By.XPath(xpaths.usernamebox));
            IWebElement pwdbox = driver.FindElement(By.XPath(xpaths.passwordbox));
            unamebox.SendKeys(uName);
            pwdbox.SendKeys(passwd);
            IWebElement loginBtn = driver.FindElement(By.XPath(xpaths.loginbtn));
            loginBtn.Click();
            String curUrl = driver.Url;
            Assert.AreEqual(curUrl, "https://fireflyautotest.staging.fireflysolutions.co.uk/dashboard");
        }

        public bool confirmCorrectUserLoggedIn(IWebDriver driver, string expectedUser)
        {
            //Method to confirm the user logged in is the one we expect
            bool isElementDisplayed = driver.FindElement(By.XPath("//nav[@aria-label='Userbar']//span[contains(text(),'" + expectedUser + "')]")).Displayed;
            return isElementDisplayed;
        }

        public bool confirmElementIsPresent(IWebDriver driver, string xpath)
        {
            //Quick method to confirm an element is displayed on the page
            bool isElementDisplayed = driver.FindElement(By.XPath(xpath)).Displayed;
            return isElementDisplayed;
        }

        public void navigateToTasksPage(IWebDriver driver)
        {
            //Mathod to navigate to the tasks page. Could be reusable.
            driver.FindElement(By.XPath(xpaths.alltasksbtn)).Click();
            confirmElementIsPresent(driver, xpaths.tasksTable);
        }

        public void selectTasksFilter(IWebDriver driver, string filter)
        {
            //Sets the task filter to the filter passed in.
            navigateToTasksPage(driver);
            driver.FindElement(By.XPath("//div[contains(text(),'Status')]/parent::div//input[@value='" + filter + "']")).Click();
        }

        public bool confirmTaskInTable(IWebDriver driver, string taskTitle)
        {

            //Method to loop through a tabe and confirm the element is present.
            //Set a variable to determine if the element is found
            bool found = false;

            //For loop to begin looping the table
            for (int i = 2; i < 9999; i++)
            {
                //If we found the element we set found to true and exit the for loop
                if (driver.FindElements(By.LinkText(taskTitle)).Count > 0)
                {
                    found = true;
                    break;
                }
                else {

                    //if not found we try to click the next page button. If we cannot i.e. there are no more pages we exit the loop
                    //and return found as false i.e. not found the element.
                    try
                    {
                        driver.FindElement(By.XPath("//button[contains(text(),'" + i + "')]")).Click();
                    }
                    catch
                    {
                        found = false;
                        break;

                    }  
                }
            }
            return found;

        }

        public void viewTask(IWebDriver driver, string taskTitle)
        {
            //Method to view the task and confirm the title is in the header
            driver.FindElement(By.LinkText(taskTitle)).Click();
            confirmElementIsPresent(driver,"//h1[contains(text(),'"+ taskTitle + "')]");
        }

        public void editTaskTitleAndConfirm(IWebDriver driver, string newTitle, string task)
        {
            //Edits the task title and confirms it has been set correctly
            viewTask(driver, task);
            driver.FindElement(By.XPath(xpaths.taskOverview)).Click();
            driver.FindElement(By.XPath(xpaths.taskEdit)).Click();
            IWebElement titleBox = driver.FindElement(By.Id("task.title"));

            //Here we scroll to the element as the save button overlaps it is we attempt to click without this
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", titleBox);
            System.Threading.Thread.Sleep(500);
            titleBox.Click();

            //Small loop to remove the current title as .Clear() does not work with Chromedriver and this type of table.
            //Given more time i would love to investigate further as there is probably a more effcient way to do this.
            int len = task.Length;
            for (int i = 1; i <= len; i++)
            {
                titleBox.SendKeys(Keys.Backspace);

            }
            System.Threading.Thread.Sleep(500);
            titleBox.SendKeys(newTitle);
            driver.FindElement(By.XPath(xpaths.taskEditSave)).Click();
            driver.FindElement(By.XPath(xpaths.taskEditView)).Click();
            IWebElement header = driver.FindElement(By.XPath(xpaths.taskTitle));
            //Get the new title from the header and confrim is is correct.
            Assert.AreEqual(header.Text, newTitle);
        }

       
    }
}