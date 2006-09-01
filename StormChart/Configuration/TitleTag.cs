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
	public class TitleTag
	{
		protected Font font;
		protected Color textColor;
		protected Color backColor;
		protected string text;

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
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		public TitleTag()
		{
			font = new Font( "Tahoma", 10 );
			textColor = Color.Black;
			backColor = Color.LightYellow;
			text = "Title";
		}
	}
}
