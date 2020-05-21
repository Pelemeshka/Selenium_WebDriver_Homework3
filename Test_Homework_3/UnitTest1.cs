using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Reflection.Emit;
using System.Threading;

namespace Test_Homework_3
{
    public class Tests
    {
        public IWebDriver driver;
        private WebDriverWait wait;

        [OneTimeSetUp]      //�������� �������� ����� ����������� ���� ������.
        public void BeforeTestSuit()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http:\\localhost:5000");
            driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]       //�������� �������� ����� ���������� ���� ������.
        public void AfterTestSuit()
        {
            driver.Close();
            driver.Quit();
        }

        [SetUp]     //�������� � ������ ������� �����.
        public void BeforeTest()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TearDown]          //�������� � ����� ������� �����.
        public void AfterTest()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }
        [Test]
        public void Test1_Login()   //���� �� �����.
        {
            driver.FindElement(By.Id("Name")).SendKeys("user");
            driver.FindElement(By.Id("Password")).SendKeys("user");
            driver.FindElement(By.CssSelector(".btn")).Click();

            // �������� �� �������� �������� Home page ����� ������.
            Assert.AreEqual("Home page", driver.FindElement(By.XPath("//h2[contains(.,'Home page')]")).Text);
        }

        [Test]
        public void Test2_CreateProduct()   // ��������� ������ ��������.
        {
            driver.FindElement(By.LinkText("All Products")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.LinkText("Create new")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.Id("ProductName")).SendKeys("Cake pops");

            var selectElement = new SelectElement(driver.FindElement(By.Id("CategoryId")));
            //Thread.Sleep(2000);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            selectElement.SelectByText("Confections");
            selectElement = new SelectElement(driver.FindElement(By.Id("SupplierId")));
            //Thread.Sleep(2000);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            selectElement.SelectByText("Specialty Biscuits, Ltd.");
            driver.FindElement(By.Id("UnitPrice")).SendKeys("15");
            driver.FindElement(By.Id("QuantityPerUnit")).SendKeys("2 boxes x 7 pieces");
            driver.FindElement(By.Id("UnitsInStock")).SendKeys("20");
            driver.FindElement(By.Id("UnitsOnOrder")).SendKeys("0");
            driver.FindElement(By.Id("ReorderLevel")).SendKeys("10");
            driver.FindElement(By.CssSelector(".btn")).Click();

            // �������� �� �������� �������� All Products ����� ������� ������ "���������" �������� Create New.
            Assert.AreEqual("All Products", driver.FindElement(By.XPath("//h2[contains(.,'All Products')]")).Text);
        }

        [Test]
        public void Test3_ChekProduct()      // �������� ����� �������� ����� ��� ��������.
        {
            driver.FindElement(By.LinkText("Cake pops")).Click();
            //Thread.Sleep(2000);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Assert.AreEqual("Cake pops", driver.FindElement(By.Id("ProductName")).GetAttribute("value"));
            Assert.AreEqual("Confections", driver.FindElement(By.XPath("//*[@id=\"CategoryId\"]/option[@selected=\"selected\"]")).Text);
            Assert.AreEqual("Specialty Biscuits, Ltd.", driver.FindElement(By.XPath("//*[@id=\"SupplierId\"]/option[@selected=\"selected\"]")).Text);
            Assert.AreEqual("15,0000", driver.FindElement(By.Id("UnitPrice")).GetAttribute("value"));
            Assert.AreEqual("2 boxes x 7 pieces", driver.FindElement(By.Id("QuantityPerUnit")).GetAttribute("value"));
            Assert.AreEqual("20", driver.FindElement(By.Id("UnitsInStock")).GetAttribute("value"));
            Assert.AreEqual("0", driver.FindElement(By.Id("UnitsOnOrder")).GetAttribute("value"));
            Assert.AreEqual("10", driver.FindElement(By.Id("ReorderLevel")).GetAttribute("value"));

            driver.FindElement(By.Id("UnitPrice")).Clear();
            driver.FindElement(By.Id("UnitPrice")).SendKeys("15");// ���������� � ���� ����, ��� ��� ���������� ������ � ������� ������
            driver.FindElement(By.CssSelector(".btn")).Click();
            //Thread.Sleep(2000);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // �������� �� �������� �������� All Products ����� ������� ������ "���������" �������� Create New.
            Assert.AreEqual("All Products", driver.FindElement(By.XPath("//h2[contains(.,'All Products')]")).Text);
        }        

        [Test]
        [Ignore("No end")]      // ���� �� ���������� ������� ������� Enter ������������ ����
        public void Test4_DeleteProduct()      // �������� �������� ��������.
        {
            driver.FindElement(By.XPath("//a[contains(text(),'Cake pops')]/../..//a[contains(text(),'Remove')]")).Click();
            Thread.Sleep(5000);
            //driver.SwitchTo().Alert().SendKeys(Keys.Enter);
            new Actions(driver).SendKeys(Keys.Enter).Build().Perform();
            Thread.Sleep(5000);

            //�������� �� ����������� �������� Cake pops � Id=ProductName.
            Assert.AreNotEqual("Cake pops", driver.FindElement(By.Id("ProductName")).Text);
        }

        [Test]
        public void Test5_Logout()      // �������� �� ������.
        {
            driver.FindElement(By.LinkText("Logout")).Click();

            // �������� �� �������� �������� Login  ����� ������� ������ Logout.
            Assert.AreEqual("Login", driver.FindElement(By.XPath("/html/body/div[1]/h2")).Text);
        }
        


    }
}