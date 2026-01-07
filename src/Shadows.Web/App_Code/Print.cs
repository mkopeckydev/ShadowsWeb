using Shadows.Data;
using Shadows.Data.CustomData;
using Shadows.Data.Facade;
using Shadows.Data.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Shadows.Web
{
    public class Print
    {
        private Print()
        {}

        #region GetReportItems
        public static MemoryStream GetReportItems(List<Item> items, Character character, string caption, ISession session, CustomWebPage page)
        {
            MemoryStream ms = new MemoryStream();

            float margin = 20f;
            Document document = new Document(PageSize.A4, margin, margin, margin, margin);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            BaseFont bf = BaseFont.CreateFont(page.Server.MapPath("Fonts/Arial.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font fontCaption = new Font(bf, 11, Font.BOLD);
            Font fontHeader = new Font(bf, 8, Font.BOLD);
            Font fontRow = new Font(bf, 6, Font.NORMAL);
            Font fontRowBold = new Font(bf, 6, Font.BOLD);
            Font fontSmall = new Font(bf, 6, Font.NORMAL);

            Paragraph par = new Paragraph();
            Phrase ph = new Phrase(String.Format("{0} {1}", character.Name, caption).Trim(), fontCaption);
            par.Add(ph);
            par.SpacingAfter = 5;
            par.Alignment = Element.ALIGN_CENTER;
            document.Add(par);

            PdfPTable tbl = new PdfPTable(2);
            float[] widths = new float[] { 2f, 8f };
            tbl.SetWidths(widths);
            tbl.WidthPercentage = 100;

            ItemFacade fItem = new ItemFacade(session);

            //poznnamky
            BusinessFacade<ItemNote> fItemNote = new BusinessFacade<ItemNote>(session);
            List<ItemNote> allNotes = fItemNote.GetListEntities(String.Empty, true, String.Format("(IdCharacter = {0})", character.Id));

            //prenacteni hodnot pro rychlost
            BusinessFacade<ItemRow> fItemRow = new BusinessFacade<ItemRow>(session);
            List<ItemRow> allRows = fItemRow.GetListEntities("InfoLevel", true);

            BusinessFacade<CharacterSkillLevel> fCharacterSkillLevel = new BusinessFacade<CharacterSkillLevel>(session);
            List<CharacterSkillLevel> charSkillLevels = fCharacterSkillLevel.GetListEntities(String.Empty, true, String.Format("(IdCharacter = {0})", character.Id));

            BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(session);
            List<CharacterItemValue> charItemValues = fCharacterItemValue.GetListEntities(String.Empty, true, String.Format("(IdCharacter = {0})", character.Id));

            foreach (Item i in items)
            {
                GetVisibleItemRowsResult row = fItem.GetVisibleItemRows(character, i, allRows, charSkillLevels, charItemValues);

                //doplneni tisku poznamky
                ItemNote itemNote = allNotes.Find(x => x.IdItem == i.Id);
                if (itemNote != null)
                {
                    if (!String.IsNullOrEmpty(itemNote.Note))
                    {
                        ItemRow noteRow = new ItemRow();
                        noteRow.InfoText = itemNote.Note;
                        row.List.Add(noteRow);
                    }
                }

                int rowsCount = row.List.Count;
                if (rowsCount < 1) continue;

                Phrase p = new Phrase(i.Caption, fontHeader);
                PdfPCell c = new PdfPCell(p);
                c.BorderColor = BaseColor.GRAY;
                c.BackgroundColor = new BaseColor(229, 229, 229);
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.Colspan = 2;

                tbl.AddCell(c);

                string imagePath = page.Server.MapPath(Path.Combine(FilesManager.IMAGES_PATH, i.ImageName));
                FileInfo fi = new FileInfo(imagePath);

                if (fi.Exists)
                {
                    Image img = Image.GetInstance(fi.FullName);
                    img.ScaleToFit(100, 100);
                    c = new PdfPCell(img);
                    c.Padding = 3;
                    c.HorizontalAlignment = Element.ALIGN_CENTER;
                    c.Rowspan = rowsCount;
                    tbl.AddCell(c);
                }
                else
                {
                    c = new PdfPCell();
                    c.Rowspan = rowsCount;
                    tbl.AddCell(c);
                }

                foreach(ItemRow r in row.List)
                {
                    c = new PdfPCell();
                    c.HorizontalAlignment = Element.ALIGN_LEFT;

                    if (!String.IsNullOrEmpty(r.InfoCaption))
                    {
                        Phrase infoCaption = new Phrase(r.InfoCaption, fontRowBold);
                        c.AddElement(infoCaption);
                    }

                    string tmpInfo = String.Empty;
                    if (!String.IsNullOrEmpty(r.CharacterClass)) tmpInfo += r.CharacterClass + ": ";

                    tmpInfo += r.InfoText;

                    Phrase info = new Phrase(tmpInfo, fontRow);
                    c.AddElement(info);

                    tbl.AddCell(c);
                }
            }

            document.Add(tbl);

            document.Close();

            return ms;
        }
        #endregion

        #region GetReportSpellBook
        public static MemoryStream GetReportSpellBook(List<SpellbookItem> items, Character character, CustomWebPage page, ISession session)
        {
            BusinessFacade<SpellbookItemNote> fItemSpellbookNote = new BusinessFacade<SpellbookItemNote>(session);

            MemoryStream ms = new MemoryStream();

            float margin = 20f;
            Document document = new Document(PageSize.A4, margin, margin, margin, margin);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            BaseFont bf = BaseFont.CreateFont(page.Server.MapPath("Fonts/Arial.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font fontCaption = new Font(bf, 14, Font.BOLD);
            Font fontHeader = new Font(bf, 12, Font.BOLD);
            Font fontRow = new Font(bf, 10, Font.NORMAL);
            Font fontRowBold = new Font(bf, 10, Font.BOLD);
            Font fontSmall = new Font(bf, 10, Font.NORMAL);

            Paragraph par = new Paragraph();
            Phrase ph = new Phrase(String.Format("Kniha kouzel {0}", character.Name).Trim(), fontCaption);
            par.Add(ph);
            par.SpacingAfter = 5;
            par.Alignment = Element.ALIGN_CENTER;
            document.Add(par);

            foreach (SpellbookItem i in items)
            {
                PdfPTable tbl = new PdfPTable(1);
                float[] widths = new float[] { 1f };
                tbl.SetWidths(widths);
                tbl.WidthPercentage = 100;
                tbl.SpacingAfter = 8;

                //nadpis

                Phrase p = new Phrase(i.Caption, fontHeader);
                PdfPCell c = new PdfPCell(p);
                c.BorderColor = BaseColor.GRAY;
                c.BackgroundColor = new BaseColor(229, 229, 229);
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.PaddingBottom = 6;
                tbl.AddCell(c);

                p = new Phrase(String.Format("{0}, stupeň {1}", i.SpellGroup, i.SpellLevel), fontRowBold);
                c = new PdfPCell(p);
                c.BorderColor = BaseColor.GRAY;
                c.BackgroundColor = new BaseColor(229, 229, 229);
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.PaddingBottom = 6;
                tbl.AddCell(c);

                //parametry

                c = new PdfPCell();
                c.HorizontalAlignment = Element.ALIGN_LEFT;

                Phrase infoCaption = new Phrase("Parametry kouzla", fontRowBold);
                c.AddElement(infoCaption);

                Phrase info = new Phrase(i.Attributes, fontRow);
                c.AddElement(info);

                tbl.AddCell(c);

                //ucinky

                c = new PdfPCell();
                c.HorizontalAlignment = Element.ALIGN_LEFT;

                infoCaption = new Phrase("Účinky kouzla", fontRowBold);
                c.AddElement(infoCaption);

                info = new Phrase(i.Description, fontRow);
                c.AddElement(info);

                tbl.AddCell(c);

                //modifikace

                c = new PdfPCell();
                c.HorizontalAlignment = Element.ALIGN_LEFT;

                infoCaption = new Phrase("Modifikace kouzla", fontRowBold);
                c.AddElement(infoCaption);

                info = new Phrase(i.Modification, fontRow);
                c.AddElement(info);

                tbl.AddCell(c);

                //runovy mag

                c = new PdfPCell();
                c.HorizontalAlignment = Element.ALIGN_LEFT;

                infoCaption = new Phrase("Runový mág (modifikace za runu síly)", fontRowBold);
                c.AddElement(infoCaption);

                info = new Phrase(i.ModificationRuneMage, fontRow);
                c.AddElement(info);

                tbl.AddCell(c);

                //poznamka
                
                List<SpellbookItemNote> notes = fItemSpellbookNote.GetListEntities(String.Empty, true, String.Format("(IdSpellbookItem = {0}) and (IdCharacter = {1})", i.Id, character.Id));
                if (notes.Count > 0)
                {
                    SpellbookItemNote note = notes[0];
                    if (!String.IsNullOrEmpty(note.Note))
                    {
                        c = new PdfPCell();
                        c.HorizontalAlignment = Element.ALIGN_LEFT;

                        infoCaption = new Phrase("Poznámka", fontRowBold);
                        c.AddElement(infoCaption);

                        info = new Phrase(note.Note, fontRow);
                        c.AddElement(info);

                        tbl.AddCell(c);
                    }
                }

                document.Add(tbl);

                document.NewPage();
            }

            document.Close();

            return ms;
        }
        #endregion
    }
}