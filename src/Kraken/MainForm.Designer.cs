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
            this.EnvironmentCmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.hideAllPanel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.selectedPathTb = new System.Windows.Forms.TextBox();
            this.configurationTree = new System.Windows.Forms.TreeView();
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
            // EnvironmentCmb
            // 
            this.EnvironmentCmb.Location = new System.Drawing.Point(67, 143);
            this.EnvironmentCmb.Name = "EnvironmentCmb";
            this.EnvironmentCmb.Size = new System.Drawing.Size(325, 21);
            this.EnvironmentCmb.TabIndex = 1;
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
            this.button2.Location = new System.Drawing.Point(25, 693);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(172, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Применить Артефакт";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Anchor =
	            ((System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Bottom |
	                                                  System.Windows.Forms.AnchorStyles.Left));
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
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(26, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(366, 27);
            this.label4.TabIndex = 7;
            this.label4.Text = "Есть следующие наборы настроек для получения конфигурационных файлов:";
            // 
            // hideAllPanel
            // 
            this.hideAllPanel.Controls.Add(this.pictureBox1);
            this.hideAllPanel.Location = new System.Drawing.Point(-2, 0);
            this.hideAllPanel.Name = "hideAllPanel";
            this.hideAllPanel.Size = new System.Drawing.Size(435, 727);
            this.hideAllPanel.TabIndex = 9;
            this.hideAllPanel.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(150, 255);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 130);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(203, 693);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(188, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Применить Переменные";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Anchor =
	            ((System.Windows.Forms.AnchorStyles) (System.Windows.Forms.AnchorStyles.Bottom |
	                                                    System.Windows.Forms.AnchorStyles.Left));
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // selectedPathTb
            // 
            this.selectedPathTb.Location = new System.Drawing.Point(25, 114);
            this.selectedPathTb.Name = "selectedPathTb";
            this.selectedPathTb.Size = new System.Drawing.Size(366, 20);
            this.selectedPathTb.TabIndex = 11;
            // 
            // configurationTree
            // 
            this.configurationTree.CheckBoxes = true;
            this.configurationTree.Location = new System.Drawing.Point(25, 201);
            this.configurationTree.Name = "configurationTree";
            this.configurationTree.Size = new System.Drawing.Size(366, 480);
            this.configurationTree.TabIndex = 12;
            this.configurationTree.TabStop = false;
            this.configurationTree.Anchor =
	            ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
	                                                     System.Windows.Forms.AnchorStyles.Bottom) |
	                                                    System.Windows.Forms.AnchorStyles.Left) |
	                                                   System.Windows.Forms.AnchorStyles.Right)));
            this.configurationTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.configurationTree_AfterCheck);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 728);
            this.Controls.Add(this.hideAllPanel);
            this.Controls.Add(this.selectedPathTb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EnvironmentCmb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.configurationTree);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выпустить Кракена!";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.hideAllPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox EnvironmentCmb;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel hideAllPanel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox selectedPathTb;
        private System.Windows.Forms.TreeView configurationTree;
    }
}