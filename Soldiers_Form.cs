using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json;
using Soldiers_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoldiersWindApps
{
    public partial class Soldiers_Form : Form
    {
        #region Variables
        private Dictionary<int, GMapMarker> markers = new Dictionary<int, GMapMarker>(); 
        private RootObject data; 
        private GMapMarker draggedMarker; 
        private bool isDragging; 
        private string jsonFilePath = "SoldierData.json"; 
        private GMapOverlay markersOverlay;

        #endregion

        #region Constructor
        public Soldiers_Form()
        {
            InitializeComponent();
            InitializeMap();
            SetupMapControl(gMapControl_Position); 
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the GMapControl with Google Maps provider and adds a marker overlay.
        /// </summary>
        private void InitializeMap()
        {
            gMapControl_Position.MapProvider = GMapProviders.GoogleMap;
            gMapControl_Position.ShowCenter = false;
            markersOverlay = new GMapOverlay("markers");
            gMapControl_Position.Overlays.Add(markersOverlay);
        }

        /// <summary>
        /// Attaches mouse event handlers to the GMapControl for marker dragging.
        /// </summary>
        /// <param name="mapControl">The GMapControl to attach the handlers to.</param>
        public void SetupMapControl(GMapControl mapControl)
        {
            mapControl.MouseDown += MapControl_MouseDown;
            mapControl.MouseMove += MapControl_MouseMove;
            mapControl.MouseUp += MapControl_MouseUp;
        }

        /// <summary>
        /// Asynchronously simulates soldier position updates from a JSON file.
        /// </summary>
        private async Task LoadAndSimulateUpdatesAsync()
        {
            try
            {
                if (!File.Exists(jsonFilePath))
                {
                    MessageBox.Show($"{jsonFilePath} not found.");
                    return;
                }

                data = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(jsonFilePath));
                if (data == null)
                {
                    MessageBox.Show($"Error deserializing {jsonFilePath}");
                    return;
                }

                foreach (var update in data.PositionUpdates)
                {
                    UpdatePositions(update);
                    await Task.Delay(2000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates soldier positions on the map based on the provided update.  Handles thread safety.
        /// </summary>
        /// <param name="update">The position update data.</param>
        private void UpdatePositions(IPositionUpdate update)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdatePositions(update))); 
                return;
            }

            foreach (var position in update.Positions)
            {
                PointLatLng point = new PointLatLng(position.Latitude, position.Longitude);
                GMarkerGoogle existingMarker = markersOverlay.Markers.OfType<GMarkerGoogle>().FirstOrDefault(m => m.Tag != null && (int)m.Tag == position.SoldierId);

                if (existingMarker != null)
                {
                    existingMarker.Position = point; 
                }
                else
                {
                    var soldier = data.Soldiers.FirstOrDefault(s => s.Id == position.SoldierId);
                    if (soldier != null)
                    {
                        GMapMarker newMarker = CreateCustomMarker(point, position.SoldierId, soldier.FirstName, soldier.LastName, soldier.Rank, soldier.Country, soldier.TrainingInfo, soldier.Color1);
                        markersOverlay.Markers.Add(newMarker);
                        markers.Add(position.SoldierId, newMarker); 
                    }
                }
            }

            gMapControl_Position.ZoomAndCenterMarkers("markers");
            gMapControl_Position.Refresh();
        }

        /// <summary>
        /// Creates a custom GMap marker with a colored circle.
        /// </summary>
        /// <param name="point">The marker's location.</param>
        /// <param name="soldierId">The soldier's ID.</param>
        /// <param name="firstName">The soldier's first name.</param>
        /// <param name="lastName">The soldier's last name.</param>
        /// <param name="timestamp">The timestamp of the position.</param>
        /// <param name="color">The marker's color.</param>
        /// <returns>A custom GMapMarker.</returns>
        private GMapMarker CreateCustomMarker(PointLatLng point, int soldierId, string firstName, string lastName,string rank,string country,string trainingInfo, Color color)
        {
            Bitmap bitmap = new Bitmap(30, 30);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillEllipse(new SolidBrush(color), 0, 0, bitmap.Width, bitmap.Height);
            }

            DateTime timestamp = GetLatestTimestamp(soldierId);

            var marker = new GMarkerGoogle(point, bitmap);
            marker.Tag = soldierId;
            marker.ToolTipText = $"Name: {firstName} {lastName}=--> Latest date: {timestamp}\n Rank: {rank}\n Country: {country}\n TrainingInfo: {trainingInfo}";
            marker.Offset = new Point(-bitmap.Width / 2, -bitmap.Height / 2);
            return marker;
        }

        /// <summary>
        /// Gets the screen-space bounding rectangle of a marker.
        /// </summary>
        /// <param name="marker">The marker.</param>
        /// <param name="e">The MouseEventArgs (not used in calculation).</param>
        /// <returns>The marker's bounding rectangle.</returns>
        private Rectangle GetMarkerBounds(GMapMarker marker, MouseEventArgs e)
        {
            var markerScreenPos = gMapControl_Position.FromLatLngToLocal(marker.Position);
            return new Rectangle(
                (int)(markerScreenPos.X - marker.Size.Width / 2),
                (int)(markerScreenPos.Y - marker.Size.Height / 2),
                marker.Size.Width,
                marker.Size.Height);
        }

        /// <summary>
        /// Adds a new position update to the data object.
        /// </summary>
        /// <param name="soldierId">The soldier's ID.</param>
        /// <param name="newPoint">The new position.</param>
        private void AddNewPositionUpdate(int soldierId, PointLatLng newPoint)
        {
            data.PositionUpdates.Add(new PositionUpdate
            {
                Timestamp = DateTime.Now,
                Positions = new List<Position> { new Position { SoldierId = soldierId, Latitude = newPoint.Lat, Longitude = newPoint.Lng } }
            });
            SaveDataToJsonFile();
        }

        /// <summary>
        /// Saves the updated data to the JSON file.
        /// </summary>
        private void SaveDataToJsonFile()
        {
            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        /// <summary>
        /// Retrieves the latest timestamp for a given soldier's position update.
        /// </summary>
        /// <param name="soldierId">The ID of the soldier.</param>
        /// <returns>The latest timestamp for the soldier's position update. Returns the default DateTime value (0001-01-01 00:00:00) if no updates are found for the soldier.</returns>
        private DateTime GetLatestTimestamp(int soldierId)
        {
            return data.PositionUpdates
                .Where(u => u.Positions.Any(p => p.SoldierId == soldierId))
                .OrderByDescending(u => u.Timestamp)
                .Select(u => u.Timestamp)
                .FirstOrDefault();
        }
        #endregion

        #region Override Methods

        /// <summary>
        /// Starts the position update simulation when the form loads.
        /// </summary>
        /// <param name="e">Standard event arguments.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadAndSimulateUpdatesAsync().ConfigureAwait(false);
        }
        #endregion

        #region Events

        /// <summary>
        /// Handles mouse down events on the map, initiating marker dragging.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void MapControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var point = gMapControl_Position.FromLocalToLatLng(e.X, e.Y);
                foreach (var marker in markersOverlay.Markers)
                {
                    Rectangle markerBounds = GetMarkerBounds(marker, e);
                    if (markerBounds.Contains(e.X, e.Y))
                    {
                        draggedMarker = marker;
                        isDragging = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles mouse move events, updating the dragged marker's position.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void MapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedMarker != null)
            {
                var point = gMapControl_Position.FromLocalToLatLng(e.X, e.Y);
                draggedMarker.Position = point;
                gMapControl_Position.Refresh();
            }
        }

        /// <summary>
        /// Handles mouse up events, ending marker dragging and saving changes.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void MapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedMarker != null)
            {
                isDragging = false;
                AddNewPositionUpdate((int)draggedMarker.Tag, draggedMarker.Position);
                draggedMarker = null;
            }
        }
        #endregion
    }
}
