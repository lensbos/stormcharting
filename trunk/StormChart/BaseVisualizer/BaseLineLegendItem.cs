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
	public class BaseLineLegendItem : BaseLegendItem
	{
		protected string text;
		protected Color lineColor;
		protected int lineWidth;
		protected int lineLength;
		protected int lineIdent;
		protected Font font;
		
		public BaseLineLegendItem() : base()
		{
			InitLineLegendItemParams();
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

		public Color LineColor
		{
			get
			{
				return GetLineColor();
			}
			set
			{
				SetLineColor( value );
			}
		}
		
		public override void Paint( Graphics graphics )
		{
			Measure( graphics );
			int left = 0;
			graphics.DrawLine( CreateLinePen( lineColor, lineWidth ), left, height / 2, left + lineLength, height / 2 );
			left += lineLength + lineIdent;
			RectangleF drawRectangle = new RectangleF( left, 0, GetMaxTextWidth( graphics ), height );
			graphics.DrawString( text, font, CreateTextBrush(), drawRectangle );
		}

		public override void Measure( Graphics graphics )
		{
			base.Measure( graphics );
			if( autoSize )
			{
				width = parent.GetDataWidth( graphics ) + parent.GetMarkerWidth( graphics ) + parent.GetTextWidth( graphics );
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

		protected Pen CreateLinePen( Color color, int width )
		{
			return new Pen( color, width );
		}

		protected virtual void InitLineLegendItemParams()
		{
			text = "Legent item";
			lineColor = Color.Red;
			lineWidth = 1;
			lineLength = 20;
			lineIdent = 5;
			font = new Font( "Tahoma", 8 );
		}

		protected virtual void SetLineColor( Color value )
		{
			lineColor = value;
		}

		protected virtual Color GetLineColor()
		{
			return lineColor;
		}

		public override int GetDataWidth( Graphics graphics )
		{
			return 0;
		}

		public override int GetMarkerWidth( Graphics graphics )
		{
			return lineLength + lineIdent * 2;
		}

		public override int GetTextWidth( Graphics graphics )
		{
			SizeF textSize = graphics.MeasureString( text, font, GetMaxTextWidth( graphics ) );
			return MathHelper.Ceiling( textSize.Width );
		}

		private int GetMaxTextWidth( Graphics graphics )
		{
			return parent.MaxLegendItemWidth - parent.GetDataWidth( graphics ) - parent.GetMarkerWidth( graphics );
		}
	}
}
