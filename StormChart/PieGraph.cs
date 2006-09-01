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
using BC.Controls.StormChart.BaseVisualizer;
using BC.Controls.StormChart.Common;
using BC.Controls.StormChart.Configuration;
using BC.Controls.StormChart.ThreeDPieVisualizer;

namespace BC.Controls.StormChart
{
	public class PieGraph
	{
		private Color[] colorsPool = new Color[ 4 ]{ Color.LightYellow, Color.LightSteelBlue, Color.LightGreen, Color.Aqua };

		public PieGraph()
		{
		}

		public Bitmap GenerateImage( ConfigurationReader configurationReader, string dataXml )
		{
			DataReader reader = new DataReader( dataXml, configurationReader.SetItems[ 0 ] );
			Bitmap result = new Bitmap( configurationReader.Width, configurationReader.Height );
			PieFoundation foundation = new PieFoundation();
			if( configurationReader.ColorsPool.Count > 0 )
			{
                colorsPool = new Color[ configurationReader.ColorsPool.Count ];
				for( int index = 0; index < colorsPool.Length; index++ )
				{
					colorsPool[ index ] = configurationReader.ColorsPool[ index ];
				}
			}
			foundation.Height = configurationReader.Height;
			foundation.Width = configurationReader.Width;
			foundation.Orientation = configurationReader.Orientation;
			foundation.BackColor = configurationReader.CanvasBackColor;
			foundation.BorderColor = configurationReader.CanvasBorderColor;
			foundation.BorderWidth = configurationReader.CanvasBorderThickness;
			foundation.Caption.Caption = configurationReader.Title.Text;
			foundation.Caption.BackColor = configurationReader.Title.BackColor;
			foundation.Caption.Font = configurationReader.Title.Font;
			foundation.Caption.FontColor = configurationReader.Title.TextColor;
			if( configurationReader.ChartChartBackground == ConfigurationReader.ChartBackgroundType.Color )
			{
				foundation.GraphBody.Background.BackgroundType = BaseVisualizer.BaseGraphBackground.BackgroundDrawType.Solid;
				foundation.GraphBody.Background.BackColor = configurationReader.ChartBackgroundColor;
			}
			else
			{
				foundation.GraphBody.Background.BackgroundType = BaseVisualizer.BaseGraphBackground.BackgroundDrawType.Image;
				foundation.GraphBody.Background.ImageFileName = configurationReader.ChartBackgroundImage;
			}
			foundation.GraphBody.Background.ForeColor = configurationReader.Grid.GridColor;
			foundation.GraphBody.Background.HorizontalLines = new double[ 0 ];
			foundation.GraphBody.Background.VerticalLines = new double[ 0 ];
			ArrayList values = new ArrayList();
			ArrayList labels = new ArrayList();
			Hashtable labelsHash = new Hashtable();

			foreach( ValueData valueData in reader.Values )
			{
				int index;
				string trimmedGroupData = valueData.GroupData.Trim();
				if( labelsHash.ContainsKey( trimmedGroupData ) )
				{		
					index = ( int )labelsHash[ trimmedGroupData ];
				}
				else
				{
					index = values.Add( ( double )0 );
					labels.Add( trimmedGroupData );
					labelsHash.Add( trimmedGroupData, index );
				}
				values[ index ] = ( double )values[ index ] + Converter.StringToDouble( valueData.Value );
			}
			( foundation.GraphBody.Graph as ThreeDPieVisualizer.PieGraph ).DataItems = new ThreeDPieVisualizer.PieDataItem[ values.Count ];
			for( int index = 0; index < values.Count; index++ )
			{
				Color color = colorsPool[ index % colorsPool.Length ];
				( foundation.GraphBody.Graph as ThreeDPieVisualizer.PieGraph ).DataItems[ index ] = new ThreeDPieVisualizer.PieDataItem( ( double )values[ index ], color );
			}

			foundation.Legend.MaxLegentWidth = configurationReader.Width / 2;
			foundation.Legend.BackColor = configurationReader.Legend.BackColor;
			foundation.Legend.Items.Clear();
			for( int index = 0; index < values.Count; index++ )
			{
				BasePieLegendItem newLegendItem = new BasePieLegendItem();
				newLegendItem.PieceColor = colorsPool[ index % colorsPool.Length ];
				newLegendItem.Text = labels[ index ] as string;
				newLegendItem.Value = ( double )values[ index ];
				newLegendItem.ValueFormat = configurationReader.NumberFormat;
				newLegendItem.Font = configurationReader.Legend.ItemFont;
				newLegendItem.ForeColor = configurationReader.Legend.TextColor;
				foundation.Legend.Items.Add( newLegendItem );
			}

			foundation.Paint( Graphics.FromImage( result ) );
			return result;
		}
	}
}
