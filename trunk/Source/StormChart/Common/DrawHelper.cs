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

namespace BC.Controls.StormChart.Common
{
	public class DrawHelper
	{

		protected static int cantWidth = 10;
		protected static int cantHeight = 10;

		public enum CornerCant
		{
			LeftTop = 1,
			LeftBottom = 2,
			RightTop = 4,
			RightBottom = 8
		}

		public static void DrawRect( Graphics graphics, Rectangle rectangle, int cornerCants, Pen pen, Brush brush )
		{
			ArrayList points = new ArrayList();
			if( ( cornerCants & ( int )CornerCant.LeftTop ) > 0 )
			{
				points.Add( new Point( rectangle.Left, rectangle.Top + cantHeight ) );
				points.Add( new Point( rectangle.Left + cantWidth, rectangle.Top ) );
			}
			else
			{
				points.Add( new Point( rectangle.Left, rectangle.Top ) );
			}
			if( ( cornerCants & ( int )CornerCant.RightTop ) > 0 )
			{
				points.Add( new Point( rectangle.Right - cantWidth, rectangle.Top ) );
				points.Add( new Point( rectangle.Right, rectangle.Top + cantHeight ) );
			}
			else
			{
				points.Add( new Point( rectangle.Right, rectangle.Top ) );
			}
			if( ( cornerCants & ( int )CornerCant.RightBottom ) > 0 )
			{
				points.Add( new Point( rectangle.Right, rectangle.Bottom - cantHeight ) );
				points.Add( new Point( rectangle.Right - cantWidth, rectangle.Bottom ) );
			}
			else
			{
				points.Add( new Point( rectangle.Right, rectangle.Bottom ) );
			}
			if( ( cornerCants & ( int )CornerCant.LeftBottom ) > 0 )
			{
				points.Add( new Point( rectangle.Left + cantWidth, rectangle.Bottom ) );
				points.Add( new Point( rectangle.Left, rectangle.Bottom - cantHeight ) );
			}
			else
			{
				points.Add( new Point( rectangle.Left, rectangle.Bottom ) );
			}
			Point[] pointsArray = new Point[ points.Count ];
			points.CopyTo( pointsArray );
			graphics.FillPolygon( brush, pointsArray );
			graphics.DrawPolygon( pen, pointsArray );
		}
	}
}
