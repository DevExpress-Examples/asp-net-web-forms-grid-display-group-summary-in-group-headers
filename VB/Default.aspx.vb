Imports DevExpress.Web
Imports System

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        Grid.SettingsBehavior.ColumnResizeMode = If(EnableResizingCheckBox.Checked, ColumnResizeMode.NextColumn, ColumnResizeMode.Disabled)

        If Not IsPostBack Then
            Grid.ExpandRow(0)
            Grid.ExpandRow(1)
        End If
    End Sub

    Protected Sub Grid_Init(ByVal sender As Object, ByVal e As EventArgs)
        Grid.Templates.GroupRowContent = New GridGroupRowContentTemplate()
    End Sub
End Class