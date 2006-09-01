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
using BC.Controls.StormChart.Common;

namespace BC.Controls.StormChart.BaseVisualizer
{
	public class BaseBarGraph : BaseGraph
	{
		protected double[][] values;
		protected double barWidth;
		protected double[] groupCenters;
		protected Color[] barColors;
		protected double lowRangeX;
		protected double highRangeX;
		protected double lowRangeY;
		protected double highRangeY;

		public double[][] Values
		{
			get
			{
				return values;
			}
			set
			{
				values = value;
			}
		}
		public double BarWidth
		{
			get
			{
				return barWidth;
			}
			set
			{
				barWidth = value;
			}
		}
		public double[] GroupCenters
		{
			get
			{
				return groupCenters;
			}
			set
			{
				groupCenters = value;
			}
		}
		public Color[] BarColors
		{
			get
			{
				return barColors;
			}
			set
			{
				barColors = value;
			}
		}
		public double LowRangeX
		{
			get
			{
				return lowRangeX;
			}
			set
			{
				lowRangeX = value;
			}
		}
		public double HighRangeX
		{
			get
			{
				return highRangeX;
			}
			set
			{
				highRangeX = value;
			}
		}
		public double LowRangeY
		{
			get
			{
				return lowRangeY;
			}
			set
			{
				lowRangeY = value;
			}
		}
		public double HighRangeY
		{
			get
			{
				return highRangeY;
			}
			set
			{
				highRangeY = value;
			}
		}

		public BaseBarGraph() : base()
		{
			InitBarGraph();
		}



		public override void Paint( Graphics graphics )
		{
			if( orientation == CanvasOrientation.Horizontal )
			{
				for( int groupIndex = 0; groupIndex < values.Length; groupIndex++ )
				{
					double groupLeft = GetCoordinateX( groupCenters[ groupIndex ] - GetGroupWidth( groupIndex ) / 2, width );
					for( int barIndex = 0; barIndex < values[ groupIndex ].Length; barIndex++ )
					{
						int barY = height - MathHelper.Round( GetCoordinateY( values[ groupIndex ][ barIndex ], height ) );
						int zeroY = height - MathHelper.Round( GetCoordinateY( 0, height ) );
						DrawVerticalBar( graphics, MathHelper.Round( groupLeft ), Math.Max( zeroY, barY ), Math.Min( barY, zeroY ), barColors[ barIndex ] );
						groupLeft += GetCoordinateX( barWidth, width );
					}
				}
			}
			else
			{
				for( int groupIndex = 0; groupIndex < values.Length; groupIndex++ )
				{
					double groupBottom = GetCoordinateY( groupCenters[ groupIndex ] - GetGroupWidth( groupIndex ) / 2, height );
					for( int barIndex = 0; barIndex < values[ groupIndex ].Length; barIndex++ )
					{
						int barX = MathHelper.Round( GetCoordinateX( values[ groupIndex ][ barIndex ], width ) );
						int zeroX = MathHelper.Round( GetCoordinateX( 0, width ) );
						DrawHorizontalBar( graphics, height - MathHelper.Round( groupBottom ), Math.Min( zeroX, barX ), Math.Max( barX, zeroX ), barColors[ barIndex ] );
						groupBottom += GetCoordinateY( barWidth, height );
					}
				}
			}
		}

		protected void DrawVerticalBar( Graphics graphics, int left, int fromY, int toY, Color color )
		{
			graphics.FillRectangle( CreateBarBrush( color ), left, toY, MathHelper.Round( GetCoordinateX( barWidth, width ) ), fromY - toY );
		}

		protected void DrawHorizontalBar( Graphics graphics, int bottom, int fromX, int toX, Color color )
		{
			int barHeight = MathHelper.Round( GetCoordinateY( barWidth, height ) );
			graphics.FillRectangle( CreateBarBrush( color ), fromX, bottom - barHeight, toX - fromX, barHeight );
		}

		protected Brush CreateBarBrush( Color color )
		{
			return new SolidBrush( color );
		}
		
		protected double GetGroupWidth( int groupIndex )
		{
			return values[ groupIndex ].Length * barWidth;
		}
		
		protected double GetCoordinateX( double value, double length )
		{
			double lowRange;
			double highRange;
			if( orientation == CanvasOrientation.Horizontal )
			{
				lowRange = lowRangeX;
				highRange = highRangeX;
			}
			else
			{
				lowRange = lowRangeY;
				highRange = highRangeY;
			}
			return GetCoordinate( value, length, lowRange, highRange );
		}
		
		protected double GetCoordinateY( double value, double length )
		{
			double lowRange;
			double highRange;
			if( orientation == CanvasOrientation.Horizontal )
			{
				lowRange = lowRangeY;
				highRange = highRangeY;
			}
			else
			{
				lowRange = lowRangeX;
				highRange = highRangeX;
			}
			return GetCoordinate( value, length, lowRange, highRange );
		}

		protected virtual void InitBarGraph()
		{
			barColors = new Color[ 4 ];
			barColors[ 0 ] = Color.Green;
			barColors[ 1 ] = Color.Yellow;
			barColors[ 2 ] = Color.Red;
			barColors[ 3 ] = Color.Blue;
			barWidth = 4;
			groupCenters = new double[ 5 ];
			groupCenters[ 0 ] = 20;
			groupCenters[ 1 ] = 40;
			groupCenters[ 2 ] = 60;
			groupCenters[ 3 ] = 80;
			groupCenters[ 4 ] = 100;
			values = new double[ 5 ][];
			values[ 0 ] = new double[ 4 ];
			values[ 0 ][ 0 ] = 15.1;
			values[ 0 ][ 1 ] = 48;
			values[ 0 ][ 2 ] = 43;
			values[ 0 ][ 3 ] = 8;

			values[ 1 ] = new double[ 4 ];
			values[ 1 ][ 0 ] = 42;
			values[ 1 ][ 1 ] = 40;
			values[ 1 ][ 2 ] = 10;
			values[ 1 ][ 3 ] = 22;

			values[ 2 ] = new double[ 4 ];
			values[ 2 ][ 0 ] = 7;
			values[ 2 ][ 1 ] = 29.5;
			values[ 2 ][ 2 ] = 3;
			values[ 2 ][ 3 ] = 35;

			values[ 3 ] = new double[ 4 ];
			values[ 3 ][ 0 ] = 24.5;
			values[ 3 ][ 1 ] = 15;
			values[ 3 ][ 2 ] = 13;
			values[ 3 ][ 3 ] = 24;

			values[ 4 ] = new double[ 4 ];
			values[ 4 ][ 0 ] = 47;
			values[ 4 ][ 1 ] = 26;
			values[ 4 ][ 2 ] = 18;
			values[ 4 ][ 3 ] = 7;

			lowRangeX = 0;
			highRangeX = 100;
			lowRangeY = 0;
			highRangeY = 100;
		}
	}
}
