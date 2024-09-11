using ReportApp.Models.CurrentShopItems;

namespace ReportApp.Services.CurrentItemsShop
{
    public class CurrentShopItemsReportGeneratorService : ReportGeneratorBase
    {
        private const string jsonPath = "./JsonExamples/CurrentShopItems2.json";
        private const string configPath = "./ReportConfigurations/CurrentShopItems.json";
        private const string templatePath = "./Templates/CurrentShopItemsTemplate.xlsx";

        private readonly List<CurrentShopItemsModel> models;
        private readonly CurrentShopItemsConfiguration configuration;

        public CurrentShopItemsReportGeneratorService(
            TemplateLoader templateLoader,
            ConfigurationLoader configurationLoader,
            ReportSerializer reportSerializer)
            : base(templateLoader, configurationLoader, reportSerializer)
        {
            template = templateLoader.LoadTemplate(templatePath);
            models = reportSerializer.DeserializeReportModels<CurrentShopItemsModel>(jsonPath);
            configuration = configurationLoader.LoadConfiguration<CurrentShopItemsConfiguration>(configPath);
        }
        protected override void FillReportData()
        {
            var worksheet = template.Workbook.Worksheets.First();
            worksheet.SetShowGridLines(false);

            var firstDataRow = configuration.DefaultRow;
            var firstDataColumn = configuration.FirstColumn;

            int currentRow = firstDataRow;

            var evenRowStyle = worksheet.Cell(firstDataRow + 1, firstDataColumn).Style;

            var oddRowStyle = worksheet.Cell(firstDataRow, firstDataColumn).Style;

            foreach (var model in models)
            {
                foreach (var item in model.Items)
                {
                    worksheet.Cell(currentRow, firstDataColumn).Value = model.PointOfPurchase;
                    worksheet.Cell(currentRow, firstDataColumn + 1).Value = model.Seller;
                    worksheet.Cell(currentRow, firstDataColumn + 2).Value = item.Name;
                    worksheet.Cell(currentRow, firstDataColumn + 3).Value = item.Quantity;
                    worksheet.Cell(currentRow, firstDataColumn + 4).Value = item.Cost;
                    worksheet.Cell(currentRow, firstDataColumn + 5).Value = item.Notes;

                    var itemTemplateRow = configuration.DefaultRow;
                    for (int col = 0; col < 6; col++)
                    {
                        worksheet.Cell(currentRow, firstDataColumn + col).Style = worksheet.Cell(itemTemplateRow, firstDataColumn + col).Style;
                    }

                    currentRow++;
                    configuration.LastRow++;
                }
            }

            for (int row = firstDataRow; row < currentRow; row++)
            {
                if (row % 2  == 1)
                {
                    worksheet.Range(row, firstDataColumn, row, firstDataColumn + 5)
                    .Style = evenRowStyle;
                }
                else
                {
                    worksheet.Range(row, firstDataColumn, row, firstDataColumn + 5)
                    .Style = oddRowStyle;
                }
            }

            var workingRange = worksheet.Range(configuration.ReportTitleRow, firstDataColumn, configuration.LastRow, firstDataColumn + 5);
            worksheet.Columns(configuration.FirstColumn, firstDataColumn + 5).AdjustToContents();
        }

    }
}


