namespace FeedDeck
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
            this.accountName = new System.Windows.Forms.TextBox();
            this.feedThreads = new System.Windows.Forms.TextBox();
            this.threadCount = new System.Windows.Forms.NumericUpDown();
            this.getActivities = new System.Windows.Forms.Button();
            this.responsePost = new System.Windows.Forms.TextBox();
            this.postReply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.threadCount)).BeginInit();
            this.SuspendLayout();
            // 
            // accountName
            // 
            this.accountName.Location = new System.Drawing.Point(26, 46);
            this.accountName.Name = "accountName";
            this.accountName.Size = new System.Drawing.Size(357, 20);
            this.accountName.TabIndex = 0;
            // 
            // feedThreads
            // 
            this.feedThreads.Location = new System.Drawing.Point(26, 88);
            this.feedThreads.Multiline = true;
            this.feedThreads.Name = "feedThreads";
            this.feedThreads.Size = new System.Drawing.Size(479, 222);
            this.feedThreads.TabIndex = 1;
            // 
            // threadCount
            // 
            this.threadCount.Location = new System.Drawing.Point(26, 334);
            this.threadCount.Name = "threadCount";
            this.threadCount.Size = new System.Drawing.Size(57, 20);
            this.threadCount.TabIndex = 2;
            // 
            // getActivities
            // 
            this.getActivities.Location = new System.Drawing.Point(401, 43);
            this.getActivities.Name = "getActivities";
            this.getActivities.Size = new System.Drawing.Size(104, 23);
            this.getActivities.TabIndex = 3;
            this.getActivities.Text = "Get Activities";
            this.getActivities.UseVisualStyleBackColor = true;
            this.getActivities.Click += new System.EventHandler(this.getActivities_Click);
            // 
            // responsePost
            // 
            this.responsePost.Location = new System.Drawing.Point(89, 334);
            this.responsePost.Name = "responsePost";
            this.responsePost.Size = new System.Drawing.Size(335, 20);
            this.responsePost.TabIndex = 4;
            // 
            // postReply
            // 
            this.postReply.Location = new System.Drawing.Point(430, 331);
            this.postReply.Name = "postReply";
            this.postReply.Size = new System.Drawing.Size(75, 23);
            this.postReply.TabIndex = 5;
            this.postReply.Text = "Post Reply";
            this.postReply.UseVisualStyleBackColor = true;
            this.postReply.Click += new System.EventHandler(this.postReply_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 380);
            this.Controls.Add(this.postReply);
            this.Controls.Add(this.responsePost);
            this.Controls.Add(this.getActivities);
            this.Controls.Add(this.threadCount);
            this.Controls.Add(this.feedThreads);
            this.Controls.Add(this.accountName);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.threadCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox accountName;
        private System.Windows.Forms.TextBox feedThreads;
        private System.Windows.Forms.NumericUpDown threadCount;
        private System.Windows.Forms.Button getActivities;
        private System.Windows.Forms.TextBox responsePost;
        private System.Windows.Forms.Button postReply;
    }
}

