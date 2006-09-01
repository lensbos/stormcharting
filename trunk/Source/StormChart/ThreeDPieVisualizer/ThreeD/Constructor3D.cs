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
using System.Collections;
using System.Drawing;

namespace BC.Controls.StormChart.ThreeDPieVisualizer.ThreeD
{
	public class Constructor3D
	{
		public static Plane[] ConstructPiePiece( double radius, double startAngle, double endAngle, double angleStep, double height, Color color )
		{
			double middleAngel = ( startAngle + endAngle ) / 2;
			Plane[] firstPlanes = InternalConstructPiePiece( radius, startAngle, middleAngel, angleStep, height, color, true, false );
			Plane[] secondPlanes = InternalConstructPiePiece( radius, middleAngel, endAngle, angleStep, height, color, false, true );
			Plane[] result = new Plane[ firstPlanes.Length + secondPlanes.Length ];
			firstPlanes.CopyTo( result, 0 );
			secondPlanes.CopyTo( result, firstPlanes.Length  );
			return result;
		}
		
		private static Plane[] InternalConstructPiePiece( double radius, double startAngle, double endAngle, double angleStep, double height, Color color, bool needStartPlane, bool needEndPlane )
		{
			ArrayList planeList = new ArrayList();
			ArrayList segmentList = new ArrayList();
			segmentList.Add( GetPoint( radius, startAngle ) );
			double currentAngle = startAngle + angleStep;
			while( currentAngle < endAngle )
			{
				segmentList.Add( GetPoint( radius, currentAngle ) );
				currentAngle += angleStep;
			}
			segmentList.Add( GetPoint( radius, endAngle ) );
			Point3D[] planePoints = new Point3D[ segmentList.Count + 1 ];
			Point2D point2D;
			for( int index = 0; index < planePoints.Length; index++ )
			{
				if( index > segmentList.Count - 1 )
				{
					planePoints[ index ] = new Point3D( 0, 0, 0 );
				}
				else
				{
					point2D = ( Point2D )segmentList[ index ];
					planePoints[ index ] = new Point3D( point2D.X, 0, point2D.Y );
				}
			}
			planeList.Add( new Plane( planePoints, color ) );
			planePoints = new Point3D[ segmentList.Count + 1 ];
			for( int index = 0; index < planePoints.Length; index++ )
			{
				if( index > segmentList.Count - 1 )
				{
					planePoints[ index ] = new Point3D( 0, height, 0 );
				}
				else
				{
					point2D = ( Point2D )segmentList[ index ];
					planePoints[ index ] = new Point3D( point2D.X, height, point2D.Y );
				}
			}
			planeList.Add( new Plane( planePoints, color ) );
			if( needStartPlane )
			{
				planePoints = new Point3D[ 4 ];
				point2D = ( Point2D )segmentList[ 0 ];
				planePoints[ 0 ] = new Point3D( point2D.X, 0, point2D.Y );
				planePoints[ 1 ] = new Point3D( point2D.X, height, point2D.Y );
				planePoints[ 2 ] = new Point3D( 0, height, 0 );
				planePoints[ 3 ] = new Point3D( 0, 0, 0 );
				planeList.Add( new Plane( planePoints, color ) );
			}
			if( needEndPlane )
			{
				planePoints = new Point3D[ 4 ];
				point2D = ( Point2D )segmentList[ segmentList.Count - 1 ];
				planePoints[ 0 ] = new Point3D( point2D.X, 0, point2D.Y );
				planePoints[ 1 ] = new Point3D( point2D.X, height, point2D.Y );
				planePoints[ 2 ] = new Point3D( 0, height, 0 );
				planePoints[ 3 ] = new Point3D( 0, 0, 0 );
				planeList.Add( new Plane( planePoints, color ) );
			}
			for( int index = 0; index < segmentList.Count - 1; index++ )
			{
				planePoints = new Point3D[ 4 ];
				point2D = ( Point2D )segmentList[ index ];
				planePoints[ 0 ] = new Point3D( point2D.X, 0, point2D.Y );
				planePoints[ 1 ] = new Point3D( point2D.X, height, point2D.Y );
				point2D = ( Point2D )segmentList[ index + 1 ];
				planePoints[ 2 ] = new Point3D( point2D.X, height, point2D.Y );
				planePoints[ 3 ] = new Point3D( point2D.X, 0, point2D.Y );
				planeList.Add( new Plane( planePoints, color ) );
			}
			Plane[] result = new Plane[ planeList.Count ];
			planeList.CopyTo( result );
			return result;
		}

		public static Point2D GetPoint( double radius, double angle )
		{
			return new Point2D( Math.Cos( angle ) * radius, Math.Sin( angle ) * radius );
		}
	}
}
