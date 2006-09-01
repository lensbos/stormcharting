/*
Copyright (C) 2004-2006  Bluecollar Enterprises
http://www.bcollar.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Drawing;

namespace BC.Controls.StormChart.ThreeDPieVisualizer.ThreeD
{
	public class Line2D
	{
		protected PointF firstPoint;  // Ax + By + C = 0
		protected PointF secondPoint;
		protected double a;
		protected double b;
		protected double c;

		public PointF FirstPoint
		{
			get
			{
				return firstPoint;
			}
		}
		public PointF SecondPoint
		{
			get
			{
				return secondPoint;
			}
		}
		public double A
		{
			get
			{
				return a;
			}
		}
		public double B
		{
			get
			{
				return b;
			}
		}
		public double C
		{
			get
			{
				return c;
			}
		}

		public Line2D( PointF firstPoint, PointF secondPoint )
		{
			this.firstPoint = firstPoint;
			this.secondPoint = secondPoint;
			CalculateLine();
		}

		public double GetX( double y )
		{
			return ( -c - b * y ) / a;
		}

		public double GetY( double x )
		{
			return ( -c - a * x ) / b;
		}

		public bool IsHorizontal()
		{
			return Math3DHelper.IsZero( a );
		}

		public double GetOrientation( PointF point )
		{
			return a * point.X + b * point.Y + c;
		}
		
		public double GetOrientation( Point2D point )
		{
			return a * point.X + b * point.Y + c;
		}
		
		private void CalculateLine()
		{
			a = firstPoint.Y - secondPoint.Y;
			b = secondPoint.X - firstPoint.X;
			c = -a * firstPoint.X - b * firstPoint.Y;
		}
	}
}
