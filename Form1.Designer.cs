
namespace BOCollector
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.HealthBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelEnemyNearby = new System.Windows.Forms.Label();
            this.ConsoleBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(150, 203);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(150, 174);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Mouse Pos";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 278);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(241, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.Name = "StatusLabel1";
            this.StatusLabel1.Size = new System.Drawing.Size(30, 17);
            this.StatusLabel1.Text = "load";
            // 
            // HealthBar
            // 
            this.HealthBar.Location = new System.Drawing.Point(57, 180);
            this.HealthBar.Name = "HealthBar";
            this.HealthBar.Size = new System.Drawing.Size(82, 13);
            this.HealthBar.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Health";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Enemy nearby";
            // 
            // labelEnemyNearby
            // 
            this.labelEnemyNearby.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labelEnemyNearby.Location = new System.Drawing.Point(100, 208);
            this.labelEnemyNearby.Name = "labelEnemyNearby";
            this.labelEnemyNearby.Size = new System.Drawing.Size(39, 13);
            this.labelEnemyNearby.TabIndex = 9;
            this.labelEnemyNearby.Text = "_";
            this.labelEnemyNearby.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ConsoleBox
            // 
            this.ConsoleBox.Location = new System.Drawing.Point(12, 12);
            this.ConsoleBox.Multiline = true;
            this.ConsoleBox.Name = "ConsoleBox";
            this.ConsoleBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConsoleBox.Size = new System.Drawing.Size(213, 156);
            this.ConsoleBox.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(150, 232);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 300);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelEnemyNearby);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HealthBar);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ConsoleBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel1;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ProgressBar HealthBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelEnemyNearby;
        private System.Windows.Forms.TextBox ConsoleBox;
        private System.Windows.Forms.Button button3;
    }
}

