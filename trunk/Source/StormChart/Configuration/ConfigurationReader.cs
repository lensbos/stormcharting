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
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using BC.Controls.StormChart.BaseVisualizer;
using System.Globalization;
using BC.Controls.StormChart.Common;

namespace BC.Controls.StormChart.Configuration
{
	public class ConfigurationReader : BaseXmlReader
	{
		public enum OutputFormatType
		{
			JPG,
			PNG
		}

		public enum GraphType
		{
			Bar,
			Line,
			Pie
		}

		public enum ChartBackgroundType
		{
			Image,
			Color
		}

		private OutputFormatType outputFormat;
		private GraphType graph;
		private Canvas.CanvasOrientation orientation;
		private ChartBackgroundType chartChartBackground;
		private Color chartBackgroundColor;
		private string chartBackgroundImage;
		private TitleTag title = new TitleTag();
		private Color canvasBackColor;
		private int canvasBorderThickness;
		private Color canvasBorderColor;
		private string numberFormat;
		private string dateFormat;
		private int width;
		private int height;
		private SetTag[] setItems;
		private GridTag grid = new GridTag();
		private XAxisTag xAxis = new XAxisTag();
		private YAxisTag yAxis = new YAxisTag();
		private LegendTag legend = new LegendTag();
		private ColorsPoolTag colorsPool = new ColorsPoolTag();


		public ColorsPoolTag ColorsPool
		{
			get
			{
				return colorsPool;
			}
		}

		public LegendTag Legend
		{
			get
			{
				return legend;
			}
		}

		public YAxisTag YAxis
		{
			get
			{
				return yAxis;
			}
		}
		
		public XAxisTag XAxis
		{
			get
			{
				return xAxis;
			}
		}

		public GridTag Grid
		{
			get
			{
				return grid;
			}
		}

		public SetTag[] SetItems
		{
			get
			{
				return setItems;
			}
		}

		public string NumberFormat
		{
			get
			{
				return numberFormat;
			}
		}
		public string DateFormat
		{
			get
			{
				return dateFormat;
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
		}
		public int Height
		{
			get
			{
				return height;
			}
		}

		public int CanvasBorderThickness
		{
			get
			{
				return canvasBorderThickness;
			}
		}
		
		public Color CanvasBackColor
		{
			get
			{
				return canvasBackColor;
			}
		}
		
		public Color CanvasBorderColor
		{
			get
			{
				return canvasBorderColor;
			}
		}

		public TitleTag Title
		{
			get
			{
				return title;
			}
		}
		
		public ChartBackgroundType ChartChartBackground
		{
			get
			{
				return chartChartBackground;
			}
		}
		
		public string ChartBackgroundImage
		{
			get
			{
				return chartBackgroundImage;
			}
		}
		
		public Color ChartBackgroundColor
		{
			get
			{
				return chartBackgroundColor;
			}
		}
		
		public Canvas.CanvasOrientation Orientation
		{
			get
			{
				return orientation;
			}
		}
		
		public OutputFormatType OutputFormat
		{
			get
			{
				return outputFormat;
			}
		}

		public GraphType Graph
		{
			get
			{
				return graph;
			}
		}
		
		public ConfigurationReader( string configurationXml ) : base( configurationXml )
		{
			try
			{
				ParseConfiguration();
			}
			catch( ConfigurationBrokenException exception )
			{
				throw exception;
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, string.Format( "({0})", exception.Message ) );
			}
		}
		
