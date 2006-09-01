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
using System.Drawing.Drawing2D;
using BC.Controls.StormChart.Common;

namespace BC.Controls.StormChart.BaseVisualizer
{
	public class BaseAxis : Canvas
	{
		public enum Position
		{
			Top,
			Bottom
		}
		
		protected Font font;
		protected Color fontColor;
		protected string label;
		protected string[] stringValues;
		protected double[] values;
		protected double highRange;
		protected double lowRange;
		protected int markLength;
		protected int valuesIdent;
		protected int labelIdent;
		protected Position titlePosition;
		protected bool shouldHasZeroValue;
		protected string zeroStringValue;

		public string ZeroStringValue
		{
			get
			{
				return zeroStringValue;
			}
			set
			{
				zeroStringValue = value;
			}
		}

		public bool ShouldHasZeroValue
		{
			get
			{
				return shouldHasZeroValue;
			}
			set
			{
				shouldHasZeroValue = value;
			}
		}

		public Color FontColor
		{
			get
			{
				return fontColor;
			}
			set
			{
				fontColor = value;
			}
		}

		public Font Font
		{
			get
			{
				return font;
			}
			set
			{
				font = value;
			}
		}
		public string[] StringValues
		{
			get
			{
				return stringValues;
			}
			set
			{
				stringValues = value;
			}
		}
		public double[] Values
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
		public double HighRange
		{
			get
			{
				return highRange;
			}
			set
			{
				highRange = value;
			}
		}
		public double LowRange
		{
			get
			{
				return lowRange;
			}
			set
			{
				lowRange = value;
			}
		}

		public Position TitlePosition
		{
			get
			{
				return GetTitlePosition();
			}
			set
			{
				SetTitlePosition( value );
			}
		}

		public string Label
		{
			get
			{
				return GetLabel();
			}
			set
			{
				SetLabel( value );
			}
		}
		
		public BaseAxis() : base()
		{
			InitAxisParams();
		}

