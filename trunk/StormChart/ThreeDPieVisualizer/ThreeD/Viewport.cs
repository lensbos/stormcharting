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
using BC.Controls.StormChart.Common;

namespace BC.Controls.StormChart.ThreeDPieVisualizer.ThreeD
{
	public class Viewport
	{
		protected Size2D viewPortSize;
		protected double distanceEye;
		private Point3D eye;
		private Plane viewPortPlane;
		private Hashtable planeColorHashtable;
		private int colorMinValue = 0;

		public double DistanceEye
		{
			get
			{
				return distanceEye;
			}
		}

		public Size2D ViewPortSize
		{
			get
			{
				return viewPortSize;
			}
		}

		public Viewport( Size2D viewPort, double distance )
		{
			planeColorHashtable = new Hashtable();
			viewPortSize = viewPort;
			distanceEye = distance;
			CalculateBounds();
		}

		public Bitmap Render( Scene3D scene )
		{
			Bitmap result = new Bitmap( MathHelper.Floor( viewPortSize.Width ), MathHelper.Floor( viewPortSize.Height ) );
			int bitmapHeight = result.Height;
			int bitmapWidth = result.Width;
			ABuffer aBuffer = new ABuffer( bitmapWidth, bitmapHeight );
			planeColorHashtable.Clear();
			foreach( Plane plane in scene.Planes )
			{
				DrawPlane( plane, aBuffer );
			}
			for( int y = 0; y < bitmapHeight; y++ )
			{
				for( int x = 0; x < bitmapWidth; x++ )
				{
					Color color = GetColorFormABuffer( aBuffer, x, y );
					result.SetPixel( x, y, color );
				}
			}

			return result;
		}

		private Color GetColorFormABuffer( ABuffer aBuffer, int x, int y )
		{
			return aBuffer.GetColors( x, y )[ 0 ];
		}

		private void DrawPlane( Plane plane, ABuffer aBuffer )
		{
			PointF[] plainReflection = GetPlainReflection( plane );
			Hashtable ranges = GetDrawPlainRanges( plainReflection );
			foreach( int y in ranges.Keys )
			{
				DrawPlaneRange planeRange = ranges[ y ] as DrawPlaneRange;
				for( int xIndex = planeRange.MinX - 1; xIndex <= planeRange.MaxX + 1; xIndex++ )
				{
					Point3D viewportPoint = new Point3D( xIndex, y, 0 );
					Point3D crossPoint;
					try
					{
						crossPoint = plane.Cross( new Line3D( eye, viewportPoint ) );
						PointF pointReflection = GetPointReflection( crossPoint );
						if( PointInside( plainReflection, pointReflection ) == false )
						{
							continue;
						}
						Color planeColor;
						if( planeColorHashtable.ContainsKey( plane ) )
						{
							planeColor = ( Color )planeColorHashtable[ plane ];
						}
						else
						{
							planeColor = GetPlaneColor( plane );
							planeColorHashtable.Add( plane, planeColor );
						}
						aBuffer.AddColor( xIndex + MathHelper.Round( viewPortSize.Width / 2 ), y + MathHelper.Round( viewPortSize.Height / 2 ), viewportPoint.GetDistance( crossPoint ), planeColor );
					}
					catch( DoesNotCrossException exception )
					{
					}
				}
			}
		}

		private bool PointInside( PointF[] plainReflection, PointF pointReflection )
		{
			double currentSide = 0;
			for( int firstIndex = 0; firstIndex < plainReflection.Length; firstIndex++ )
			{
				int secondIndex = ( firstIndex + 1 ) % plainReflection.Length;

				PointF firstPoint = plainReflection[ firstIndex ];
				PointF secondPoint = plainReflection[ secondIndex ];
				Line2D line = new Line2D( firstPoint, secondPoint );
				double side = line.GetOrientation( pointReflection );
				if( currentSide == 0 )
				{
					currentSide = side;
				}
				else
				{
					if( currentSide * side < 0 )
					{
						return false;
					}
				}
			}
			return true;
		}

		private Hashtable GetDrawPlainRanges( PointF[] plainReflection )
		{
			Hashtable result = new Hashtable();
			for( int firstIndex = 0; firstIndex < plainReflection.Length; firstIndex++ )
			{
				int secondIndex = ( firstIndex + 1 ) % plainReflection.Length;
				Point[] points = Line2DVisualizer.VisualizeLine2D( new Point( MathHelper.Round( plainReflection[ firstIndex ].X ), MathHelper.Round( plainReflection[ firstIndex ].Y ) ), new Point( MathHelper.Round( plainReflection[ secondIndex ].X ), MathHelper.Round( plainReflection[ secondIndex ].Y ) ) );
				foreach (Point point in points)
				{
					SetRangeForDrawPlane( result, point.Y, point.X );
				}
			}
			return result;
		}

		private void SetRangeForDrawPlane( Hashtable ranges, int y, int x )
		{
			if( ranges.ContainsKey( y ) == false )
			{
				ranges.Add( y, new DrawPlaneRange( x, x ) );
			}
			else
			{
				( ranges[ y ] as DrawPlaneRange ).SetX( x );
			}
		}

		private PointF[] GetPlainReflection( Plane plane )
		{
			PointF[] planeReflection = new PointF[ plane.Points.Length ];
			for( int index = 0; index < planeReflection.Length; index++ )
			{
				planeReflection[ index ] = GetPointReflection( plane.Points[ index ] );
			}
			return planeReflection;
		}

		private Color GetPlaneColor( Plane plane )
		{
			Point3D lightVector = new Point3D( 0, 0, 1 );
			double cosA = Math.Sqrt( Math.Abs( lightVector.GetCosA( new Point3D( plane.A, plane.B, plane.C ) ) ) );
			Color planeColor = plane.PlaneColor;
			return Color.FromArgb( planeColor.A, MathHelper.Round( ( planeColor.R - colorMinValue ) * cosA + colorMinValue ), MathHelper.Round( ( planeColor.G - colorMinValue ) * cosA + colorMinValue ), MathHelper.Round( ( planeColor.B - colorMinValue ) * cosA + colorMinValue ) );
		}

		private PointF GetPointReflection( Point3D point3D )
		{
			Line3D viewLine = new Line3D( eye, point3D );
			Point3D reflectionPoint = viewPortPlane.Cross( viewLine );
			return new PointF( ( float )( reflectionPoint.X ), ( float )( reflectionPoint.Y ) );
		}

		private void CalculateBounds()
		{
			eye = new Point3D( 0, 0, -distanceEye );
			viewPortPlane = new Plane( 
				new Point3D[ 4 ] { 
									new Point3D( -viewPortSize.Width / 2, -viewPortSize.Height / 2, 0 ),
									new Point3D( -viewPortSize.Width / 2, viewPortSize.Height / 2, 0 ),
									new Point3D( viewPortSize.Width / 2, viewPortSize.Height / 2, 0 ),
									new Point3D( viewPortSize.Width / 2, -viewPortSize.Height / 2, 0 )
								 } );
		}

		private class DrawPlaneRange
		{
			public int MinX;
			public int MaxX;

			public DrawPlaneRange( int minX, int maxX )
			{
				MinX = minX;
				MaxX = maxX;
			}

			public void SetX( int x )
			{
				if( x < MinX )
				{
					MinX = x;
				}
				if( x > MaxX )
				{
					MaxX = x;
				}
			}
		}
	}
}
