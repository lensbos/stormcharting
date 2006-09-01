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
using BC.Controls.StormChart.LineVisualizer;

namespace BC.Controls.StormChart
{
	public class LineGraph
	{
		private int aboveIdent;
		private int belowIdent;
		private double yStep;

		public LineGraph()
		{
			InitLineGraphParams();
		}

		public Bitmap GenerateImage( ConfigurationReader configurationReader, string dataXml )
		{
			DataReader[] readers = new DataReader[ configurationReader.SetItems.Length ];
			int maxXValuesCount = 0;
			for( int index = 0; index < readers.Length; index++ )
			{
				readers[ index ] = new DataReader( dataXml, configurationReader.SetItems[ index ] );
				if( maxXValuesCount < readers[ index ].Values.Length )
				{
					maxXValuesCount = readers[ index ].Values.Length;
				}
			}

			Bitmap result = new Bitmap( configurationReader.Width, configurationReader.Height );

			LineFoundation foundation = new LineFoundation();
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
			( foundation.GraphBody.Graph as BaseLineGraph ).LowRangeX = 0;
			( foundation.GraphBody.Graph as BaseLineGraph ).HighRangeX = ( maxXValuesCount + 1 ) * 10;
			foundation.GraphBody.Background.LowRangeX = 0;
			foundation.GraphBody.Background.HighRangeX = ( maxXValuesCount + 1 ) * 10;
			ArrayList xAxisValuesList = new ArrayList();
			ArrayList xAxisStringValuesList = new ArrayList();
			for( int readersIndex = 0; readersIndex < readers.Length; readersIndex++ )
			{
				int markPeriodCounter = 0;
				if( configurationReader.XAxis.MarkDrawPeriod == 0 )
					configurationReader.XAxis.MarkDrawPeriod = 1;

				int markPeriodHit = ( readers[ readersIndex ].Values.Length /  configurationReader.XAxis.MarkDrawPeriod );
				int valuePeriodCounter = 0;
				int listIndex = 0;

				bool isDateTime = false;

				try
				{
					Convert.ToDateTime( readers[ readersIndex ].Values[ 0 ].XValue );
					isDateTime = true;
				}	
				catch
				{
					isDateTime = false;
				}

				for( int index = 0; index < readers[ readersIndex ].Values.Length; index++ )
				{
					if( markPeriodCounter == 0 )
					{
						SetValueByIndex( xAxisValuesList, listIndex, ( double )index * 10 + 10 );

						if( valuePeriodCounter == 0 )
						{
							if( isDateTime == true )
								SetValueByIndex( xAxisStringValuesList, listIndex, ProcessStringWithFormat( readers[ readersIndex ].Values[ index ].XValue, configurationReader.DateFormat ) );
							else
								SetValueByIndex( xAxisStringValuesList, listIndex, ProcessStringWithFormat( readers[ readersIndex ].Values[ index ].XValue, configurationReader.NumberFormat ) );
						}
						else
						{
							SetValueByIndex( xAxisStringValuesList, listIndex, "" );
						}
						listIndex++;
					}

					valuePeriodCounter = ( valuePeriodCounter + 1 ) % configurationReader.XAxis.ValueDrawPeriod;
					
					//markPeriodCounter = ( markPeriodCounter + 1 ) % configurationReader.XAxis.MarkDrawPeriod;

					// if there is only one point, then markPeriodHit = 0, and the following
					// line should not be executed (throws divide-by-zero exception)
					if (markPeriodHit != 0)
						markPeriodCounter = ( markPeriodCounter + 1 ) % markPeriodHit;
				}
			}
			foundation.GraphBody.HorizontalAxis.Values = new double[ xAxisValuesList.Count ];
			xAxisValuesList.CopyTo( foundation.GraphBody.HorizontalAxis.Values );
			foundation.GraphBody.Background.VerticalLines = new double[ xAxisValuesList.Count ];
			xAxisValuesList.CopyTo( foundation.GraphBody.Background.VerticalLines );
			foundation.GraphBody.HorizontalAxis.StringValues = new string[ xAxisStringValuesList.Count ];
			xAxisStringValuesList.CopyTo( foundation.GraphBody.HorizontalAxis.StringValues );
			double lowYValue = double.MaxValue;
			double highYValue = double.MinValue;
			if( configurationReader.YAxis.IsValueAuto() )
			{
				for( int readersIndex = 0; readersIndex < readers.Length; readersIndex++ )
				{
					foreach( ValueData value in readers[ readersIndex ].Values )
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

				if( configurationReader.YAxis.Indent > 0 )
				{
					double AverageValue = highYValue / 2;

					double IndentVal = (double)configurationReader.YAxis.Indent;
					IndentVal /= 100;
					
					IndentVal *= AverageValue;
	
					if( lowYValue != 0 )
						lowYValue = ( lowYValue ) - IndentVal;

					if( configurationReader.YAxis.IsBelowZeroAllowed == false && lowYValue < 0 )
						lowYValue = 0;

					highYValue = ( highYValue ) + IndentVal;
				}
				else
				{
					lowYValue -= belowIdent;
					highYValue += aboveIdent;
				}
			}
			else
			{
				lowYValue = configurationReader.YAxis.MinValue;
				highYValue = configurationReader.YAxis.MaxValue;
			}
			
			if( highYValue == lowYValue )
			{
				if( configurationReader.YAxis.MinValue != configurationReader.YAxis.MaxValue )
				{
					lowYValue = configurationReader.YAxis.MinValue;
					highYValue = configurationReader.YAxis.MaxValue;
				}
				else
				{
					lowYValue = -100;
					highYValue = 100;
				}
			}

			foundation.GraphBody.VerticalAxis.HighRange = highYValue;
			foundation.GraphBody.VerticalAxis.LowRange = lowYValue;
			( foundation.GraphBody.Graph as BaseLineGraph ).HighRangeY = highYValue;
			( foundation.GraphBody.Graph as BaseLineGraph ).LowRangeY = lowYValue;
			foundation.GraphBody.Background.LowRangeY = lowYValue;
			foundation.GraphBody.Background.HighRangeY = highYValue;

			
			
			if(  configurationReader.Grid.HorizontalDivisionLines >= 0 )
				yStep = ( highYValue - lowYValue ) / ( configurationReader.Grid.HorizontalDivisionLines + 1 );
			
			double yMark = lowYValue;
			ArrayList yValues = new ArrayList();
			if( yStep != 0 )
			{
				while( yMark <= highYValue )
				{
					yValues.Add( yMark );
					yMark += yStep;
				}
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
			( foundation.GraphBody.Graph as BaseLineGraph ).GraphItems = new BaseLineGraph.LineGraphItem[ readers.Length ];
			for( int index = 0; index < readers.Length; index++ )
			{
				BaseLineGraph.LineGraphPoint[] points = new BaseLineGraph.LineGraphPoint[ readers[ index ].Values.Length ];
				for( int pointIndex = 0; pointIndex < points.Length; pointIndex++ )
				{
					points[ pointIndex ] = new BaseLineGraph.LineGraphPoint( pointIndex * 10 + 10, Converter.StringToDouble( readers[ index ].Values[ pointIndex ].YValue ) );
				}
				( foundation.GraphBody.Graph as BaseLineGraph ).GraphItems[ index ] = new BaseLineGraph.LineGraphItem( points );
				( foundation.GraphBody.Graph as BaseLineGraph ).GraphItems[ index ].LineColor = configurationReader.SetItems[ index ].SetColor;
				( foundation.GraphBody.Graph as BaseLineGraph ).GraphItems[ index ].LineWidth = configurationReader.SetItems[ index ].LineWidth;
			}
			foundation.Legend.MaxLegentWidth = configurationReader.Width / 2;
			foundation.Legend.BackColor = configurationReader.Legend.BackColor;
			foundation.Legend.Visible = configurationReader.Legend.IsVisible;
			foundation.Legend.Items.Clear();
			for( int index = 0; index < readers.Length; index++ )
			{
				BaseLineLegendItem newLegendItem = new BaseLineLegendItem();
				newLegendItem.LineColor = configurationReader.SetItems[ index ].SetColor;
				newLegendItem.LineWidth = configurationReader.SetItems[ index ].LineWidth;
				newLegendItem.Text = configurationReader.SetItems[ index ].Name;
				newLegendItem.ForeColor = configurationReader.Legend.TextColor;
				newLegendItem.Font = configurationReader.Legend.ItemFont;
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
				return value.ToString( numberFormat );
			}
			catch
			{

			}

			return inputString;
		}

		protected void SetValueByIndex( ArrayList list, int index, object value )
		{
			if( list.Count - 1 < index )
			{
				list.Add( value );
			}
			else
			{
				list[ index ] = value;
			}
		}

		protected virtual void InitLineGraphParams()
		{
			belowIdent = 0;
			yStep = 200;
			aboveIdent = 10;
		}
	}
}
