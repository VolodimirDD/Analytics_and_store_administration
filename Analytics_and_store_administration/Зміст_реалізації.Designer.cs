namespace ВКР
{
    partial class Зміст_реалізації
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
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.backButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.idtovarTextBox = new System.Windows.Forms.TextBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.user = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.kvoTextBox = new System.Windows.Forms.TextBox();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.searchrealTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.searchtovarTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.idpokupkaTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(870, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(226, 22);
            this.label5.TabIndex = 43;
            this.label5.Text = "Додаткові дані : Реалізація";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(778, 99);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(442, 470);
            this.dataGridView2.TabIndex = 42;
            // 
            // backButton
            // 
            this.backButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.backButton.Location = new System.Drawing.Point(47, 489);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(101, 56);
            this.backButton.TabIndex = 40;
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 233);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "ID товару :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Кількість :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(522, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Пошук по ID товару :";
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(647, 96);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(100, 20);
            this.searchTextBox.TabIndex = 36;
            this.searchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchTextBox_KeyPress);
            // 
            // idtovarTextBox
            // 
            this.idtovarTextBox.Location = new System.Drawing.Point(79, 230);
            this.idtovarTextBox.Name = "idtovarTextBox";
            this.idtovarTextBox.Size = new System.Drawing.Size(100, 20);
            this.idtovarTextBox.TabIndex = 35;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(646, 489);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(101, 56);
            this.DeleteButton.TabIndex = 34;
            this.DeleteButton.Text = "Видалити";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(447, 489);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(101, 56);
            this.EditButton.TabIndex = 33;
            this.EditButton.Text = "Редагувати";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(229, 489);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(101, 56);
            this.AddButton.TabIndex = 32;
            this.AddButton.Text = "Додати";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(319, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 22);
            this.label1.TabIndex = 31;
            this.label1.Text = "Дані про таблицю \"Зміст реалізації\"";
            // 
            // user
            // 
            this.user.AutoSize = true;
            this.user.Font = new System.Drawing.Font("Palatino Linotype", 10F, System.Drawing.FontStyle.Bold);
            this.user.Location = new System.Drawing.Point(481, 9);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(140, 19);
            this.user.TabIndex = 30;
            this.user.Text = "Вивід користувача";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(229, 136);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(518, 330);
            this.dataGridView1.TabIndex = 29;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // kvoTextBox
            // 
            this.kvoTextBox.Location = new System.Drawing.Point(79, 182);
            this.kvoTextBox.Name = "kvoTextBox";
            this.kvoTextBox.Size = new System.Drawing.Size(100, 20);
            this.kvoTextBox.TabIndex = 44;
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(1267, 99);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.Size = new System.Drawing.Size(754, 470);
            this.dataGridView3.TabIndex = 45;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(1585, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(190, 22);
            this.label6.TabIndex = 46;
            this.label6.Text = "Додаткові дані : Товар";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(995, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 13);
            this.label7.TabIndex = 48;
            this.label7.Text = "Пошук по ID покупки :";
            // 
            // searchrealTextBox
            // 
            this.searchrealTextBox.Location = new System.Drawing.Point(1120, 73);
            this.searchrealTextBox.Name = "searchrealTextBox";
            this.searchrealTextBox.Size = new System.Drawing.Size(100, 20);
            this.searchrealTextBox.TabIndex = 47;
            this.searchrealTextBox.TextChanged += new System.EventHandler(this.searchrealTextBox_TextChanged);
            this.searchrealTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchrealTextBox_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1803, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 13);
            this.label8.TabIndex = 50;
            this.label8.Text = "Пошук по ID товару :";
            // 
            // searchtovarTextBox
            // 
            this.searchtovarTextBox.Location = new System.Drawing.Point(1921, 73);
            this.searchtovarTextBox.Name = "searchtovarTextBox";
            this.searchtovarTextBox.Size = new System.Drawing.Size(100, 20);
            this.searchtovarTextBox.TabIndex = 49;
            this.searchtovarTextBox.TextChanged += new System.EventHandler(this.searchtovarTextBox_TextChanged);
            this.searchtovarTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchtovarTextBox_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 136);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 52;
            this.label9.Text = "ID покупки :";
            // 
            // idpokupkaTextBox
            // 
            this.idpokupkaTextBox.Location = new System.Drawing.Point(79, 133);
            this.idpokupkaTextBox.Name = "idpokupkaTextBox";
            this.idpokupkaTextBox.Size = new System.Drawing.Size(100, 20);
            this.idpokupkaTextBox.TabIndex = 51;
            // 
            // Зміст_реалізації
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2033, 587);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.idpokupkaTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.searchtovarTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.searchrealTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.kvoTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.idtovarTextBox);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.user);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Зміст_реалізації";
            this.Text = "Зміст_реалізації";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Зміст_реалізації_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Зміст_реалізації_FormClosed);
            this.Load += new System.EventHandler(this.Зміст_реалізації_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.TextBox idtovarTextBox;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label user;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox kvoTextBox;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox searchrealTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox searchtovarTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox idpokupkaTextBox;
    }
}