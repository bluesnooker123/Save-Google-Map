using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace GMap_Image
{
    using System.Globalization;
    using System.Reflection;
    using GMap.NET.Projections;
    using GMap.NET.MapProviders;
    using GMap.NET;
    using GMap.NET.WindowsForms;
    using GMap.NET.WindowsForms.Markers;
    using GMap.NET.WindowsForms.ToolTips;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void func_ZoomChanged()
        {
            txtZoom.Text = map.Zoom.ToString();
        }
        private void btnLoadIntoMap_Click(object sender, EventArgs e)
        {
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleSatelliteMap;
            
            //map.MapProvider = GMapProviders.BingMap;
            double lat = Convert.ToDouble(txtLat.Text);
            double longt = Convert.ToDouble(txtLong.Text);
            map.Position = new PointLatLng(lat, longt);
            map.MinZoom = 1;
            map.MaxZoom = 25;
            map.Zoom = 10;
/*            double temp =  (360 / (Convert.ToDouble(txtWidth.Text) + Convert.ToDouble(txtHeight.Text)));
            if (temp > 100)
                map.Zoom = 100;
            else if (temp < 1)
                map.Zoom = 1;
            txtZoom.Text = map.Zoom.ToString();
            */
            map.OnMapZoomChanged += new MapZoomChanged(func_ZoomChanged);


            GMapOverlay polygons = new GMapOverlay("polygons");
            List<PointLatLng> points = new List<PointLatLng>();

            points.Add(new PointLatLng(lat - Convert.ToDouble(txtWidth.Text) / 2, longt - Convert.ToDouble(txtHeight.Text) / 2));
            points.Add(new PointLatLng(lat - Convert.ToDouble(txtWidth.Text) / 2, longt + Convert.ToDouble(txtHeight.Text) / 2));
            points.Add(new PointLatLng(lat + Convert.ToDouble(txtWidth.Text) / 2, longt + Convert.ToDouble(txtHeight.Text) / 2));
            points.Add(new PointLatLng(lat + Convert.ToDouble(txtWidth.Text) / 2, longt - Convert.ToDouble(txtHeight.Text) / 2));
            GMapPolygon polygon = new GMapPolygon(points, "MyPolygon");
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 2);
            polygons.Polygons.Add(polygon);
            map.Overlays.Add(polygons);
            map.Zoom -= 1;
            map.Zoom += 1;


        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            DateTime cur = DateTime.Now;
            Image b = map.ToImage();
            string savePath = "Saved Image/image_" + cur.Hour.ToString() + "_"
                + cur.Minute.ToString() + "_"
                + cur.Second.ToString() + ".jpg";
            b.Save(savePath, ImageFormat.Jpeg);
        }
    }
}
