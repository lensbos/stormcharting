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
	public class SetTag
	{
		protected string xValuePath;
		protected string yValuePath;
		protected string valuePath;
		protected string groupBy;
		protected string name;
		protected Color setColor;
		protected string filterTagPath;
		protected string filterValue;
		protected string containerPath;
		protected int lineWidth;

		public string ValuePath
		{
			get
			{
				return valuePath;
			}
			set
			{
				valuePath = value;
			}
		}

		public int LineWidth
		{
			get
			{
				return lineWidth;
			}
			set
			{
				lineWidth = value;
			}
		}

		public string ContainerPath
		{
			get
			{
				return containerPath;
			}
			set
			{
				containerPath = value;
			}
		}

		public string XValuePath
		{
			get
			{
				return xValuePath;
			}
			set
			{
				xValuePath = value;
			}
		}
		public string YValuePath
		{
			get
			{
				return yValuePath;
			}
			set
			{
				yValuePath = value;
			}
		}
		public string GroupBy
		{
			get
			{
				return groupBy;
			}
			set
			{
				groupBy = value;
			}
		}
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}
		public Color SetColor
		{
			get
			{
				return setColor;
			}
			set
			{
				setColor = value;
			}
		}
		public string FilterTagPath
		{
			get
			{
				return filterTagPath;
			}
			set
			{
				filterTagPath = value;
			}
		}
		public string FilterValue
		{
			get
			{
				return filterValue;
			}
			set
			{
				filterValue = value;
			}
		}

		public SetTag()
		{
			xValuePath = "";
			yValuePath = "";
			valuePath = "";
			groupBy = "";
			name = "";
			setColor = Color.Black;
			filterTagPath = "";
			filterValue = "";
			containerPath = "";
			lineWidth = 1;
		}

		public bool IsFilterEmpty()
		{
			return filterTagPath.Length == 0;
		}
	}
}
