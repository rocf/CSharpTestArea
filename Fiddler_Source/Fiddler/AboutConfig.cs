namespace Fiddler
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AboutConfig : UserControl
    {
        private static char[] _arrForbiddenChars = new char[] { '*', ' ', '$', '%', '@', '?', '!' };
        private IContainer components;
        private DataGridView dgvGrid;
        private LinkLabel lnkHelpOnPrefs;
        private static TabPage oPage = null;
        private static PreferenceBag.PrefWatcher? oPW = null;
        private static AboutConfig oView = null;
        private Panel pnlTop;

        public AboutConfig()
        {
            this.InitializeComponent();
        }

        private void AllPrefChange(object sender, PrefChangeEventArgs oArg)
        {
            if (!FiddlerApplication.isClosing)
            {
                base.BeginInvoke(new MethodInvoker(delegate {
                    foreach (DataGridViewRow row in (IEnumerable) this.dgvGrid.Rows)
                    {
                        object obj2 = row.Cells[0].Value ?? string.Empty;
                        if (obj2.ToString() == oArg.PrefName)
                        {
                            if ((oArg.ValueString == string.Empty) && (FiddlerApplication.Prefs[oArg.PrefName] == null))
                            {
                                this.dgvGrid.Rows.Remove(row);
                            }
                            else
                            {
                                row.Cells[1].Value = oArg.ValueString;
                            }
                            break;
                        }
                    }
                }));
                this.FillGrid();
            }
        }

        private void dgvGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (((e.ColumnIndex == 0) && (this.dgvGrid.Rows[e.RowIndex].Cells[0].Value != null)) && string.IsNullOrEmpty(this.dgvGrid.Rows[e.RowIndex].ErrorText))
            {
                e.Cancel = true;
            }
        }

        private void dgvGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            object obj2 = this.dgvGrid.Rows[e.RowIndex].Cells[0].Value ?? string.Empty;
            object obj3 = this.dgvGrid.Rows[e.RowIndex].Cells[1].Value ?? string.Empty;
            string sName = obj2.ToString();
            string str2 = obj3.ToString();
            if (this.isValidName(sName))
            {
                this.dgvGrid.Rows[e.RowIndex].ErrorText = string.Empty;
                MessageBox.Show(string.Format("Setting: '{0}' to '{1}'", sName, str2), "Changing...");
                FiddlerApplication.Prefs.SetStringPref(sName, str2);
            }
            else
            {
                this.dgvGrid.Rows[e.RowIndex].ErrorText = "Illegal Preference name";
            }
        }

        private void dgvGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            string sPrefName = e.Row.Cells[0].Value.ToString();
            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to delete the preference?\n\n\t" + sPrefName, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            {
                FiddlerApplication.Prefs.RemovePref(sPrefName);
            }
            else
            {
                e.Cancel = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FillGrid()
        {
            base.BeginInvoke(new MethodInvoker(delegate {
                this.dgvGrid.SuspendLayout();
                this.dgvGrid.Rows.Clear();
                foreach (string str in (FiddlerApplication.Prefs as PreferenceBag).GetPrefArray())
                {
                    oView.dgvGrid.Rows.Add(new object[] { str, FiddlerApplication.Prefs[str] });
                }
                this.dgvGrid.Sort(oView.dgvGrid.Columns[0], ListSortDirection.Ascending);
                this.dgvGrid.ResumeLayout();
                this.dgvGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }));
        }

        private void InitializeComponent()
        {
            this.dgvGrid = new System.Windows.Forms.DataGridView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lnkHelpOnPrefs = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvGrid
            // 
            this.dgvGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGrid.Location = new System.Drawing.Point(0, 18);
            this.dgvGrid.Name = "dgvGrid";
            this.dgvGrid.Size = new System.Drawing.Size(420, 276);
            this.dgvGrid.TabIndex = 0;
            this.dgvGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvGrid_CellBeginEdit);
            this.dgvGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGrid_CellEndEdit);
            this.dgvGrid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvGrid_UserDeletingRow);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lnkHelpOnPrefs);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(420, 18);
            this.pnlTop.TabIndex = 1;
            // 
            // lnkHelpOnPrefs
            // 
            this.lnkHelpOnPrefs.AutoSize = true;
            this.lnkHelpOnPrefs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkHelpOnPrefs.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkHelpOnPrefs.Location = new System.Drawing.Point(3, 3);
            this.lnkHelpOnPrefs.Name = "lnkHelpOnPrefs";
            this.lnkHelpOnPrefs.Size = new System.Drawing.Size(138, 13);
            this.lnkHelpOnPrefs.TabIndex = 0;
            this.lnkHelpOnPrefs.TabStop = true;
            this.lnkHelpOnPrefs.Text = "Get Help for preferences...";
            this.lnkHelpOnPrefs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHelpOnPrefs_LinkClicked);
            // 
            // AboutConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvGrid);
            this.Controls.Add(this.pnlTop);
            this.Name = "AboutConfig";
            this.Size = new System.Drawing.Size(420, 294);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        private bool isValidName(string sName)
        {
            return (((!string.IsNullOrEmpty(sName) && (0x100 > sName.Length)) && (0 > sName.IndexOf("internal", StringComparison.OrdinalIgnoreCase))) && (0 > sName.IndexOfAny(_arrForbiddenChars)));
        }

        private void lnkHelpOnPrefs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilities.LaunchHyperlink(CONFIG.GetUrl("REDIR") + "FIDDLERPREFLIST");
        }

        public static void ShowAboutConfigPage()
        {
            if (oPage != null)
            {
                FiddlerApplication.UI.actActivateTabByTitle("about:config", FiddlerApplication.UI.tabsViews);
            }
            else
            {
                oPage = new TabPage("about:config");
                oPage.Font = new Font(oPage.Font.FontFamily, CONFIG.flFontSize);
                oView = new AboutConfig();
                oPage.Controls.Add(oView);
                oView.Dock = DockStyle.Fill;
                FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);
                oView.dgvGrid.MultiSelect = false;
                oView.dgvGrid.ReadOnly = false;
                oView.dgvGrid.ShowEditingIcon = true;
                oView.dgvGrid.RowHeadersWidth = 40;
                oView.dgvGrid.RowHeadersVisible = true;
                oView.dgvGrid.AllowUserToResizeRows = false;
                oView.dgvGrid.AllowUserToAddRows = true;
                oView.dgvGrid.ColumnCount = 2;
                oView.dgvGrid.Columns[0].Name = "Name";
                oView.dgvGrid.Columns[1].Name = "Value";
                oPW = new PreferenceBag.PrefWatcher?(FiddlerApplication.Prefs.AddWatcher(string.Empty, new EventHandler<PrefChangeEventArgs>(oView.AllPrefChange)));
                oView.FillGrid();
                FiddlerApplication.UI.tabsViews.SelectedTab = oPage;
            }
        }

    }
}

