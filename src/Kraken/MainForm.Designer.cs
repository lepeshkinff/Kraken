namespace Kraken
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.EnvironmentTb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.configurationsList = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.selectedPathLabel = new System.Windows.Forms.Label();
            this.hideAllPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.hideAllPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(26, 82);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(366, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Выбрать папку решения";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EnvironmentTb
            // 
            this.EnvironmentTb.Location = new System.Drawing.Point(67, 143);
            this.EnvironmentTb.Name = "EnvironmentTb";
            this.EnvironmentTb.Size = new System.Drawing.Size(325, 20);
            this.EnvironmentTb.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Среда";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(26, 336);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(172, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Применить Артефакт";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(342, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Добро пожаловать в загрузчик файлов конфигурации из octopus!";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(26, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(407, 37);
            this.label3.TabIndex = 5;
            this.label3.Text = "Он поможет не только загрузить все нужные config файлы из последнего деплоя на ук" +
    "азанную среду,";
            // 
            // configurationsList
            // 
            this.configurationsList.FormattingEnabled = true;
            this.configurationsList.Location = new System.Drawing.Point(26, 229);
            this.configurationsList.Name = "configurationsList";
            this.configurationsList.Size = new System.Drawing.Size(366, 95);
            this.configurationsList.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(26, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(366, 42);
            this.label4.TabIndex = 7;
            this.label4.Text = "Есть следующие наборы настроек для получения конфигурационных файлов:";
            // 
            // selectedPathLabel
            // 
            this.selectedPathLabel.AutoSize = true;
            this.selectedPathLabel.Location = new System.Drawing.Point(26, 116);
            this.selectedPathLabel.Name = "selectedPathLabel";
            this.selectedPathLabel.Size = new System.Drawing.Size(0, 13);
            this.selectedPathLabel.TabIndex = 8;
            // 
            // hideAllPanel
            // 
            this.hideAllPanel.Controls.Add(this.pictureBox1);
            this.hideAllPanel.Location = new System.Drawing.Point(12, 10);
            this.hideAllPanel.Name = "hideAllPanel";
            this.hideAllPanel.Size = new System.Drawing.Size(400, 349);
            this.hideAllPanel.TabIndex = 9;
            this.hideAllPanel.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(3, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(394, 300);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(204, 336);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(188, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Применить Переменные";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 373);
            this.Controls.Add(this.hideAllPanel);
            this.Controls.Add(this.selectedPathLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.configurationsList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EnvironmentTb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выпустить Кракена!";
            this.hideAllPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox EnvironmentTb;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox configurationsList;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label selectedPathLabel;
		private System.Windows.Forms.Panel hideAllPanel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button3;
	}
}