        public override void Paint( Graphics graphics )
		{
        	Measure( graphics );
			if( orientation == CanvasOrientation.Horizontal )
			{
				if( titlePosition == Position.Bottom )
				{
					graphics.DrawLine( CreatePen(), 0, 0, width, 0 );
					int top = 0;
					if( shouldHasZeroValue && lowRange < 0 && highRange > 0 )
					{
						int x = MathHelper.Round( GetCoordinate( 0, width ) );
						graphics.DrawLine( CreatePen(), x, top, x, top + markLength );
					}
					if( values.Length > 0 )
					{
						foreach( double value in values )
						{
							int x = MathHelper.Round( GetCoordinate( value, width ) );
							graphics.DrawLine( CreatePen(), x, top, x, top + markLength );
						}
						top += markLength;
					}
					if( ( shouldHasZeroValue && lowRange < 0 && highRange > 0 ) || ( stringValues.Length > 0 ) )
					{
						top += labelIdent;
					}
					if( shouldHasZeroValue && lowRange < 0 && highRange > 0 )
					{
						double value = 0;
						string stringValue = zeroStringValue;
						int center = MathHelper.Round( GetCoordinate( value, width ) );
						SizeF valueSize = graphics.MeasureString( stringValue, font );
						int x = center - MathHelper.Round( valueSize.Width / 2 );
						graphics.DrawString( stringValue, font, CreateTextBrush(), x, top );
					}
					if( stringValues.Length > 0 )
					{
						for( int valueIndex = 0; valueIndex < values.Length; valueIndex++ )
						{
							if( valueIndex < stringValues.Length )
							{
								double value = values[ valueIndex ];
								string stringValue = stringValues[ valueIndex ];
								int center = MathHelper.Round( GetCoordinate( value, width ) );
								SizeF valueSize = graphics.MeasureString( stringValue, font );
								int x = center - MathHelper.Round( valueSize.Width / 2 );
								graphics.DrawString( stringValue, font, CreateTextBrush(), x, top );
							}
						}
						SizeF testValueSize = graphics.MeasureString( "0", font );
						top += MathHelper.Ceiling( testValueSize.Height );
					}
					top += labelIdent;
					int labelCenter = width / 2;
					SizeF labelSize = graphics.MeasureString( label, font );
					graphics.DrawString( label, font, CreateTextBrush(), labelCenter - MathHelper.Round( labelSize.Width / 2 ), top );
				}
				else
				{
					graphics.DrawLine( CreatePen(), 0, 0, width, 0 );
					int bottom = height;
					if( shouldHasZeroValue && lowRange < 0 && highRange > 0 )
					{
						int x = MathHelper.Round( GetCoordinate( 0, width ) );
						graphics.DrawLine( CreatePen(), x, bottom, x, bottom - markLength );
					}
					if( values.Length > 0 )
					{
						foreach( double value in values )
						{
							int x = MathHelper.Round( GetCoordinate( value, width ) );
							graphics.DrawLine( CreatePen(), x, bottom, x, bottom - markLength );
						}
						bottom -= markLength;
					}
					if( ( shouldHasZeroValue && lowRange < 0 && highRange > 0 ) || ( stringValues.Length > 0 ) )
					{
						bottom += labelIdent;
					}
					if( shouldHasZeroValue && lowRange < 0 && highRange > 0 )
					{
						double value = 0;
						string stringValue = zeroStringValue;
						int center = MathHelper.Round( GetCoordinate( value, width ) );
						SizeF valueSize = graphics.MeasureString( stringValue, font );
						int x = center - MathHelper.Round( valueSize.Width / 2 );
						graphics.DrawString( stringValue, font, CreateTextBrush(), x, bottom - MathHelper.Ceiling( valueSize.Height ) );
					}
					if( stringValues.Length > 0 )
					{
						for( int valueIndex = 0; valueIndex < values.Length; valueIndex++ )
						{
							if( valueIndex < stringValues.Length )
							{
								double value = values[ valueIndex ];
								string stringValue = stringValues[ valueIndex ];
								int center = MathHelper.Round( GetCoordinate( value, width ) );
								SizeF valueSize = graphics.MeasureString( stringValue, font );
								int x = center - MathHelper.Round( valueSize.Width / 2 );
								graphics.DrawString( stringValue, font, CreateTextBrush(), x, bottom - MathHelper.Ceiling( valueSize.Height ) );
							}
						}
						SizeF testValueSize = graphics.MeasureString( "0", font );
						bottom -= MathHelper.Ceiling( testValueSize.Height );
					}
					bottom -= labelIdent;
					int labelCenter = width / 2;
					SizeF labelSize = graphics.MeasureString( label, font );
					graphics.DrawString( label, font, CreateTextBrush(), labelCenter - MathHelper.Round( labelSize.Width / 2 ), bottom - MathHelper.Ceiling( labelSize.Height ) );
				}
			}
			else
			{
				graphics.DrawLine( CreatePen(), width, 0, width, height );
				int right = width;
				if( shouldHasZeroValue && lowRange < 0 && highRange > 0 )
				{
					int y = MathHelper.Round( height - GetCoordinate( 0, height ) );
					graphics.DrawLine( CreatePen(), right, y, right - markLength, y );
				}
				if( values.Length > 0 )
				{
					foreach( double value in values )
					{
						int y = MathHelper.Round( height - GetCoordinate( value, height ) );
						graphics.DrawLine( CreatePen(), right, y, right - markLength, y );
					}
					right -= markLength;
				}
				if( ( shouldHasZeroValue && lowRange < 0 && highRange > 0 ) || ( stringValues.Length > 0 ) )
				{
					right -= valuesIdent;
				}
				int maxValueWidth = 0;
				if( shouldHasZeroValue && lowRange < 0 && highRange > 0 )
				{
					double value = 0;
					int valueCenter = MathHelper.Round( height - GetCoordinate( value, height ) );
					string stringValue = zeroStringValue;
					SizeF valueSize = graphics.MeasureString( stringValue, font );
					graphics.DrawString( stringValue, font, CreateTextBrush(), right - MathHelper.Round( valueSize.Width ), valueCenter - MathHelper.Round( valueSize.Height / 2 ) );
					if( maxValueWidth < MathHelper.Ceiling( valueSize.Width ) )
					{
						maxValueWidth = MathHelper.Ceiling( valueSize.Width );
					}
				}
				if( stringValues.Length > 0 )
				{
					for( int valueIndex = 0; valueIndex < values.Length; valueIndex++ )
					{
						if( valueIndex < stringValues.Length )
						{
							double value = values[ valueIndex ];
							int valueCenter = MathHelper.Round( height - GetCoordinate( value, height ) );
							string stringValue = stringValues[ valueIndex ];
							SizeF valueSize = graphics.MeasureString( stringValue, font );
							graphics.DrawString( stringValue, font, CreateTextBrush(), right - MathHelper.Round( valueSize.Width ), valueCenter - MathHelper.Round( valueSize.Height / 2 ) );
							if( maxValueWidth < MathHelper.Ceiling( valueSize.Width ) )
							{
								maxValueWidth = MathHelper.Ceiling( valueSize.Width );
							}
						}
					}
					right -= maxValueWidth;
				}
				right -= labelIdent;
				SizeF labelSize = graphics.MeasureString( label, font );
				int labelX = right - MathHelper.Ceiling( labelSize.Height );
				int labelY = height / 2 - MathHelper.Round( labelSize.Width / 2 );
				GraphicsContainer container = graphics.BeginContainer();
				graphics.RotateTransform( -90 );
				graphics.DrawString( label, font, CreateTextBrush(), -labelY - MathHelper.Round( labelSize.Width ), labelX );
				graphics.EndContainer( container );
			}
		}

