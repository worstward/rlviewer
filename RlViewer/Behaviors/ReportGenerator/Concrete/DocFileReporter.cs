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
        { }



        public override void GenerateReport(string reportFilePath)
        {
            using (DocX document = DocX.Create(reportFilePath))
            {
                for (int i = 0; i < FilePaths.Length; i++)
                {
                    var docToInsert = PrepareReport(reportFilePath, FilePaths[i]);
                    document.InsertDocument(docToInsert);
                    docToInsert.Dispose();
                }
                document.Save();
            }
        }



        


        private DocX PrepareReport(string reportFilePath, string locatorFilePath)
        {
                DocX document = DocX.Create(reportFilePath);
            
                var fileHeader = Factories.Header.Abstract.HeaderFactory.GetFactory(
                    new Files.FileProperties(locatorFilePath)).Create(locatorFilePath);

                Paragraph p = document.InsertParagraph();
                p.Alignment = Alignment.center;
                p.Append(System.IO.Path.GetFileName(locatorFilePath)).Bold().FontSize(20)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine);

                foreach (var subHeaderInfo in fileHeader.HeaderInfo)
                {
                    Paragraph subHeader = document.InsertParagraph();
                    subHeader.Alignment = Alignment.center;
                    subHeader.Append(System.IO.Path.GetFileName(subHeaderInfo.HeaderName)).Bold().FontSize(14);


                    Table subHeaderTable = document.AddTable(subHeaderInfo.Params.Count(), 2);
                    subHeaderTable.Alignment = Alignment.center;
                    subHeaderTable.Design = TableDesign.TableGrid;

                    int index = 0;
                    foreach (var entry in subHeaderInfo.Params)
                    {
                        subHeaderTable.Rows[index].Cells[0].Paragraphs.First().Append(entry.Item1);
                        subHeaderTable.Rows[index].Cells[1].Paragraphs.First().Append(entry.Item2);
                        index++;
                    }

                    document.InsertTable(subHeaderTable);
                }

                return document;

            
        }


        

        private void GenerateMultipleFilesReport()
        {
 
        }


    }
}
