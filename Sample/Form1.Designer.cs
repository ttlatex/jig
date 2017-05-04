namespace A1
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.LblTitle = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.TbxID = new System.Windows.Forms.TextBox();
            this.LblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LblTitle
            // 
            this.LblTitle.AutoSize = true;
            this.LblTitle.Font = new System.Drawing.Font("MS UI Gothic", 13F);
            this.LblTitle.Location = new System.Drawing.Point(249, 35);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(108, 18);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "タイトルが入る";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(475, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "検索";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TbxID
            // 
            this.TbxID.Location = new System.Drawing.Point(126, 127);
            this.TbxID.Name = "TbxID";
            this.TbxID.Size = new System.Drawing.Size(265, 19);
            this.TbxID.TabIndex = 2;
            // 
            // LblResult
            // 
            this.LblResult.AutoSize = true;
            this.LblResult.Location = new System.Drawing.Point(177, 279);
            this.LblResult.Name = "LblResult";
            this.LblResult.Size = new System.Drawing.Size(125, 12);
            this.LblResult.TabIndex = 3;
            this.LblResult.Text = "検索結果が表示されます";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 409);
            this.Controls.Add(this.LblResult);
            this.Controls.Add(this.TbxID);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LblTitle);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblTitle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TbxID;
        private System.Windows.Forms.Label LblResult;
    }
}

