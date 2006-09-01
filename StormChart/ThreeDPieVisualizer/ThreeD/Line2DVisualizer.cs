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
	public class Line2DVisualizer
	{
		public static Point[] VisualizeLine2D( Point firstPoint, Point secondPoint )
		{
			if( firstPoint.X == secondPoint.X && firstPoint.Y == secondPoint.Y )
			{
				return new Point[ 1 ] { firstPoint };
			}
			ArrayList points = new ArrayList();
			// Ax + By + C = 0
			long A = secondPoint.Y - firstPoint.Y;
			long B = firstPoint.X - secondPoint.X;
			long C = -A * firstPoint.X - B * firstPoint.Y;
			if( Math.Abs( secondPoint.X - firstPoint.X ) > Math.Abs( secondPoint.Y - firstPoint.Y ) )
			{
				int minX = Math.Min( firstPoint.X, secondPoint.X );
				int maxX = Math.Max( firstPoint.X, secondPoint.X );
				for( int xIndex = minX; xIndex <= maxX; xIndex++ )
				{
					int y = MathHelper.Round( ( -C - A * xIndex ) / B );
					points.Add( new Point( xIndex, y ) );
				}
			}
			else
			{
				int minY = Math.Min( firstPoint.Y, secondPoint.Y );
				int maxY = Math.Max( firstPoint.Y, secondPoint.Y );
				for( int yIndex = minY; yIndex <= maxY; yIndex++ )
				{
					int x = MathHelper.Round( ( -C - B * yIndex ) / A );
					points.Add( new Point( x, yIndex ) );
				}
			}
			Point[] result = new Point[ points.Count ];
			points.CopyTo( result );
			return result;
		}
	}
}
