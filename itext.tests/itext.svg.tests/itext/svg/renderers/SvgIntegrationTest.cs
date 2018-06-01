/*
This file is part of the iText (R) project.
Copyright (c) 1998-2018 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Svg.Converter;
using iText.Svg.Processors;

namespace iText.Svg.Renderers {
    public class SvgIntegrationTest {
        /// <exception cref="System.IO.IOException"/>
        public virtual void Convert(Stream svg, Stream pdfOutputStream) {
            PdfDocument doc = new PdfDocument(new PdfWriter(pdfOutputStream, new WriterProperties().SetCompressionLevel
                (0)));
            doc.AddNewPage();
            SvgConverter.DrawOnDocument(svg, doc, 1);
            doc.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void ConvertToSinglePage(Stream svg, Stream pdfOutputStream) {
            WriterProperties writerprops = new WriterProperties().SetCompressionLevel(0);
            SvgConverter.CreatePdf(svg, pdfOutputStream, writerprops);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void ConvertToSinglePage(Stream svg, Stream pdfOutputStream, ISvgConverterProperties properties
            ) {
            WriterProperties writerprops = new WriterProperties().SetCompressionLevel(0);
            SvgConverter.CreatePdf(svg, properties, pdfOutputStream, writerprops);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        public virtual void ConvertAndCompareVisually(String src, String dest, String fileName) {
            Convert(new FileStream(src + fileName + ".svg", FileMode.Open, FileAccess.Read), new FileStream(dest + fileName
                 + ".pdf", FileMode.Create));
            CompareTool compareTool = new CompareTool();
            String compareResult = compareTool.CompareVisually(dest + fileName + ".pdf", src + "cmp_" + fileName + ".pdf"
                , dest, "diff_");
            NUnit.Framework.Assert.IsNull(compareResult);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        public virtual void ConvertAndCompareSinglePageStructurally(String src, String dest, String fileName) {
            ConvertToSinglePage(new FileStream(src + fileName + ".svg", FileMode.Open, FileAccess.Read), new FileStream
                (dest + fileName + ".pdf", FileMode.Create));
            CompareTool compareTool = new CompareTool();
            String compareResult = compareTool.CompareByContent(dest + fileName + ".pdf", src + "cmp_" + fileName + ".pdf"
                , dest, "diff_");
            NUnit.Framework.Assert.IsNull(compareResult);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        public virtual void ConvertAndCompareSinglePageStructurally(String src, String dest, String fileName, ISvgConverterProperties
             properties) {
            ConvertToSinglePage(new FileStream(src + fileName + ".svg", FileMode.Open, FileAccess.Read), new FileStream
                (dest + fileName + ".pdf", FileMode.Create), properties);
            CompareTool compareTool = new CompareTool();
            String compareResult = compareTool.CompareByContent(dest + fileName + ".pdf", src + "cmp_" + fileName + ".pdf"
                , dest, "diff_");
            NUnit.Framework.Assert.IsNull(compareResult);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        public virtual void ConvertAndCompareSinglePageVisually(String src, String dest, String fileName) {
            ConvertToSinglePage(new FileStream(src + fileName + ".svg", FileMode.Open, FileAccess.Read), new FileStream
                (dest + fileName + ".pdf", FileMode.Create));
            CompareTool compareTool = new CompareTool();
            String compareResult = compareTool.CompareVisually(dest + fileName + ".pdf", src + "cmp_" + fileName + ".pdf"
                , dest, "diff_");
            NUnit.Framework.Assert.IsNull(compareResult);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        public virtual void ConvertAndCompareSinglePageVisually(String src, String dest, String fileName, ISvgConverterProperties
             properties) {
            ConvertToSinglePage(new FileStream(src + fileName + ".svg", FileMode.Open, FileAccess.Read), new FileStream
                (dest + fileName + ".pdf", FileMode.Create), properties);
            CompareTool compareTool = new CompareTool();
            String compareResult = compareTool.CompareVisually(dest + fileName + ".pdf", src + "cmp_" + fileName + ".pdf"
                , dest, "diff_");
            NUnit.Framework.Assert.IsNull(compareResult);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        public virtual void ConvertAndCompareStructurally(String src, String dest, String fileName) {
            Convert(new FileStream(src + fileName + ".svg", FileMode.Open, FileAccess.Read), new FileStream(dest + fileName
                 + ".pdf", FileMode.Create));
            CompareTool compareTool = new CompareTool();
            String compareResult = compareTool.CompareByContent(dest + fileName + ".pdf", src + "cmp_" + fileName + ".pdf"
                , dest, "diff_");
            NUnit.Framework.Assert.IsNull(compareResult);
        }
    }
}
