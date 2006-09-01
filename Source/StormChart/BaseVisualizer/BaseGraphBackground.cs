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

namespace BC.Controls.StormChart.BaseVisualizer
{
	public class BaseGraphBackground : Canvas
	{
		public enum BackgroundDrawType
		{
			Solid,
			VerticalBands,
			HorizontalBands,
			Cells,
			Image
		}

		protected double[] verticalLines;
		protected double[] horizontalLines;
		protected double lowRangeY;
		protected double highRangeY;
		protected double lowRangeX;
		protected double highRangeX;
		protected BackgroundDrawType backgroundType;
		protected Color firstBackgroundColor;
		protected Color secondBackgroundColor;
		protected bool zeroHasFirstColor;
		protected int borderWidth;
		protected int linesWidth;
		protected string imageFileName;
		protected bool shouldHasZeroY;

		public bool ShouldHasZeroY
		{
			get
			{
				return shouldHasZeroY;
			}
			set
			{
				shouldHasZeroY = value;
			}
		}

		public string ImageFileName
		{
			get
			{
				return imageFileName;
			}
			set
			{
				imageFileName = value;
			}
		}

		public int LinesWidth
		{
			get
			{
				return linesWidth;
			}
			set
			{
				linesWidth = value;
			}
		}

		public int BorderWidth
		{
			get
			{
				return borderWidth;
			}
			set
			{
				borderWidth = value;
			}
		}

