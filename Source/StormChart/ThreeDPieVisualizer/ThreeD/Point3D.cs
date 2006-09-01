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
	public struct Point3D
	{
		public double X;
		public double Y;
		public double Z;

		public Point3D( double x, double y, double z )
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Point3D MultiplyToVectors( Point3D vector )
		{
			Point3D newPoint = new Point3D();
			newPoint.X = Y * vector.Z - Z * vector.Y;
			newPoint.Y = Z * vector.X - X * vector.Z;
			newPoint.Z = X * vector.Y - Y * vector.X;
			return newPoint;
		}

		public Point3D Negative()
		{
			return new Point3D( -X, -Y, -Z );
		}

		public double GetLength()
		{
			return Math.Sqrt( Math3DHelper.Sqr( X ) + Math3DHelper.Sqr( Y ) + Math3DHelper.Sqr( Z ) );
		}
		
		public double GetDistance( Point3D point )
		{
			return Math.Sqrt( Math3DHelper.Sqr( X - point.X ) + Math3DHelper.Sqr( Y - point.Y ) + Math3DHelper.Sqr( Z - point.Z ) );
		}

		public double GetCosA( Point3D vector )
		{
			double scalar = X * vector.X + Y * vector.Y + Z * vector.Z;
			return scalar / vector.GetLength() / GetLength();
		}
	}
}
