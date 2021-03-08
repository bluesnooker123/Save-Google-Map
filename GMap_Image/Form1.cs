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


    using Geohash;
    using NetTopologySuite.Geometries;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public partial class Form1 : Form
    {
        
        GMapPolygon inner_polygon;
        

        public Form1()
        {
            InitializeComponent();
            InitMap();
        }

        private void InitMap()
        {
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleSatelliteMap;

            map.Position = new PointLatLng(0, 0);
            map.MinZoom = 1;
            map.MaxZoom = 24;
            map.Zoom = 5;
            //map.ShowCenter = false;

        }
        private void map_MouseOver(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            PointLatLng p = map.FromLocalToLatLng(e.X, e.Y);
            txtLat.Text = Convert.ToString(p.Lat);
            txtLong.Text = Convert.ToString(p.Lng);
        }

        private void btnLoadIntoMap_Click(object sender, EventArgs e)
        {
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            //map.MapProvider = GMapProviders.GoogleSatelliteMap;
            //map.MapProvider = GMapProviders.BingMap;
            double lat = Convert.ToDouble(txtLat.Text);
            double longt = Convert.ToDouble(txtLong.Text);
            map.Position = new PointLatLng(lat, longt);
            map.MinZoom = 1;
            map.MaxZoom = 24;
            map.Zoom = 6;

            
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            DateTime cur = DateTime.Now;
            Image b = map.ToImage();
            string savePath = "Saved Image/image_" + cur.Hour.ToString() + "_"
                + cur.Minute.ToString() + "_"
                + cur.Second.ToString() + ".jpg";
            b.Save(savePath, ImageFormat.Jpeg);
            MessageBox.Show("Saved to \"" + savePath + "\" successfully");
        }

        private void btnCreatePolygon_Click(object sender, EventArgs e)
        {
            GMapOverlay polygons = new GMapOverlay("polygons");
            List<PointLatLng> points = new List<PointLatLng>();

            /*
            points.Add(new PointLatLng(14.87548828125, 51.05520733858494));
            points.Add(new PointLatLng(12.1728515625, 50.17689812200107));
            points.Add(new PointLatLng(14.26025390625, 48.531157010976706));
            points.Add(new PointLatLng(15.073242187499998, 49.05227025601607));
            points.Add(new PointLatLng(17.02880859375, 48.67645370777654));
            points.Add(new PointLatLng(18.852539062499996, 49.5822260446217));
            points.Add(new PointLatLng(14.87548828125, 51.05520733858494));
            */


            MessageBox.Show(map.Position.ToString());
            double rate = 1 / (map.Zoom * map.Zoom * map.Zoom) + 1 / map.Zoom;
            points.Add(new PointLatLng(map.Position.Lat - 3 * rate, map.Position.Lng - 3 * rate));
            points.Add(new PointLatLng(map.Position.Lat - 4 * rate, map.Position.Lng + 1 * rate));
            points.Add(new PointLatLng(map.Position.Lat - 3.5 * rate, map.Position.Lng + 2.2 * rate));
            points.Add(new PointLatLng(map.Position.Lat - 1 * rate, map.Position.Lng + 2.1 * rate));
            points.Add(new PointLatLng(map.Position.Lat + 3 * rate, map.Position.Lng + 3 * rate));
            points.Add(new PointLatLng(map.Position.Lat + 3.2 * rate, map.Position.Lng - 3 * rate));
            points.Add(new PointLatLng(map.Position.Lat + 2.3 * rate, map.Position.Lng - 5.5 * rate));
            points.Add(new PointLatLng(map.Position.Lat - 1.2 * rate, map.Position.Lng - 3 * rate));
            GMapPolygon polygon = new GMapPolygon(points, "MyPolygon");
            polygon.Fill = new SolidBrush(Color.FromArgb(30, Color.Red));
            polygon.Stroke = new Pen(Color.Red, 2);
            polygons.Polygons.Add(polygon);
            map.Overlays.Add(polygons);
            map.Zoom -= 1;
            map.Zoom += 1;
        }


        private static Polygon GetTestPolygon(GeometryFactory geometryFactory)
        {
            var p1 = new Coordinate() { Y = 14.87548828125, X = 51.05520733858494 };
            var p2 = new Coordinate() { Y = 12.1728515625, X = 50.17689812200107 };
            var p3 = new Coordinate() { Y = 14.26025390625, X = 48.531157010976706 };
            var p4 = new Coordinate() { Y = 15.073242187499998, X = 49.05227025601607 };

            var p5 = new Coordinate() { Y = 17.02880859375, X = 48.67645370777654 };
            var p6 = new Coordinate() { Y = 18.852539062499996, X = 49.5822260446217 };
            var p7 = new Coordinate() { Y = 14.87548828125, X = 51.05520733858494 };


            var polygon = geometryFactory.CreatePolygon(new[] { p1, p2, p3, p4, p5, p6, p7, p1 });

            return polygon;
        }

        public async System.Threading.Tasks.Task<List<string>> Should_Get_Hashes_For_PolygonAsync_LongRunning()
        {
            var hasher = new Geohasher();

            var geometryFactory = new GeometryFactory();

            var progess = new Progress<HashingProgress>();

            progess.ProgressChanged += (e, hp) =>
            {
                Debug.WriteLine($"Processed: {hp.HashesProcessed}, Queued: {hp.QueueSize}, Running Since: {hp.RunningSince}");
            };

            Polygon polygon = GetTestPolygon(geometryFactory);

//            var result = await hasher.GetHashesAsync(polygon, 4, mode: Mode.Contains, progress: progess);
            var result = await hasher.GetHashesAsync(polygon, 1, mode: Mode.Contains);
            //var result = await hasher.GetHashesAsync(polygon, 4, mode: Mode.Contains);

            return result;
            //Assert.AreEqual(112, result.Count);

        }
        private void btnGenerateGeohash_Click(object sender, EventArgs e)
        {
            var hasher = new Geohasher();

            var geometryFactory = new GeometryFactory();
            Polygon polygon = GetTestPolygon(geometryFactory);

            List<string> res = hasher.GetHashesAsync(polygon).Result;
            MessageBox.Show(res[0], "Test1");


            //Task<List<string>> inner_polygon = Should_Get_Hashes_For_PolygonAsync_LongRunning();
            //List<string> inner_polygon_Loaded =  inner_polygon.Result;

            //string[] arr_result = inner_polygon_Loaded.ToArray();
            //MessageBox.Show(arr_result[0], "sss");
            int a = 3;
        }

        private void Get_Geohash_Using_Python()
        {
            run_cmd("../../Geohash_python/get_geohash_from_polygon.py", "11111");

            GMapOverlay inner_polygons = new GMapOverlay("inner_polygons");
            inner_polygon.Fill = new SolidBrush(Color.FromArgb(30, Color.Blue));
            inner_polygon.Stroke = new Pen(Color.Blue, 2);
            inner_polygons.Polygons.Add(inner_polygon);
            map.Overlays.Add(inner_polygons);
            map.Zoom -= 1;
            map.Zoom += 1;

        }
        private void run_cmd(string cmd, string args)
        {
            //please first run "pip install polygon-geohasher"

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:/Users/Legendary/AppData/Local/Programs/Python/Python37/python.exe";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    
                   // MessageBox.Show(result,"title");
                    get_Polygon_from_string(result);
                }
            }
             
        }
        private void get_Polygon_from_string(string org_str)
        {
            string temp_str1 = org_str.Substring(10);
            string temp_str2 = temp_str1.Remove(temp_str1.Length - 4, 4);
            MessageBox.Show(temp_str2, "title22222");
            string[] arr_str = temp_str2.Split(',');

            List<PointLatLng> points = new List<PointLatLng>();
            for (int i = 0; i < arr_str.Length; i ++)
            {
                string temp_str3 = (i != 0) ? arr_str[i].Substring(1) : arr_str[i];
                string[] arr_temp_str = temp_str3.Split(' ');
                points.Add(new PointLatLng(Convert.ToDouble(arr_temp_str[0]), Convert.ToDouble(arr_temp_str[1])));
            }
            inner_polygon = new GMapPolygon(points, "MyInnerPolygon");
        }
    }
}
