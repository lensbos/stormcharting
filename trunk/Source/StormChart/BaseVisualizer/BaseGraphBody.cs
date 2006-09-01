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

namespace BC.Controls.StormChart.BaseVisualizer
{
	public class BaseGraphBody : Canvas
	{
		protected BaseAxis verticalAxis;
		protected BaseAxis horizontalAxis;
		protected BaseGraphBackground background;
		protected BaseGraph graph;

		public BaseGraphBody() : base()
		{
			InitGraphBodyParams();
		}

		public BaseGraphBackground Background
		{
			get
			{
				return background;
			}
		}
		public BaseGraph Graph
		{
			get
			{
				return graph;
			}
		}

		public BaseAxis VerticalAxis
		{
			get
			{
				return GetVerticalAxis();
			}
		}
		
		public BaseAxis HorizontalAxis
		{
			get
			{
				return GetHorizontalAxis();
			}
		}

		public override void Paint( Graphics graphics )
		{
			Measure( graphics );
			if( orientation == CanvasOrientation.Horizontal )
			{
				int verticalAxisX = 0;
				int verticalAxisY = 0;
				int verticalAxisWidth = verticalAxis.Width;
				int horizontalAxisHeight = horizontalAxis.Height;
				int verticalAxisHeight = height - horizontalAxisHeight;
				int horizontalAxisX = verticalAxisWidth;
				int horizontalAxisY = verticalAxisHeight;
				int horizontalAxisWidth = width - verticalAxisWidth;
				int graphX = verticalAxisX + verticalAxisWidth;
				int graphY = 0;
				background.Width = width - graphX;
				background.Height = height - horizontalAxisHeight;
				graph.Width = width - graphX;
				graph.Height = height - horizontalAxisHeight;
				verticalAxis.Height = verticalAxisHeight;
				horizontalAxis.Width = horizontalAxisWidth;
				GraphicsContainer container = graphics.BeginContainer();
				graphics.TranslateTransform( graphX, graphY );
				graphics.Clip = new Region( new Rectangle( 0, 0, background.Width + 1, background.Height + 1 ) );
				background.Paint( graphics );
				graphics.EndContainer( container );

				container = graphics.BeginContainer();
				graphics.TranslateTransform( verticalAxisX, verticalAxisY );
				verticalAxis.Paint( graphics );
				graphics.EndContainer( container );
				container = graphics.BeginContainer();
				graphics.TranslateTransform( horizontalAxisX, horizontalAxisY );
				horizontalAxis.Paint( graphics );
				graphics.EndContainer( container );

				// draw the data lines last, so that the grid lines
				// will not obscure them
				container = graphics.BeginContainer();
				graphics.TranslateTransform( graphX, graphY );
				graphics.Clip = background.GetVisibleRegion();
				graph.Paint( graphics );
				graphics.EndContainer( container );
			}
			else
			{
				int horizontalAxisX = 0;
				int horizontalAxisY = 0;
				int horizontalAxisWidth = horizontalAxis.Width;
				int verticalAxisHeight = verticalAxis.Height;
				int horizontalAxisHeight = height - verticalAxisHeight;
				int verticalAxisX = horizontalAxisWidth;
				int verticalAxisY = horizontalAxisHeight;
				int verticalAxisWidth = width - horizontalAxisWidth;
				int graphX = horizontalAxisX + horizontalAxisWidth;
				int graphY = 0;
				background.Width = width - graphX;
				background.Height = height - verticalAxisHeight;
				graph.Width = width - graphX;
				graph.Height = height - verticalAxisHeight;
				verticalAxis.Width = verticalAxisWidth;
				horizontalAxis.Height = horizontalAxisHeight;
				GraphicsContainer container = graphics.BeginContainer();
				graphics.TranslateTransform( graphX, graphY );
				graphics.Clip = new Region( new Rectangle( 0, 0, background.Width, background.Height ) );
				background.Paint( graphics );
				graphics.EndContainer( container );
				
				container = graphics.BeginContainer();
				graphics.TranslateTransform( verticalAxisX, verticalAxisY );
				verticalAxis.Paint( graphics );
				graphics.EndContainer( container );
				container = graphics.BeginContainer();
				graphics.TranslateTransform( horizontalAxisX, horizontalAxisY );
				horizontalAxis.Paint( graphics );
				graphics.EndContainer( container );

				// draw the data lines last, so that the grid lines
				// will not obscure them
				container = graphics.BeginContainer();
				graphics.TranslateTransform( graphX, graphY );
				graphics.Clip = background.GetVisibleRegion();
				graph.Paint( graphics );
				graphics.EndContainer( container );
			}
		}

		public override void Measure( Graphics graphics )
		{
			base.Measure( graphics );
			verticalAxis.Measure( graphics );
			horizontalAxis.Measure( graphics );
			background.Measure( graphics );
			graph.Measure( graphics );
		}

		protected virtual void InitGraphBodyParams()
		{
			verticalAxis = new BaseAxis();
			verticalAxis.Label = "Y Axis";
			verticalAxis.Orientation = CanvasOrientation.Vertical;
			verticalAxis.ShouldHasZeroValue = true;
			horizontalAxis = new BaseAxis();
			horizontalAxis.Label = "X Axis";
			background = new BaseGraphBackground();
			background.ShouldHasZeroY = true;
			graph = new BaseBarGraph(); 
		}

		protected override void SetOrientation( CanvasOrientation value )
		{
			base.SetOrientation( value );
			background.Orientation = value;
			graph.Orientation = value;
			if( value == CanvasOrientation.Horizontal )
			{
				verticalAxis.Orientation = CanvasOrientation.Vertical;
				horizontalAxis.TitlePosition = BaseAxis.Position.Bottom;
				horizontalAxis.Orientation = CanvasOrientation.Horizontal;
			}
			else
			{
				verticalAxis.Orientation = CanvasOrientation.Horizontal;
				verticalAxis.TitlePosition = BaseAxis.Position.Bottom;
				horizontalAxis.Orientation = CanvasOrientation.Vertical;
			}
		}

		protected virtual BaseAxis GetVerticalAxis()
		{
			return verticalAxis;
		}

		protected virtual BaseAxis GetHorizontalAxis()
		{
			return horizontalAxis;
		}
	}
}
