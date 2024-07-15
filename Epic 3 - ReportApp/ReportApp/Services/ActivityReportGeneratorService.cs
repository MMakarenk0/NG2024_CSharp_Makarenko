
using ClosedXML.Excel;
using ClosedXML.Report;
using ReportApp.Models;
using System.Text.Json;

namespace ReportApp.Services;

public class ActivityReportGeneratorService
{
    private Dictionary<string, Func<ActivityReportModel, object>> KeyValuePairs { get; set; } = new Dictionary<string, Func<ActivityReportModel, object>>
    {
        { "FirstName", r => r.ReportGeneratedFor.FirstName },
        { "LastName", r => r.ReportGeneratedFor.LastName },
        { "Day", r => r.WorkdayStartTime.ToShortDateString() },
        { "Office", r => r.Office },
        { "Additional Info", r => r.Complains }
    };
    private ActivityReportSettings Settings { get; set; }

    public XLTemplate GetReportTemplate()
    {
        return new(@"./Templates/ActivityReport.xlsx");
    }

    public ActivityReportConfiguration GetConfiguration(string path = @"./ReportConfigurations/Activity.json")
    {
        var configuration = new ActivityReportConfiguration().LoadFromFile(path);
        return configuration;
    }

    public void FillSettings(ActivityReportModel model)
    {
        Admin? generatedByAdmin = null;
        Client? generatedByClient = null;
        var generatedFor = $"{model.ReportGeneratedFor.FirstName}  {model.ReportGeneratedFor.LastName}";

        if (model.GeneratedByAdmin != null)
        {
            generatedByAdmin = model.GeneratedByAdmin;
        }
        else
        {
            generatedByClient = model.GeneratedByClient;
        }

        Settings = new ActivityReportSettings
        {
            GeneratedByAdmin = generatedByAdmin,
            GeneratedByClient = generatedByClient,
            GeneratedFor = generatedFor,
        };
    }

    public void FillHeader(XLTemplate template, ActivityReportConfiguration configuration)
    {
        if (Settings != null)
        {
            var isGeneratedByAdmin = Settings.GeneratedByAdmin != null ? true : false;
            template.AddVariable("GeneratedFor", Settings.GeneratedFor);

            if (isGeneratedByAdmin)
            {
                template.AddVariable("GeneratedBy", 
                    $"{Settings.GeneratedByAdmin.FirstName} {Settings.GeneratedByAdmin.LastName} (Admin)");
            }
            else
            {
                template.AddVariable("GeneratedBy",
                    $"{Settings.GeneratedByClient.FirstName} {Settings.GeneratedByClient.LastName} (Client)");
            }
        }
    }

    public void CleanTestData(XLTemplate template, ActivityReportConfiguration configuration, int actualLastColumn)
    {
        var worksheet = template.Workbook.Worksheets.First();
        for (int row = configuration.DefaultRow; row <= configuration.LastRow; row++)
        {
            for (int column = actualLastColumn; column <= configuration.LastColumn; column++)
            {
                worksheet.Cell(row, column).Clear();
            }
        }
    }

    public ActivityReportModel GetReportModel(Client client, Admin? admin = null)
    {
        var today = DateTime.Now;
        var checkInCheckOut = new CheckInCheckOutService()
            .CalculateWorkHours(
                client,
                new DateTime(today.Year, today.Month, today.Day, 8, 0, 0),
                DateTime.Now);

        var reportModel = new ActivityReportModel()
        {
            GeneratedByAdmin = admin,
            Office = "New York, Wallstreet",
            GeneratedByClient = null,
            WorkdayStartTime = checkInCheckOut.ClientCheckedIn,
            WorkdayEndTime = checkInCheckOut.ClientCheckedOut,
            ReportGeneratedFor = client
        };

        FillSettings(reportModel);
        
        return reportModel;
    }

    public ActivityReportModel SerializeReportModel(string path = @"./JsonExamples/ActivityReportClientGenerated.json")
    {
        var jsonContent = File.ReadAllText(path);
        var model = JsonSerializer.Deserialize<ActivityReportModel>(jsonContent);
        FillSettings(model);
        return model;
    }

