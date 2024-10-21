namespace SoldiersWindApps
{
    partial class Soldiers_Form
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
            this.gMapControl_Position = new GMap.NET.WindowsForms.GMapControl();
            this.SuspendLayout();
            // 
            // gMapControl_Position
            // 
            this.gMapControl_Position.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gMapControl_Position.Bearing = 0F;
            this.gMapControl_Position.CanDragMap = true;
            this.gMapControl_Position.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapControl_Position.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl_Position.GrayScaleMode = true;
            this.gMapControl_Position.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl_Position.LevelsKeepInMemory = 5;
            this.gMapControl_Position.Location = new System.Drawing.Point(0, 0);
            this.gMapControl_Position.MarkersEnabled = true;
            this.gMapControl_Position.MaxZoom = 16;
            this.gMapControl_Position.MinZoom = 5;
            this.gMapControl_Position.MouseWheelZoomEnabled = true;
            this.gMapControl_Position.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl_Position.Name = "gMapControl_Position";
            this.gMapControl_Position.NegativeMode = false;
            this.gMapControl_Position.PolygonsEnabled = true;
            this.gMapControl_Position.RetryLoadTile = 0;
            this.gMapControl_Position.RoutesEnabled = true;
            this.gMapControl_Position.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl_Position.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl_Position.ShowTileGridLines = false;
            this.gMapControl_Position.Size = new System.Drawing.Size(2186, 1429);
            this.gMapControl_Position.TabIndex = 0;
            this.gMapControl_Position.Zoom = 16D;
            // 
            // Soldiers_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(2186, 1429);
            this.Controls.Add(this.gMapControl_Position);
            this.Name = "Soldiers_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControl_Position;
    }
}

