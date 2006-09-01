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
	public class BaseLineGraph : BaseGraph
	{
		protected LineGraphItem[] graphItems;
		protected double lowRangeX;
		protected double highRangeX;
		protected double lowRangeY;
		protected double highRangeY;

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

		public LineGraphItem[] GraphItems
		{
			get
			{
				return graphItems;
			}
			set
			{
				graphItems = value;
			}
		}

		public BaseLineGraph() : base()
		{
			InitLineGraphParams();
		}

		public override void Paint( Graphics graphics )
		{
			foreach( LineGraphItem item in graphItems )
			{
				// we need to show a dot if there is only one point:
				if (item.Points.Length == 1)
				{
					using (Brush b = new SolidBrush(item.UpColor))
					{
						int x = MathHelper.Round(GetCoordinateX(item.Points[0].X));
						int y = MathHelper.Round(GetCoordinateY(item.Points[0].Y));						

						graphics.FillEllipse(b, x - 2, height - (y - 2), 5, 5);
					}
				}
				else
				{
					for( int pointIndex = 0; pointIndex < item.Points.Length - 1; pointIndex++ )
					{
						if( orientation == CanvasOrientation.Horizontal )
						{
							// should dispose of the pen appropriately
							using (Pen pen = CreateLinePen(item.LineWidth, GetNextLineColor(item, pointIndex)))
							{
								graphics.DrawLine( pen, MathHelper.Round( GetCoordinateX( item.Points[ pointIndex ].X ) ), height - MathHelper.Round( GetCoordinateY( item.Points[ pointIndex ].Y ) ), MathHelper.Round( GetCoordinateX( item.Points[ pointIndex + 1 ].X ) ), height - MathHelper.Round( GetCoordinateY( item.Points[ pointIndex + 1 ].Y ) ) );
							}
						}
						else
						{
							using (Pen pen = CreateLinePen(item.LineWidth, GetNextLineColor(item, pointIndex)))
							{
								graphics.DrawLine( pen, MathHelper.Round( GetCoordinateX( item.Points[ pointIndex ].Y ) ), height - MathHelper.Round( GetCoordinateY( item.Points[ pointIndex ].X ) ), MathHelper.Round( GetCoordinateX( item.Points[ pointIndex + 1 ].Y ) ), height - MathHelper.Round( GetCoordinateY( item.Points[ pointIndex + 1 ].X ) ) );
							}
						}
					}
				}
			}
		}

		protected virtual Pen CreateLinePen( int width, Color color )
		{
			return new Pen( color, width );
		}

		protected virtual Color GetNextLineColor( LineGraphItem graphItem, int pointIndex )
		{
			if( IsNextLineUp( graphItem, pointIndex ) )
			{
				return graphItem.UpColor;
			}
			return graphItem.DownColor;
		}
		
		protected virtual bool IsNextLineUp( LineGraphItem graphItem, int pointIndex )
		{
			double currentY = graphItem.Points[ pointIndex ].Y;
			double nextY = graphItem.Points[ pointIndex + 1 ].Y;
			if( nextY < currentY )
			{
				return false;
			}
			return true;
		}

		protected virtual double GetCoordinateX( double value )
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
			return GetCoordinate( value, width, lowRange, highRange );
		}
		
		protected virtual double GetCoordinateY( double value )
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
			return GetCoordinate( value, height, lowRange, highRange );
		}

		protected virtual void InitLineGraphParams()
		{
			lowRangeX = 5;
			highRangeX = 50;
			lowRangeY = -10;
			highRangeY = 40;
			graphItems = new LineGraphItem[ 1 ];
			LineGraphItem lineGraphItem = new LineGraphItem( new LineGraphPoint[ 4 ] { new LineGraphPoint( 10, 10 ), new LineGraphPoint( 20, 30 ), new LineGraphPoint( 25, 38 ), new LineGraphPoint( 40, 20 ) } );
			lineGraphItem.UpColor = Color.Blue;
			lineGraphItem.DownColor = Color.Red;
			graphItems[ 0 ] = lineGraphItem;
		}
		
		public class LineGraphItem
		{
			protected Color upColor;
			protected Color downColor;
			protected int lineWidth;
			protected LineGraphPoint[] points;

			public LineGraphPoint[] Points
			{
				get
				{
					return points;
				}
			}

			public int LineWidth
			{
				get
				{
					return lineWidth;
				}
				set
				{
					lineWidth = value;
				}
			}

			public Color UpColor
			{
				get
				{
					return upColor;
				}
				set
				{
					upColor = value;
				}
			}
			public Color DownColor
			{
				get
				{
					return downColor;
				}
				set
				{
					downColor = value;
				}
			}

			public Color LineColor
			{
				set
				{
					DownColor = value;
					UpColor = value;
				}
				get
				{
					return UpColor;
				}
			}

			public LineGraphItem( LineGraphPoint[] pointsArray )
			{
				points = pointsArray;
				InitLineGraphItemParams();
			}

			protected virtual void InitLineGraphItemParams()
			{
				LineColor = Color.Black;
				lineWidth = 1;
			}
		}

		public class LineGraphPoint
		{
			protected double x;
			protected double y;

			public double X
			{
				get
				{
					return x;
				}
				set
				{
					x = value;
				}
			}
			public double Y
			{
				get
				{
					return y;
				}
				set
				{
					y = value;
				}
			}

			public LineGraphPoint( double xValue, double yValue )
			{
				x = xValue;
				y = yValue;
			}
		}
	}
}
