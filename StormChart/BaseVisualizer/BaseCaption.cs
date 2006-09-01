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
	public class BaseCaption : Canvas
	{
		protected string caption;
		protected Font font;
		protected Color fontColor;
		protected int verticalIdent;
		protected int horizontalIdent;

        public BaseCaption() : base()
		{
			InitCaptionParams();
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

		public string Caption
		{
			get
			{
				return caption;
			}
			set
			{
				caption = value;
			}
		}

		public override void Paint( Graphics graphics )
		{
			Measure( graphics );
			DrawHelper.DrawRect( graphics, new Rectangle( 0, 0, width, height ), ( int )DrawHelper.CornerCant.LeftTop, CreatePen(), CreateBackBrush() );
			graphics.DrawString( caption, font, CreateTextBrush(), new RectangleF( horizontalIdent, verticalIdent, width - horizontalIdent * 2, height - verticalIdent * 2 ) );
		}

		protected override void InitParams()
		{
			base.InitParams();
			backColor = Color.LightYellow;
		}

		protected virtual void InitCaptionParams()
		{
			caption = "Chart";
			font = new Font( "Tahoma", 8 );
			fontColor = Color.Black;
			verticalIdent = 5;
			horizontalIdent = 20;
		}

		protected virtual Pen CreatePen()
		{
			return new Pen( BorderColor, 1 );
		}

		protected virtual Brush CreateBackBrush()
		{
			return new SolidBrush( BackColor );
		}

		protected virtual Brush CreateTextBrush()
		{
			return new SolidBrush( fontColor );
		}

		public override void Measure( Graphics graphics )
		{
			base.Measure( graphics );
			if( AutoSize )
			{
				SizeF stringSize = graphics.MeasureString( caption, font );
				width = MathHelper.Ceiling( stringSize.Width ) + horizontalIdent * 2;
				height = MathHelper.Ceiling( stringSize.Height ) + verticalIdent * 2;
			}
		}

		protected virtual string GetCaption()
		{
			return caption;
		}

		protected virtual void SetCaption( string value )
		{
			caption = value;
		}
	}
}
