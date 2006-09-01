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
	public class BaseFoundation : Canvas
	{
		protected BaseGraphBody graphBody;
		protected BaseLegend legend;
		protected BaseCaption caption;
		protected int legendIdent;
		protected int captionIdent;
		protected int surroundIdent;
		protected int borderWidth;
		
		public BaseFoundation() : base()
		{
			InitFoundationParams();
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

		public BaseCaption Caption
		{
			get
			{
				return caption;
			}
		}

		public BaseGraphBody GraphBody
		{
			get
			{
				return graphBody;
			}
		}

		public BaseLegend Legend
		{
			get
			{
				return GetLegend();
			}
		}
		
		public override void Paint( Graphics graphics )
		{
			Measure( graphics );
			int captionY = GetTotalSurroundIdent();
			int graphBodyX = GetTotalSurroundIdent();
			int graphBodyY = caption.Height + captionIdent + GetTotalSurroundIdent();
			int legendX = width - legend.Width - 1 - GetTotalSurroundIdent();
			int legendY = caption.Height + GetTotalSurroundIdent();

			// only pad around the right side, if the legend is visible
			if( legend.Visible == true )
			{
				graphBody.Width = width - legend.Width - legendIdent - GetTotalSurroundIdent() * 2;
				graphBody.Height = height - graphBodyY - GetTotalSurroundIdent() * 2;
			}
			else
			{
				graphBody.Width = width - GetTotalSurroundIdent() * 2;
				graphBody.Height = height - graphBodyY - GetTotalSurroundIdent() * 2;
			}

			int captionX;
			if( orientation == CanvasOrientation.Horizontal )
			{
				captionX = graphBody.VerticalAxis.Width + GetTotalSurroundIdent();
			}
			else
			{
				captionX = graphBody.HorizontalAxis.Width + GetTotalSurroundIdent();
			}
			graphics.FillRectangle( CreateBrush(), 0, 0, width, height );
			GraphicsContainer container = graphics.BeginContainer();
			graphics.TranslateTransform( captionX, captionY );
			caption.Paint( graphics );
			graphics.EndContainer( container );

			/* draw the legend */
			if( legend.Visible == true )
			{
				container = graphics.BeginContainer();
				graphics.TranslateTransform( legendX, legendY );
				legend.Paint( graphics );
				graphics.EndContainer( container );
			}

			container = graphics.BeginContainer();
			graphics.TranslateTransform( graphBodyX, graphBodyY );
			graphBody.Paint( graphics );
			graphics.EndContainer( container );
			if( borderWidth > 0 )
			{
				graphics.DrawRectangle( CreateBorderPen(), borderWidth / 2, borderWidth / 2, width - borderWidth, height - borderWidth );
			}
		}

		public override void Measure( Graphics graphics )
		{
			base.Measure( graphics );
			graphBody.Measure( graphics );
			legend.Measure( graphics );
			caption.Measure( graphics );
		}

		protected Pen CreateBorderPen()
		{
			return new Pen( borderColor, borderWidth );
		}
		
		protected int GetTotalSurroundIdent()
		{
			return surroundIdent + borderWidth;
		}
		
		protected Brush CreateBrush()
		{
			return new SolidBrush( backColor );
		}
		
		protected virtual void InitFoundationParams()
		{
			graphBody = new BaseGraphBody();
			legend = new BaseLegend();
			BaseBarLegendItem newItem = new BaseBarLegendItem();
			newItem.BarColor = Color.Red;
			legend.Items.Add( newItem );
			newItem = new BaseBarLegendItem();
			newItem.BarColor = Color.LightBlue;
			legend.Items.Add( newItem );
			caption = new BaseCaption();
			legendIdent = 10;
			captionIdent = 0;
			surroundIdent = 10;
			borderWidth = 0;
		}

		protected override void SetOrientation( CanvasOrientation value )
		{
			base.SetOrientation( value );
			legend.Orientation = value;
			graphBody.Orientation = value;
			caption.Orientation = value;
		}

		protected virtual BaseLegend GetLegend()
		{
			return legend;
		}
	}
}