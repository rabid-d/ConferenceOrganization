namespace ReportViewerWinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.reportsComboBox = new System.Windows.Forms.ComboBox();
            this.conferenceComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.reportDataGridView = new System.Windows.Forms.DataGridView();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.viewReportTabPage = new System.Windows.Forms.TabPage();
            this.newUserTabPage = new System.Windows.Forms.TabPage();
            this.saveUserProgressBar = new System.Windows.Forms.ProgressBar();
            this.saveUserButton = new System.Windows.Forms.Button();
            this.choosePhotoButton = new System.Windows.Forms.Button();
            this.photoPathTextBox = new System.Windows.Forms.TextBox();
            this.photoLabel = new System.Windows.Forms.Label();
            this.bioLabel = new System.Windows.Forms.Label();
            this.bioRichTextBox = new System.Windows.Forms.RichTextBox();
            this.positonTextBox = new System.Windows.Forms.TextBox();
            this.workTextBox = new System.Windows.Forms.TextBox();
            this.degreeTextBox = new System.Windows.Forms.TextBox();
            this.fullNameTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.reportDataGridView)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.viewReportTabPage.SuspendLayout();
            this.newUserTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // reportsComboBox
            // 
            this.reportsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reportsComboBox.FormattingEnabled = true;
            this.reportsComboBox.Location = new System.Drawing.Point(15, 6);
            this.reportsComboBox.Name = "reportsComboBox";
            this.reportsComboBox.Size = new System.Drawing.Size(776, 23);
            this.reportsComboBox.TabIndex = 0;
            // 
            // conferenceComboBox
            // 
            this.conferenceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conferenceComboBox.FormattingEnabled = true;
            this.conferenceComboBox.Location = new System.Drawing.Point(15, 35);
            this.conferenceComboBox.Name = "conferenceComboBox";
            this.conferenceComboBox.Size = new System.Drawing.Size(776, 23);
            this.conferenceComboBox.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(15, 64);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // reportDataGridView
            // 
            this.reportDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reportDataGridView.Location = new System.Drawing.Point(15, 93);
            this.reportDataGridView.Name = "reportDataGridView";
            this.reportDataGridView.RowTemplate.Height = 25;
            this.reportDataGridView.Size = new System.Drawing.Size(776, 295);
            this.reportDataGridView.TabIndex = 3;
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.viewReportTabPage);
            this.mainTabControl.Controls.Add(this.newUserTabPage);
            this.mainTabControl.Location = new System.Drawing.Point(12, 12);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(811, 427);
            this.mainTabControl.TabIndex = 4;
            // 
            // viewReportTabPage
            // 
            this.viewReportTabPage.Controls.Add(this.reportsComboBox);
            this.viewReportTabPage.Controls.Add(this.reportDataGridView);
            this.viewReportTabPage.Controls.Add(this.conferenceComboBox);
            this.viewReportTabPage.Controls.Add(this.okButton);
            this.viewReportTabPage.Location = new System.Drawing.Point(4, 24);
            this.viewReportTabPage.Name = "viewReportTabPage";
            this.viewReportTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.viewReportTabPage.Size = new System.Drawing.Size(803, 399);
            this.viewReportTabPage.TabIndex = 0;
            this.viewReportTabPage.Text = "View reports";
            this.viewReportTabPage.UseVisualStyleBackColor = true;
            // 
            // newUserTabPage
            // 
            this.newUserTabPage.Controls.Add(this.statusLabel);
            this.newUserTabPage.Controls.Add(this.saveUserProgressBar);
            this.newUserTabPage.Controls.Add(this.saveUserButton);
            this.newUserTabPage.Controls.Add(this.choosePhotoButton);
            this.newUserTabPage.Controls.Add(this.photoPathTextBox);
            this.newUserTabPage.Controls.Add(this.photoLabel);
            this.newUserTabPage.Controls.Add(this.bioLabel);
            this.newUserTabPage.Controls.Add(this.bioRichTextBox);
            this.newUserTabPage.Controls.Add(this.positonTextBox);
            this.newUserTabPage.Controls.Add(this.workTextBox);
            this.newUserTabPage.Controls.Add(this.degreeTextBox);
            this.newUserTabPage.Controls.Add(this.fullNameTextBox);
            this.newUserTabPage.Location = new System.Drawing.Point(4, 24);
            this.newUserTabPage.Name = "newUserTabPage";
            this.newUserTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.newUserTabPage.Size = new System.Drawing.Size(803, 399);
            this.newUserTabPage.TabIndex = 1;
            this.newUserTabPage.Text = "Add a user";
            this.newUserTabPage.UseVisualStyleBackColor = true;
            // 
            // saveUserProgressBar
            // 
            this.saveUserProgressBar.Location = new System.Drawing.Point(6, 327);
            this.saveUserProgressBar.Name = "saveUserProgressBar";
            this.saveUserProgressBar.Size = new System.Drawing.Size(319, 23);
            this.saveUserProgressBar.TabIndex = 10;
            // 
            // saveUserButton
            // 
            this.saveUserButton.Location = new System.Drawing.Point(6, 298);
            this.saveUserButton.Name = "saveUserButton";
            this.saveUserButton.Size = new System.Drawing.Size(75, 23);
            this.saveUserButton.TabIndex = 9;
            this.saveUserButton.Text = "Save User";
            this.saveUserButton.UseVisualStyleBackColor = true;
            this.saveUserButton.Click += new System.EventHandler(this.saveUserButton_Click);
            // 
            // choosePhotoButton
            // 
            this.choosePhotoButton.Location = new System.Drawing.Point(331, 254);
            this.choosePhotoButton.Name = "choosePhotoButton";
            this.choosePhotoButton.Size = new System.Drawing.Size(100, 23);
            this.choosePhotoButton.TabIndex = 8;
            this.choosePhotoButton.Text = "Choose File...";
            this.choosePhotoButton.UseVisualStyleBackColor = true;
            this.choosePhotoButton.Click += new System.EventHandler(this.choosePhotoButton_Click);
            // 
            // photoPathTextBox
            // 
            this.photoPathTextBox.Location = new System.Drawing.Point(6, 254);
            this.photoPathTextBox.Name = "photoPathTextBox";
            this.photoPathTextBox.Size = new System.Drawing.Size(319, 23);
            this.photoPathTextBox.TabIndex = 7;
            this.photoPathTextBox.Text = "c:\\Users\\andr\\Downloads\\Screenshot 2022-07-14 161637.jpg";
            // 
            // photoLabel
            // 
            this.photoLabel.AutoSize = true;
            this.photoLabel.Location = new System.Drawing.Point(6, 236);
            this.photoLabel.Name = "photoLabel";
            this.photoLabel.Size = new System.Drawing.Size(42, 15);
            this.photoLabel.TabIndex = 6;
            this.photoLabel.Text = "Photo:";
            // 
            // bioLabel
            // 
            this.bioLabel.AutoSize = true;
            this.bioLabel.Location = new System.Drawing.Point(3, 119);
            this.bioLabel.Name = "bioLabel";
            this.bioLabel.Size = new System.Drawing.Size(131, 15);
            this.bioLabel.TabIndex = 5;
            this.bioLabel.Text = "Professional biography:";
            // 
            // bioRichTextBox
            // 
            this.bioRichTextBox.Location = new System.Drawing.Point(6, 137);
            this.bioRichTextBox.Name = "bioRichTextBox";
            this.bioRichTextBox.Size = new System.Drawing.Size(319, 96);
            this.bioRichTextBox.TabIndex = 4;
            this.bioRichTextBox.Text = "John Doe (male) and Jane Doe (female) are multiple-use placeholder names that are" +
    " used when the true name of a person is unknown or is being intentionally concea" +
    "led.";
            // 
            // positonTextBox
            // 
            this.positonTextBox.Location = new System.Drawing.Point(6, 93);
            this.positonTextBox.Name = "positonTextBox";
            this.positonTextBox.PlaceholderText = "Work position";
            this.positonTextBox.Size = new System.Drawing.Size(319, 23);
            this.positonTextBox.TabIndex = 3;
            this.positonTextBox.Text = "Worker";
            // 
            // workTextBox
            // 
            this.workTextBox.Location = new System.Drawing.Point(6, 64);
            this.workTextBox.Name = "workTextBox";
            this.workTextBox.PlaceholderText = "Work place";
            this.workTextBox.Size = new System.Drawing.Size(319, 23);
            this.workTextBox.TabIndex = 2;
            this.workTextBox.Text = "Computer scientist";
            // 
            // degreeTextBox
            // 
            this.degreeTextBox.Location = new System.Drawing.Point(6, 35);
            this.degreeTextBox.Name = "degreeTextBox";
            this.degreeTextBox.PlaceholderText = "Degree";
            this.degreeTextBox.Size = new System.Drawing.Size(319, 23);
            this.degreeTextBox.TabIndex = 1;
            this.degreeTextBox.Text = "University of California, Berkeley (B.S., M.S.)";
            // 
            // fullNameTextBox
            // 
            this.fullNameTextBox.Location = new System.Drawing.Point(6, 6);
            this.fullNameTextBox.Name = "fullNameTextBox";
            this.fullNameTextBox.PlaceholderText = "Full name";
            this.fullNameTextBox.Size = new System.Drawing.Size(319, 23);
            this.fullNameTextBox.TabIndex = 0;
            this.fullNameTextBox.Text = "John Doe";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(10, 353);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 15);
            this.statusLabel.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 450);
            this.Controls.Add(this.mainTabControl);
            this.Name = "Form1";
            this.Text = "Conference Organization";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.reportDataGridView)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.viewReportTabPage.ResumeLayout(false);
            this.newUserTabPage.ResumeLayout(false);
            this.newUserTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox reportsComboBox;
        private ComboBox conferenceComboBox;
        private Button okButton;
        private DataGridView reportDataGridView;
        private TabControl mainTabControl;
        private TabPage viewReportTabPage;
        private TabPage newUserTabPage;
        private TextBox degreeTextBox;
        private TextBox fullNameTextBox;
        private TextBox positonTextBox;
        private TextBox workTextBox;
        private Label bioLabel;
        private RichTextBox bioRichTextBox;
        private Button saveUserButton;
        private Button choosePhotoButton;
        private TextBox photoPathTextBox;
        private Label photoLabel;
        private ProgressBar saveUserProgressBar;
        private Label statusLabel;
    }
}