using DevExpress.Web;
using System;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        Grid.SettingsBehavior.ColumnResizeMode = EnableResizingCheckBox.Checked ? ColumnResizeMode.NextColumn : ColumnResizeMode.Disabled;

        if(!IsPostBack) {
            Grid.ExpandRow(0);
            Grid.ExpandRow(1);
        }
    }

    protected void Grid_Init(object sender, EventArgs e) {
        Grid.Templates.GroupRowContent = new GridGroupRowContentTemplate();
    }
}