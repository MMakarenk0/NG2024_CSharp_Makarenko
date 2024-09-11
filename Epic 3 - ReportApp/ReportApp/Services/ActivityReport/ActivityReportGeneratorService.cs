using ClosedXML.Excel;
using ReportApp.Models.ActivityReport;
using ReportApp.Models.Entity;

namespace ReportApp.Services.ActivityReport
{
    public class ActivityReportGeneratorService : ReportGeneratorBase
    {
        private const string templatePath = "./Templates/ActivityReportTemplate.xlsx";
        private const string jsonPath = "./JsonExamples/ActivityReportAdminGenerated.json";
        private const string configPath = "./ReportConfigurations/Activity.json";

        private readonly ActivityReportModel model;
        private readonly ActivityReportConfiguration configuration;
        private Dictionary<string, Func<ActivityReportModel, object>> KeyValuePairs { get; set; }
        private ActivityReportSettings Settings { get; set; }
        public ActivityReportGeneratorService(
            TemplateLoader templateLoader,
            ConfigurationLoader configurationLoader,
            ReportSerializer reportSerializer)
            : base(templateLoader, configurationLoader, reportSerializer)
        {
            InitializeKeyValuePairs();
            template = templateLoader.LoadTemplate(templatePath);
            model = reportSerializer.DeserializeReportModel<ActivityReportModel>(jsonPath);
            configuration = configurationLoader.LoadConfiguration<ActivityReportConfiguration>(configPath);
        }
        private void InitializeKeyValuePairs()
        {
            KeyValuePairs = new Dictionary<string, Func<ActivityReportModel, object>>
            {
                { "FirstName", r => r.ReportGeneratedFor.FirstName },
                { "LastName", r => r.ReportGeneratedFor.LastName },
                { "Day", r => r.WorkdayStartTime.ToShortDateString() },
                { "Office", r => r.Office },
                { "Additional Info", r => r.Complains }
            };
        }

        protected override void FillSettings()
        {
            Admin? generatedByAdmin = null;
            Client? generatedByClient = null;
            var generatedFor = $"{model.ReportGeneratedFor.FirstName} {model.ReportGeneratedFor.LastName}";

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

        protected override void FillHeader()
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

        protected override void FillReportData()
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

                CleanTestData(lastDataColumn);
                if (configuration.LastColumn != lastDataColumn)
                {
                    DrawBorders(worksheet, lastDataColumn);
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

        private void CleanTestData(int actualLastColumn)
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

        private void DrawBorders(IXLWorksheet worksheet, int actualLastColumn)
        {
            var rightBorder = worksheet.Range(configuration.ReportTitleRow, actualLastColumn, configuration.LastRow, actualLastColumn);

            rightBorder.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rightBorder.Style.Border.RightBorderColor = XLColor.Black;

            var bottomBorder = worksheet.Range(configuration.LastRow, configuration.FirstColumn, configuration.LastRow, actualLastColumn);

            bottomBorder.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            bottomBorder.Style.Border.BottomBorderColor = XLColor.Black;
        }
    }
}
