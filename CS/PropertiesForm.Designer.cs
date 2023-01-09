//
// (C) Copyright 2003-2019 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//


namespace Revit.SDK.Samples.ParameterUtils.CS
{
    partial class PropertiesForm
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
      this.okButton = new System.Windows.Forms.Button();
      this.propertyListView = new System.Windows.Forms.ListView();
      this.buttonToType = new System.Windows.Forms.Button();
      this.buttonToInst = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(386, 636);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 21);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "&OK";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // propertyListView
      // 
      this.propertyListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.propertyListView.FullRowSelect = true;
      this.propertyListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.propertyListView.HideSelection = false;
      this.propertyListView.Location = new System.Drawing.Point(12, 11);
      this.propertyListView.Name = "propertyListView";
      this.propertyListView.Size = new System.Drawing.Size(449, 619);
      this.propertyListView.TabIndex = 2;
      this.propertyListView.UseCompatibleStateImageBehavior = false;
      this.propertyListView.View = System.Windows.Forms.View.Details;
      // 
      // buttonToType
      // 
      this.buttonToType.Location = new System.Drawing.Point(172, 636);
      this.buttonToType.Name = "buttonToType";
      this.buttonToType.Size = new System.Drawing.Size(75, 23);
      this.buttonToType.TabIndex = 3;
      this.buttonToType.Text = "ToType";
      this.buttonToType.UseVisualStyleBackColor = true;
      this.buttonToType.Click += new System.EventHandler(this.buttonToType_Click);
      // 
      // buttonToInst
      // 
      this.buttonToInst.Location = new System.Drawing.Point(282, 636);
      this.buttonToInst.Name = "buttonToInst";
      this.buttonToInst.Size = new System.Drawing.Size(75, 23);
      this.buttonToInst.TabIndex = 4;
      this.buttonToInst.Text = "ToInst";
      this.buttonToInst.UseVisualStyleBackColor = true;
      this.buttonToInst.Click += new System.EventHandler(this.buttonToInst_Click);
      // 
      // PropertiesForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.okButton;
      this.ClientSize = new System.Drawing.Size(473, 668);
      this.Controls.Add(this.buttonToInst);
      this.Controls.Add(this.buttonToType);
      this.Controls.Add(this.propertyListView);
      this.Controls.Add(this.okButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PropertiesForm";
      this.ShowInTaskbar = false;
      this.Text = "Properties";
      this.Load += new System.EventHandler(this.PropertiesForm_Load);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListView propertyListView;
        private System.Windows.Forms.Button buttonToType;
        private System.Windows.Forms.Button buttonToInst;
    }
}