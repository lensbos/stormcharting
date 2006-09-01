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
	public abstract class Base3DObject
	{
		
		public Base3DObject()
		{
		}

		public abstract void RotateAroundX( double angle );

		public abstract void RotateAroundY( double angle );

		public abstract void RotateAroundZ( double angle );

		public abstract void Move( Point3D moveVector );

		protected virtual void RotatePointAroundX( ref Point3D point, double angle )
		{
			double newY = point.Y * Math.Cos( angle ) - point.Z * Math.Sin( angle );
			double newZ = point.Y * Math.Sin( angle ) + point.Z * Math.Cos( angle );
            point.Y = newY;
			point.Z = newZ;
		}

		protected virtual void RotatePointAroundZ( ref Point3D point, double angle )
		{
			double newY = point.Y * Math.Cos( angle ) - point.X * Math.Sin( angle );
			double newX = point.Y * Math.Sin( angle ) + point.X * Math.Cos( angle );
			point.Y = newY;
			point.X = newX;
		}

		protected virtual void RotatePointAroundY( ref Point3D point, double angle )
		{
			double newZ = point.Z * Math.Cos( angle ) - point.X * Math.Sin( angle );
			double newX = point.Z * Math.Sin( angle ) + point.X * Math.Cos( angle );
			point.X = newX;
			point.Z = newZ;
		}

		protected virtual void MovePoint( ref Point3D point, Point3D moveVector )
		{
			point.X += moveVector.X;
			point.Y += moveVector.Y;
			point.Z += moveVector.Z;
		}
	}
}