		public override void Measure( Graphics graphics )
		{
			base.Measure( graphics );
			if( AutoSize )
			{
				SizeF labelSize = graphics.MeasureString( label, font );
				if( orientation == CanvasOrientation.Horizontal )
				{
					height = MathHelper.Ceiling( labelSize.Height ) + labelIdent + 1;
					if( values.Length > 0 )
					{
						height += markLength;
					}
					if( stringValues.Length > 0 )
					{
						SizeF valueSize = graphics.MeasureString( "0", font );
						height += MathHelper.Ceiling( valueSize.Height ) + valuesIdent;
					}
				}
				else
				{
					width = MathHelper.Ceiling( labelSize.Height ) + labelIdent + 1;
					if( values.Length > 0 )
					{
						width += markLength;
					}
					if( ( shouldHasZeroValue && lowRange < 0 && highRange > 0 ) || ( stringValues.Length > 0 ) )
					{
						width += valuesIdent;
					}
					int valuesWidth = 0;
					if( shouldHasZeroValue && lowRange < 0 && highRange > 0 )
					{
						SizeF valueSize = graphics.MeasureString( zeroStringValue, font );
						int valueWidth = MathHelper.Ceiling( valueSize.Width );
						if( valuesWidth < valueWidth )
						{
							valuesWidth = valueWidth;
						}
					}
					if( stringValues.Length > 0 )
					{
						foreach( string stringValue in stringValues )
						{
							SizeF valueSize = graphics.MeasureString( stringValue, font );
							int valueWidth = MathHelper.Ceiling( valueSize.Width );
							if( valuesWidth < valueWidth )
							{
								valuesWidth = valueWidth;
							}
						}
						width += valuesWidth;
					}
				}
			}
		}

		protected double GetCoordinate( double value, double length )
		{
			return base.GetCoordinate( value, length, lowRange, highRange );
		}

		protected virtual Pen CreatePen()
		{
			return new Pen( borderColor, 1 );
		}

		protected virtual Brush CreateBackBrush()
		{
			return new SolidBrush( backColor );
		}

		protected virtual Brush CreateTextBrush()
		{
			return new SolidBrush( fontColor );
		}

		protected virtual void InitAxisParams()
		{
			zeroStringValue = "0";
			shouldHasZeroValue = false;
			font = new Font( "Tahoma", 8 );
			label = "Axis Label";
			highRange = 100;
			lowRange = 10;
			values = new double[ 4 ];
			stringValues = new string[ 4 ];
			values[ 0 ] = 20;
			values[ 1 ] = 40;
			values[ 2 ] = 60;
			values[ 3 ] = 80;
			stringValues[ 0 ] = "20.0";
			stringValues[ 1 ] = "40.0";
			stringValues[ 2 ] = "60.0";
			stringValues[ 3 ] = "80.0";
			markLength = 5;
			valuesIdent = 5;
			labelIdent = 10;
			titlePosition = Position.Bottom;
		}

		protected virtual Position GetTitlePosition()
		{
			return titlePosition;
		}

		protected virtual void SetTitlePosition( Position value )
		{
			titlePosition = value;
		}

		protected virtual string GetLabel()
		{
			return label;
		}

		protected virtual void SetLabel( string value )
		{
			label = value;
		}
	}
}
