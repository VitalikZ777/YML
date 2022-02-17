namespace YML_Forms
{
    partial class Form1
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
            this.button3 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.checkBox_preorder = new System.Windows.Forms.CheckBox();
            this.button10 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_progress = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox_vsitovaru = new System.Windows.Forms.CheckBox();
            this.checkBox_vnayavnosti = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(339, 163);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 84);
            this.button3.TabIndex = 6;
            this.button3.Text = "Сформувати файл вигрузки для Rozetka";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.Location = new System.Drawing.Point(33, 29);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(194, 207);
            this.treeView1.TabIndex = 14;
            this.treeView1.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCheck);
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // checkBox_preorder
            // 
            this.checkBox_preorder.AutoSize = true;
            this.checkBox_preorder.Location = new System.Drawing.Point(243, 49);
            this.checkBox_preorder.Name = "checkBox_preorder";
            this.checkBox_preorder.Size = new System.Drawing.Size(194, 17);
            this.checkBox_preorder.TabIndex = 22;
            this.checkBox_preorder.Text = "Вибирати товари під замовлення";
            this.checkBox_preorder.UseVisualStyleBackColor = true;
            this.checkBox_preorder.CheckedChanged += new System.EventHandler(this.checkBox_preorder_CheckedChanged);
            // 
            // button10
            // 
            this.button10.AutoSize = true;
            this.button10.Location = new System.Drawing.Point(267, 306);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 24;
            this.button10.Text = "Проба";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Visible = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(33, 277);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(497, 23);
            this.progressBar1.TabIndex = 25;
            // 
            // label_progress
            // 
            this.label_progress.AutoSize = true;
            this.label_progress.Location = new System.Drawing.Point(30, 248);
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(131, 13);
            this.label_progress.TabIndex = 26;
            this.label_progress.Text = "Всього товарів додано:0";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(433, 163);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(88, 84);
            this.button11.TabIndex = 27;
            this.button11.Text = "Сформувати файл вигрузки для Prom";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 311);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Додано до XML:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(245, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 84);
            this.button1.TabIndex = 29;
            this.button1.Text = "Сформувати файл вигрузки для Hotline";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox_vsitovaru
            // 
            this.checkBox_vsitovaru.AutoSize = true;
            this.checkBox_vsitovaru.Location = new System.Drawing.Point(243, 72);
            this.checkBox_vsitovaru.Name = "checkBox_vsitovaru";
            this.checkBox_vsitovaru.Size = new System.Drawing.Size(275, 17);
            this.checkBox_vsitovaru.TabIndex = 30;
            this.checkBox_vsitovaru.Text = "Вибирати товари ВСІ (наявність+під замовлення)";
            this.checkBox_vsitovaru.UseVisualStyleBackColor = true;
            this.checkBox_vsitovaru.CheckedChanged += new System.EventHandler(this.checkBox_vsitovaru_CheckedChanged);
            // 
            // checkBox_vnayavnosti
            // 
            this.checkBox_vnayavnosti.AutoSize = true;
            this.checkBox_vnayavnosti.Checked = true;
            this.checkBox_vnayavnosti.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_vnayavnosti.Location = new System.Drawing.Point(243, 26);
            this.checkBox_vnayavnosti.Name = "checkBox_vnayavnosti";
            this.checkBox_vnayavnosti.Size = new System.Drawing.Size(173, 17);
            this.checkBox_vnayavnosti.TabIndex = 31;
            this.checkBox_vnayavnosti.Text = "Вибирати товари в наявності";
            this.checkBox_vnayavnosti.UseVisualStyleBackColor = true;
            this.checkBox_vnayavnosti.CheckedChanged += new System.EventHandler(this.checkBox_vnayavnosti_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 334);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Сформовано ссилку:";
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(149, 331);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(381, 20);
            this.tbUrl.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(558, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Вибір виробника:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(561, 61);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(185, 95);
            this.listBox1.TabIndex = 36;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(253, 109);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(44, 17);
            this.radioButton1.TabIndex = 38;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Так";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(254, 128);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(35, 17);
            this.radioButton2.TabIndex = 39;
            this.radioButton2.Text = "Ні";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.groupBox1.Location = new System.Drawing.Point(245, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 58);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Копіювати файл на сервер";
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.Location = new System.Drawing.Point(552, 194);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 41;
            this.button2.Text = "Проба";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 362);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_vnayavnosti);
            this.Controls.Add(this.checkBox_vsitovaru);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.label_progress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.checkBox_preorder);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "YML";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.CheckBox checkBox_preorder;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_progress;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_vsitovaru;
        private System.Windows.Forms.CheckBox checkBox_vnayavnosti;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
    }
}

