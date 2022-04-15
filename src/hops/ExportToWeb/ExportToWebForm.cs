using System;
using Eto.Forms;

namespace Hops
{
    class ExportToWebForm : Dialog<bool>
    {
        public ExportToWebForm()
        {
            Title = "Export to web";

            var projectName_Textbox = new TextBox();
            projectName_Textbox.Size = new Eto.Drawing.Size(250, -1);
            projectName_Textbox.PlaceholderText = "Project Name";

            var folderPath_Textbox = new TextBox();
            folderPath_Textbox.ReadOnly = true;
            folderPath_Textbox.Size = new Eto.Drawing.Size(250, -1);
            folderPath_Textbox.PlaceholderText = "Export Directory";

            bool onWindows = Rhino.Runtime.HostUtils.RunningOnWindows;
            DefaultButton = new Button { Text = onWindows ? "OK" : "Apply" };
            DefaultButton.Click += (sender, e) =>
            {
                if (String.IsNullOrEmpty(projectName_Textbox.Text) || String.IsNullOrEmpty(folderPath_Textbox.Text))
                {
                    DialogResult result = MessageBox.Show(this, "Project name and directory path are required fields.", "Required Field Missing", MessageBoxButtons.OK, MessageBoxType.Information, MessageBoxDefaultButton.OK);
                    if (result == DialogResult.Ok)
                        return;
                }
                Close(true);
            };
            AbortButton = new Button { Text = "C&ancel" };
            AbortButton.Click += (sender, e) => Close(false);
            var buttons = new TableLayout();
            if (onWindows)
            {
                buttons.Spacing = new Eto.Drawing.Size(5, 5);
                buttons.Rows.Add(new TableRow(null, DefaultButton, AbortButton));
            }
            else
                buttons.Rows.Add(new TableRow(null, AbortButton, DefaultButton));

            var projectNameRow = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Items = { projectName_Textbox }
            };

            var folderPickButton = new Rhino.UI.Controls.ImageButton();
            folderPickButton.Image = Rhino.Resources.Assets.Rhino.Eto.Bitmaps.TryGet(Rhino.Resources.ResourceIds.FolderopenPng, new Eto.Drawing.Size(24, 24));
            folderPickButton.Click += (sender, e) =>
            {
                var dlg = new SelectFolderDialog();
                // work around an issue with the parent window on Mac
                Window parent = onWindows ? this : null;
                if (dlg.ShowDialog(parent) == DialogResult.Ok)
                {
                    folderPath_Textbox.Text = dlg.Directory;
                }
            };

            var folderPathRow = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = buttons.Spacing.Width,
                Items = { folderPath_Textbox, folderPickButton }
            };

            Content = new TableLayout
            {
                Padding = new Eto.Drawing.Padding(10),
                Spacing = new Eto.Drawing.Size(5, 5),
                Rows = {
                        new TableRow { ScaleHeight = true, Cells = { projectNameRow } },
                        new TableRow { ScaleHeight = true, Cells = { folderPathRow } },
                        buttons
                    }
            };
            Closed += (s, e) => { FolderPath = folderPath_Textbox.Text; ProjectName = projectName_Textbox.Text; };
        }

        public string FolderPath
        {
            get;
            set;
        }
        public string ProjectName
        {
            get;
            set;
        }
    
    }
}
