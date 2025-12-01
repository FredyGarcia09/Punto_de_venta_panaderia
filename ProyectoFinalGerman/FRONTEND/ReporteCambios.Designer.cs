namespace ProyectoFinalGerman
{
    partial class ReporteCambios
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRegresar = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.clbProductos = new System.Windows.Forms.CheckedListBox();
            this.dtpMes1 = new System.Windows.Forms.DateTimePicker();
            this.dtpMes2 = new System.Windows.Forms.DateTimePicker();
            this.btnComparar = new System.Windows.Forms.Button();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.chartVentas = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartVentas)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRegresar);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(1, -6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1882, 78);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // btnRegresar
            // 
            this.btnRegresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegresar.Image = global::ProyectoFinalGerman.Properties.Resources.atras;
            this.btnRegresar.Location = new System.Drawing.Point(11, 9);
            this.btnRegresar.Name = "btnRegresar";
            this.btnRegresar.Size = new System.Drawing.Size(63, 61);
            this.btnRegresar.TabIndex = 0;
            this.btnRegresar.UseVisualStyleBackColor = true;
            this.btnRegresar.Click += new System.EventHandler(this.btnRegresar_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(1, 652);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1882, 119);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ProyectoFinalGerman.Properties.Resources.Logotipo_panadería_artesanal_vintage_amarillo;
            this.pictureBox1.Location = new System.Drawing.Point(1813, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(58, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // clbProductos
            // 
            this.clbProductos.FormattingEnabled = true;
            this.clbProductos.Location = new System.Drawing.Point(7, 113);
            this.clbProductos.Name = "clbProductos";
            this.clbProductos.Size = new System.Drawing.Size(184, 514);
            this.clbProductos.TabIndex = 0;
            // 
            // dtpMes1
            // 
            this.dtpMes1.Location = new System.Drawing.Point(514, 113);
            this.dtpMes1.Name = "dtpMes1";
            this.dtpMes1.Size = new System.Drawing.Size(200, 22);
            this.dtpMes1.TabIndex = 1;
            // 
            // dtpMes2
            // 
            this.dtpMes2.Location = new System.Drawing.Point(956, 117);
            this.dtpMes2.Name = "dtpMes2";
            this.dtpMes2.Size = new System.Drawing.Size(200, 22);
            this.dtpMes2.TabIndex = 2;
            // 
            // btnComparar
            // 
            this.btnComparar.Location = new System.Drawing.Point(1399, 113);
            this.btnComparar.Name = "btnComparar";
            this.btnComparar.Size = new System.Drawing.Size(119, 35);
            this.btnComparar.TabIndex = 3;
            this.btnComparar.Text = "Comparar";
            this.btnComparar.UseVisualStyleBackColor = true;
            this.btnComparar.Click += new System.EventHandler(this.btnComparar_Click);
            // 
            // dgvDatos
            // 
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Location = new System.Drawing.Point(222, 158);
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.RowHeadersWidth = 51;
            this.dgvDatos.RowTemplate.Height = 24;
            this.dgvDatos.Size = new System.Drawing.Size(809, 469);
            this.dgvDatos.TabIndex = 8;
            // 
            // chartVentas
            // 
            chartArea1.Name = "ChartArea1";
            this.chartVentas.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartVentas.Legends.Add(legend1);
            this.chartVentas.Location = new System.Drawing.Point(1062, 158);
            this.chartVentas.Name = "chartVentas";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartVentas.Series.Add(series1);
            this.chartVentas.Size = new System.Drawing.Size(801, 469);
            this.chartVentas.TabIndex = 9;
            this.chartVentas.Text = "chart1";
            // 
            // ReporteCambios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1884, 718);
            this.Controls.Add(this.chartVentas);
            this.Controls.Add(this.dgvDatos);
            this.Controls.Add(this.btnComparar);
            this.Controls.Add(this.dtpMes2);
            this.Controls.Add(this.dtpMes1);
            this.Controls.Add(this.clbProductos);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ReporteCambios";
            this.Text = "ReporteCambios";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ReporteCambios_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartVentas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRegresar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckedListBox clbProductos;
        private System.Windows.Forms.DateTimePicker dtpMes1;
        private System.Windows.Forms.DateTimePicker dtpMes2;
        private System.Windows.Forms.Button btnComparar;
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartVentas;
    }
}