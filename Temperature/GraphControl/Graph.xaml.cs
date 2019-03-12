using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphControl
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph : UserControl, IUpdating
    {
        private IGraphInterface graph;

        public Graph(IGraphInterface graph)
        {
            InitializeComponent();
            this.graph = graph;
        }

        public void Update()
        {
            graph.UpdateAll();
            InvalidateVisual();
        }

        public double InnerMargin { get; set; } = .15;
     
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            graph.UpdateTransform(ActualWidth * (1 - 2 * InnerMargin), ActualHeight * (1 - 2 * InnerMargin), 
                ActualWidth * InnerMargin, ActualHeight * InnerMargin);
            drawingContext.DrawDrawing(graph.Drawing);
            
        }
    }
}
