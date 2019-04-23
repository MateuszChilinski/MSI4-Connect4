using System;

namespace Connect4Game
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Connect4Board = new System.Windows.Forms.PictureBox();
            this.BlackRadioButton = new System.Windows.Forms.RadioButton();
            this.ChooseLabel = new System.Windows.Forms.Label();
            this.RedRadioButton = new System.Windows.Forms.RadioButton();
            this.RestartButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Connect4Board)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Connect4Board);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.BlackRadioButton);
            this.splitContainer1.Panel2.Controls.Add(this.ChooseLabel);
            this.splitContainer1.Panel2.Controls.Add(this.RedRadioButton);
            this.splitContainer1.Panel2.Controls.Add(this.RestartButton);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 615;
            this.splitContainer1.TabIndex = 0;
            // 
            // Connect4Board
            // 
            this.Connect4Board.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Connect4Board.Location = new System.Drawing.Point(0, 0);
            this.Connect4Board.Name = "Connect4Board";
            this.Connect4Board.Size = new System.Drawing.Size(615, 450);
            this.Connect4Board.TabIndex = 0;
            this.Connect4Board.TabStop = false;
            this.Connect4Board.MouseClick += Connect4Board_Click;
            this.Connect4Board.MouseMove += Connect4BoardOnMouseMove;

            // 
            // BlackRadioButton
            // 
            this.BlackRadioButton.AutoSize = true;
            this.BlackRadioButton.Location = new System.Drawing.Point(2, 106);
            this.BlackRadioButton.Name = "BlackRadioButton";
            this.BlackRadioButton.Size = new System.Drawing.Size(73, 21);
            this.BlackRadioButton.TabIndex = 3;
            this.BlackRadioButton.Text = "Czerwony";
            this.BlackRadioButton.UseVisualStyleBackColor = true;
            this.BlackRadioButton.Click += new System.EventHandler(this.BlackRadioButton_CheckedChanged);
            // 
            // ChooseLabel
            // 
            this.ChooseLabel.AutoSize = true;
            this.ChooseLabel.Location = new System.Drawing.Point(3, 59);
            this.ChooseLabel.Name = "ChooseLabel";
            this.ChooseLabel.Size = new System.Drawing.Size(98, 17);
            this.ChooseLabel.TabIndex = 2;
            this.ChooseLabel.Text = "Wybierz kolor:";
            // 
            // RedRadioButton
            // 
            this.RedRadioButton.AutoSize = true;
            this.RedRadioButton.Checked = true;
            this.RedRadioButton.Location = new System.Drawing.Point(2, 79);
            this.RedRadioButton.Name = "RedRadioButton";
            this.RedRadioButton.Size = new System.Drawing.Size(90, 21);
            this.RedRadioButton.TabIndex = 1;
            this.RedRadioButton.TabStop = true;
            this.RedRadioButton.Text = "Zolty";
            this.RedRadioButton.UseVisualStyleBackColor = true;
            this.RedRadioButton.Click += new System.EventHandler(this.RedRadioButton_CheckedChanged);
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(39, 12);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(110, 35);
            this.RestartButton.TabIndex = 0;
            this.RestartButton.Text = "Restartuj gre";
            this.RestartButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Connect4Board)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox Connect4Board;
        private System.Windows.Forms.RadioButton BlackRadioButton;
        private System.Windows.Forms.Label ChooseLabel;
        private System.Windows.Forms.RadioButton RedRadioButton;
        private System.Windows.Forms.Button RestartButton;
    }
}

