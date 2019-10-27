namespace Archivos
{
    partial class Ventana_Indice
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
            this.gridPrimerBloque = new System.Windows.Forms.DataGridView();
            this.gridSegundoBloque = new System.Windows.Forms.DataGridView();
            this.gridSecPrimBloq = new System.Windows.Forms.DataGridView();
            this.gridSecSegBloq = new System.Windows.Forms.DataGridView();
            this.gridArbol = new System.Windows.Forms.DataGridView();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridPrimerBloque)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSegundoBloque)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSecPrimBloq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSecSegBloq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridArbol)).BeginInit();
            this.SuspendLayout();
            // 
            // gridPrimerBloque
            // 
            this.gridPrimerBloque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPrimerBloque.Location = new System.Drawing.Point(0, 26);
            this.gridPrimerBloque.Name = "gridPrimerBloque";
            this.gridPrimerBloque.Size = new System.Drawing.Size(451, 212);
            this.gridPrimerBloque.TabIndex = 0;
            // 
            // gridSegundoBloque
            // 
            this.gridSegundoBloque.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSegundoBloque.Location = new System.Drawing.Point(486, 26);
            this.gridSegundoBloque.Name = "gridSegundoBloque";
            this.gridSegundoBloque.Size = new System.Drawing.Size(451, 212);
            this.gridSegundoBloque.TabIndex = 1;
            // 
            // gridSecPrimBloq
            // 
            this.gridSecPrimBloq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSecPrimBloq.Location = new System.Drawing.Point(0, 276);
            this.gridSecPrimBloq.Name = "gridSecPrimBloq";
            this.gridSecPrimBloq.Size = new System.Drawing.Size(451, 246);
            this.gridSecPrimBloq.TabIndex = 2;
            // 
            // gridSecSegBloq
            // 
            this.gridSecSegBloq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSecSegBloq.Location = new System.Drawing.Point(486, 276);
            this.gridSecSegBloq.Name = "gridSecSegBloq";
            this.gridSecSegBloq.Size = new System.Drawing.Size(451, 246);
            this.gridSecSegBloq.TabIndex = 3;
            // 
            // gridArbol
            // 
            this.gridArbol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArbol.Location = new System.Drawing.Point(0, 26);
            this.gridArbol.Name = "gridArbol";
            this.gridArbol.Size = new System.Drawing.Size(937, 496);
            this.gridArbol.TabIndex = 4;
            this.gridArbol.Visible = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(87, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Mostrar arbol";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Ventana_Indice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 523);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.gridArbol);
            this.Controls.Add(this.gridSecSegBloq);
            this.Controls.Add(this.gridSecPrimBloq);
            this.Controls.Add(this.gridSegundoBloque);
            this.Controls.Add(this.gridPrimerBloque);
            this.Name = "Ventana_Indice";
            this.Text = "Ventana_Indice";
            ((System.ComponentModel.ISupportInitialize)(this.gridPrimerBloque)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSegundoBloque)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSecPrimBloq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSecSegBloq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridArbol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridPrimerBloque;
        private System.Windows.Forms.DataGridView gridSegundoBloque;
        private System.Windows.Forms.DataGridView gridSecPrimBloq;
        private System.Windows.Forms.DataGridView gridSecSegBloq;
        private System.Windows.Forms.DataGridView gridArbol;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}