    public void FillReportDataFromModel(XLTemplate template, ActivityReportConfiguration configuration, ActivityReportModel model) 
    {
        var worksheet = template.Workbook.Worksheets.First();
        worksheet.SetShowGridLines(false);

        var firstDataRow = configuration.DefaultRow;
        var firstDataColumn = configuration.FirstColumn;

        var groupAmount = 2;
        var lastDataColumn = configuration.LastColumn;

        if (model.GeneratedByAdmin == null)
        {
            groupAmount = 1;
            lastDataColumn -= 3;
            worksheet.Range(configuration.FirstRowForDynamicGroup, lastDataColumn + 1, configuration.FirstRowForDynamicGroup, configuration.LastColumn)
                .Delete(XLShiftDeletedCells.ShiftCellsLeft);
            worksheet.Range(configuration.FirstRowForStaticGroup, lastDataColumn + 1, configuration.FirstRowForStaticGroup, configuration.LastColumn)
                .Delete(XLShiftDeletedCells.ShiftCellsLeft);


            var titleCell = worksheet.Cell(configuration.ReportTitleRow, firstDataColumn);
            var style = titleCell.Style;
            var title = titleCell.Value.ToString();

            var previousRange = worksheet.Range(configuration.ReportTitleRow, firstDataColumn, configuration.ReportTitleRow, configuration.LastColumn).Unmerge();
            previousRange.Clear();

            var newRange = worksheet.Range(configuration.ReportTitleRow, firstDataColumn, configuration.ReportTitleRow, lastDataColumn).Merge();
            newRange.Style = style;
            newRange.Value = title;

            CleanTestData(template, configuration, lastDataColumn);
            if (configuration.LastColumn != lastDataColumn)
            {
                DrawBorders(worksheet, configuration, lastDataColumn);
            }
        }
        else
        {
            KeyValuePairs.Add("Name", r => r.GeneratedByAdmin.PreferedName);
            KeyValuePairs.Add("Pronouns", r => r.GeneratedByAdmin.Pronouns);
            KeyValuePairs.Add("Works At", r => r.GeneratedByAdmin.City);
        }

        for (int group = 1; group <= groupAmount; group++)
        {
            for (int row = 0; row < model.Complains.Count; row++) 
            {
                int column = firstDataColumn;
                foreach (var property in KeyValuePairs)
                {
                    if (property.Key.Equals("Additional Info"))
                    {
                        worksheet.Cell(row + firstDataRow, column).Value = model.Complains[row].Description;
                    }
                    else
                    {
                        worksheet.Cell(row + firstDataRow, column).Value = property.Value(model).ToString();
                    }
                    column++;
                }
                configuration.LastRow++;
            }
        }

        var workingRange = worksheet.Range(configuration.ReportTitleRow, firstDataColumn, configuration.LastRow, lastDataColumn);
        worksheet.Columns(configuration.FirstColumn, configuration.LastColumn).AdjustToContents();
    }

    public void DrawBorders(IXLWorksheet worksheet, ActivityReportConfiguration configuration, int actualLastColumn)
    {
        var rightBorder = worksheet.Range(configuration.ReportTitleRow, actualLastColumn, configuration.LastRow, actualLastColumn);
        
        rightBorder.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rightBorder.Style.Border.RightBorderColor = XLColor.Black;

        var bottomBorder = worksheet.Range(configuration.LastRow, configuration.FirstColumn, configuration.LastRow, actualLastColumn);

        bottomBorder.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        bottomBorder.Style.Border.BottomBorderColor = XLColor.Black;
    }

    static void ExtractKeys(JsonElement element, List<string> keys, string parentKey = "")
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in element.EnumerateObject())
            {
                string currentKey = string.IsNullOrEmpty(parentKey) ? property.Name : $"{parentKey}.{property.Name}";
                keys.Add(currentKey);
                ExtractKeys(property.Value, keys, currentKey);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement arrayElement in element.EnumerateArray())
            {
                ExtractKeys(arrayElement, keys, parentKey);
            }
        }
    }

    public void GenerateReport()
    {
        var template = GetReportTemplate();

        var client = new Client
        {
            FirstName = "John",
            LastName = "Snow",
            PreferedName = "THE TRUE KING",
            City = "Winterfell",
            Pronouns = "King/Wifey"
        };

        var admin = new Admin()
        {
            FirstName = "Soul",
            LastName = "Of Cinder",
            PreferedName = "Keeper of the flame",
            City = "Kiln Of The First Flame",
            Pronouns = "So so boss"
        };

        var model = SerializeReportModel();

        FillReportDataFromModel(template, GetConfiguration(), model);
        FillHeader(template, GetConfiguration());

        template.Generate();
        template.SaveAs("../../../Reports/Report.xlsx");
    }
}
