using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.Reflection.Emit;
using System.Text;

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
            driver = new ChromeDriver(chromeOptions);


            var dataSetHeader = new Dictionary<string, string>()
            {
                {"accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7"},
                {"accept-language","nl"},
                {"cache-control","max-age=0"},
                {"origin","https://cofre.sieg.com"},
                {"priority","u=0, i"},
                {"referer","https://cofre.sieg.com/lista-xml?cnpjemit=20391248000138&year=2026&month=1&ordertype=1"},
                {"sec-ch-ua","\"Chromium\";v=\"146\", \"Not-A.Brand\";v=\"24\", \"Google Chrome\";v=\"146\""},
                {"sec-ch-ua-mobile","?0"},
                {"sec-ch-ua-platform","\"Windows\""},
                {"sec-fetch-dest","document"},
                {"sec-fetch-mode","navigate"},
                {"sec-fetch-site","same-origin"},
                {"upgrade-insecure-requests","1"},
                {"user-agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/146.0.0.0 Safari/537.36"},
                {"Cookie","[{\"key\":\"Cookie\",\"value\":\"_ga=GA1.2.2025178744.1774455373; _gid=GA1.2.386459544.1774455373; COFRE.AUTH=ae7bad81-e2e0-49ba-826d-353b8d079360; _sleek_session=%7B%22init%22%3A%222026-03-25T16%3A16%3A20.210Z%22%7D; _hjSession_3570950=eyJpZCI6ImI3ZGE0MDg2LTQ1OTgtNDJjOC1iMTE3LTkyMDM5YmM0ZGVkOCIsImMiOjE3NzQ0NTUzODA1MjUsInMiOjAsInIiOjAsInNiIjowLCJzciI6MCwic2UiOjAsImZzIjoxLCJzcCI6MH0=; _clck=uqjle0%5E2%5Eg4n%5E0%5E2275; _sleek_product=%7B%22token%22%3A%227679684321f19c33ff9ad5fe2d5c41cfacd624cfa%22%2C%22user_data%22%3A%7B%22user_id%22%3A11303973%2C%22admin_id%22%3A0%2C%22sso%22%3Atrue%2C%22anonymous%22%3Afalse%2C%22data_name%22%3A%22vincius_barbosa%22%2C%22data_full_name%22%3A%22%22%2C%22data_mail%22%3A%22vinicius.barbosa%40sieg.com%22%2C%22data_img%22%3A%22https%3A%2F%2Fstorage.sleekplan.com%2Fstatic%2Fimage%2Fuser.png%22%2C%22segments%22%3A%5B%22pe-e-al672e14311b256%22%2C%22clientes-hub-e-iris672e0a1f908d3%22%2C%22planos672e06590fd4c%22%2C%22time-sieg672cb379445dc%22%5D%2C%22notify%22%3A1%2C%22notify_settings%22%3A%7B%22mention%22%3Atrue%2C%22changelog%22%3Atrue%2C%22subscribed%22%3Atrue%7D%7D%7D; __AntiXsrfToken=0e56b46135e94ace8f0e51787117b0a5; _cx.tracking.apikey=1c065310ebbb7dc39a79b506ddbf4d38; _cx.tracking.external_id_client=11; _cx.tracking.email=vinicius.barbosa@sieg.com; _cx.survey.status=no-survey-found; darkModeCookie=Off; chatWidgetWindowState0BB3ECA1AB45443CBCF7AFF2F1834F1B=false; _BEAMER_FILTER_BY_URL_fxzWhwyU5092=false; _hjSession_3570964=eyJpZCI6IjEzNmM4NWRkLTlkNjctNDhhYy1hMjY3LTAxOWJmNGFjZjA5ZiIsImMiOjE3NzQ0NTUzOTQxNzUsInMiOjAsInIiOjAsInNiIjowLCJzciI6MCwic2UiOjAsImZzIjoxLCJzcCI6MX0=; _hjSessionUser_3570950=eyJpZCI6IjIwOGRjODEzLWU3MDctNWMwNS1hYzFiLTNlY2Y0YmE2NzgzMiIsImNyZWF0ZWQiOjE3NzQ0NTUzODA1MjQsImV4aXN0aW5nIjp0cnVlfQ==; _hjSessionUser_3570964=eyJpZCI6IjRkNDAzYjVhLWY5MTItNWM2NS05YjFiLWRlYjM0ZjFhNjBlZCIsImNyZWF0ZWQiOjE3NzQ0NTUzOTQxNzQsImV4aXN0aW5nIjp0cnVlfQ==; modalNps=4; AWSALB=7DR17OfYxqK3JQegEuNV+79ck4rokcht/LmEwgiR/p+Spb61eMJT8Ww4Qkki93KsXUJyjtgUkPYuWUUEBq2+5A5ckPkZRLr+w+HjNHLzUWJfeYbuwTLpv6kYQwkE; AWSALBCORS=7DR17OfYxqK3JQegEuNV+79ck4rokcht/LmEwgiR/p+Spb61eMJT8Ww4Qkki93KsXUJyjtgUkPYuWUUEBq2+5A5ckPkZRLr+w+HjNHLzUWJfeYbuwTLpv6kYQwkE; _ga_0XCNMGZ1WP=GS2.2.s1774455380$o1$g1$t1774455622$j11$l0$h0; _clsk=a0n7xl%5E1774455624006%5E8%5E1%5Eq.clarity.ms%2Fcollect\",\"enabled\":true}]"}
            };

            client = new HttpClient();
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
                {"__EVENTTARGET",""},
                {"__EVENTARGUMENT",""},
                {"__LASTFOCUS",""},
                {"__VIEWSTATE","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"},
                {"__VIEWSTATEGENERATOR","8AD95B3C"},
                {"__EVENTVALIDATION","aoP4Jo/P/RNwrL3fuiOILdz1D6hecQm/mLKLPTrmVzx70KmyEvOGbHiKZI62BBkqIU+tSPGGj/EizOhGXjhoVCrPPUo8oo71vBzH/2ucZz+zi7bUj5R4+SPTq9FoqbM0p14R1ewyEC4BqKKuKE23gz8PwEO6bs4um5IIZFDsH1Cx4dZmEuT8PY4tEsjqy2B1LhnasBUVF7Pg3FVzcSDnQbMDpoxixBRmaL6atwxUnPHqBY9KOJ3PYTaaBuf16RkzcB7m2UzPZpHW85wRWtalnjDbfoGT4C0ZtEZ553gAF6Igp9TdTywz+bo8XZGiNDgTGobG99aG+MNce9AGug/i3BI/SKS4dIGYTDxAMPHEDK1SmuLZ30UpevOMZva9X+7eiwsiynOCC/3xpHjVMxhluOpOKrIPN27fv+l+T0mPVjxW9WzapGn91DKNfFAZLbFLNGNudvSRHAhI3CZusFmR3ze34PwlVeevcTM2KSGGRG3n7jVjQP+bWWGnmHt3CGqOku7zjShe/1p0A5ppBlYnHbeO29raEAu+zUbJzogncDy1kKdwecDInVn1YhDChz2cTvop6J7eF9ySdn5s6bY32HNVYEsoOpdi1IcB/l2BIypLADTkIXjKlO+oHPv7tY/I"},
                {"ctl00$ctl00$Headerconsole$hddCurrentCompanyId","11"},
                {"ctl00$ctl00$Headerconsole$hddCompaniesToMask","[\"11\",\"24464\"]"},
                {"ctl00$ctl00$MainContent$Menuconsole$UsageCurrentMonth1$txtMonthReport",""},
                {"ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfSelectedXml","33260120391248000138550010000012341852912866,2026,01,06,56551061915,20391248000138,\r\n                                \r\n                                \r\n                                    1234\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012351675351694,2026,01,07,56551061915,20391248000138,\r\n                                \r\n                                \r\n                                    1235\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012361560349309,2026,01,07,56551061915,20391248000138,\r\n                                \r\n                                \r\n                                    1236\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012371695149178,2026,01,07,56551061915,20391248000138,\r\n                                \r\n                                \r\n                                    1237\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012381603181344,2026,01,07,08986321000119,20391248000138,\r\n                                \r\n                                \r\n                                    1238\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012391932498817,2026,01,09,35189811000167,20391248000138,\r\n                                \r\n                                \r\n                                    1239\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012401327347075,2026,01,09,25203135000185,20391248000138,\r\n                                \r\n                                \r\n                                    1240\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012411152298760,2026,01,13,28580065004836,20391248000138,\r\n                                \r\n                                \r\n                                    1241\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012421920739518,2026,01,13,25203135000185,20391248000138,\r\n                                \r\n                                \r\n                                    1242\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012431756846539,2026,01,16,51006299000122,20391248000138,\r\n                                \r\n                                \r\n                                    1243\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012441076785068,2026,01,19,50567288000744,20391248000138,\r\n                                \r\n                                \r\n                                    1244\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012451195079679,2026,01,19,08986321000119,20391248000138,\r\n                                \r\n                                \r\n                                    1245\r\n                                \r\n                            ,undefined,undefined,;33260120391248000138550010000012461159765867,2026,01,19,62628974000171,20391248000138,\r\n                                \r\n                                \r\n                                    1246\r\n                                \r\n                            ,undefined,undefined,;"},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfDownloadPermission","true"},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$rdlOptionsOrOrganization\"\"","5"},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$txtDayFrom","1"},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$txtDayTo","31"},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$hdfDownloadXmlFileUrlV2",""},
                { "ctl00$ctl00$MainContent$cphMainContent$DownloadXml$btnDownloadSelected","Confirmar"},
                {"ctl00$ctl00$MainContent$cphMainContent$ListXmlFile$Tags$hddTagsbyKey",""},
                {"ctl00$ctl00$MainContent$cphMainContent$ListXmlFile$ddlOrder_Xml","1"},
                {"ctl00$ctl00$MainContent$cphMainContent$UploadXml$fileUploadXmlCofre",""}
            };

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
            var cookieValue = "_ga=GA1.2.284433704.1774461175; _gid=GA1.2.1005984474.1774461175; COFRE.AUTH=c96c4614-e540-4432-8970-feec3c5ab46f; _sleek_session=%7B%22init%22%3A%222026-03-25T17%3A53%3A07.465Z%22%7D; _hjSessionUser_3570950=eyJpZCI6IjQzZDk0NmYyLTRkMmMtNTMzZS05OWUzLTk3NjliZmQ0NjNkNSIsImNyZWF0ZWQiOjE3NzQ0NjExODgzNTQsImV4aXN0aW5nIjpmYWxzZX0=; _hjSession_3570950=eyJpZCI6IjdmOGQzMzUxLWY4NDktNDZmNS1iODM5LThhOGU1YzM4ZWNhNiIsImMiOjE3NzQ0NjExODgzNTUsInMiOjAsInIiOjAsInNiIjowLCJzciI6MCwic2UiOjAsImZzIjoxLCJzcCI6MH0=; _clck=4it31u%5E2%5Eg4n%5E0%5E2275; _sleek_product=%7B%22token%22%3A%227679684321f19c33ff9ad5fe2d5c41cfacd624cfa%22%2C%22user_data%22%3A%7B%22user_id%22%3A11303973%2C%22admin_id%22%3A0%2C%22sso%22%3Atrue%2C%22anonymous%22%3Afalse%2C%22data_name%22%3A%22vincius_barbosa%22%2C%22data_full_name%22%3A%22%22%2C%22data_mail%22%3A%22vinicius.barbosa%40sieg.com%22%2C%22data_img%22%3A%22https%3A%2F%2Fstorage.sleekplan.com%2Fstatic%2Fimage%2Fuser.png%22%2C%22segments%22%3A%5B%22pe-e-al672e14311b256%22%2C%22clientes-hub-e-iris672e0a1f908d3%22%2C%22planos672e06590fd4c%22%2C%22time-sieg672cb379445dc%22%5D%2C%22notify%22%3A1%2C%22notify_settings%22%3A%7B%22mention%22%3Atrue%2C%22changelog%22%3Atrue%2C%22subscribed%22%3Atrue%7D%7D%7D; __AntiXsrfToken=be7a58d1cfb14e64b5b831b22e8dc0dd; modalNps=1; _cx.tracking.apikey=1c065310ebbb7dc39a79b506ddbf4d38; _cx.tracking.external_id_client=11; _cx.tracking.email=vinicius.barbosa@sieg.com; _cx.survey.authorization=533ae8fd4a14baee8614bd9c28445871; _cx.survey.identify={\"external_id_client\":\"11\",\"email\":\"vinicius.barbosa@sieg.com\"}; _ga_0XCNMGZ1WP=GS2.2.s1774461188$o1$g1$t1774461196$j52$l0$h0; _cx.survey.status=no-survey-found; AWSALB=jMsciLfPnrn3WEDEXeVn4rvcmxuA0gd34IqltcAcbOI6E5sfx35niZYoDlHIabH+EozHZRrK6iH47Qu7cSVwJ5eKbT6UuYxHvhf+U5fpo1u1l6Oy6xmch6RYFq24; AWSALBCORS=jMsciLfPnrn3WEDEXeVn4rvcmxuA0gd34IqltcAcbOI6E5sfx35niZYoDlHIabH+EozHZRrK6iH47Qu7cSVwJ5eKbT6UuYxHvhf+U5fpo1u1l6Oy6xmch6RYFq24; darkModeCookie=Off; chatWidgetWindowState0BB3ECA1AB45443CBCF7AFF2F1834F1B=false; _hjSessionUser_3570964=eyJpZCI6IjlmZjIyZjAzLThjMzQtNTkzZi04ZDdkLWMyZjliNGY3NjM1YyIsImNyZWF0ZWQiOjE3NzQ0NjEyMDE3MDYsImV4aXN0aW5nIjpmYWxzZX0=; _hjSession_3570964=eyJpZCI6Ijc0ZDM5NjQyLWNiYTMtNDZmOS04MGYzLWIyMmUwMmUyNDMxNiIsImMiOjE3NzQ0NjEyMDE3MDcsInMiOjAsInIiOjAsInNiIjowLCJzciI6MCwic2UiOjAsImZzIjoxLCJzcCI6MH0=; _BEAMER_FILTER_BY_URL_fxzWhwyU5092=false; _clsk=1imsiw2%5E1774461202052%5E2%5E1%5Eq.clarity.ms%2Fcollect";
            client.DefaultRequestHeaders.Add("Cookie", cookieValue);

            var content = new FormUrlEncodedContent(dataSendPost);
            var response = await client.PostAsync("https://cofre.sieg.com/lista-xml?cnpjemit=20391248000138&year=2026&month=1&ordertype=1", content);
            
            Console.WriteLine($"conteudo da response {response.Content}");
            Console.WriteLine(response.StatusCode);

            byte[] zipFileData = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync("notas_fiscais.zip", zipFileData);

            string zipPath = "notas_fiscais.zip";
            string extractPath = "./xml_extraidos";

            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }
                
            //ZipFile.ExtractToDirectory(zipPath, extractPath);

            driver.Navigate().GoToUrl($"https://hub.sieg.com/");
            Helper.WaitForLoad(driver);


            driver.Navigate().GoToUrl($"https://hub.sieg.com/");
            Helper.WaitForLoad(driver);

        }

    }
}
