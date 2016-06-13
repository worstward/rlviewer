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
        public DocFileReporter(Files.LocatorFile file)
            : base(file)
        { }


        public override void GenerateReport(string fileName)
        {
            using (DocX document = DocX.Create(fileName))
            {
                Paragraph p = document.InsertParagraph();
                p.Alignment = Alignment.center;
                p.Append(System.IO.Path.GetFileName(File.Properties.FilePath)).Bold().FontSize(20)
                .Append(Environment.NewLine)
                .Append(Environment.NewLine);
                
                Table t = document.AddTable(2, 3);
                // Specify some properties for this Table.
                t.Alignment = Alignment.center;
                t.Design = TableDesign.TableGrid;
                // Add content to this Table.
                t.Rows[0].Cells[0].Paragraphs.First().Append("A");
                t.Rows[0].Cells[1].Paragraphs.First().Append("B");
                t.Rows[0].Cells[2].Paragraphs.First().Append("C");
                t.Rows[1].Cells[0].Paragraphs.First().Append("D");
                t.Rows[1].Cells[1].Paragraphs.First().Append("E");
                t.Rows[1].Cells[2].Paragraphs.First().Append("F");
                // Insert the Table into the document.
                document.InsertTable(t);
                document.Save();
            }
        }
    }
}
