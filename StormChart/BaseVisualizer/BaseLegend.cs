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
using System.Drawing.Drawing2D;
using BC.Controls.StormChart.Common;

namespace BC.Controls.StormChart.BaseVisualizer
{
	public class BaseLegend : Canvas
	{
		protected LegendItemCollection items;
		
		protected int verticalIdent;
		protected int horizontalIdent;
		protected int lineSpacing;
		protected int maxLegendItemWidth;
		protected bool _Visible;

		public LegendItemCollection Items
		{
			get
			{
				return items;
			}
		}

		public int MaxLegendItemWidth
		{
			get
			{
				return maxLegendItemWidth;
			}
		}

		public int MaxLegentWidth
		{
			get
			{
				return maxLegendItemWidth + horizontalIdent * 2;
			}
			set
			{
				maxLegendItemWidth = value - horizontalIdent * 2;
			}
		}

		public bool Visible
		{
			get { return _Visible; }
			set { _Visible = value; }
		}


		public BaseLegend() : base()
		{
			items = new LegendItemCollection( this );
			InitLegendParams();
		}

		public override void Paint( Graphics graphics )
		{
			Size itemsSize = GetItemsSize();
			DrawHelper.DrawRect( graphics, new Rectangle( 0, 0, width, height ), ( int )( DrawHelper.CornerCant.LeftTop | DrawHelper.CornerCant.RightBottom ), CreatePen(), CreateBrush() );
			int top = verticalIdent;
			for( int index = 0; index < items.Count; index++ )
			{
				GraphicsContainer container = graphics.BeginContainer();
				graphics.TranslateTransform( horizontalIdent, top );
				items[ index ].Paint( graphics );
				graphics.EndContainer( container );
				top += items[ index ].Height + lineSpacing;
			}
		}

		public override void Measure( Graphics graphics )
		{
			base.Measure( graphics );
			if( autoSize )
			{
				foreach( BaseLegendItem legentItem in items )
				{
					legentItem.Measure( graphics );
				}
				Size size = GetItemsSize();
				width = size.Width + horizontalIdent * 2;
				height = size.Height + verticalIdent * 2;
			}
		}

		public virtual int GetDataWidth( Graphics graphics )
		{
			int result = 0;
			foreach( BaseLegendItem legendItem in items )
			{
				if( result < legendItem.GetDataWidth( graphics ) )
				{
					result = legendItem.GetDataWidth( graphics );
				}
			}
			return result;
		}

		public virtual int GetMarkerWidth( Graphics graphics )
		{
			int result = 0;
			foreach( BaseLegendItem legendItem in items )
			{
				if( result < legendItem.GetMarkerWidth( graphics ) )
				{
					result = legendItem.GetMarkerWidth( graphics );
				}
			}
			return result;
		}

		public virtual int GetTextWidth( Graphics graphics )
		{
			int result = 0;
			foreach( BaseLegendItem legendItem in items )
			{
				if( result < legendItem.GetTextWidth( graphics ) )
				{
					result = legendItem.GetTextWidth( graphics );
				}
			}
			return result;
		}

		protected override void InitParams()
		{
			base.InitParams();
			backColor = Color.LightYellow;
		}

		protected virtual void InitLegendParams()
		{
			MaxLegentWidth = 300;
			verticalIdent = 10;
			horizontalIdent = 10;
			lineSpacing = 3;
		}
		
		protected virtual Pen CreatePen()
		{
			return new Pen( borderColor, 1 );
		}

		protected virtual Brush CreateBrush()
		{
			return new SolidBrush( backColor );
		}
		
		protected Size GetItemsSize()
		{
			Size result = new Size( 0, 0 );
			for( int index = 0; index < items.Count; index++ )
			{
				result.Height += items[ index ].Height;
				if( result.Width < items[ index ].Width )
				{
					result.Width = items[ index ].Width;
				}
			}
			result.Height += Math.Max( items.Count - 1, 0 ) * lineSpacing;
			return result;
		}

		public class LegendItemCollection : IEnumerable
		{
			protected ArrayList items = new ArrayList();
			protected BaseLegend parent;

			public LegendItemCollection( BaseLegend parent )
			{
				this.parent = parent;
			}

			public int Count
			{
				get
				{
					return items.Count;
				}
			}
			
			public BaseLegendItem this[ int index ]
			{
				get
				{
					return items[ index ] as BaseLegendItem;
				}
				set
				{
					items[ index ] = value;
				}
			}

			public int Add( BaseLegendItem newItem )
			{
				newItem.Parent = this.parent;
				return items.Add( newItem );
			}

			public void Clear()
			{
				items.Clear();
			}

			public void Remove( BaseLegendItem itemToRemove )
			{
				items.Remove( itemToRemove );	
			}

			public void RemoveAt( int index )
			{
				items.RemoveAt( index );
			}

			public IEnumerator GetEnumerator()
			{
				return items.GetEnumerator();
			}
		}
	}
}
