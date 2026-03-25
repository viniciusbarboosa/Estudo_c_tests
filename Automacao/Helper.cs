using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Automacao
{
    public static class Helper
    {
        public static void WaitForLoad(this IWebDriver driver, int timeoutSec = 15, int timeoutMin = 0)
        {
            var timeOut = timeoutMin > 0 ? TimeSpan.FromMinutes(timeoutMin) : TimeSpan.FromSeconds(timeoutSec);

            var js = (IJavaScriptExecutor)driver;
            var wait = new WebDriverWait(driver, timeOut);
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }
    }
}
