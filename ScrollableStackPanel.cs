using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Stackpanel
{
    public class ScrollableStackPanel : StackPanel
    {
        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(
            "Options", typeof(ScrollAbleStackPanelOptions), typeof(ScrollableStackPanel), new PropertyMetadata(ScrollAbleStackPanelOptions.ShowAlwaysLastElement));

        public ScrollAbleStackPanelOptions Options
        {
            get { return (ScrollAbleStackPanelOptions)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (Options == ScrollAbleStackPanelOptions.ShowAllElements)
                return MeasureAllElements(constraint);
            else
                return MeasureElementsStartWithLastItem(constraint);
        }

        private Size MeasureElementsStartWithLastItem(Size constraint)
        {
            Size size = new Size();
            UIElementCollection internalChildren = InternalChildren;
            Size availableSize = constraint;

            for (int i = internalChildren.Count - 1; i >= 0; i--)
            {
                UIElement uiElement = internalChildren[i];
                if (uiElement != null)
                {
                    if (Orientation == Orientation.Vertical)
                    {
                        uiElement.Measure(availableSize);
                        Size desiredSize = uiElement.DesiredSize;
                        size.Width = Math.Max(size.Width, desiredSize.Width);
                        size.Height += desiredSize.Height;
                        availableSize.Height -= uiElement.DesiredSize.Height;
                    }
                }
                else
                {
                    uiElement.Measure(availableSize);
                    Size desiredSize = uiElement.DesiredSize;
                    size.Height = Math.Max(size.Height, desiredSize.Height);
                    size.Width += desiredSize.Width;

                    availableSize.Width -= uiElement.DesiredSize.Width;
                }
            }
            if (size.Height > constraint.Height)
                size.Height = constraint.Height;
            if (size.Width > constraint.Width)
                size.Width = constraint.Width;
            return size;
        }

        private Size MeasureAllElements(Size constraint)
        {
            Size size = new Size();
            UIElementCollection internalChildren = InternalChildren;
            Size availableSize = constraint;

            for (int i = 0; i !=internalChildren.Count ; i++)
            {
                UIElement uiElement = internalChildren[i];
                if (uiElement != null)
                {
                    if (Orientation == Orientation.Vertical)
                    {
                        uiElement.Measure(availableSize);
                        Size desiredSize = uiElement.DesiredSize;
                        size.Width = Math.Max(size.Width, desiredSize.Width);
                        size.Height += desiredSize.Height;
                        availableSize.Height -= uiElement.DesiredSize.Height;
                    }
                    else
                    {
                        uiElement.Measure(availableSize);
                        Size desiredSize = uiElement.DesiredSize;
                        size.Height = Math.Max(size.Height, desiredSize.Height);
                        size.Width += desiredSize.Width;
                        availableSize.Width -= uiElement.DesiredSize.Width;
                    }
                }
            }
            return size;
        }

        internal Size GetMeasuredOverride(Size constraint)
        {
            return MeasureOverride(constraint);
        }
    }

    internal enum ScrollAbleStackPanelOptions
    {
        ShowAlwaysLastElement,
        ShowAllElements
    }
}
