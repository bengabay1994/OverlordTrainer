
namespace OverLordTrainer
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
            this.HeaderFoundLabel = new System.Windows.Forms.Label();
            this.FoundGameL = new System.Windows.Forms.Label();
            this.BrownMinionsTB = new System.Windows.Forms.TextBox();
            this.LifeForceMultiplierCB = new System.Windows.Forms.CheckBox();
            this.InfiniteHealthCB = new System.Windows.Forms.CheckBox();
            this.InfiniteManaCB = new System.Windows.Forms.CheckBox();
            this.GetGoldTB = new System.Windows.Forms.TextBox();
            this.GoldB = new System.Windows.Forms.Button();
            this.MaxMinionSummonTB = new System.Windows.Forms.TextBox();
            this.ManMinionsSummonB = new System.Windows.Forms.Button();
            this.OneHitKillCB = new System.Windows.Forms.CheckBox();
            this.DamageMultiplierCB = new System.Windows.Forms.CheckBox();
            this.DamageMultiplierTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // HeaderFoundLabel
            // 
            this.HeaderFoundLabel.AutoSize = true;
            this.HeaderFoundLabel.Location = new System.Drawing.Point(12, 9);
            this.HeaderFoundLabel.Name = "HeaderFoundLabel";
            this.HeaderFoundLabel.Size = new System.Drawing.Size(41, 15);
            this.HeaderFoundLabel.TabIndex = 1;
            this.HeaderFoundLabel.Text = "Game:";
            // 
            // FoundGameL
            // 
            this.FoundGameL.AutoSize = true;
            this.FoundGameL.Location = new System.Drawing.Point(59, 9);
            this.FoundGameL.Name = "FoundGameL";
            this.FoundGameL.Size = new System.Drawing.Size(29, 15);
            this.FoundGameL.TabIndex = 2;
            this.FoundGameL.Text = "N/A";
            // 
            // BrownMinionsTB
            // 
            this.BrownMinionsTB.Location = new System.Drawing.Point(199, 26);
            this.BrownMinionsTB.Name = "BrownMinionsTB";
            this.BrownMinionsTB.Size = new System.Drawing.Size(100, 23);
            this.BrownMinionsTB.TabIndex = 4;
            // 
            // LifeForceMultiplierCB
            // 
            this.LifeForceMultiplierCB.AutoSize = true;
            this.LifeForceMultiplierCB.Location = new System.Drawing.Point(12, 30);
            this.LifeForceMultiplierCB.Name = "LifeForceMultiplierCB";
            this.LifeForceMultiplierCB.Size = new System.Drawing.Size(187, 19);
            this.LifeForceMultiplierCB.TabIndex = 5;
            this.LifeForceMultiplierCB.Text = "Life Force Multiplier (LCtrl + L)";
            this.LifeForceMultiplierCB.UseVisualStyleBackColor = true;
            this.LifeForceMultiplierCB.CheckedChanged += new System.EventHandler(this.LifeForceMultiplierCB_CheckedChanged);
            // 
            // InfiniteHealthCB
            // 
            this.InfiniteHealthCB.AutoSize = true;
            this.InfiniteHealthCB.Location = new System.Drawing.Point(12, 56);
            this.InfiniteHealthCB.Name = "InfiniteHealthCB";
            this.InfiniteHealthCB.Size = new System.Drawing.Size(160, 19);
            this.InfiniteHealthCB.TabIndex = 6;
            this.InfiniteHealthCB.Text = "Infinite Health (LCtrl + H)";
            this.InfiniteHealthCB.UseVisualStyleBackColor = true;
            this.InfiniteHealthCB.CheckedChanged += new System.EventHandler(this.InfiniteHealthCB_CheckedChanged);
            // 
            // InfiniteManaCB
            // 
            this.InfiniteManaCB.AutoSize = true;
            this.InfiniteManaCB.Location = new System.Drawing.Point(12, 81);
            this.InfiniteManaCB.Name = "InfiniteManaCB";
            this.InfiniteManaCB.Size = new System.Drawing.Size(157, 19);
            this.InfiniteManaCB.TabIndex = 7;
            this.InfiniteManaCB.Text = "Infinite Mana (LCtrl + M)";
            this.InfiniteManaCB.UseVisualStyleBackColor = true;
            this.InfiniteManaCB.CheckedChanged += new System.EventHandler(this.InfiniteManaCB_CheckedChanged);
            // 
            // GetGoldTB
            // 
            this.GetGoldTB.Location = new System.Drawing.Point(147, 105);
            this.GetGoldTB.Name = "GetGoldTB";
            this.GetGoldTB.Size = new System.Drawing.Size(100, 23);
            this.GetGoldTB.TabIndex = 8;
            // 
            // GoldB
            // 
            this.GoldB.Location = new System.Drawing.Point(12, 105);
            this.GoldB.Name = "GoldB";
            this.GoldB.Size = new System.Drawing.Size(129, 23);
            this.GoldB.TabIndex = 9;
            this.GoldB.Text = "Get Gold  (LCtrl + G)";
            this.GoldB.UseVisualStyleBackColor = true;
            this.GoldB.Click += new System.EventHandler(this.GoldB_Click);
            // 
            // MaxMinionSummonTB
            // 
            this.MaxMinionSummonTB.Location = new System.Drawing.Point(283, 135);
            this.MaxMinionSummonTB.Name = "MaxMinionSummonTB";
            this.MaxMinionSummonTB.Size = new System.Drawing.Size(100, 23);
            this.MaxMinionSummonTB.TabIndex = 10;
            // 
            // ManMinionsSummonB
            // 
            this.ManMinionsSummonB.Location = new System.Drawing.Point(12, 135);
            this.ManMinionsSummonB.Name = "ManMinionsSummonB";
            this.ManMinionsSummonB.Size = new System.Drawing.Size(265, 23);
            this.ManMinionsSummonB.TabIndex = 11;
            this.ManMinionsSummonB.Text = "Max Minion To Summon (LCtrl + Numpad9)";
            this.ManMinionsSummonB.UseVisualStyleBackColor = true;
            this.ManMinionsSummonB.Click += new System.EventHandler(this.ManMinionsSummonB_Click);
            // 
            // OneHitKillCB
            // 
            this.OneHitKillCB.AutoSize = true;
            this.OneHitKillCB.Location = new System.Drawing.Point(12, 165);
            this.OneHitKillCB.Name = "OneHitKillCB";
            this.OneHitKillCB.Size = new System.Drawing.Size(305, 19);
            this.OneHitKillCB.TabIndex = 12;
            this.OneHitKillCB.Text = "One Hit Kill (also kill your minions in 1 hit) (LCtrl + K)";
            this.OneHitKillCB.UseVisualStyleBackColor = true;
            this.OneHitKillCB.CheckedChanged += new System.EventHandler(this.OneHitKillCB_CheckedChanged);
            // 
            // DamageMultiplierB
            // 
            this.DamageMultiplierCB.AutoSize = true;
            this.DamageMultiplierCB.Location = new System.Drawing.Point(12, 191);
            this.DamageMultiplierCB.Name = "DamageMultiplierB";
            this.DamageMultiplierCB.Size = new System.Drawing.Size(241, 19);
            this.DamageMultiplierCB.TabIndex = 13;
            this.DamageMultiplierCB.Text = "Damage Multiplier (Default x2, LCtrl + D)";
            this.DamageMultiplierCB.UseVisualStyleBackColor = true;
            this.DamageMultiplierCB.CheckedChanged += new System.EventHandler(this.DamageMultiplierB_CheckedChanged);
            // 
            // DamageMultiplierTB
            // 
            this.DamageMultiplierTB.Location = new System.Drawing.Point(259, 187);
            this.DamageMultiplierTB.Name = "DamageMultiplierTB";
            this.DamageMultiplierTB.Size = new System.Drawing.Size(100, 23);
            this.DamageMultiplierTB.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DamageMultiplierTB);
            this.Controls.Add(this.DamageMultiplierCB);
            this.Controls.Add(this.OneHitKillCB);
            this.Controls.Add(this.ManMinionsSummonB);
            this.Controls.Add(this.MaxMinionSummonTB);
            this.Controls.Add(this.GoldB);
            this.Controls.Add(this.GetGoldTB);
            this.Controls.Add(this.InfiniteManaCB);
            this.Controls.Add(this.InfiniteHealthCB);
            this.Controls.Add(this.LifeForceMultiplierCB);
            this.Controls.Add(this.BrownMinionsTB);
            this.Controls.Add(this.FoundGameL);
            this.Controls.Add(this.HeaderFoundLabel);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label HeaderFoundLabel;
        private System.Windows.Forms.Label FoundGameL;
        private System.Windows.Forms.TextBox BrownMinionsTB;
        private System.Windows.Forms.CheckBox LifeForceMultiplierCB;
        private System.Windows.Forms.CheckBox InfiniteHealthCB;
        private System.Windows.Forms.CheckBox InfiniteManaCB;
        private System.Windows.Forms.TextBox GetGoldTB;
        private System.Windows.Forms.Button GoldB;
        private System.Windows.Forms.TextBox MaxMinionSummonTB;
        private System.Windows.Forms.Button ManMinionsSummonB;
        private System.Windows.Forms.CheckBox OneHitKillCB;
        private System.Windows.Forms.CheckBox DamageMultiplierCB;
        private System.Windows.Forms.TextBox DamageMultiplierTB;
    }
}