		private void ParseType( XmlElement element )
		{
			switch( element.InnerText.ToUpper() )
			{
				case "BAR":
					graph = GraphType.Bar;
					return;
				case "LINE":
					graph = GraphType.Line;
					return;
				case "PIE":
					graph = GraphType.Pie;
					return;
			}
//			throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( "StormChart/Configuration/Type" ) );
			throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) ) );
		}

		private string GetAdditionalExceptionMessage( string valuePath )
		{
			return GetAdditionalExceptionMessage( new string[ 1 ] { valuePath } );
		}

		private string GetAdditionalExceptionMessage( string[] valuesPath )
		{
			StringBuilder resultBuilder = new StringBuilder();
			resultBuilder.Append( "(value" );
			if( valuesPath.Length > 1 )
			{
				resultBuilder.Append( "s" );
			}
			resultBuilder.Append( " of " );
			bool firstPath = true;
			foreach( string path in valuesPath )
			{
				if( firstPath )
				{
					firstPath = false;
				}
				else
				{
					resultBuilder.Append( ", " );
				}
				resultBuilder.AppendFormat( "\"{0}\"", path );
			}
			resultBuilder.Append( " is incorrect)" );
			return resultBuilder.ToString();
		}

		private void ParseOrientation( XmlElement element )
		{
			switch( element.InnerText.ToUpper() )
			{
				case "VERTICAL":
					orientation = Canvas.CanvasOrientation.Vertical;
					return;
				case "HORIZONTAL":
					orientation = Canvas.CanvasOrientation.Horizontal;
					return;
			}
//			throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( "StormChart/Configuration/Orientation" ) );
			throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) ) );
		}

		private void ParseFormat( XmlElement element )
		{
			switch( element.InnerText.ToUpper() )
			{
				case "JPG":
					outputFormat = OutputFormatType.JPG;
					return;
				case "PNG":
					outputFormat = OutputFormatType.PNG;
					return;
			}
//			throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( "StormChart/Configuration/Format" ) );
			throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) ) );
		}

		private void ParseBackground( XmlElement element )
		{
			switch( element.GetAttribute( "type" ).ToUpper() )
			{
				case "COLOR":
					try
					{
						chartBackgroundColor = ParseColor( element.InnerText );
					}
					catch( Exception exception )
					{
						throw new ConfigurationBrokenException( exception, GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) ) );
					}
					chartChartBackground = ChartBackgroundType.Color;
					return;
				case "IMAGE":
					chartBackgroundImage = element.InnerText;
					chartChartBackground = ChartBackgroundType.Image;
					if( File.Exists( chartBackgroundImage ) == false )
					{
						throw new ConfigurationBrokenException( string.Format( "(Image file \"{0}\" from tag \"{1}\" not found)", chartBackgroundImage, GetXPathOfXmlElement( element ) ) );
					}
					return;
			}
