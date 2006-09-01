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
using BC.Controls.StormChart.BaseVisualizer;

namespace BC.Controls.StormChart.ThreeDPieVisualizer
{
	public class PieGraphBody : BaseGraphBody
	{
		public PieGraphBody() : base()
		{
		}

		public override void Paint( Graphics graphics )
		{
			Measure( graphics );
			background.Width = width;
			background.Height = height;
			graphics.Clip = new Region( new Rectangle( 0, 0, width + 1, height + 1 ) );
			background.Paint( graphics );
			graph.Width = width;
			graph.Height = height;
			graphics.Clip = background.GetVisibleRegion();
			graph.Paint( graphics );
		}

		public override void Measure( Graphics graphics )
		{
			background.Measure( graphics );
			graph.Measure( graphics );
		}

		protected override void SetOrientation( CanvasOrientation value )
		{
			graph.Orientation = value;
		}

		protected override void InitGraphBodyParams()
		{
			base.InitGraphBodyParams();
			verticalAxis.AutoSize = false;
			verticalAxis.Height = 0;
			verticalAxis.Width = 0;
			horizontalAxis.AutoSize = false;
			horizontalAxis.Height = 0;
			horizontalAxis.Width = 0;
			graph = new PieGraph();
		}
	}
}
