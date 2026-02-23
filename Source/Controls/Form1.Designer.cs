namespace KLib.Controls
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            collectionListBox1 = new CollectionListBox();
            SuspendLayout();
            // 
            // collectionListBox1
            // 
            collectionListBox1.AddDropDownItems = (System.ComponentModel.BindingList<string>)resources.GetObject("collectionListBox1.AddDropDownItems");
            collectionListBox1.Alphabetize = true;
            collectionListBox1.AutoSize = true;
            collectionListBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            collectionListBox1.Collection = null;
            collectionListBox1.CreateNewItem = null;
            collectionListBox1.GetDisplayText = null;
            collectionListBox1.ListTitle = "Collection";
            collectionListBox1.Location = new System.Drawing.Point(67, 43);
            collectionListBox1.MinimumSize = new System.Drawing.Size(200, 225);
            collectionListBox1.Name = "collectionListBox1";
            collectionListBox1.ShowAddDropDown = false;
            collectionListBox1.ShowUpDownButtons = true;
            collectionListBox1.Size = new System.Drawing.Size(258, 225);
            collectionListBox1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(collectionListBox1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CollectionListBox collectionListBox1;
    }
}