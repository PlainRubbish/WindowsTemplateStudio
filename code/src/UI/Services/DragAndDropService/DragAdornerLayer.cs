﻿using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Microsoft.Templates.UI.Services
{
    public class DragAdornerLayer : Adorner
    {        
        private Rectangle _child = null;
        private double _offsetLeft = 0;
        private double _offsetTop = 0;
        
        public DragAdornerLayer(UIElement adornedElement, Size size, Brush brush) : base(adornedElement)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = brush;
            rect.Width = size.Width;
            rect.Height = size.Height;
            rect.IsHitTestVisible = false;
            this._child = rect;
        }
               
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(this._offsetLeft, this._offsetTop));
            return result;
        }
        
        public double OffsetLeft
        {
            get => this._offsetLeft;
            set
            {
                _offsetLeft = value;
                UpdateLocation();
            }
        }        
        
        public void SetOffsets(double left, double top)
        {
            this._offsetLeft = left;
            this._offsetTop = top;
            this.UpdateLocation();
        }
        
        public double OffsetTop
        {
            get => _offsetTop;
            set
            {
                this._offsetTop = value;
                UpdateLocation();
            }
        }
        
        protected override Size MeasureOverride(Size constraint)
        {
            this._child.Measure(constraint);
            return this._child.DesiredSize;
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            this._child.Arrange(new Rect(finalSize));
            return finalSize;
        }
        
        protected override Visual GetVisualChild(int index) => _child;        

        protected override int VisualChildrenCount => 1;
        
        private void UpdateLocation()
        {
            var adornerLayer = this.Parent as AdornerLayer;
            if (adornerLayer != null)
            {
                adornerLayer.Update(this.AdornedElement);
            }                
        }        
    }
}