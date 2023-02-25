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

Console.Write("Enter your username: ");
string username = Console.ReadLine()!;
Console.Write("Enter your password: ");
string password = Console.ReadLine()!;

var page = await browser.NewPageAsync();
Console.WriteLine("Logging in...");
await page.GotoAsync("http://apps.iti.gov.eg/ManagementSystem/intlogin.aspx");
await page.GetByPlaceholder("Your username").ClickAsync();
await page.GetByPlaceholder("Your username").FillAsync(username);
await page.GetByPlaceholder("Password").ClickAsync();
await page.GetByPlaceholder("Password").FillAsync(password);
await page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
Console.WriteLine("Logged in...");
await page.GotoAsync("http://apps.iti.gov.eg/ManagementSystem/Student/StudentGrade.aspx");

Console.WriteLine("Scrapping your grades. Hold on...\n");
var pageContent = await page.ContentAsync();
var table = page.Locator("#ContentPlaceHolder1_UcViewGrade1_UpdatePanel1 div").Nth(1)
    .Locator("#ContentPlaceHolder1_UcViewGrade1_GrdViewcourseEval > tbody").Nth(0)
    .Locator("td");

var tableRows = await table.ElementHandlesAsync();

Dictionary<string, string> grades = new(10);
string currentCourse = string.Empty;

for (int i = 0; i < tableRows.Count; i++)
{
    string textScrapped = await tableRows[i].InnerHTMLAsync();

    if (textScrapped.Length > 1)
    {
        grades.Add(textScrapped, string.Empty);
        currentCourse = textScrapped;
    }
    else
    {
        grades[currentCourse] = textScrapped;
        Console.WriteLine($"{currentCourse}: {grades[currentCourse]}");
    }
}

Console.WriteLine("\nHave a nice Day...");
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();