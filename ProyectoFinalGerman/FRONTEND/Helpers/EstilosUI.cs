using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalGerman.FRONTEND.Helpers
{
    public static class EstilosUI
    {
        public static Color ColorFondoHeader = Color.FromArgb(215, 170, 150);
        public static Color ColorTextoHeader = Color.White;
        public static Color ColorFondoFila = Color.White;
        public static Color ColorFilaAlterna = Color.FromArgb(245, 240, 235); 
        public static Color ColorSeleccion = Color.FromArgb(180, 130, 110); 
        public static Color ColorFondoGrid = Color.White;

        public static void AplicarEstiloGrid(DataGridView dgv)
        {
            // General
            dgv.BackgroundColor = SystemColors.Control;
            dgv.BorderStyle = BorderStyle.None; 
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;

            // Encabezado
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorFondoHeader;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextoHeader;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            dgv.ColumnHeadersHeight = 35;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Filas
            dgv.DefaultCellStyle.BackColor = ColorFondoFila;
            dgv.DefaultCellStyle.ForeColor = Color.DimGray;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = ColorSeleccion;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            dgv.RowTemplate.Height = 30;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorFilaAlterna;

            // Limpiar
            dgv.RowHeadersVisible = false; 
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
