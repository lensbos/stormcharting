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
	public class ABuffer
	{
		private double Eps = 0.2;
		private ArrayList[,] items;
		private int width;
		private int height;

		public int Width
		{
			get
			{
				return width;
			}
		}
		public int Height
		{
			get
			{
				return height;
			}
		}

		public ABuffer( int width, int height )
		{
			this.width = width;
			this.height = height;
			items = new ArrayList[ height, width ];
			for( int heightIndex = 0; heightIndex < height; heightIndex++ )
			{
				for( int widthIndex = 0; widthIndex < width; widthIndex++ )
				{
					items[ heightIndex, widthIndex ] = new ArrayList();
				}
			}
		}

		public void AddColor( int x, int y, double distance, Color color )
		{
			if( x < 0 || y < 0 || x >= width || y >= height )
			{
				return;
			}
			items[ y, x ].Add( new ABufferItem( distance, color ) );
		}

		public Color[] GetColors( int x, int y )
		{
			if( items[ y, x ].Count == 0 )
			{
				return new Color[ 1 ] { Color.FromArgb( 0, Color.White ) };
			}
			items[ y, x ].Sort( new ABufferItem( 0, Color.Black ) );
			ABufferItem bufferItem = items[ y, x ][ 0 ] as ABufferItem;
			int R = 0;
			int G = 0;
			int B = 0;
			double prevDistance = bufferItem.Distance;
			int count = 0;
			for( int index = 0; index < items[ y, x ].Count; index++ )
			{
				bufferItem = items[ y, x ][ index ] as ABufferItem;
				if( Math.Abs( bufferItem.Distance - prevDistance ) > Eps )
				{
					break;
				}
				R += bufferItem.ItemColor.R;
				G += bufferItem.ItemColor.G;
				B += bufferItem.ItemColor.B;
				count++;
			}
			R /= count;
			G /= count;
			B /= count;
			return new Color[ 1 ] { Color.FromArgb( 255, R, G ,B ) };
		}
	}
}