//			throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( "StormChart/Configuration/ChartBackground/@type" ) );
			throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) + "/@type" ) );
		}

		private void ParseTitle( XmlElement element )
		{
			BindAttributeToObject( element, "BackgroundColor", title, "BackColor" );
			BindAttributeToObject( element, "Color", title, "TextColor" );
			BindAttributeToFont( element, "Font", "Size", title, "Font" );
			try
			{
				BindValueToObject( title, "Text", element.InnerText );
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) ) );
			}
		}

		private string GetXPathOfXmlElement( XmlElement element )
		{
			StringBuilder resultBuilder = new StringBuilder();
			XmlNode current = element;
			bool firstElement = true;
			while( current.NodeType == XmlNodeType.Element )
			{
				if( firstElement )
				{
					firstElement = false;
				}
				else
				{
					resultBuilder.Insert( 0, "/" );
				}
				resultBuilder.Insert( 0, current.Name );
				current = current.ParentNode;
			}
			return resultBuilder.ToString();
		}

		private Color ParseColor( string htmlColor )
		{
			try
			{
				htmlColor = htmlColor.Replace( '#',' ' );
				UInt32 alpha = 0xFF000000;
				return Color.FromArgb( ( int )(UInt32.Parse( htmlColor, NumberStyles.HexNumber ) | alpha ) );
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, "Can't parse color" );
			}
		}

		private void ParseConfiguration()
		{
			XmlElement stormChartElement = null;
			XmlElement configurationElement = null;
			try
			{
				stormChartElement = FindElement( "StormChart", xmlDocument.DocumentElement );
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, "Can't find \"StormChart\" element" );
			}
			try
			{
				configurationElement = FindElement( "Configuration", stormChartElement );
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, "Can't find \"Configuration\" element" );
			}
			foreach( XmlNode node in configurationElement.ChildNodes )
			{
				if( node is XmlElement )
				{
					XmlElement element = node as XmlElement;
					switch( element.Name.ToUpper() )
					{
						case "TYPE":
							ParseType( element );
							break;
						case "ORIENTATION":
							ParseOrientation( element );
							break;
						case "FORMAT":
							ParseFormat( element );
							break;
						case "CHARTBACKGROUND":
							ParseBackground( element );
							break;
						case "TITLE":
							ParseTitle( element );
							break;
						case "CANVAS":
							ParseCanvas( element );
							break;
						case "NUMBERFORMAT":
							ParseNumberFormat( element );
							break;
						case "DATEFORMAT":
							ParseDateFormat( element );
							break;
						case "HEIGHT":
							ParseHeight( element );
							break;
						case "WIDTH":
							ParseWidth( element );
							break;
						case "GRID":
							ParseGrid( element );
							break;
						case "XAXIS":
							ParseXAxis( element );
							break;
						case "YAXIS":
							ParseYAxis( element );
							break;
						case "LEGEND":
							ParseLegend( element );
							break;
						case "COLORSPOOL":
							ParseColorsPool( element );
							break;
					}
				}
			}
			ArrayList setsArrayList = new ArrayList();
			foreach( XmlNode node in stormChartElement )
			{
				if( node is XmlElement )
				{
					XmlElement element = node as XmlElement;
					switch( element.Name.ToUpper() )
					{
						case "SET":
							SetTag newTag = new SetTag();
							ParseSet( element, newTag );
							setsArrayList.Add( newTag );
							break;
					}
				}
			}
			setItems = new SetTag[ setsArrayList.Count ];
			setsArrayList.CopyTo( setItems );
			if( setItems.Length == 0 )
			{
				throw new ConfigurationBrokenException( "Can't find \"Set\" element" );
			}
		}

		private void ParseColorsPool( XmlElement element )
		{
			foreach( XmlNode childNode in element.ChildNodes )
			{
				if( childNode is XmlElement )
				{
					XmlElement childElement = childNode as XmlElement;
					try
					{
						if( childElement.Name.ToUpper() == "COLOR" )
						{
							colorsPool.AddColor( ParseColor( childElement.InnerText ) );
						}
					}
					catch( Exception exception )
					{
						throw new ConfigurationBrokenException( exception, GetAdditionalExceptionMessage( GetXPathOfXmlElement( childElement ) ) );
					}
				}
			}
		}

		private void ParseSet( XmlElement xmlElement, SetTag tag )
		{
			BindAttributeToObject( xmlElement, "name", tag, "Name" );
			BindAttributeToObject( xmlElement, "color", tag, "SetColor" );
			BindAttributeToObject( xmlElement, "thickness", tag, "LineWidth" );
			foreach( XmlNode node in xmlElement.ChildNodes )
			{
				if( node is XmlElement )
				{
					XmlElement element = node as XmlElement;
					switch( element.Name.ToUpper() )
					{
						case "CONTAINER":
							tag.ContainerPath = element.InnerText;
							break;
						case "XVALUE":
							tag.XValuePath = element.InnerText;
							break;
						case "YVALUE":
							tag.YValuePath = element.InnerText;
							break;
						case "VALUE":
							tag.ValuePath = element.InnerText;
							break;
						case "GROUPBY":
							tag.GroupBy = element.InnerText;
							break;
						case "FILTER":
							BindAttributeToObject( element, "Tag", tag, "FilterTagPath" );
							tag.FilterValue = element.InnerText;
							break;
					}
				}
			}
			if( tag.ValuePath.Length == 0 && tag.XValuePath.Length == 0 && tag.YValuePath.Length == 0 )
			{
				throw new ConfigurationBrokenException( string.Format( "(\"{0}\" element contains neither XValue nor YValue nor Value tags)", GetXPathOfXmlElement( xmlElement ) ) );
			}
			if( tag.ContainerPath.Length == 0 )
			{
				throw new ConfigurationBrokenException( string.Format( "(\"{0}\" element does not contain Container tag)", GetXPathOfXmlElement( xmlElement ) ) );
			}
			switch( graph )
			{
				case GraphType.Bar:
					if( tag.XValuePath.Length == 0 || tag.YValuePath.Length == 0 || tag.GroupBy.Length == 0 )
					{
						throw new ConfigurationBrokenException( string.Format( "(\"{0}\" tag of Bar graph type should contains XValue, YValue and GroupBy tags)", GetXPathOfXmlElement( xmlElement ) ) );
					}
					break;
				case GraphType.Line:
					if( tag.XValuePath.Length == 0 || tag.YValuePath.Length == 0 )
					{
						throw new ConfigurationBrokenException( string.Format( "(\"{0}\" tag of Line graph type should contains XValue and YValue tags)", GetXPathOfXmlElement( xmlElement ) ) );
					}
					break;
				case GraphType.Pie:
					if( tag.ValuePath.Length == 0 || tag.GroupBy.Length == 0 )
					{
						throw new ConfigurationBrokenException( string.Format( "(\"{0}\" tag of Pie3D graph type should contains Value and GroupBy tags)", GetXPathOfXmlElement( xmlElement ) ) );
					}
					break;
			}
		}

		private void ParseLegend( XmlElement element )
		{
			BindAttributeToObject( element, "BackgroundColor", legend, "BackColor" );
			BindAttributeToObject( element, "Color", legend, "TextColor" );
			BindAttributeToObject( element, "Visible", legend, "Visible" );
			BindAttributeToFont( element, "Font", "Size", legend, "ItemFont" );
		}

		private void ParseXAxis( XmlElement element )
		{
			BindAttributeToObject( element, "Label", xAxis, "LabelText" );
			BindAttributeToFont( element, "Font", "Size", xAxis, "Font" );
			BindAttributeToObject( element, "Color", xAxis, "AxisColor" );
			BindAttributeToObject( element, "ValueDrawPeriod", xAxis, "ValueDrawPeriod" );
			BindAttributeToObject( element, "MarkDrawPeriod", xAxis, "MarkDrawPeriod" );
			BindAttributeToObject( element, "Indent", xAxis, "Indent" );
			if( xAxis.ValueDrawPeriod <= 0 )
			{
				throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) + "/@ValueDrawPeriod" ) );
			}
			if( xAxis.MarkDrawPeriod <= 0 )
			{
				throw new ConfigurationBrokenException( GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) + "/@MarkDrawPeriod" ) );
			}
		}

		private void ParseYAxis( XmlElement element )
		{
			BindAttributeToObject( element, "Label", yAxis, "LabelText" );
			BindAttributeToFont( element, "Font", "Size", yAxis, "Font" );
			BindAttributeToObject( element, "Color", yAxis, "AxisColor" );
			BindAttributeToObject( element, "MinValue", yAxis, "MinValue" );
			BindAttributeToObject( element, "MaxValue", yAxis, "MaxValue" );
			BindAttributeToObject( element, "Indent", yAxis, "Indent" );
			BindAttributeToObject( element, "BelowZeroAllowed", yAxis, "BelowZeroAllowed" );
		}

		private void ParseGrid( XmlElement element )
		{
			BindAttributeToObject( element, "Color", grid, "GridColor" );
			BindAttributeToObject( element, "Thickness", grid, "Thickness" );
			BindAttributeToObject( element, "HorizontalDivisionLines", grid, "HorizontalDivisionLines" );
		}

		private void ParseWidth( XmlElement element )
		{
			try
			{
				width = Int32.Parse( element.InnerText );
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) ) );
			}
		}

		private void ParseHeight( XmlElement element )
		{
			try
			{
				height = Int32.Parse( element.InnerText );
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, GetAdditionalExceptionMessage( GetXPathOfXmlElement( element ) ) );
			}
		}

		private void ParseNumberFormat( XmlElement element )
		{
			try
			{
				numberFormat = element.InnerText;
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, GetXPathOfXmlElement( element ) );
			}
		}

		private void ParseDateFormat( XmlElement element )
		{
			try
			{
				dateFormat = element.InnerText;
			}
			catch( Exception exception )
			{
				throw new ConfigurationBrokenException( exception, GetXPathOfXmlElement( element ) );
			}
		}

		private void ParseCanvas( XmlElement element )
		{
			BindAttributeToObject( element, "BackgroundColor", this, "canvasBackColor" );
/*			if( element.HasAttribute( "BackgroundColor" ) )
			{
				canvasBackColor = ParseColor( element.GetAttribute( "BackgroundColor" ) );
			}*/
			BindAttributeToObject( element, "BorderThickness", this, "canvasBorderThickness" );
/*			if( element.HasAttribute( "BorderThickness" ) )
			{
				canvasBorderThickness = Int32.Parse( element.GetAttribute( "BorderThickness" ) );
			}*/
			BindAttributeToObject( element, "BorderColor", this, "canvasBorderColor" );
/*			if( element.HasAttribute( "BorderColor" ) )
			{
				canvasBorderColor = ParseColor( element.GetAttribute( "BorderColor" ) );
			}*/
		}

		private void BindAttributeToFont( XmlElement xmlElement, string fontNameAttributeName, string fontSizeAttributeName, object target, string memeberName )
		{
			if( xmlElement.HasAttribute( fontNameAttributeName ) )
			{
				float fontSize = 10;
				if( xmlElement.HasAttribute( fontSizeAttributeName ) )
				{
					try
					{
						fontSize = float.Parse( xmlElement.GetAttribute( fontSizeAttributeName ) );
					}
					catch( Exception exception )
					{
						throw new ConfigurationBrokenException( exception, GetAdditionalExceptionMessage( GetXPathOfXmlElement( xmlElement ) + "/@" + fontSizeAttributeName ) );
					}
				}
				string fontName = xmlElement.GetAttribute( fontNameAttributeName );
				Font newFont;
				try
				{
					newFont = new Font( fontName, fontSize );
				}
				catch( Exception exception )
				{
					throw new ConfigurationBrokenException( exception, GetAdditionalExceptionMessage( new string[ 2 ] { GetXPathOfXmlElement( xmlElement ) + "/@" + fontNameAttributeName, GetXPathOfXmlElement( xmlElement ) + "/@" + fontSizeAttributeName } ) );
				}
				Invoke( target, memeberName, newFont );
			}
		}
		
		private void BindAttributeToObject( XmlElement xmlElement, string attributeName, object target, string memberName )
		{
			if( xmlElement.HasAttribute( attributeName ) )
			{
				string attributeValue = xmlElement.GetAttribute( attributeName );
				try
				{
					BindValueToObject( target, memberName, attributeValue );
				}
				catch( Exception exception )
				{
					throw new ConfigurationBrokenException( exception, GetAdditionalExceptionMessage( GetXPathOfXmlElement( xmlElement ) + "/@" + attributeName ) );
				}
			}
		}

		private void BindValueToObject( object target, string memberName, string value )
		{
			Type targetType = target.GetType();
			MemberInfo[] members = targetType.FindMembers( MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.FilterName , memberName );
			foreach( MemberInfo memberInfo in members )
			{
				if( memberInfo.MemberType == MemberTypes.Field )
				{
					FieldInfo fieldInfo = memberInfo as FieldInfo;
					ProcessTypeForInvoke( target, memberName, fieldInfo.FieldType, value );
				}
				if( memberInfo.MemberType == MemberTypes.Property )
				{
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					ProcessTypeForInvoke( target, memberName, propertyInfo.PropertyType, value );
				}
			}
		}

		private void ProcessTypeForInvoke( object target, string memberName, Type memberType, string value )
		{
			switch( memberType.ToString() )
			{
				case "System.Int32":
					Invoke( target, memberName, ValueToInt32( value ) );
					break;
				case "System.String":
					Invoke( target, memberName, ValueToString( value ) );
					break;
				case "System.Drawing.Color":
					Invoke( target, memberName, ValueToColor( value ) );
					break;
				case "System.Double":
					Invoke( target, memberName, ValueToDouble( value ) );
					break;
			}
		}

		private object ValueToDouble( string value )
		{
			return Converter.StringToDouble( value );
		}	

		private object ValueToColor( string value )
		{
			return ParseColor( value );
		}

		private object ValueToString( string value )
		{
			return value;
		}

		private object ValueToInt32( string value )
		{
			return Int32.Parse( value );
		}

		private void Invoke( object target, string memberName, object value )
		{
			target.GetType().InvokeMember( memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField | BindingFlags.SetProperty, null, target, new object[ 1 ]{ value } );
		}
	}
}
