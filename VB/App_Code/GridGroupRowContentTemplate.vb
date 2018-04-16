Imports DevExpress.Web
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class GridGroupRowContentTemplate
    Implements ITemplate

    Private Const MainTableCssClassName As String = "summaryTable", VisibleColumnCssClassName As String = "gridVisibleColumn", SummaryTextContainerCssClassName As String = "summaryTextContainer", SummaryCellCssClassNameFormat As String = "summaryCell_{0}", GroupTextFormat As String = "{0}: {1}"

    Protected Property Container() As GridViewGroupRowTemplateContainer
    Protected ReadOnly Property Grid() As ASPxGridView
        Get
            Return Container.Grid
        End Get
    End Property

    Protected Property MainTable() As Table
    Protected Property GroupTextRow() As TableRow
    Protected Property SummaryTextRow() As TableRow

    Protected ReadOnly Property IndentCount() As Integer
        Get
            Return Grid.GroupCount - GroupLevel - 1
        End Get
    End Property
    Protected ReadOnly Property GroupLevel() As Integer
        Get
            Return Grid.DataBoundProxy.GetRowLevel(Container.VisibleIndex)
        End Get
    End Property
    Protected ReadOnly Property VisibleColumns() As List(Of GridViewColumn)
        Get
            Return Grid.VisibleColumns.Except(Grid.GetGroupedColumns()).ToList()
        End Get
    End Property

    Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
        Me.Container = CType(container, GridViewGroupRowTemplateContainer)
        CreateGroupRowTable()
        Me.Container.Controls.Add(MainTable)

        ApplyStyles()
    End Sub

    Protected Sub CreateGroupRowTable()
        MainTable = New Table()

        GroupTextRow = CreateRow()
        SummaryTextRow = CreateRow()

        CreateGroupTextCell()
        CreateIndentCells()
        For Each column In VisibleColumns
            CreateSummaryTextCell(column)
        Next column
    End Sub

    Protected Sub CreateGroupTextCell()
        Dim cell = CreateCell(GroupTextRow)
        cell.Text = String.Format(GroupTextFormat, Container.Column, Container.GroupText)
        cell.ColumnSpan = VisibleColumns.Count + IndentCount
    End Sub

    Protected Sub CreateSummaryTextCell(ByVal column As GridViewColumn)
        Dim cell = CreateCell(SummaryTextRow)
        Dim dataColumn = TryCast(column, GridViewDataColumn)
        If dataColumn Is Nothing Then
            Return
        End If
        Dim summaryItems = FindSummaryItems(dataColumn)
        If summaryItems.Count = 0 Then
            Return
        End If

        Dim div = New WebControl(HtmlTextWriterTag.Div) With {.CssClass = SummaryTextContainerCssClassName}
        cell.Controls.Add(div)

        Dim text = GetGroupSummaryText(summaryItems, column)
        div.Controls.Add(New LiteralControl(text))
    End Sub

    Protected Function GetGroupSummaryText(ByVal items As List(Of ASPxSummaryItem), ByVal column As GridViewColumn) As String
        Dim sb = New StringBuilder()
        For i = 0 To items.Count - 1
            If i > 0 Then
                sb.Append("<br />")
            End If
            Dim item = items(i)
            Dim summaryValue = Grid.GetGroupSummaryValue(Container.VisibleIndex, item)
            sb.Append(item.GetGroupRowDisplayText(column, summaryValue))
        Next i
        Return sb.ToString()
    End Function

    Protected Sub ApplyStyles()
        MainTable.CssClass = MainTableCssClassName
        VisibleColumns(0).HeaderStyle.CssClass = VisibleColumnCssClassName

        Dim startIndex = GroupLevel + 1
        For i = 0 To SummaryTextRow.Cells.Count - 1
            SummaryTextRow.Cells(i).CssClass = String.Format(SummaryCellCssClassNameFormat, i + startIndex)
        Next i
    End Sub

    Protected Sub CreateIndentCells()
        For i = 0 To IndentCount - 1
            CreateCell(SummaryTextRow)
        Next i
    End Sub
    Protected Function FindSummaryItems(ByVal column As GridViewDataColumn) As List(Of ASPxSummaryItem)
        Return Grid.GroupSummary.Where(Function(i) i.FieldName = column.FieldName).ToList()
    End Function
    Protected Function CreateRow() As TableRow
        Dim row = New TableRow()
        MainTable.Rows.Add(row)
        Return row
    End Function
    Protected Function CreateCell(ByVal row As TableRow) As TableCell
        Dim cell = New TableCell()
        row.Cells.Add(cell)
        Return cell
    End Function
End Class