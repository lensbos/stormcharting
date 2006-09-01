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
using BC.Controls.StormChart.BaseVisualizer;
using BC.Controls.StormChart.Common;
using BC.Controls.StormChart.ThreeDPieVisualizer.ThreeD;

namespace BC.Controls.StormChart.ThreeDPieVisualizer
{
	public class PieGraph : BaseGraph
	{
		protected PieDataItem[] dataItems;

		public PieDataItem[] DataItems
		{
			get
			{
				return dataItems;
			}
			set
			{
				dataItems = value;
			}
		}

		public PieGraph() : base()
		{
			InitPieGraphParams();
		}

		protected virtual void InitPieGraphParams()
		{
			dataItems = new PieDataItem[ 2 ];
			dataItems[ 0 ] = new PieDataItem( 30, Color.Green );
			dataItems[ 1 ] = new PieDataItem( 70, Color.Red );
		}

		public override void Paint( Graphics graphics )
		{
			double minLength = Math.Min( width, height );
			int eyeDistance = MathHelper.Round( minLength * 2 );
			int radius = MathHelper.Round( 0.6 * minLength );
			int cutDistance = MathHelper.Round( 0.05 * minLength );
			int pieHeight = MathHelper.Round( 0.2 * minLength );
			int moveDistance = MathHelper.Round( 0.9 * minLength );
			Viewport view = new Viewport( new Size2D( width, height ), eyeDistance );
			Scene3D scene = new Scene3D();
			double totalValue = 0;
			foreach( PieDataItem item in dataItems )
			{
				totalValue += item.Value;
			}
			double startAngle = 0;
			double endAngle = 0;
			foreach( PieDataItem item in dataItems )
			{
				endAngle = startAngle + item.Value / totalValue * 2 * Math.PI;
				Plane[] pie = Constructor3D.ConstructPiePiece( radius, startAngle, endAngle, MathHelper.DegreeToRadian( 5 ), pieHeight, item.ItemColor );
				double middleAngle = ( startAngle + endAngle ) / 2;
				Point2D moveVector2D = Constructor3D.GetPoint( cutDistance, middleAngle );
				foreach( Plane plane in pie )
				{
					plane.Move( new Point3D( moveVector2D.X, 0, moveVector2D.Y ) );
				}
				scene.Planes.AddRange( pie );
				startAngle = endAngle;
			}
			scene.Move( new Point3D( 0, -pieHeight / 2, 0 ) );
			scene.RotateAroundX( MathHelper.DegreeToRadian( 40 ) );
			scene.Move( new Point3D( 0, 0, moveDistance ) );
			graphics.DrawImage( view.Render( scene ), 0, 0 );
		}
	}
}
