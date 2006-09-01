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

namespace BC.Controls.StormChart.ThreeDPieVisualizer.ThreeD
{
	public class Line3D : Base3DObject
	{
		private Point3D firstPoint;
		private Point3D secondPoint;
		private double x0;//    / x = t*k + x0
		private double y0;//   <  y = t*l + y0
		private double z0;//    \ z = t*m + z0
		private double k;
		private double l;
		private double m;

		public double X0
		{
			get
			{
				return x0;
			}
		}
		public double Y0
		{
			get
			{
				return y0;
			}
		}
		public double Z0
		{
			get
			{
				return z0;
			}
		}
		public double K
		{
			get
			{
				return k;
			}
		}
		public double L
		{
			get
			{
				return l;
			}
		}
		public double M
		{
			get
			{
				return m;
			}
		}

		public Point3D FirstPoint
		{
			get
			{
				return firstPoint;
			}
		}
		public Point3D SecondPoint
		{
			get
			{
				return secondPoint;
			}
		}

		public Line3D( Point3D firstPoint, Point3D secondPoint )
		{
			this.firstPoint = firstPoint;
			this.secondPoint = secondPoint;
			CalculateLine();
		}

		public override void RotateAroundX( double angle )
		{
			RotatePointAroundX( ref firstPoint, angle );
			RotatePointAroundX( ref secondPoint, angle );
			CalculateLine();
		}

		public override void RotateAroundY( double angle )
		{
			RotatePointAroundY( ref firstPoint, angle );
			RotatePointAroundY( ref secondPoint, angle );
			CalculateLine();
		}

		public override void RotateAroundZ( double angle )
		{
			RotatePointAroundZ( ref firstPoint, angle );
			RotatePointAroundZ( ref secondPoint, angle );
			CalculateLine();
		}

		public override void Move( Point3D moveVector )
		{
			MovePoint( ref firstPoint, moveVector );
			MovePoint( ref secondPoint, moveVector );
			CalculateLine();
		}

		public Point3D GetPoint( double t )
		{
			return new Point3D( t * k + x0, t * l + y0, t * m + z0 );
		}

		private void CalculateLine()
		{
			x0 = firstPoint.X;
			y0 = firstPoint.Y;
			z0 = firstPoint.Z;
			k = secondPoint.X - x0;
			l = secondPoint.Y - y0;
			m = secondPoint.Z - z0;
		}
	}
}
