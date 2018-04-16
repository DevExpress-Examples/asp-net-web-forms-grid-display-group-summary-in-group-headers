using DevExpress.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public class GridGroupRowContentTemplate : ITemplate {
    const string
        MainTableCssClassName = "summaryTable",
        VisibleColumnCssClassName = "gridVisibleColumn",
        SummaryTextContainerCssClassName = "summaryTextContainer",
        SummaryCellCssClassNameFormat = "summaryCell_{0}",
        GroupTextFormat = "{0}: {1}";

    protected GridViewGroupRowTemplateContainer Container { get; set; }
    protected ASPxGridView Grid { get { return Container.Grid; } }

    protected Table MainTable { get; set; }
    protected TableRow GroupTextRow { get; set; }
    protected TableRow SummaryTextRow { get; set; }

    protected int IndentCount { get { return Grid.GroupCount - GroupLevel - 1; } }
    protected int GroupLevel { get { return Grid.DataBoundProxy.GetRowLevel(Container.VisibleIndex); } }
    protected List<GridViewColumn> VisibleColumns { get { return Grid.VisibleColumns.Except(Grid.GetGroupedColumns()).ToList(); } }

    public void InstantiateIn(Control container) {
        Container = (GridViewGroupRowTemplateContainer)container;
        CreateGroupRowTable();
        Container.Controls.Add(MainTable);

        ApplyStyles();
    }

    protected void CreateGroupRowTable() {
        MainTable = new Table();

        GroupTextRow = CreateRow();
        SummaryTextRow = CreateRow();

        CreateGroupTextCell();
        CreateIndentCells();
        foreach(var column in VisibleColumns)
            CreateSummaryTextCell(column);
    }

    protected void CreateGroupTextCell() {
        var cell = CreateCell(GroupTextRow);
        cell.Text = string.Format(GroupTextFormat, Container.Column, Container.GroupText);
        cell.ColumnSpan = VisibleColumns.Count + IndentCount;
    }

    protected void CreateSummaryTextCell(GridViewColumn column) {
        var cell = CreateCell(SummaryTextRow);
        var dataColumn = column as GridViewDataColumn;
        if(dataColumn == null)
            return;
        var summaryItems = FindSummaryItems(dataColumn);
        if(summaryItems.Count == 0)
            return;

        var div = new WebControl(HtmlTextWriterTag.Div) { CssClass = SummaryTextContainerCssClassName };
        cell.Controls.Add(div);

        var text = GetGroupSummaryText(summaryItems, column);
        div.Controls.Add(new LiteralControl(text));
    }

    protected string GetGroupSummaryText(List<ASPxSummaryItem> items, GridViewColumn column) {
        var sb = new StringBuilder();
        for(var i = 0; i < items.Count; i++) {
            if(i > 0) sb.Append("<br />");
            var item = items[i];
            var summaryValue = Grid.GetGroupSummaryValue(Container.VisibleIndex, item);
            sb.Append(item.GetGroupRowDisplayText(column, summaryValue));
        }
        return sb.ToString();
    }

    protected void ApplyStyles() {
        MainTable.CssClass = MainTableCssClassName;
        VisibleColumns[0].HeaderStyle.CssClass = VisibleColumnCssClassName;

        var startIndex = GroupLevel + 1;
        for(var i = 0; i < SummaryTextRow.Cells.Count; i++)
            SummaryTextRow.Cells[i].CssClass = string.Format(SummaryCellCssClassNameFormat, i + startIndex);
    }

    protected void CreateIndentCells() {
        for(var i = 0; i < IndentCount; i++)
            CreateCell(SummaryTextRow);
    }
    protected List<ASPxSummaryItem> FindSummaryItems(GridViewDataColumn column) {
        return Grid.GroupSummary.Where(i => i.FieldName == column.FieldName).ToList();
    }
    protected TableRow CreateRow() {
        var row = new TableRow();
        MainTable.Rows.Add(row);
        return row;
    }
    protected TableCell CreateCell(TableRow row) {
        var cell = new TableCell();
        row.Cells.Add(cell);
        return cell;
    }
}