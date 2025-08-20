namespace KLib.Controls
{
    partial class CollectionListBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionListBox));
            listBox = new System.Windows.Forms.ListBox();
            addDropDown = new System.Windows.Forms.ComboBox();
            removeButton = new System.Windows.Forms.Button();
            listBoxLabel = new System.Windows.Forms.Label();
            downButton = new System.Windows.Forms.Button();
            upButton = new System.Windows.Forms.Button();
            addButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // listBox
            // 
            listBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            listBox.FormattingEnabled = true;
            listBox.Location = new System.Drawing.Point(3, 26);
            listBox.Name = "listBox";
            listBox.Size = new System.Drawing.Size(211, 224);
            listBox.TabIndex = 0;
            listBox.DrawItem += listBox_DrawItem;
            listBox.SelectedIndexChanged += listBox_SelectedIndexChanged;
            // 
            // addDropDown
            // 
            addDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            addDropDown.FormattingEnabled = true;
            addDropDown.Location = new System.Drawing.Point(3, 255);
            addDropDown.Name = "addDropDown";
            addDropDown.Size = new System.Drawing.Size(95, 28);
            addDropDown.TabIndex = 1;
            addDropDown.Visible = false;
            addDropDown.SelectedIndexChanged += addDropDown_SelectedIndexChanged;
            addDropDown.VisibleChanged += addDropDown_VisibleChanged;
            // 
            // removeButton
            // 
            removeButton.Location = new System.Drawing.Point(114, 254);
            removeButton.Name = "removeButton";
            removeButton.Size = new System.Drawing.Size(100, 29);
            removeButton.TabIndex = 2;
            removeButton.Text = "Remove";
            removeButton.UseVisualStyleBackColor = true;
            removeButton.Click += removeButton_Click;
            // 
            // listBoxLabel
            // 
            listBoxLabel.AutoSize = true;
            listBoxLabel.Location = new System.Drawing.Point(3, 3);
            listBoxLabel.Name = "listBoxLabel";
            listBoxLabel.Size = new System.Drawing.Size(76, 20);
            listBoxLabel.TabIndex = 4;
            listBoxLabel.Text = "Collection";
            // 
            // downButton
            // 
            downButton.Image = (System.Drawing.Image)resources.GetObject("downButton.Image");
            downButton.Location = new System.Drawing.Point(220, 64);
            downButton.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            downButton.Name = "downButton";
            downButton.Size = new System.Drawing.Size(32, 32);
            downButton.TabIndex = 5;
            downButton.UseVisualStyleBackColor = true;
            downButton.Click += downButton_Click;
            // 
            // upButton
            // 
            upButton.Image = (System.Drawing.Image)resources.GetObject("upButton.Image");
            upButton.Location = new System.Drawing.Point(220, 26);
            upButton.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            upButton.Name = "upButton";
            upButton.Size = new System.Drawing.Size(32, 32);
            upButton.TabIndex = 6;
            upButton.UseVisualStyleBackColor = true;
            upButton.Click += upButton_Click;
            // 
            // addButton
            // 
            addButton.Location = new System.Drawing.Point(4, 254);
            addButton.Name = "addButton";
            addButton.Size = new System.Drawing.Size(100, 29);
            addButton.TabIndex = 7;
            addButton.Text = "Add";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // CollectionListBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            Controls.Add(addButton);
            Controls.Add(upButton);
            Controls.Add(downButton);
            Controls.Add(listBoxLabel);
            Controls.Add(removeButton);
            Controls.Add(addDropDown);
            Controls.Add(listBox);
            MinimumSize = new System.Drawing.Size(200, 225);
            Name = "CollectionListBox";
            Size = new System.Drawing.Size(258, 286);
            Resize += CollectionListBox_Resize;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.ComboBox addDropDown;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Label listBoxLabel;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button addButton;
    }
}
