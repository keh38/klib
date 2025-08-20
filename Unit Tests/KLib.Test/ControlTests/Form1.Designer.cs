namespace ControlTests
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
            this.collectionListBox1 = new KLib.Controls.CollectionListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // collectionListBox1
            // 
            this.collectionListBox1.AddDropDownItems = ((System.ComponentModel.BindingList<string>)(resources.GetObject("collectionListBox1.AddDropDownItems")));
            this.collectionListBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.collectionListBox1.Collection = null;
            this.collectionListBox1.CreateNewItem = null;
            this.collectionListBox1.GetDisplayText = null;
            this.collectionListBox1.ListTitle = "Collection";
            this.collectionListBox1.Location = new System.Drawing.Point(75, 63);
            this.collectionListBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.collectionListBox1.MinimumSize = new System.Drawing.Size(200, 180);
            this.collectionListBox1.Name = "collectionListBox1";
            this.collectionListBox1.ShowAddDropDown = true;
            this.collectionListBox1.Size = new System.Drawing.Size(265, 224);
            this.collectionListBox1.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(421, 83);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(289, 338);
            this.propertyGrid1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(872, 519);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.collectionListBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Control Tests";
            this.ResumeLayout(false);

        }

        #endregion

        private KLib.Controls.CollectionListBox collectionListBox1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}

