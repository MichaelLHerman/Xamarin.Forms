﻿using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
using WBrush = Windows.UI.Xaml.Media.Brush;
using WSolidColorBrush = Windows.UI.Xaml.Media.SolidColorBrush;
using WGradientStop = Windows.UI.Xaml.Media.GradientStop;
using WLinearGradientBrush = Windows.UI.Xaml.Media.LinearGradientBrush;

namespace Xamarin.Forms.Platform.UWP
{
	public static class ColorExtensions
	{
		public static Windows.UI.Color GetContrastingColor(this Windows.UI.Color color)
		{
			var nThreshold = 105;
			int bgLuminance = Convert.ToInt32(color.R * 0.2 + color.G * 0.7 + color.B * 0.1);

			Windows.UI.Color contrastingColor = 255 - bgLuminance < nThreshold ? Colors.Black : Colors.White;
			return contrastingColor;
		}

		public static Color ToFormsColor(this Windows.UI.Color color)
		{
			return Color.FromRgba(color.R, color.G, color.B, color.A);
		}

		public static Color ToFormsColor(this WSolidColorBrush solidColorBrush)
		{
			return solidColorBrush.Color.ToFormsColor();
		}

		public static WBrush ToBrush(this Color color)
		{
			return new WSolidColorBrush(color.ToWindowsColor());
		}

		public static WBrush ToBrush(this Brush brush)
		{
			if (brush is SolidColorBrush solidColorBrush)
			{
				return solidColorBrush.Color.ToBrush();
			}

			if (brush is LinearGradientBrush linearGradientBrush)
			{
				var orderedStops = linearGradientBrush.GradientStops.OrderBy(x => x.Offset).ToList();
				var gradientStopCollection = new GradientStopCollection();

				foreach (var item in orderedStops)
					gradientStopCollection.Add(new WGradientStop { Offset = item.Offset, Color = item.Color.ToWindowsColor() });

				var p1 = linearGradientBrush.StartPoint;
				var x1 = p1.X;
				var y1 = p1.Y;

				var p2 = linearGradientBrush.EndPoint;
				var x2 = p2.X;
				var y2 = p2.Y;

				var deltaX = Math.Pow(x2 - x1, 2);
				var deltaY = Math.Pow(y2 - y1, 2);

				var radians = Math.Atan2(y2 - y1, x2 - x1);
				var angle = radians * (180 / Math.PI);

				return new WLinearGradientBrush(gradientStopCollection, angle);
			}

			return null;
		}

		public static Windows.UI.Color ToWindowsColor(this Color color)
		{
			return Windows.UI.Color.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
		}
	}
}