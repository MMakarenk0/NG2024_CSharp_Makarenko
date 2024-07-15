using ClosedXML.Excel;
using ClosedXML.Report;
using ReportApp.Interfaces;
using ReportApp.Models.CurrentShopItems;

namespace ReportApp.Services.CurrentShopItems
{
    public class CurrentShopItemsReportGeneratorService : IReportGenerator
    {
        private readonly ITemplateLoader _templateLoader;
        private readonly IConfigurationLoader<CurrentShopItemsConfiguration> _configurationLoader;
        private readonly ICollectionReportSerializer<CurrentShopItemsModel> _reportSerializer;

        public CurrentShopItemsReportGeneratorService(
            ITemplateLoader templateLoader,
            IConfigurationLoader<CurrentShopItemsConfiguration> configurationLoader,
            ICollectionReportSerializer<CurrentShopItemsModel> reportSerializer)
        {
            _templateLoader = templateLoader;
            _configurationLoader = configurationLoader;
            _reportSerializer = reportSerializer;
        }

        public void GenerateReport(string jsonPath, string outputPath)
        {
            var template = _templateLoader.LoadTemplate("./Templates/CurrentShopItemsTemplate.xlsx");
            var configuration = _configurationLoader.LoadConfiguration("./ReportConfigurations/CurrentShopItems.json");

            var models = _reportSerializer.DeserializeReportModels(jsonPath);
            FillReportDataFromModel(template, configuration, models);

            template.Generate();
            template.SaveAs(outputPath);
        }
        public void FillReportDataFromModel(XLTemplate template, CurrentShopItemsConfiguration configuration, List<CurrentShopItemsModel> models)
        {
            var worksheet = template.Workbook.Worksheets.First();
            worksheet.SetShowGridLines(false);

            var firstDataRow = configuration.DefaultRow;
            var firstDataColumn = configuration.FirstColumn;

            var lastDataColumn = configuration.LastColumn;
            int initialLastRow = configuration.LastRow;

            worksheet.Range(configuration.FirstRowForDynamicGroup, lastDataColumn, configuration.FirstRowForDynamicGroup, configuration.LastColumn)
                .Delete(XLShiftDeletedCells.ShiftCellsLeft);
            worksheet.Range(configuration.FirstRowForStaticGroup, lastDataColumn, configuration.FirstRowForStaticGroup, configuration.LastColumn)
                .Delete(XLShiftDeletedCells.ShiftCellsLeft);

            var titleCell = worksheet.Cell(configuration.ReportTitleRow, firstDataColumn);
            var style = titleCell.Style;
            var title = titleCell.Value.ToString();

            var previousRange = worksheet.Range(configuration.ReportTitleRow, firstDataColumn, configuration.ReportTitleRow, configuration.LastColumn).Unmerge();
            previousRange.Clear();

            var newRange = worksheet.Range(configuration.ReportTitleRow, firstDataColumn, configuration.ReportTitleRow, lastDataColumn - 1).Merge();
            newRange.Style = style;
            newRange.Value = title;

            int currentRow = firstDataRow;

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
            var workingRange = worksheet.Range(configuration.ReportTitleRow, firstDataColumn, configuration.LastRow, lastDataColumn);
            worksheet.Columns(configuration.FirstColumn, configuration.LastColumn).AdjustToContents();
        }
    }
}


