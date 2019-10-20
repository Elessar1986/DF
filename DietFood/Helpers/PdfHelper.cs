using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DietFood.Models;
using DietFood.Models.Enums;
using iText;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace DietFood.Helpers
{
    public class PdfHelper
    {
        public byte[] CreateTable()
        {
            MemoryStream dest = new MemoryStream();
            var writer = new PdfWriter(dest);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(10, 10, 10, 10);
            var font = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
            var bold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            var table = new Table(new float[] { 2, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.AddHeaderCell(new Cell().Add( new Paragraph("Name").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("1").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("2").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("3").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("4").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("5").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("6").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("7").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("8").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("9").SetFont(bold)));
            for (int i = 0; i < 100; i++)
            {
                table.AddCell(new Cell().Add(new Paragraph(i.ToString()).SetFont(font)));
            }

            document.Add(table);
            document.Close();
            return dest.ToArray();
        }

        public byte[] CreateTable(Calculation model, string week, string program, DaysOfWeek day, int weight)
        {
            MemoryStream dest = new MemoryStream();
            var writer = new PdfWriter(dest);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(10, 10, 10, 10);

            var font = PdfFontFactory.CreateFont("wwwroot/FreeSans.ttf", "Cp1251", true);
            var bold = PdfFontFactory.CreateFont("wwwroot/FreeSansBold.ttf", "Cp1251", true);
            var table = new Table(new float[] { 2, 1, 1, 1, 1, 1 });
            table.SetWidth(UnitValue.CreatePercentValue(100));

            table.AddHeaderCell(new Cell(1, 6).Add(new Paragraph($"Неделя: {week} ({day.ToString()}) | Програма: {program} | Вес клиента: {weight.ToString()}").SetFont(bold)));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Название блюда").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Вес").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Калории").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Белки").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Жиры").SetFont(bold)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Углеводы").SetFont(bold)));
            for (int i = 0; i < 5; i++)
            {
                if (model.DishCalculations.Count(x => x.MealType == i) > 0)
                {
                    var mealNameCel = new Cell(1, 6).Add(new Paragraph(((MealName)i).ToString()).SetFont(bold));
                    table.AddCell(mealNameCel);
                    foreach (var item in model.DishCalculations.Where(x => x.MealType == i))
                    {
                        table.AddCell(new Cell().Add(new Paragraph(item.Name.ToString()).SetFont(font)));
                        if (item.ConstWeight > 0)
                            table.AddCell(new Cell().Add(new Paragraph(item.ConstWeight.ToString()).SetFont(font).SetFontColor(ColorConstants.RED)));
                        else
                            table.AddCell(new Cell().Add(new Paragraph(item.Weight.ToString()).SetFont(font)));
                        table.AddCell(new Cell().Add(new Paragraph(item.Calories.ToString()).SetFont(font)));
                        table.AddCell(new Cell().Add(new Paragraph(item.Proteins.ToString()).SetFont(font)));
                        table.AddCell(new Cell().Add(new Paragraph(item.Fats.ToString()).SetFont(font)));
                        table.AddCell(new Cell().Add(new Paragraph(item.Carbohydrates.ToString()).SetFont(font)));
                    }
                }
            }
            document.Add(table);
            document.Close();
            return dest.ToArray();
        }

    }
}
