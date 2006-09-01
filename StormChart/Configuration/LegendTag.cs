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

namespace BC.Controls.StormChart.Configuration
{
	public class LegendTag
	{
		protected Color backColor;
		protected Font itemFont;
		protected Color textColor;
		protected string _Visible;

		public Color BackColor
		{
			get
			{
				return backColor;
			}
			set
			{
				backColor = value;
			}
		}
		public Font ItemFont
		{
			get
			{
				return itemFont;
			}
			set
			{
				itemFont = value;
			}
		}
		public Color TextColor
		{
			get
			{
				return textColor;
			}
			set
			{
				textColor = value;
			}
		}

		public string Visible
		{
			get { return _Visible; }
			set { _Visible = value; }
		}

		public bool IsVisible
		{
			get
			{
				// Default to Yes
				if( _Visible == null || _Visible.Length == 0 )
					return true;
				if( _Visible.ToUpper().CompareTo( "NO" ) == 0 )
					return false;
				return true;
			}
		}
		public LegendTag()
		{
			itemFont = new Font( "Tahoma", 10 );
			backColor = Color.LightYellow;
			textColor = Color.Black;
		}
	}
}