		public double[] VerticalLines
		{
			get
			{
				return verticalLines;
			}
			set
			{
				verticalLines = value;
			}
		}
		public double[] HorizontalLines
		{
			get
			{
				return horizontalLines;
			}
			set
			{
				horizontalLines = value;
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
		public BackgroundDrawType BackgroundType
		{
			get
			{
				return backgroundType;
			}
			set
			{
				backgroundType = value;
			}
		}

		public BaseGraphBackground() : base()
		{
			InitGraphBackGroundParams();
		}

		public override void Paint( Graphics graphics )
		{
			if( orientation == CanvasOrientation.Horizontal )
			{
				switch( backgroundType )
				{
					case BackgroundDrawType.Image:
						Bitmap image = new Bitmap( imageFileName );
						graphics.DrawImage( image, 0, 0, width, height );
						break;
					case BackgroundDrawType.HorizontalBands:
						Color[] colors = new Color[2];
						colors[ 0 ] = firstBackgroundColor;
						colors[ 1 ] = secondBackgroundColor;
						int colorIndex = 1;
						if( zeroHasFirstColor )
						{
							colorIndex = 0;
						}
						ArrayList linesList = new ArrayList();
						linesList.Add( lowRangeY );
						if( shouldHasZeroY && lowRangeY < 0 && highRangeY > 0 )
						{
							linesList.Add( ( double )0 );
						}
						foreach( double line in horizontalLines )
						{
							linesList.Add( line );
						}
						linesList.Add( highRangeY );
						for( int lineIndex = 0; lineIndex < linesList.Count - 1; lineIndex++ )
						{
							int secondLineIndex = lineIndex + 1;
							DrawHorizontalBand( graphics, ( double )linesList[ lineIndex ], ( double )linesList[ secondLineIndex ], colors[ colorIndex ] );
							colorIndex = 1 - colorIndex;
						}
						break;
					case BackgroundDrawType.Solid:
						graphics.FillRectangle( CreateSolidBrush(), 0, 0, width, height );
						break;
				}
				if( shouldHasZeroY && lowRangeY < 0 && highRangeY > 0 )
				{
					int horizontalLineY = height - MathHelper.Round( GetCoordinateY( 0, height ) );
					graphics.DrawLine( CreateLinesPen(), 0, horizontalLineY, width, horizontalLineY );
				}
				foreach( double hLine in horizontalLines )
				{
					int hLineY = height - MathHelper.Round( GetCoordinateY( hLine, height ) );
					graphics.DrawLine( CreateLinesPen(), 0, hLineY, width, hLineY );
				}
				foreach( double verticalLine in verticalLines )
				{
					int verticalLineX = MathHelper.Round( GetCoordinateX( verticalLine, width ) );
					graphics.DrawLine( CreateLinesPen(), verticalLineX, 0, verticalLineX, height );
				}
			}
			else
			{
				switch( backgroundType )
				{
					case BackgroundDrawType.Image:
						Bitmap image = new Bitmap( imageFileName );
						graphics.DrawImage( image, 0, 0, width, height );
						break;
					case BackgroundDrawType.HorizontalBands:
						Color[] colors = new Color[2];
						colors[ 0 ] = firstBackgroundColor;
						colors[ 1 ] = secondBackgroundColor;
						int colorIndex = 1;
						if( zeroHasFirstColor )
						{
							colorIndex = 0;
						}
						ArrayList linesList = new ArrayList();
						linesList.Add( lowRangeY );
						if( shouldHasZeroY && lowRangeY < 0 && highRangeY > 0 )
						{
							linesList.Add( ( double )0 );
						}
						foreach( double line in horizontalLines )
						{
							linesList.Add( line );
						}
						linesList.Add( highRangeY );
						for( int lineIndex = 0; lineIndex < linesList.Count - 1; lineIndex++ )
						{
							int secondLineIndex = lineIndex + 1;
							DrawHorizontalBand( graphics, ( double )linesList[ lineIndex ], ( double )linesList[ secondLineIndex ], colors[ colorIndex ] );
							colorIndex = 1 - colorIndex;
						}
						break;
					case BackgroundDrawType.Solid:
						graphics.FillRectangle( CreateSolidBrush(), 0, 0, width, height );
						break;
				}
				if( shouldHasZeroY && lowRangeY < 0 && highRangeY > 0 )
				{
					int horizontalLineX = MathHelper.Round( GetCoordinateX( 0, width ) );
					graphics.DrawLine( CreateLinesPen(), horizontalLineX, 0, horizontalLineX, height );
				}
				foreach( double horizontalLine in horizontalLines )
				{
					int horizontalLineX = MathHelper.Round( GetCoordinateX( horizontalLine, width ) );
					graphics.DrawLine( CreateLinesPen(), horizontalLineX, 0, horizontalLineX, height );
				}
				foreach( double verticalLine in verticalLines )
				{
					int verticalLineY = MathHelper.Round( GetCoordinateY( verticalLine, height ) );
					graphics.DrawLine( CreateLinesPen(), 0, height - verticalLineY, width, height - verticalLineY );
				}
			}
			graphics.DrawRectangle( CreateBorderPen(), 0, 0, width, height );
		}

		public virtual Region GetVisibleRegion()
		{
			//return new Region( new Rectangle( 1, 1, width - 1, height - 1 ) );
			return new Region(new Rectangle(-1, -1, width + 2, height + 2));
		}
		
		protected Pen CreateLinesPen()
		{
			return new Pen( foreColor, linesWidth );
		}
		
		protected Pen CreateBorderPen()
		{
			return new Pen( borderColor, borderWidth );
		}

		protected Brush CreateSolidBrush()
		{
			return new SolidBrush( backColor );
		}

		protected void DrawHorizontalBand( Graphics graphics, double from, double to, Color color )
		{
			int fromCoordinate;
			int toCoordinate;
			if( orientation == CanvasOrientation.Horizontal )
			{
				fromCoordinate = MathHelper.Round( GetCoordinateY( from, height ) );
				toCoordinate = MathHelper.Round( GetCoordinateY( to, height ) );
				graphics.FillRectangle( new SolidBrush( color ), 0, height - toCoordinate, width, toCoordinate - fromCoordinate );
			}
			else
			{
				fromCoordinate = MathHelper.Round( GetCoordinateX( from, width ) );
				toCoordinate = MathHelper.Round( GetCoordinateX( to, width ) );
				graphics.FillRectangle( new SolidBrush( color ), fromCoordinate, 0, toCoordinate - fromCoordinate, height );
			}
		}

		protected double GetCoordinateY( double value, double lenght )
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
			return base.GetCoordinate( value, lenght, lowRange, highRange );
		}

		protected double GetCoordinateX( double value, double lenght )
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
			return base.GetCoordinate( value, lenght, lowRange, highRange );
		}

		protected virtual void InitGraphBackGroundParams()
		{
			shouldHasZeroY = false;
			verticalLines = new double[ 4 ];
			verticalLines[ 0 ] = 20;
			verticalLines[ 1 ] = 40;
			verticalLines[ 2 ] = 60;
			verticalLines[ 3 ] = 80;
			horizontalLines = new double[ 4 ];
			horizontalLines[ 0 ] = 20;
			horizontalLines[ 1 ] = 40;
			horizontalLines[ 2 ] = 60;
			horizontalLines[ 3 ] = 80;
			lowRangeY = 10;
			highRangeY = 100;
			lowRangeX = 10;
			highRangeX = 100;
			backgroundType = BackgroundDrawType.HorizontalBands;
			imageFileName = "";
			zeroHasFirstColor = true;
			firstBackgroundColor = Color.White;
			secondBackgroundColor = Color.WhiteSmoke;
			foreColor = Color.LightGray;
			borderWidth = 1;
			linesWidth = 1;
		}
	}
}
