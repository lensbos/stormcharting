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
using BC.Controls.StormChart.BarVisualizer;
using BC.Controls.StormChart.BaseVisualizer;
using BC.Controls.StormChart.Common;
using BC.Controls.StormChart.Configuration;

namespace BC.Controls.StormChart
{
	public class BarGraph
	{
		private int aboveIdent;
		private int belowIdent;
		private double yStep;
		private Color[] colorsSequence = new Color[ 4 ]{ Color.Red, Color.Blue, Color.Green, Color.Yellow };

		public BarGraph()
		{
			InitBarGraphParams();
		}

		public Bitmap GenerateImage( ConfigurationReader configurationReader, string dataXml )
		{
			DataReader dataReader = new DataReader( dataXml, configurationReader.SetItems[ 0 ] );
			Bitmap result = new Bitmap( configurationReader.Width, configurationReader.Height );
			SortedList sortedValues = new SortedList();
			foreach( ValueData valueData in dataReader.Values )
			{
				if( valueData.IsXYEmpty() == false )
				{
					string trimmedXValue = valueData.XValue.Trim();
					if( sortedValues.ContainsKey( trimmedXValue ) == false )
					{
						sortedValues.Add( trimmedXValue, new SortedList() );
					}
					( sortedValues[ trimmedXValue ] as SortedList ).Add( valueData.GroupData.Trim(), valueData );
				}
			}
			int maxXValuesCount = sortedValues.Count;
			if( configurationReader.ColorsPool.Count > 0 )
			{
				colorsSequence = new Color[ configurationReader.ColorsPool.Count ];
				for( int index = 0; index < colorsSequence.Length; index++ )
				{
					colorsSequence[ index ] = configurationReader.ColorsPool[ index ];
				}
			}
			BarFoundation foundation = new BarFoundation();
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
			foundation.GraphBody.Background.LinesWidth = configurationReader.Grid.Thickness;
			foundation.GraphBody.HorizontalAxis.Font = configurationReader.XAxis.Font;
			foundation.GraphBody.HorizontalAxis.FontColor = configurationReader.XAxis.AxisColor;
			foundation.GraphBody.HorizontalAxis.Label = configurationReader.XAxis.LabelText;
			foundation.GraphBody.VerticalAxis.Font = configurationReader.YAxis.Font;
			foundation.GraphBody.VerticalAxis.FontColor = configurationReader.YAxis.AxisColor;
			foundation.GraphBody.VerticalAxis.Label = configurationReader.YAxis.LabelText;
			foundation.GraphBody.HorizontalAxis.LowRange = 0;
			foundation.GraphBody.HorizontalAxis.HighRange = ( maxXValuesCount + 1 ) * 10;
			( foundation.GraphBody.Graph as BaseBarGraph ).LowRangeX = 0;
			( foundation.GraphBody.Graph as BaseBarGraph ).HighRangeX = ( maxXValuesCount + 1 ) * 10;
			( foundation.GraphBody.Graph as BaseBarGraph ).GroupCenters = new double[ maxXValuesCount ];
			foundation.GraphBody.HorizontalAxis.Values = new double[ maxXValuesCount ];
			foundation.GraphBody.HorizontalAxis.StringValues = new string[ maxXValuesCount ];
			foundation.GraphBody.Background.LowRangeX = 0;
			foundation.GraphBody.Background.HighRangeX = ( maxXValuesCount + 1 ) * 10;
			foundation.GraphBody.Background.VerticalLines = new double[ maxXValuesCount ];
			for( int index = 0; index < maxXValuesCount; index++ )
			{
				foundation.GraphBody.HorizontalAxis.Values[ index ] = index * 10 + 10;
				foundation.GraphBody.HorizontalAxis.StringValues[ index ] = ProcessStringWithFormat( sortedValues.GetKey( index ) as string, configurationReader.NumberFormat );
				( foundation.GraphBody.Graph as BaseBarGraph ).GroupCenters[ index ] = index * 10 + 10;
				foundation.GraphBody.Background.VerticalLines[ index ] = index * 10 + 10;
			}
			double lowYValue = double.MaxValue;
			double highYValue = double.MinValue;
			if( configurationReader.YAxis.IsValueAuto() )
			{
				for( int index = 0; index < maxXValuesCount; index++ )
				{
					foreach( ValueData value in ( sortedValues.GetByIndex( index ) as SortedList ).Values )
					{
						double yValue = Converter.StringToDouble( value.YValue );
						if( lowYValue > yValue )
						{
							lowYValue = yValue;
						}
						if( highYValue < yValue )
						{
							highYValue = yValue;
						}
					}
				}
				lowYValue -= belowIdent;
				highYValue += aboveIdent;
			}
			else
			{
				lowYValue = configurationReader.YAxis.MinValue;
				highYValue = configurationReader.YAxis.MaxValue;
			}
			foundation.GraphBody.VerticalAxis.HighRange = highYValue;
			foundation.GraphBody.VerticalAxis.LowRange = lowYValue;
			( foundation.GraphBody.Graph as BaseBarGraph ).HighRangeY = highYValue;
			( foundation.GraphBody.Graph as BaseBarGraph ).LowRangeY = lowYValue;
			foundation.GraphBody.Background.LowRangeY = lowYValue;
			foundation.GraphBody.Background.HighRangeY = highYValue;
			if( configurationReader.Grid.HorizontalDivisionLines  >= 0 )
				yStep = ( highYValue - lowYValue ) / ( configurationReader.Grid.HorizontalDivisionLines + 1 );

			double yMark = lowYValue;
			ArrayList yValues = new ArrayList();
			while( yMark <= highYValue )
			{
				yValues.Add( yMark );
				yMark += yStep;
			}
			foundation.GraphBody.VerticalAxis.Values = new double[ yValues.Count ];
			foundation.GraphBody.VerticalAxis.StringValues = new string[ yValues.Count ];
			foundation.GraphBody.Background.HorizontalLines = new double[ yValues.Count ];
			for( int index = 0; index < yValues.Count; index++ )
			{
				foundation.GraphBody.VerticalAxis.Values[ index ] = ( double )yValues[ index ];
				foundation.GraphBody.VerticalAxis.StringValues[ index ] = ( ( double )yValues[ index ] ).ToString( configurationReader.NumberFormat );
				foundation.GraphBody.Background.HorizontalLines[ index ] = ( double )yValues[ index ];
			}
			( foundation.GraphBody.Graph as BaseBarGraph ).Values = new double[ maxXValuesCount ][];
			int maxBarColors = 0;
			for( int index = 0; index < maxXValuesCount; index++ )
			{
				SortedList ySortedValues = sortedValues.GetByIndex( index ) as SortedList;
				( foundation.GraphBody.Graph as BaseBarGraph ).Values[ index ] = new double[ ySortedValues.Count ];
				if( maxBarColors < ySortedValues.Count )
				{
					maxBarColors = ySortedValues.Count;
				}
				for( int yValuesIndex = 0; yValuesIndex < ySortedValues.Count; yValuesIndex++ )
				{
					( foundation.GraphBody.Graph as BaseBarGraph ).Values[ index ][ yValuesIndex ] = Converter.StringToDouble( ( ySortedValues.GetByIndex( yValuesIndex ) as ValueData ).YValue );
				}
			}
			string[] legentTexts = new string[ maxBarColors ];
			double[] legentValues = new double[ maxBarColors ];
			for( int index = 0; index < maxBarColors; index++ )
			{
				legentValues[ index ] = 0;
			}
			for( int index = 0; index < maxXValuesCount; index++ )
			{
				SortedList ySortedValues = sortedValues.GetByIndex( index ) as SortedList;
				for( int yValuesIndex = 0; yValuesIndex < ySortedValues.Count; yValuesIndex++ )
				{
					legentTexts[ yValuesIndex ] = ( ySortedValues.GetByIndex( yValuesIndex ) as ValueData ).GroupData.Trim();
					legentValues[ yValuesIndex ] += Converter.StringToDouble( ( ySortedValues.GetByIndex( yValuesIndex ) as ValueData ).YValue );
				}
			}
			( foundation.GraphBody.Graph as BaseBarGraph ).BarColors = new Color[ maxBarColors ];
			( foundation.GraphBody.Graph as BaseBarGraph ).BarWidth = ( double )( 10 -  1 ) / maxBarColors;
			foundation.Legend.MaxLegentWidth = configurationReader.Width / 2;
			foundation.Legend.BackColor = configurationReader.Legend.BackColor;
			foundation.Legend.Items.Clear();
			for( int index = 0; index < maxBarColors; index++ )
			{
				( foundation.GraphBody.Graph as BaseBarGraph ).BarColors[ index ] = colorsSequence[ index % colorsSequence.Length ];
				BaseBarLegendItem newLegendItem = new BaseVisualizer.BaseBarLegendItem();
				newLegendItem.BarColor = colorsSequence[ index % colorsSequence.Length ];
				newLegendItem.Text = legentTexts[ index ];
				newLegendItem.Value = legentValues[ index ];
				newLegendItem.ValueFormat = configurationReader.NumberFormat;
				newLegendItem.Font = configurationReader.Legend.ItemFont;
				newLegendItem.ForeColor = configurationReader.Legend.TextColor;
				foundation.Legend.Items.Add( newLegendItem );
			}
			foundation.Paint( Graphics.FromImage( result ) );
			return result;
		}

		private string ProcessStringWithFormat( string inputString, string numberFormat )
		{
			try
			{
				double value = Converter.StringToDouble( inputString );
				return value.ToString( numberFormat );
			}
			catch
			{
			
			}
			try
			{
				DateTime value = Convert.ToDateTime( inputString );
				return value.ToString( "MM/dd/yyyy" );
			}
			catch
			{

			}
			return inputString;
		}

		protected virtual void InitBarGraphParams()
		{
			belowIdent = 0;
			yStep = 200;
			aboveIdent = 10;
		}
	}
}
