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
	public abstract class Canvas : BaseObject
	{
		public enum CanvasOrientation
		{
			Vertical,
			Horizontal
		}

		protected int width;
		protected int height;
		protected Color backColor;
		protected Color foreColor;
		protected Color borderColor;
		protected bool autoSize;
		protected CanvasOrientation orientation;
		
		public CanvasOrientation Orientation
		{
			get
			{
				return GetOrientation();
			}
			set
			{
				SetOrientation( value );
			}
		}

		public bool AutoSize
		{
			get
			{
				return GetAutoSize();
			}
			set
			{
				SetAutoSize( value );
			}
		}
		
		public Color BorderColor
		{
			get
			{
				return GetBorderColor();
			}
			set
			{
				SetBorderColor( value );
			}
		}
		
		public Color BackColor
		{
			get
			{
				return GetBackColor();
			}
			set
			{
				SetBackColor( value );
			}
		}

		public Color ForeColor
		{
			get
			{
				return GetForeColor();
			}
			set
			{
				SetForeColor( value );
			}
		}
		
		public int Width
		{
			get
			{
				return GetWidth();
			}
			set
			{
				SetWidth( value );
			}
		}

		public int Height
		{
			get
			{
				return GetHeight();
			}
			set
			{
				SetHeight( value );
			}
		}

		public bool CanChangeHeight
		{
			get
			{
				return GetCanChangeHeight();
			}
		}

		public Canvas() : base()
		{
			InitParams();
		}

		public abstract void Paint( Graphics graphics );

		public virtual void Measure( Graphics graphics )
		{
		}

		protected virtual void InitParams()
		{
			width = 50;
			height = 50;
			backColor = Color.White;
			foreColor = Color.Black;
			borderColor = Color.Black;
			autoSize = true;
			orientation = CanvasOrientation.Horizontal;
		}

		protected virtual int GetWidth()
		{
			return width;
		}

		protected virtual void SetWidth( int value )
		{
			width = value;
		}

		protected virtual bool GetCanChangeWidth()
		{
			return true;
		}

		protected virtual int GetHeight()
		{
			return height;
		}

		protected virtual void SetHeight( int value )
		{
			height = value;
		}

		protected virtual bool GetCanChangeHeight()
		{
			return true;
		}

		protected virtual Color GetBackColor()
		{
			return backColor;
		}

		protected virtual void SetBackColor( Color value )
		{
			backColor = value;
		}

		protected virtual Color GetForeColor()
		{
			return foreColor;
		}

		protected virtual void SetForeColor( Color value )
		{
			foreColor = value;
		}

		protected virtual Color GetBorderColor()
		{
			return borderColor;
		}

		protected virtual void SetBorderColor( Color value )
		{
			borderColor = value;
		}

		protected virtual bool GetAutoSize()
		{
			return autoSize;
		}
		
		protected virtual void SetAutoSize( bool value )
		{
			autoSize = value;
		}

		protected virtual void SetOrientation( CanvasOrientation value )
		{
			orientation = value;			
		}

		protected virtual CanvasOrientation GetOrientation()
		{
			return orientation;			
		}

		protected double GetCoordinate( double value, double length, double lowRange, double highRange )
		{
			return length / ( highRange - lowRange ) * ( value - lowRange);
		}
	}
}
