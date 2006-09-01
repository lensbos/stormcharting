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
	public class YAxisTag
	{
		protected string labelText;
		protected Font font;
		protected Color axisColor;
		protected double minValue;
		protected double maxValue;
		protected int indentValue;
		protected string belowZeroAllowed;
		
		public string LabelText
		{
			get
			{
				return labelText;
			}
			set
			{
				labelText = value;
			}
		}
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
		public Color AxisColor
		{
			get
			{
				return axisColor;
			}
			set
			{
				axisColor = value;
			}
		}
		public double MinValue
		{
			get
			{
				return minValue;
			}
			set
			{
				minValue = value;
			}
		}
		public double MaxValue
		{
			get
			{
				return maxValue;
			}
			set
			{
				maxValue = value;
			}
		}

		public string BelowZeroAllowed
		{
			get { return belowZeroAllowed; }
			set { belowZeroAllowed = value; }
		}

		public bool IsBelowZeroAllowed
		{
			get
			{
				if( belowZeroAllowed == null || belowZeroAllowed.Length == 0 )
					return false;
				if( belowZeroAllowed.ToUpper().CompareTo( "YES" ) == 0 )
					return true;
				return false;
			}
		}

		public int Indent
		{
			get { return indentValue; }
			set { indentValue = value; }
		}

		public YAxisTag()
		{
			axisColor = Color.Black;
			labelText = "X Axis";
			font = new Font( "Tahoma", 10 );
			minValue = 0;
			maxValue = 0;
			indentValue = 0;
		}

		public bool IsValueAuto()
		{
			return minValue == maxValue;
		}

	}
}
