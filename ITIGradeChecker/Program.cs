using Microsoft.Playwright;

Console.WriteLine("Hello, ITI !");

using var playwright = await Playwright.CreateAsync();

await using var browser = await playwright.Chromium.LaunchAsync(new()
{
    Channel = "msedge",
#if DEBUG
    Headless = false
#endif
});

var page = await browser.NewPageAsync();
await page.GotoAsync("http://apps.iti.gov.eg/ManagementSystem/intlogin.aspx");
await page.GetByPlaceholder("Your username").ClickAsync();
await page.GetByPlaceholder("Your username").FillAsync("YO35619");
await page.GetByPlaceholder("Password").ClickAsync();
await page.GetByPlaceholder("Password").FillAsync("iti4");
await page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
await page.GotoAsync("http://apps.iti.gov.eg/ManagementSystem/Student/StudentGrade.aspx");
var pageContent = await page.ContentAsync();

Console.ReadLine();