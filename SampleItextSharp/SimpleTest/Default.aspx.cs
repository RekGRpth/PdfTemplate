﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace fr.cedricmartel.SampleItextSharp.SimpleTest
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly Hashtable headerData = new Hashtable();
        private readonly List<Hashtable> loopData = new List<Hashtable>();
        private readonly Hashtable bodyData = new Hashtable();
        private readonly Hashtable footerData = new Hashtable();
        private readonly Hashtable headerTableData = new Hashtable();
        private readonly List<Hashtable> loopTableData = new List<Hashtable>();
        private readonly Hashtable footerTableData = new Hashtable();

        private Random randonGen;

        protected void GenerationPdf(object sender, EventArgs e)
        {
            randonGen = new Random();
            // templates load
            var template = Server.MapPath("test.xml");
            var pdfTemplate = new Moon.PDFTemplateItextSharp.PDFTemplateItextSharp(template);
            // TODO fonts externes 

            //  parameters load
            headerData.Add("{titreDocument}", "SERVICE DEPARTEMENTAL D'INCENDIE \nET DE SECOURS DES DEUX-SEVRES");
            headerData.Add("{logoUrl}", Server.MapPath("LogoPdf.jpg"));
            footerData.Add("{titreDocument}", "Titre du document");

            // data load
            DateTime debut = new DateTime(2016, 1, 1);
            loopTableData.Add(new Hashtable
                {
                    {"{Date}", debut.AddDays(-1)},
                    {"{Centre}", "des données qui prennent toute la ligne pour voir "}, 
                    {"{Frais}", ""},
                    {"{Nombre}", ""},
                    {"{Base}", ""},
                    {"{Montant}", ""}, 
                    {"{Background}", RandomColor()}
                });
            for (int i = 0; i < 200; i++)
            {
                var donnees1 = new Hashtable
                {
                    {"{Date}", debut.AddDays(i)},
                    {"{Centre}", "Centre 1\nMore information"},
                    {"{Frais}", 5},
                    {"{Nombre}", "200,00"},
                    {"{Base}", "5,00"},
                    {"{Montant}", i}, 
                    {"{Background}", RandomColor()}
                };
                loopTableData.Add(donnees1);
            }
            footerTableData.Add("{Total}", 250.5);

            // pdf generation
            pdfTemplate.Draw(headerData, loopData, bodyData, footerData, headerTableData, loopTableData, footerTableData);

            // save file locally
            string fileDirectory = Server.MapPath("../Output/");
            string fileName = "SimpleTest-" + String.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now) + ".pdf";
            using (var filePdf = new FileStream(fileDirectory + fileName, FileMode.Create))
            {
                using (MemoryStream stream = pdfTemplate.Close())
                {
                    byte[] content = stream.ToArray();
                    filePdf.Write(content, 0, content.Length);
                }
            }

            Resulat.Text = "Generated PDF: <a href='../Output/" + fileName + "'>" + fileName + "</a><br/><br/><iframe src='../Output/" + fileName + "' width='1024' height='600' />";
        }

        private string RandomColor()
        {
            Color randomColor =
                Color.FromArgb(
                (byte)randonGen.Next(255),
                (byte)randonGen.Next(255),
                (byte)randonGen.Next(255),
                (byte)randonGen.Next(255));
            return "#" + randomColor.R.ToString("X2") + randomColor.G.ToString("X2") + randomColor.B.ToString("X2");
        }
    }
}