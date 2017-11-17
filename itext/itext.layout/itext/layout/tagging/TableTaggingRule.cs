using System.Collections.Generic;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Layout.Element;

namespace iText.Layout.Tagging {
    internal class TableTaggingRule : ITaggingRule {
        public virtual bool OnTagFinish(LayoutTaggingHelper taggingHelper, TaggingHintKey tableHintKey) {
            IList<TaggingHintKey> kidKeys = taggingHelper.GetAccessibleKidsHint(tableHintKey);
            IDictionary<int, SortedDictionary<int, TaggingHintKey>> tableTags = new SortedDictionary<int, SortedDictionary
                <int, TaggingHintKey>>();
            IList<TaggingHintKey> tableCellTagsUnindexed = new List<TaggingHintKey>();
            IList<TaggingHintKey> nonCellKids = new List<TaggingHintKey>();
            foreach (TaggingHintKey kidKey in kidKeys) {
                if (PdfName.TD.Equals(kidKey.GetAccessibleElement().GetRole()) || PdfName.TH.Equals(kidKey.GetAccessibleElement
                    ().GetRole())) {
                    if (kidKey.GetAccessibleElement() is Cell) {
                        Cell cell = (Cell)kidKey.GetAccessibleElement();
                        int rowInd = cell.GetRow();
                        int colInd = cell.GetCol();
                        SortedDictionary<int, TaggingHintKey> rowTags = tableTags.Get(rowInd);
                        if (rowTags == null) {
                            rowTags = new SortedDictionary<int, TaggingHintKey>();
                            tableTags.Put(rowInd, rowTags);
                        }
                        rowTags.Put(colInd, kidKey);
                    }
                    else {
                        tableCellTagsUnindexed.Add(kidKey);
                    }
                }
                else {
                    nonCellKids.Add(kidKey);
                }
            }
            bool createTBody = true;
            if (tableHintKey.GetAccessibleElement() is Table) {
                Table modelElement = (Table)tableHintKey.GetAccessibleElement();
                createTBody = modelElement.GetHeader() != null && !modelElement.IsSkipFirstHeader() || modelElement.GetFooter
                    () != null && !modelElement.IsSkipLastFooter();
            }
            TaggingDummyElement tbodyTag = null;
            tbodyTag = new TaggingDummyElement(createTBody ? PdfName.TBody : null);
            foreach (TaggingHintKey nonCellKid in nonCellKids) {
                PdfName kidRole = nonCellKid.GetAccessibleElement().GetRole();
                if (!PdfName.THead.Equals(kidRole) && !PdfName.TFoot.Equals(kidRole)) {
                    taggingHelper.MoveKidHint(nonCellKid, tableHintKey);
                }
            }
            foreach (TaggingHintKey nonCellKid in nonCellKids) {
                PdfName kidRole = nonCellKid.GetAccessibleElement().GetRole();
                if (PdfName.THead.Equals(kidRole)) {
                    taggingHelper.MoveKidHint(nonCellKid, tableHintKey);
                }
            }
            taggingHelper.AddKidsHint(tableHintKey, JavaCollectionsUtil.SingletonList<TaggingHintKey>(LayoutTaggingHelper
                .GetOrCreateHintKey(tbodyTag)), -1);
            foreach (TaggingHintKey nonCellKid in nonCellKids) {
                PdfName kidRole = nonCellKid.GetAccessibleElement().GetRole();
                if (PdfName.TFoot.Equals(kidRole)) {
                    taggingHelper.MoveKidHint(nonCellKid, tableHintKey);
                }
            }
            foreach (SortedDictionary<int, TaggingHintKey> rowTags in tableTags.Values) {
                TaggingDummyElement row = new TaggingDummyElement(PdfName.TR);
                TaggingHintKey rowTagHint = LayoutTaggingHelper.GetOrCreateHintKey(row);
                foreach (TaggingHintKey cellTagHint in rowTags.Values) {
                    taggingHelper.MoveKidHint(cellTagHint, rowTagHint);
                }
                if (tableCellTagsUnindexed != null) {
                    foreach (TaggingHintKey cellTagHint in tableCellTagsUnindexed) {
                        taggingHelper.MoveKidHint(cellTagHint, rowTagHint);
                    }
                    tableCellTagsUnindexed = null;
                }
                taggingHelper.AddKidsHint(tbodyTag, JavaCollectionsUtil.SingletonList<TaggingDummyElement>(row), -1);
            }
            return true;
        }
    }
}
