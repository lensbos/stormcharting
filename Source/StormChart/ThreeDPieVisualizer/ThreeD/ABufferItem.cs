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

namespace BC.Controls.StormChart.ThreeDPieVisualizer.ThreeD
{
	public class ABufferItem : IComparer
	{
		protected double distance;
		protected Color itemColor;

		public double Distance
		{
			get
			{
				return distance;
			}
		}
		public Color ItemColor
		{
			get
			{
				return itemColor;
			}
		}

		public ABufferItem( double distance, Color itemColor )
		{
			this.distance = distance;
			this.itemColor = itemColor;
		}

		public int Compare( object x, object y )
		{
			if( x is ABufferItem && x is ABufferItem )
			{
				ABufferItem itemX = x as ABufferItem;
				ABufferItem itemY = y as ABufferItem;
				if( itemX.Distance < itemY.Distance )
				{
					return -1;
				}
				if( itemX.Distance > itemY.Distance )
				{
					return 1;
				}
			}
			return 0;
		}
	}
}
