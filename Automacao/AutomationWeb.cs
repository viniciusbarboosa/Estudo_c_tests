using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Network;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
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

            var handler = new HttpClientHandler() { 
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

            //            element = driver.FindElement(By.XPath("//span[text()='Marcar Todos']"));
            //          element = element.FindElement(By.XPath("./ancestor::button"));
            //          element.Click();
            //          Helper.WaitForLoad(driver);

            //        element = driver.FindElement(By.XPath("//*[@id=\"ctl00\"]/div[3]/div[2]/div[2]/div[2]/div[1]/button[1]"));
            //        element.Click();
            //        Helper.WaitForLoad(driver);


            //        element = driver.FindElement(By.Id("MainContent_cphMainContent_DownloadXml_btnDownloadSelected"));
            //        element.Click();
            //       Helper.WaitForLoad(driver);

            var dataSendPost = new Dictionary<string, string>()
            {
                {"__EVENTTARGET", driver.FindElement(By.Id("__EVENTTARGET")).GetAttribute("value")??""},
                {"__EVENTARGUMENT",driver.FindElement(By.Id("__EVENTARGUMENT")).GetAttribute("value")??""},
                {"__LASTFOCUS",driver.FindElement(By.Id("__LASTFOCUS")).GetAttribute("value")??""},
                {"__VIEWSTATE", driver.FindElement(By.Id("__VIEWSTATE")).GetAttribute("value") ?? ""},
                {"__VIEWSTATEGENERATOR",driver.FindElement(By.Id("__VIEWSTATEGENERATOR")).GetAttribute("value") ?? ""},
                {"__EVENTVALIDATION",driver.FindElement(By.Id("__EVENTVALIDATION")).GetAttribute("value") ?? ""},
                {"ctl00$ctl00$Headerconsole$hddCurrentCompanyId",driver.FindElement(By.Id("hddCurrentCompanyId")).GetAttribute("value") ?? ""},
                {"ctl00$ctl00$Headerconsole$hddCompaniesToMask",driver.FindElement(By.Id("hddCompaniesToMask")).GetAttribute("value") ?? ""},
                {"ctl00$ctl00$MainContent$Menuconsole$UsageCurrentMonth1$txtMonthReport",""},
                {"ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfSelectedXml",driver.FindElement(By.Id("hdfSelectedXml")).GetAttribute("value") ?? ""},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfDownloadPermission",driver.FindElement(By.Id("hdfDownloadPermission")).GetAttribute("value") ?? ""},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$rdlOptionsOrOrganization",driver.FindElement(By.Id("MainContent_cphMainContent_DownloadXml_rdlOptionsOrOrganization_0")).GetAttribute("value") ?? ""},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$txtDayFrom",driver.FindElement(By.Id("MainContent_cphMainContent_DownloadXml_txtDayFrom")).GetAttribute("value") ?? ""},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$txtDayTo",driver.FindElement(By.Id("MainContent_cphMainContent_DownloadXml_txtDayTo")).GetAttribute("value")??""},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfDownloadXmlFileUrlV2",""},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$btnDownloadSelected","Confirmar"},
                {"ctl00$ctl00$MainContent$cphMainContent$ListXmlFile$Tags$hddTagsbyKey",""},
                {"ctl00$ctl00$MainContent$cphMainContent$ListXmlFile$ddlOrder_Xml","1"},
                {"ctl00$ctl00$MainContent$cphMainContent$UploadXml$fileUploadXmlCofre",""}
            };

            //Cookie cookie = driver.Manage().Cookies.GetCookieNamed("COFRE.AUTH");
            //Console.WriteLine(cookie.Value);
            //var cookie = driver.Manage().Cookies.GetCookieNamed("_ga").Value;
            //Console.WriteLine(cookie);
            var cookies = driver.Manage().Cookies.AllCookies;
            string allCookiesString = string.Join("; ", cookies.Select(c => $"{c.Name}={c.Value}"));
            //Console.WriteLine(allCookiesString);

            client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            client.DefaultRequestHeaders.Add("accept-language", "nl");
            client.DefaultRequestHeaders.Add("cache-control", "max-age=0");
            client.DefaultRequestHeaders.Add("origin", "https://cofre.sieg.com");
            client.DefaultRequestHeaders.Add("priority", "u=0, i");
            client.DefaultRequestHeaders.Add("referer", "https://cofre.sieg.com/lista-xml?cnpjemit=20391248000138&year=2026&month=1&ordertype=1");
            client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"146\", \"Not-A.Brand\";v=\"24\", \"Google Chrome\";v=\"146\"");
            client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            client.DefaultRequestHeaders.Add("sec-fetch-dest", "document");
            client.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
            client.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
            client.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");
            var cookieValue = allCookiesString;
            client.DefaultRequestHeaders.Add("Cookie", cookieValue);
            //127.0.0.1:8888
            var content = new FormUrlEncodedContent(dataSendPost);
            var response = await client.PostAsync("https://cofre.sieg.com/lista-xml?cnpjemit=20391248000138&year=2026&month=1&ordertype=1", content);
            
          // Console.WriteLine($"conteudo da response {await response.Content.ReadAsStringAsync()}");
            Console.WriteLine(response.StatusCode);
        
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"ERRO NA API{response.StatusCode} - {errorContent}");
            }


            byte[] zipFileData = await response.Content.ReadAsByteArrayAsync();
            await System.IO.File.WriteAllBytesAsync("notas_fiscais.zip", zipFileData);

            string zipPath = "notas_fiscais.zip";
            string extractPath = "./xml_extraidos";

            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            try
            {
                ZipFile.ExtractToDirectory(zipPath, extractPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            

        }

    }
}
