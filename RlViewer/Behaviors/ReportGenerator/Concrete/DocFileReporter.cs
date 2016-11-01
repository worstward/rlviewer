using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Drawing;
using Novacode;

namespace RlViewer.Behaviors.ReportGenerator.Concrete
{
    class DocFileReporter : Abstract.Reporter
    {
        public DocFileReporter(params string[] fileNames)
            : base(fileNames)
        {

        }

        public override void GenerateReport(string reportFilePath, ReporterSettings reporterSettings)
        {
            using (DocX document = DocX.Create(reportFilePath))
            {
                for (int i = 0; i < FilesToProcess.Length; i++)
                {
                    if (System.IO.File.Exists(FilesToProcess[i]))
                    {
                        OnNameReport("Генерация отчета");
                        DocX docToInsert = null;

                        try
                        {
                            docToInsert = PrepareReport(reportFilePath, FilesToProcess[i], reporterSettings);
                        }
                        catch (Exception ex)
                        {
                            Logging.Logger.Log(Logging.SeverityGrades.Error,
                                string.Format("Can't create report for file {0}, reason: {1}", FilesToProcess[i], ex.Message));
                            continue;
                        }

                        document.InsertDocument(docToInsert);

                        if (i != FilesToProcess.Length - 1)
                        {
                            document.InsertSectionPageBreak();
                        }

                        OnProgressReport((int)(i / (float)FilesToProcess.Length * 100));
                        OnCancelWorker();

                    }
                }
                document.Save();
            }
        }


        private DocX PrepareReport(string reportFilePath, string locatorFilePath, ReporterSettings reporterSettings)
        {

            DocX document = DocX.Create(reportFilePath);

            var prop = new Files.FileProperties(locatorFilePath);
            var fileHeader = Factories.Header.Abstract.HeaderFactory.GetFactory(prop)
                .Create(locatorFilePath);

            var file = Factories.File.Abstract.FileFactory.GetFactory(prop)
                .Create(prop, fileHeader, null);


            Paragraph p = document.InsertParagraph();
            p.Alignment = Alignment.center;
            p.Append(System.IO.Path.GetFileName(locatorFilePath)).Bold().FontSize(20)
            .Append(Environment.NewLine)
            .Append(Environment.NewLine);


            if (reporterSettings.AddArea)
            {
                p.Append(string.Format("Площадь засвеченной поверхности: {0}км2",
                (Factories.AreaSizeCalc.Abstract.AreaSizeCalcFactory.GetFactory(file
                .Properties).Create(file.Header).CalculateArea(file.Width, file.Height) / 1000 / 1000)
                .ToString(".################################")))
                .Append(Environment.NewLine);
            }


            var cornerCoord = Factories.CornerCoords.Abstract.CornerCoordFactory
                .GetFactory(file.Properties)
                .Create(file, reporterSettings.FirstLineOffset, reporterSettings.LastLineOffset, reporterSettings.ReadToEnd);


            if (reporterSettings.AddTimes)
            {
                Paragraph timesParagraph = document.InsertParagraph();
                timesParagraph.Alignment = Alignment.left;
                foreach (var entry in cornerCoord.GetZoneStartAndEndTimes())
                {
                    timesParagraph.Append(string.Format("{0}: {1}", entry.Item1, entry.Item2));
                    timesParagraph.Append(Environment.NewLine);
                }
            }

            if (reporterSettings.AddCenter)
            {
                Paragraph cornersParagraph = document.InsertParagraph();
                cornersParagraph.Alignment = Alignment.left;
                foreach (var entry in cornerCoord.GetCenterCoordinates())
                {
                    cornersParagraph.Append(string.Format("{0}: {1}", entry.Item1, entry.Item2));
                    cornersParagraph.Append(Environment.NewLine);
                }
            }


            if (reporterSettings.AddCorners)
            {
                Paragraph cornersParagraph = document.InsertParagraph();
                cornersParagraph.Alignment = Alignment.left;
                foreach (var entry in cornerCoord.GetCoornerCoordinates())
                {
                    cornersParagraph.Append(string.Format("{0}: {1}", entry.Item1, entry.Item2));
                    cornersParagraph.Append(Environment.NewLine);
                }
            }

            if (reporterSettings.AddParametersTable)
            {
                foreach (var subHeaderInfo in file.Header.HeaderInfo)
                {
                    var headerTable = PrepareHeaderInfoTable(document, subHeaderInfo);
                    headerTable.AutoFit = AutoFit.Window;
                    document.InsertTable(headerTable);
                }
            }

            return document;
        }



        private Table PrepareHeaderInfoTable(DocX document, HeaderInfoOutput headerInfo)
        {
            Paragraph subHeader = document.InsertParagraph();
            subHeader.Alignment = Alignment.center;
            subHeader.Append(System.IO.Path.GetFileName(headerInfo.HeaderName)).Bold().FontSize(14);

            Table subHeaderTable = document.AddTable(headerInfo.Params.Count(), 2);
            subHeaderTable.Alignment = Alignment.center;
            subHeaderTable.Design = TableDesign.TableGrid;

            int index = 0;
            foreach (var entry in headerInfo.Params)
            {
                subHeaderTable.Rows[index].Cells[0].Paragraphs.First().Append(entry.Item1);
                subHeaderTable.Rows[index].Cells[1].Paragraphs.First().Append(entry.Item2);
                index++;
            }

            return subHeaderTable;
        }



    }
}
