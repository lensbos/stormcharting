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
	public class BaseBarLegendItem : BaseLegendItem
	{
		protected string text;
		protected double value;
		protected string valueFormat;
		protected Color barColor;
		protected int barWidth;
		protected int barHeight;
		protected int barIdent;
		protected Font font;
		
		public BaseBarLegendItem() : base()
		{
			InitBarLegendItemParams();
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

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}
		public double Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}
		public string ValueFormat
		{
			get
			{
				return valueFormat;
			}
			set
			{
				valueFormat = value;
			}
		}

		public Color BarColor
		{
			get
			{
				return GetBarColor();
			}
			set
			{
				SetBarColor( value );
			}
		}
		
		public override void Paint( Graphics graphics )
		{
			Measure( graphics );
			int left = 0;
			SizeF valueSize = graphics.MeasureString( value.ToString( valueFormat ), font );
			graphics.DrawString( value.ToString( valueFormat ), font, CreateTextBrush(), left + parent.GetDataWidth( graphics ) - MathHelper.Ceiling( valueSize.Width ), ( height - MathHelper.Round( valueSize.Height ) ) / 2 );
			left += parent.GetDataWidth( graphics ) + barIdent;
			int barY = MathHelper.Round( ( double )height / 2 - ( double )barHeight / 2 );
			graphics.FillRectangle( CreateBarBrush(), left, barY, barWidth, barHeight );
			graphics.DrawRectangle( CreatePen(), left, barY, barWidth, barHeight );
			left += barWidth + barIdent;
			RectangleF drawRectangle = new RectangleF( left, 0, GetMaxTextWidth( graphics ), height );
			graphics.DrawString( text, font, CreateTextBrush(), drawRectangle );
		}

		private int GetMaxTextWidth( Graphics graphics )
		{
			return parent.MaxLegendItemWidth - parent.GetDataWidth( graphics ) - parent.GetMarkerWidth( graphics );
		}

		public override void Measure( Graphics graphics )
		{
			base.Measure( graphics );
			if( autoSize )
			{
				width = parent.GetDataWidth( graphics ) + parent.GetMarkerWidth( graphics ) + parent.GetTextWidth( graphics );
//				Size textSize = DrawHelper.MeasureString( text, font, parent.MaxLegendWidth - parent.GetDataWidth( graphics ) - parent.GetMarkerWidth( graphics ), graphics );
				SizeF textSize = graphics.MeasureString( text, font, GetMaxTextWidth( graphics ) );
				height = MathHelper.Ceiling( textSize.Height );
			}
		}

		protected Pen CreatePen()
		{
			return new Pen( foreColor, 1 );
		}
		
		protected Brush CreateTextBrush()
		{
			return new SolidBrush( foreColor );
		}

		protected Brush CreateBarBrush()
		{
			return new SolidBrush( barColor );
		}

		protected virtual void InitBarLegendItemParams()
		{
			text = "Legent item";
			value = 100.1;
			valueFormat = "#.##";
			barColor = Color.Red;
			barWidth = 20;
			barHeight = 5;
			barIdent = 5;
			font = new Font( "Tahoma", 8 );
		}

		protected virtual void SetBarColor( Color value )
		{
			barColor = value;
		}

		protected virtual Color GetBarColor()
		{
			return barColor;
		}

		public override int GetDataWidth( Graphics graphics )
		{
			SizeF valueSize = graphics.MeasureString( value.ToString( valueFormat ), font );
			return MathHelper.Ceiling( valueSize.Width );
		}

		public override int GetMarkerWidth( Graphics graphics )
		{
			return barWidth + barIdent * 2;
		}

		public override int GetTextWidth( Graphics graphics )
		{
			SizeF textSize = graphics.MeasureString( text, font, GetMaxTextWidth( graphics ) );
			return MathHelper.Ceiling( textSize.Width );
		}
	}
}
