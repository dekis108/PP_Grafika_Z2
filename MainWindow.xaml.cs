using PZ2.Model;
using PZ3.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PZ3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Importer _importer;
        Drawer _drawer;

        private bool middleMouseDown = false;
        private Point middleClickPoint;
        private Point start = new Point();
        private Point diffOffset = new Point();

        private static int zoomMax = 30;
        private static double zoomMin = 5;
        private static int zoomCurent = 1;
        private static double _rotateOffset = 0.5;

        private string noFilter = "No Filter";

        private string option1;
        private string option2;
        private string option3;

        private string resOption1;
        private string resOption2;
        private string resOption3;

        private GeometryModel3D hitgeo;
        private Dictionary<long, GeometryModel3D> entitiesModels = new Dictionary<long, GeometryModel3D>();
        private List<GeometryModel3D> selectedEntities = new List<GeometryModel3D>();
        private List<GeometryModel3D> linesModels = new List<GeometryModel3D>();


        public MainWindow()
        {
            InitializeComponent();
            _importer = new Importer();
            _importer.LoadModel();

            InitDrawer(_importer.PowerGrid);

            List<string> connectionFilters = new List<string>() {
                noFilter,
                option1,
                option2,
                option3
            };
            comboConnectivity.ItemsSource = connectionFilters;
            comboConnectivity.SelectedItem = comboConnectivity.Items[0];
            comboConnectivity.SelectionChanged += comboConnectivity_SelectionChanged;

            List<string> resistenceFilters = new List<string>() {
                noFilter,
                resOption1,
                resOption2,
                resOption3
            };
            comboResistance.ItemsSource = resistenceFilters;
            comboResistance.SelectedItem = comboResistance.Items[0];
            comboResistance.SelectionChanged += comboResistance_SelectionChanged;



            _drawer.Draw();


            /*
             *             foreach (var pair in _drawer.DrawPowerEntities(_importer.PowerGrid.PowerEntities))
            {
                entitiesModels.Add(pair.Key, pair.Value);
            }

            //models.AddRange(_drawer.DrawPowerEntities(_importer.PowerGrid.PowerEntities));
            linesModels.AddRange(_drawer.DrawLines(_importer.PowerGrid.LineEntities));
             */
        }

        private void InitDrawer(PowerGrid powerGrid)
        {
            _drawer = new Drawer(Map, powerGrid.PowerEntities, powerGrid.LineEntities);

            noFilter = ConnectionFilter.noFilter;

            option1 = ConnectionFilter.option1;
            option2 = ConnectionFilter.option2;
            option3 = ConnectionFilter.option3;

            resOption1 = ResistanceFilter.option1;
            resOption2 = ResistanceFilter.option2;
            resOption3 = ResistanceFilter.option3;
        }

        private void viewport1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewPortDisplay.CaptureMouse();
            start = e.GetPosition(this);
            diffOffset.X = trasnlation.OffsetX;
            diffOffset.Y = trasnlation.OffsetY;
        }

        private void viewport1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            viewPortDisplay.ReleaseMouseCapture();
        }

        private void viewport1_MouseMove(object sender, MouseEventArgs e)
        {
            if (viewPortDisplay.IsMouseCaptured && !middleMouseDown)
            {
                Point end = e.GetPosition(this);
                double offsetX = end.X - start.X;
                double offsetY = end.Y - start.Y;
                double w = this.Width;
                double h = this.Height;
                double translateX = -(offsetX * 100) / w;
                double translateY = +(offsetY * 100) / h;
                trasnlation.OffsetX = diffOffset.X + (translateX / (100 * scale.ScaleX));
                trasnlation.OffsetY = diffOffset.Y + (translateY / (100 * scale.ScaleX));

            }

            if (middleMouseDown)
            {
                viewPortDisplay.CaptureMouse();
                Point mouse = e.GetPosition(this);
                double diffX = mouse.X - middleClickPoint.X;
                double diffY = mouse.Y - middleClickPoint.Y;

                diffX *= -1;
                diffY *= -1;
                //double diffX = mouse.X - viewPortDisplay.X;
                //double diffY = mouse.Y - middleClickPoint.Y;



                rotation.Axis = new Vector3D(diffY, diffX, 0);
                rotation.Angle = Math.Sqrt(diffX * diffX + diffY * diffY) * _rotateOffset;


                Console.WriteLine("X" + diffX + " Y" + diffY);
                
            }
        }


        private void viewport1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = e.MouseDevice.GetPosition(this);
            double scaleX = 1;
            double scaleY = 1;
            if (e.Delta > 0 && zoomCurent < zoomMax)
            {
                scaleX = scale.ScaleX + 0.1;
                scaleY = scale.ScaleY + 0.1;
                zoomCurent++;
                scale.ScaleX = scaleX;
                scale.ScaleY = scaleY;
            }
            else if (e.Delta <= 0 && zoomCurent > -zoomMin)
            {
                scaleX = scale.ScaleX - 0.1;
                scaleY = scale.ScaleY - 0.1;
                zoomCurent--;
                scale.ScaleX = scaleX;
                scale.ScaleY = scaleY;
            }
        }

        private void viewPortDisplay_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Middle)
            {
                middleMouseDown = true;
                middleClickPoint = e.GetPosition(this);
                Console.WriteLine(middleMouseDown);
                return;
            }


            //hit testing
            if (e.ChangedButton != MouseButton.Left) return;

            Point mouseposition = e.GetPosition(this);
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);

            PointHitTestParameters pointparams =
                     new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams =
                     new RayHitTestParameters(testpoint3D, testdirection);

            //test for a result in the Viewport3D     
            hitgeo = null;
            VisualTreeHelper.HitTest(viewPortDisplay, null, HTResult, pointparams);
        }

        private void viewPortDisplay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                middleMouseDown = false;
                viewPortDisplay.ReleaseMouseCapture();
                Console.WriteLine(middleMouseDown);
            }
        }

        private void viewPortDisplay_MouseLeave(object sender, MouseEventArgs e)
        {
            middleMouseDown = false;
        }

        private HitTestResultBehavior HTResult(HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;

            ResetSelected();

            if (rayResult != null)
            {
                bool gasit = false;
                //for (int i = 0; i < models.Count; i++)
                foreach(var model in entitiesModels.Values)
                {
                    if (gasit) break;

                    if (model == rayResult.ModelHit)
                    {
                        hitgeo = (GeometryModel3D)rayResult.ModelHit;
                        gasit = true;

                        //var model = (GeometryModel3D)models[i];

                        string tag = (string)model.GetValue(Drawer.TagDP);
                        MessageBox.Show(tag, "Entity information", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }

                if (!gasit)
                {
                    foreach(var model in linesModels)
                    {
                        if (gasit) break;

                        if (model == rayResult.ModelHit)
                        {
                            hitgeo = (GeometryModel3D)rayResult.ModelHit;
                            gasit = true;


                            long start = (long)model.GetValue(Drawer.StartDP);
                            long end = (long)model.GetValue(Drawer.EndDP);

                            EntitySelected(start);
                            EntitySelected(end);

                        }
                    }
                }

                if (!gasit)
                {
                    hitgeo = null;
                }
            }

            return HitTestResultBehavior.Stop;
        }

        private void ResetSelected()
        {
            foreach(var model in selectedEntities)
            {
                _drawer.Reset(model);
            }
            selectedEntities.Clear();
        }

        private void EntitySelected(long id)
        {
            try
            {
                _drawer.EntitySelected(entitiesModels[id]);
                selectedEntities.Add(entitiesModels[id]);
            }
            catch { }
        }

        private void comboConnectivity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _drawer.displayFilter.SetConnectionFilter((string)comboConnectivity.SelectedItem);
            _drawer.Draw();
        }

        private void comboResistance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _drawer.displayFilter.SetResistanceFilter((string)comboResistance.SelectedItem);
            _drawer.Draw();
        }


    }
}
