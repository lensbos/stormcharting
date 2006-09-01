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
using BC.Controls.StormChart.Configuration;

namespace BC.Controls.StormChart
{
	public class GraphManager
	{
		private ConfigurationReader configurationReader;

		public ConfigurationReader ConfigurationReader
		{
			get
			{
				return configurationReader;
			}
		}

		public GraphManager( string configurationXml )
		{
			configurationReader = new ConfigurationReader( configurationXml );
		}

		public Bitmap GenerateImage( string dataXml )
		{
			switch( configurationReader.Graph )
			{
				case ConfigurationReader.GraphType.Bar:
					BarGraph barGraph = new BarGraph();
					return barGraph.GenerateImage( configurationReader, dataXml );
				case ConfigurationReader.GraphType.Line:
					LineGraph lineGraph = new LineGraph();
					return lineGraph.GenerateImage( configurationReader, dataXml );
				case ConfigurationReader.GraphType.Pie:
					PieGraph pieGraph = new PieGraph();
					return pieGraph.GenerateImage( configurationReader, dataXml );
			}
			return null;
		}
	}
}
