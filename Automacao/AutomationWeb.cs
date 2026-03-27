using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Network;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.IO.Compression;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Automacao
{
    public class AutomationWeb
    {
        public IWebDriver driver;
        private HttpClient client;

        public AutomationWeb()
        {
            var chromeOptions = new ChromeOptions();
            var fileDirectory = "C:\\Users\\Sieg\\Documents";
            chromeOptions.AddUserProfilePreference("download.default_directory", fileDirectory);
            chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

            var cookieContainer = new CookieContainer();
            var proxy = new WebProxy
            {
                Address = new Uri("http://127.0.0.1:8888"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false
            };

            var handler = new HttpClientHandler()
            {
                Proxy = proxy,
                UseProxy = true,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            driver = new ChromeDriver(chromeOptions);
            client = new HttpClient(handler);
        }

        public async Task SearchNotesWeb()
        {
            Actions actions = new Actions(driver);
            IWebElement element;

            driver.Navigate().GoToUrl("https://auth.sieg.com/login?ReturnUrl=https%3a%2f%2fhub.sieg.com%2fdefault.aspx");

            driver.FindElement(By.Id("txtEmail")).SendKeys("");
            driver.FindElement(By.Id("txtPassword")).SendKeys("");
            driver.FindElement(By.XPath("//*[@id=\"btnSubmit\"]")).Click();
            Helper.WaitForLoad(driver);
            driver.FindElement(By.XPath("//*[@id=\"navItems\"]/button")).Click();
            Helper.WaitForLoad(driver);

            string year = "2026";
            string month = "1";
            string cnpj = "20391248000138";

            driver.Navigate().GoToUrl($"https://cofre.sieg.com/lista-xml?cnpjemit={cnpj}&year={year}&month={month}&ordertype=1");
            Helper.WaitForLoad(driver);

            element = driver.FindElement(By.Id("table_info"));

            if (element.Text == "Mostrando 0 até 0 de 0")
            {
                Console.WriteLine("Não possui NOTAS");
                driver.Navigate().GoToUrl($"https://hub.sieg.com/");
                Helper.WaitForLoad(driver);
                return;
            }

            element = driver.FindElement(By.XPath("//span[text()='Marcar Todos']"));
            element = element.FindElement(By.XPath("./ancestor::button"));
            element.Click();
            Helper.WaitForLoad(driver);

            //        element = driver.FindElement(By.XPath("//*[@id=\"ctl00\"]/div[3]/div[2]/div[2]/div[2]/div[1]/button[1]"));
            //        element.Click();
            //        Helper.WaitForLoad(driver);


            //        element = driver.FindElement(By.Id("MainContent_cphMainContent_DownloadXml_btnDownloadSelected"));
            //        element.Click();
            //       Helper.WaitForLoad(driver);

            //Cookie cookie = driver.Manage().Cookies.GetCookieNamed("COFRE.AUTH");
            //Console.WriteLine(cookie.Value);
            //var cookie = driver.Manage().Cookies.GetCookieNamed("_ga").Value;
            //Console.WriteLine(cookie);

            var cookies = driver.Manage().Cookies.AllCookies;
            string cookieString = string.Join("; ", cookies.Select(c => $"{c.Name}={c.Value}"));
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            client.DefaultRequestHeaders.Add("accept-language", "nl");
            client.DefaultRequestHeaders.Add("origin", "https://cofre.sieg.com");
            client.DefaultRequestHeaders.Add("referer", driver.Url);
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/146.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Cookie", cookieString);

            //ESTUDAR E SE APROFUNDAR AQUI NA PARTE DE MONTAR MULTIPART
            using var content = new MultipartFormDataContent("----WebKitFormBoundary35QFqHJ3XCYhoJFw");
            content.Add(new StringContent(driver.FindElement(By.Id("__EVENTTARGET")).GetAttribute("value") ?? ""), "__EVENTTARGET");
            content.Add(new StringContent(driver.FindElement(By.Id("__EVENTARGUMENT")).GetAttribute("value") ?? ""), "__EVENTARGUMENT");
            content.Add(new StringContent(driver.FindElement(By.Id("__LASTFOCUS")).GetAttribute("value") ?? ""), "__LASTFOCUS");
            content.Add(new StringContent(driver.FindElement(By.Id("__VIEWSTATE")).GetAttribute("value") ?? ""), "__VIEWSTATE");
            content.Add(new StringContent(driver.FindElement(By.Id("__VIEWSTATEGENERATOR")).GetAttribute("value") ?? ""), "__VIEWSTATEGENERATOR");
            content.Add(new StringContent(driver.FindElement(By.Id("__EVENTVALIDATION")).GetAttribute("value") ?? ""), "__EVENTVALIDATION");
            content.Add(new StringContent("11"), "ctl00$ctl00$Headerconsole$hddCurrentCompanyId");
            content.Add(new StringContent("[\"11\",\"24464\"]"), "ctl00$ctl00$Headerconsole$hddCompaniesToMask");
            content.Add(new StringContent(""), "ctl00$ctl00$MainContent$Menuconsole$UsageCurrentMonth1$txtMonthReport");

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("GetSelected();");
            string keyNotes = (string)js.ExecuteScript("return document.getElementById('hdfSelectedXml').value;");
      

            Console.WriteLine("NOTAS STRING AQUI ?" + keyNotes);
            content.Add(new StringContent(keyNotes ?? ""), "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfSelectedXml");

            content.Add(new StringContent("true"), "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfDownloadPermission");
            content.Add(new StringContent("5"), "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$rdlOptionsOrOrganization");
            content.Add(new StringContent("1"), "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$txtDayFrom");
            content.Add(new StringContent("31"), "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$txtDayTo");
            content.Add(new StringContent(""), "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfDownloadXmlFileUrlV2");
            content.Add(new StringContent("Confirmar"), "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$btnDownloadSelected");
            content.Add(new StringContent(""), "ctl00$ctl00$MainContent$cphMainContent$ListXmlFile$Tags$hddTagsbyKey");
            content.Add(new StringContent("1"), "ctl00$ctl00$MainContent$cphMainContent$ListXmlFile$ddlOrder_Xml");

            //esse campo passa vazio na req 
            var fileContent = new ByteArrayContent(Array.Empty<byte>());
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            fileContent.Headers.Add("Content-Disposition", "form-data; name=\"ctl00$ctl00$MainContent$cphMainContent$UploadXml$fileUploadXmlCofre\"; filename=\"\"");
            content.Add(fileContent);


            var response = await client.PostAsync(driver.Url, content);

            if (response.IsSuccessStatusCode)
            {
                byte[] zipFileData = await response.Content.ReadAsByteArrayAsync();
                await System.IO.File.WriteAllBytesAsync("notas_fiscais.zip", zipFileData);
                Console.WriteLine("Zip criado");
                
            }
            else
            {
                Console.WriteLine($"Erro na busca da nf");
            }

        }

    }
}
