/*
 * Created by SharpDevelop.
 * User: HKA_250
 * Date: 26/08/2012
 * Time: 12:08 | HKA
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace A_Star_8_Puzzle
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.pbGame = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnResolve = new System.Windows.Forms.Button();
            this.bntNew = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.timerPlay = new System.Windows.Forms.Timer(this.components);
            this.lblFinish = new System.Windows.Forms.Label();
            this.lblCountStatistic = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Candara", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(153, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "8 Puzzle A*";
            // 
            // pbGame
            // 
            this.pbGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbGame.Location = new System.Drawing.Point(12, 103);
            this.pbGame.Name = "pbGame";
            this.pbGame.Size = new System.Drawing.Size(288, 288);
            this.pbGame.TabIndex = 1;
            this.pbGame.TabStop = false;
            this.pbGame.Paint += new System.Windows.Forms.PaintEventHandler(this.pbGame_Paint);
            this.pbGame.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbGame_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnReset);
            this.groupBox1.Controls.Add(this.btnResolve);
            this.groupBox1.Controls.Add(this.bntNew);
            this.groupBox1.Controls.Add(this.lblCount);
            this.groupBox1.Location = new System.Drawing.Point(306, 178);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 213);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Số bước đi";
            // 
            // btnReset
            // 
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.Location = new System.Drawing.Point(7, 166);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(99, 29);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnResolve
            // 
            this.btnResolve.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResolve.Location = new System.Drawing.Point(7, 118);
            this.btnResolve.Name = "btnResolve";
            this.btnResolve.Size = new System.Drawing.Size(99, 32);
            this.btnResolve.TabIndex = 2;
            this.btnResolve.Text = "Giải";
            this.btnResolve.UseVisualStyleBackColor = true;
            this.btnResolve.Click += new System.EventHandler(this.btnResolve_Click);
            // 
            // bntNew
            // 
            this.bntNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bntNew.Location = new System.Drawing.Point(7, 68);
            this.bntNew.Name = "bntNew";
            this.bntNew.Size = new System.Drawing.Size(99, 34);
            this.bntNew.TabIndex = 1;
            this.bntNew.Text = "Tạo mới";
            this.bntNew.UseVisualStyleBackColor = true;
            this.bntNew.Click += new System.EventHandler(this.bntNew_Click);
            // 
            // lblCount
            // 
            this.lblCount.Font = new System.Drawing.Font("Candara", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCount.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblCount.Location = new System.Drawing.Point(6, 38);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(100, 27);
            this.lblCount.TabIndex = 0;
            this.lblCount.Text = "0";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerPlay
            // 
            this.timerPlay.Tick += new System.EventHandler(this.timerPlay_Tick);
            // 
            // lblFinish
            // 
            this.lblFinish.AutoSize = true;
            this.lblFinish.Font = new System.Drawing.Font("Candara", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblFinish.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblFinish.Location = new System.Drawing.Point(35, 32);
            this.lblFinish.Name = "lblFinish";
            this.lblFinish.Size = new System.Drawing.Size(0, 18);
            this.lblFinish.TabIndex = 3;
            // 
            // lblCountStatistic
            // 
            this.lblCountStatistic.AutoSize = true;
            this.lblCountStatistic.Font = new System.Drawing.Font("Candara", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCountStatistic.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblCountStatistic.Location = new System.Drawing.Point(35, 66);
            this.lblCountStatistic.Name = "lblCountStatistic";
            this.lblCountStatistic.Size = new System.Drawing.Size(0, 15);
            this.lblCountStatistic.TabIndex = 4;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 397);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(406, 21);
            this.progressBar1.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(433, 427);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblCountStatistic);
            this.Controls.Add(this.lblFinish);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbGame);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "A Star 8 Puzzle";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Timer timerPlay;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnResolve;
		private System.Windows.Forms.Button bntNew;
		private System.Windows.Forms.Label lblCount;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pbGame;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFinish;
        private System.Windows.Forms.Label lblCountStatistic;
        private System.Windows.Forms.ProgressBar progressBar1;
	}
}
