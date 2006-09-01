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
	public class Plane : Base3DObject
	{
		protected Color planeColor;
		protected Point3D[] pointsList;
		protected double a;   //  A*x0 + B*y0 + C*z0 + D = 0
		protected double b;
		protected double c;
		protected double d;
		protected double x0;
		protected double y0;
		protected double z0;

		public Color PlaneColor
		{
			get
			{
				return planeColor;
			}
			set
			{
				planeColor = value;
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
		public double D
		{
			get
			{
				return d;
			}
		}
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

		public Point3D[] Points
		{
			get
			{
				return pointsList;
			}
		}

		public Plane( Point3D[] points ) : this( points, Color.Black )
		{
		}
		
		public Plane( Point3D[] points, Color color )
		{
			pointsList = points;
			planeColor = color;
			CalculatePlane();
		}

		private void CalculatePlane()
		{
			Point3D vector1 = pointsList[ 1 ];
			MovePoint( ref vector1, pointsList[ 0 ].Negative() );
			Point3D vector2 = pointsList[ 2 ];
			MovePoint( ref vector2, pointsList[ 1 ].Negative() );
			Point3D normal = vector1.MultiplyToVectors( vector2 );
			a = normal.X;
			b = normal.Y;
			c = normal.Z;
			x0 = pointsList[ 0 ].X;
			y0 = pointsList[ 0 ].Y;
			z0 = pointsList[ 0 ].Z;
			d = -a * x0 - b * y0 - c * z0;
		}

		public override void RotateAroundX( double angle )
		{
			for( int index = 0; index < pointsList.Length; index++ )
			{
				RotatePointAroundX( ref pointsList[ index ], angle );
			}
			CalculatePlane();
		}

		public override void RotateAroundY( double angle )
		{
			for( int index = 0; index < pointsList.Length; index++ )
			{
				RotatePointAroundY( ref pointsList[ index ], angle );
			}
			CalculatePlane();
		}

		public override void RotateAroundZ( double angle )
		{
			for( int index = 0; index < pointsList.Length; index++ )
			{
				RotatePointAroundZ( ref pointsList[ index ], angle );
			}
			CalculatePlane();
		}

		public override void Move( Point3D moveVector )
		{
			for( int index = 0; index < pointsList.Length; index++ )
			{
				MovePoint( ref pointsList[ index ], moveVector );
			}
			CalculatePlane();
		}

		public Point3D Cross( Line3D line )
		{
			double denominator = a * line.K + b * line.L + c * line.M;
			if( Math3DHelper.IsZero( denominator ) )
			{
				throw new DoesNotCrossException( "Line does not cross Plane" );
			}
			double t = ( -a * line.X0 - b * line.Y0 - c * line.Z0 - d ) / denominator;
			return line.GetPoint( t );
		}
	}
}
