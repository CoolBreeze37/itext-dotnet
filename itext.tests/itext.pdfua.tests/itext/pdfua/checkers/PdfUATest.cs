/*
This file is part of the iText (R) project.
Copyright (c) 1998-2024 Apryse Group NV
Authors: Apryse Software.

This program is offered under a commercial and under the AGPL license.
For commercial licensing, contact us at https://itextpdf.com/sales.  For AGPL licensing, see below.

AGPL licensing:
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Pdfua;
using iText.Pdfua.Exceptions;
using iText.Test;
using iText.Test.Pdfa;

namespace iText.Pdfua.Checkers {
    // Android-Conversion-Skip-Line (TODO DEVSIX-7377 introduce pdf/ua validation on Android)
    [NUnit.Framework.Category("IntegrationTest")]
    public class PdfUATest : ExtendedITextTest {
        private static readonly String DESTINATION_FOLDER = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/pdfua/PdfUATest/";

        private static readonly String SOURCE_FOLDER = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/pdfua/PdfUATest/";

        private static readonly String DOG = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/pdfua/img/DOG.bmp";

        private static readonly String FONT = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/pdfua/font/FreeSans.ttf";

        private static readonly String FOX = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/pdfua/img/FOX.bmp";

        private UaValidationTestFramework framework;

        [NUnit.Framework.OneTimeSetUp]
        public static void Before() {
            CreateOrClearDestinationFolder(DESTINATION_FOLDER);
        }

        [NUnit.Framework.SetUp]
        public virtual void InitializeFramework() {
            framework = new UaValidationTestFramework(DESTINATION_FOLDER);
        }

        [NUnit.Framework.Test]
        public virtual void CheckPoint01_007_suspectsHasEntryTrue() {
            PdfUATestPdfDocument pdfDoc = new PdfUATestPdfDocument(new PdfWriter(new MemoryStream(), PdfUATestPdfDocument
                .CreateWriterProperties()));
            PdfDictionary markInfo = (PdfDictionary)pdfDoc.GetCatalog().GetPdfObject().Get(PdfName.MarkInfo);
            NUnit.Framework.Assert.IsNotNull(markInfo);
            markInfo.Put(PdfName.Suspects, new PdfBoolean(true));
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfUAConformanceException), () => pdfDoc.Close());
            NUnit.Framework.Assert.AreEqual(PdfUAExceptionMessageConstants.SUSPECTS_ENTRY_IN_MARK_INFO_DICTIONARY_SHALL_NOT_HAVE_A_VALUE_OF_TRUE
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void CheckPoint01_007_suspectsHasEntryFalse() {
            PdfUATestPdfDocument pdfDoc = new PdfUATestPdfDocument(new PdfWriter(new MemoryStream(), PdfUATestPdfDocument
                .CreateWriterProperties()));
            PdfDictionary markInfo = (PdfDictionary)pdfDoc.GetCatalog().GetPdfObject().Get(PdfName.MarkInfo);
            markInfo.Put(PdfName.Suspects, new PdfBoolean(false));
            NUnit.Framework.Assert.DoesNotThrow(() => pdfDoc.Close());
        }

        [NUnit.Framework.Test]
        public virtual void CheckPoint01_007_suspectsHasNoEntry() {
            // suspects entry is optional so it is ok to not have it according to the spec
            PdfUATestPdfDocument pdfDoc = new PdfUATestPdfDocument(new PdfWriter(new MemoryStream(), PdfUATestPdfDocument
                .CreateWriterProperties()));
            NUnit.Framework.Assert.DoesNotThrow(() => pdfDoc.Close());
        }

        [NUnit.Framework.Test]
        public virtual void EmptyPageDocument() {
            String outPdf = DESTINATION_FOLDER + "emptyPageDocument.pdf";
            using (PdfDocument pdfDocument = new PdfUATestPdfDocument(new PdfWriter(outPdf, PdfUATestPdfDocument.CreateWriterProperties
                ()))) {
                pdfDocument.AddNewPage();
            }
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outPdf, SOURCE_FOLDER + "cmp_emptyPageDocument.pdf"
                , DESTINATION_FOLDER, "diff_"));
            NUnit.Framework.Assert.IsNull(new VeraPdfValidator().Validate(outPdf));
        }

        // Android-Conversion-Skip-Line (TODO DEVSIX-7377 introduce pdf/ua validation on Android)
        [NUnit.Framework.Test]
        public virtual void DocumentWithNoLangEntryTest() {
            String outPdf = DESTINATION_FOLDER + "documentWithNoLangEntryTest.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outPdf, new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_7)));
            pdfDoc.SetTagged();
            ValidationContainer validationContainer = new ValidationContainer();
            validationContainer.AddChecker(new PdfUA1Checker(pdfDoc));
            pdfDoc.GetDiContainer().Register(typeof(ValidationContainer), validationContainer);
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfUAConformanceException), () => pdfDoc.Close());
            NUnit.Framework.Assert.AreEqual(PdfUAExceptionMessageConstants.DOCUMENT_SHALL_CONTAIN_VALID_LANG_ENTRY, e.
                Message);
        }

        [NUnit.Framework.Test]
        public virtual void DocumentWithEmptyStringLangEntryTest() {
            String outPdf = DESTINATION_FOLDER + "documentWithEmptyStringLangEntryTest.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outPdf, new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_7)));
            pdfDoc.SetTagged();
            ValidationContainer validationContainer = new ValidationContainer();
            validationContainer.AddChecker(new PdfUA1Checker(pdfDoc));
            pdfDoc.GetDiContainer().Register(typeof(ValidationContainer), validationContainer);
            pdfDoc.GetCatalog().SetLang(new PdfString(""));
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfUAConformanceException), () => pdfDoc.Close());
            NUnit.Framework.Assert.AreEqual(PdfUAExceptionMessageConstants.DOCUMENT_SHALL_CONTAIN_VALID_LANG_ENTRY, e.
                Message);
        }

        [NUnit.Framework.Test]
        public virtual void DocumentWithComplexLangEntryTest() {
            String outPdf = DESTINATION_FOLDER + "documentWithComplexLangEntryTest.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outPdf, new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_7)));
            pdfDoc.SetTagged();
            ValidationContainer validationContainer = new ValidationContainer();
            validationContainer.AddChecker(new PdfUA1Checker(pdfDoc));
            pdfDoc.GetDiContainer().Register(typeof(ValidationContainer), validationContainer);
            pdfDoc.GetCatalog().SetLang(new PdfString("qaa-Qaaa-QM-x-southern"));
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            pdfDoc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outPdf, SOURCE_FOLDER + "cmp_documentWithComplexLangEntryTest.pdf"
                , DESTINATION_FOLDER, "diff_"));
            NUnit.Framework.Assert.IsNull(new VeraPdfValidator().Validate(outPdf));
        }

        // Android-Conversion-Skip-Line (TODO DEVSIX-7377 introduce pdf/ua validation on Android)
        [NUnit.Framework.Test]
        public virtual void DocumentWithoutViewerPreferencesTest() {
            String outPdf = DESTINATION_FOLDER + "documentWithoutViewerPreferencesTest.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outPdf, new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_7)));
            pdfDoc.SetTagged();
            ValidationContainer validationContainer = new ValidationContainer();
            validationContainer.AddChecker(new PdfUA1Checker(pdfDoc));
            pdfDoc.GetDiContainer().Register(typeof(ValidationContainer), validationContainer);
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfUAConformanceException), () => pdfDoc.Close());
            NUnit.Framework.Assert.AreEqual(PdfUAExceptionMessageConstants.MISSING_VIEWER_PREFERENCES, e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void DocumentWithEmptyViewerPreferencesTest() {
            String outPdf = DESTINATION_FOLDER + "documentWithEmptyViewerPreferencesTest.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outPdf, new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_7)));
            pdfDoc.SetTagged();
            ValidationContainer validationContainer = new ValidationContainer();
            validationContainer.AddChecker(new PdfUA1Checker(pdfDoc));
            pdfDoc.GetDiContainer().Register(typeof(ValidationContainer), validationContainer);
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences());
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfUAConformanceException), () => pdfDoc.Close());
            NUnit.Framework.Assert.AreEqual(PdfUAExceptionMessageConstants.MISSING_VIEWER_PREFERENCES, e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void DocumentWithInvalidViewerPreferencesTest() {
            String outPdf = DESTINATION_FOLDER + "documentWithEmptyViewerPreferencesTest.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outPdf, new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_7)));
            pdfDoc.SetTagged();
            ValidationContainer validationContainer = new ValidationContainer();
            validationContainer.AddChecker(new PdfUA1Checker(pdfDoc));
            pdfDoc.GetDiContainer().Register(typeof(ValidationContainer), validationContainer);
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(false));
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfUAConformanceException), () => pdfDoc.Close());
            NUnit.Framework.Assert.AreEqual(PdfUAExceptionMessageConstants.VIEWER_PREFERENCES_IS_FALSE, e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void CheckNameEntryShouldPresentInAllOCGDictionariesTest() {
            framework.AddBeforeGenerationHook((pdfDocument) => {
                pdfDocument.AddNewPage();
                PdfDictionary ocProperties = new PdfDictionary();
                PdfDictionary d = new PdfDictionary();
                PdfArray configs = new PdfArray();
                PdfDictionary config = new PdfDictionary();
                config.Put(PdfName.Name, new PdfString("CustomName"));
                configs.Add(config);
                ocProperties.Put(PdfName.D, d);
                ocProperties.Put(PdfName.Configs, configs);
                pdfDocument.GetCatalog().Put(PdfName.OCProperties, ocProperties);
            }
            );
            framework.AssertBothFail("pdfuaOCGPropertiesCheck01", PdfUAExceptionMessageConstants.NAME_ENTRY_IS_MISSING_OR_EMPTY_IN_OCG
                );
        }

        [NUnit.Framework.Test]
        public virtual void CheckAsKeyInContentConfigDictTest() {
            framework.AddBeforeGenerationHook((pdfDocument) => {
                pdfDocument.AddNewPage();
                PdfDictionary ocProperties = new PdfDictionary();
                PdfArray configs = new PdfArray();
                PdfDictionary config = new PdfDictionary();
                config.Put(PdfName.Name, new PdfString("CustomName"));
                config.Put(PdfName.AS, new PdfArray());
                configs.Add(config);
                ocProperties.Put(PdfName.Configs, configs);
                pdfDocument.GetCatalog().Put(PdfName.OCProperties, ocProperties);
            }
            );
            framework.AssertBothFail("pdfuaOCGPropertiesCheck02", PdfUAExceptionMessageConstants.OCG_SHALL_NOT_CONTAIN_AS_ENTRY
                );
        }

        [NUnit.Framework.Test]
        public virtual void NameEntryisEmptyTest() {
            framework.AddBeforeGenerationHook((pdfDocument) => {
                PdfDictionary ocProperties = new PdfDictionary();
                PdfDictionary d = new PdfDictionary();
                d.Put(PdfName.Name, new PdfString(""));
                PdfArray configs = new PdfArray();
                PdfDictionary config = new PdfDictionary();
                config.Put(PdfName.Name, new PdfString(""));
                configs.Add(config);
                ocProperties.Put(PdfName.D, d);
                ocProperties.Put(PdfName.Configs, configs);
                pdfDocument.GetCatalog().Put(PdfName.OCProperties, ocProperties);
            }
            );
            framework.AssertBothFail("pdfuaOCGPropertiesCheck03", PdfUAExceptionMessageConstants.NAME_ENTRY_IS_MISSING_OR_EMPTY_IN_OCG
                );
        }

        [NUnit.Framework.Test]
        public virtual void ConfigsEntryisNotAnArrayTest() {
            framework.AddBeforeGenerationHook((pdfDocument) => {
                PdfDictionary ocProperties = new PdfDictionary();
                PdfDictionary d = new PdfDictionary();
                d.Put(PdfName.Name, new PdfString(""));
                PdfDictionary configs = new PdfDictionary();
                ocProperties.Put(PdfName.D, d);
                ocProperties.Put(PdfName.Configs, configs);
                pdfDocument.GetCatalog().Put(PdfName.OCProperties, ocProperties);
            }
            );
            framework.AssertBothFail("pdfuaOCGPropertiesCheck04", PdfUAExceptionMessageConstants.OCG_PROPERTIES_CONFIG_SHALL_BE_AN_ARRAY
                );
        }

        [NUnit.Framework.Test]
        public virtual void NameEntryShouldBeUniqueBetweenDefaultAndAdditionalConfigsTest() {
            framework.AddBeforeGenerationHook((pdfDocument) => {
                PdfDictionary ocProperties = new PdfDictionary();
                PdfDictionary d = new PdfDictionary();
                d.Put(PdfName.Name, new PdfString("CustomName"));
                PdfArray configs = new PdfArray();
                PdfDictionary config = new PdfDictionary();
                config.Put(PdfName.Name, new PdfString("CustomName"));
                configs.Add(config);
                ocProperties.Put(PdfName.D, d);
                ocProperties.Put(PdfName.Configs, configs);
                pdfDocument.GetCatalog().Put(PdfName.OCProperties, ocProperties);
            }
            );
            framework.AssertBothValid("pdfuaOCGPropertiesCheck");
        }

        [NUnit.Framework.Test]
        public virtual void ValidOCGsTest() {
            framework.AddBeforeGenerationHook((pdfDocument) => {
                PdfDictionary ocProperties = new PdfDictionary();
                PdfDictionary d = new PdfDictionary();
                d.Put(PdfName.Name, new PdfString("CustomName"));
                PdfArray configs = new PdfArray();
                PdfArray ocgs = new PdfArray();
                PdfDictionary config = new PdfDictionary();
                config.Put(PdfName.Name, new PdfString("CustomName"));
                configs.Add(config);
                PdfDictionary ocg = new PdfDictionary();
                ocg.Put(PdfName.Name, new PdfString("CustomName"));
                ocgs.Add(ocg);
                ocProperties.Put(PdfName.D, d);
                ocProperties.Put(PdfName.Configs, configs);
                ocProperties.Put(PdfName.OCGs, configs);
                pdfDocument.GetCatalog().Put(PdfName.OCProperties, ocProperties);
            }
            );
            framework.AssertBothValid("pdfuaOCGsPropertiesCheck");
        }

        [NUnit.Framework.Test]
        public virtual void ManualPdfUaCreation() {
            String outPdf = DESTINATION_FOLDER + "manualPdfUaCreation.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outPdf, new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                (PdfVersion.PDF_1_7)));
            Document document = new Document(pdfDoc, PageSize.A4.Rotate());
            //TAGGED PDF
            //Make document tagged
            pdfDoc.SetTagged();
            //PDF/UA
            //Set document metadata
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");
            Paragraph p = new Paragraph();
            //PDF/UA
            //Embed font
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED
                );
            p.SetFont(font);
            p.Add("The quick brown ");
            iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(FOX));
            //PDF/UA
            //Set alt text
            img.GetAccessibilityProperties().SetAlternateDescription("Fox");
            p.Add(img);
            p.Add(" jumps over the lazy ");
            img = new iText.Layout.Element.Image(ImageDataFactory.Create(DOG));
            //PDF/UA
            //Set alt text
            img.GetAccessibilityProperties().SetAlternateDescription("Dog");
            p.Add(img);
            document.Add(p);
            p = new Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n").SetFont(font).SetFontSize(20);
            document.Add(p);
            List list = new List().SetFont(font).SetFontSize(20);
            list.Add(new ListItem("quick"));
            list.Add(new ListItem("brown"));
            list.Add(new ListItem("fox"));
            list.Add(new ListItem("jumps"));
            list.Add(new ListItem("over"));
            list.Add(new ListItem("the"));
            list.Add(new ListItem("lazy"));
            list.Add(new ListItem("dog"));
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outPdf, SOURCE_FOLDER + "cmp_manualPdfUaCreation.pdf"
                , DESTINATION_FOLDER, "diff_"));
            NUnit.Framework.Assert.IsNull(new VeraPdfValidator().Validate(outPdf));
        }
        // Android-Conversion-Skip-Line (TODO DEVSIX-7377 introduce pdf/ua validation on Android)
    }
}
