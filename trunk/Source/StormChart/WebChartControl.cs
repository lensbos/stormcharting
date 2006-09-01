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
using System.Drawing.Imaging;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Xml;
using BC.Controls.StormChart;
using BC.Controls.StormChart.Configuration;
using System.IO;

namespace BC.Controls.StormChart
{
	/// <summary>
	/// Summary description for WebCustomControl1.
	/// </summary>
	[ToolboxData("<{0}:WebStormChart runat=server></{0}:WebStormChart>")]
	public class WebStormChart : System.Web.UI.WebControls.WebControl
	{
		

		private string configurationFile = "";
		private string dataFile = "";
		private bool useFile = true;
		private string tempDirectory = "";
		private string chartAspxPage = "";

		public WebStormChart( )
		{
		}

		public override void Dispose()
		{
			base.Dispose();
		
		}

		[Bindable(true),
		Category("Appearance"),
		DefaultValue("Chart.aspx")]
		public string ChartAspxPage
		{
			get
			{
				return chartAspxPage;
			}
			set
			{
				chartAspxPage = value;
			}
		}

		[Bindable(true),
		Category("Appearance"),
		DefaultValue("")]
		public string TempDirectory
		{
			get
			{
				return tempDirectory;
			}
			set
			{
				tempDirectory = value;
			}
		}

		[Bindable(true),
			Category("Appearance"),
			DefaultValue("")]
		public string ConfigurationFile
		{
			get
			{
				return configurationFile;
			}
			set
			{
				configurationFile = value;
			}
		}

		[Bindable(true),
		Category("Appearance"),
		DefaultValue(true)]
		public bool UseFile
		{
			get
			{
				return useFile;
			}
			set
			{
				useFile = value;
			}
		}

		[Bindable(true),
		Category("Appearance"),
		DefaultValue("")]
		public string DataFile
		{
			get
			{
				return dataFile;
			}
			set
			{
				dataFile = value;
			}
		}

		/// <summary>
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			try
			{
				string configurationString;
				try
				{
					configurationString = ReadXml( configurationFile );
				}
				catch( Exception exception )
				{
					throw new ConfigurationBrokenException( exception, string.Format( "({0})", exception.Message ) );
				}
				string dataString;
				try
				{
					dataString = ReadXml( dataFile );
				}
				catch( Exception exception )
				{
					throw new DataBrokenException( exception, string.Format( "({0})", exception.Message ) );
				}
				if( useFile )
				{
					GraphManager graphManager = new GraphManager( configurationString );
					string outputFileName = Path.GetTempFileName(); //GetTempImageName();
					outputFileName = Path.GetFileNameWithoutExtension( outputFileName );
					Bitmap image = graphManager.GenerateImage( dataString );
					switch( graphManager.ConfigurationReader.OutputFormat )
					{
						case ConfigurationReader.OutputFormatType.JPG:
							outputFileName += ".jpg";
							image.Save( GetPhysicalTempDirectory() + outputFileName, ImageFormat.Jpeg );
							break;
						case ConfigurationReader.OutputFormatType.PNG:
							outputFileName += ".png";
							image.Save( GetPhysicalTempDirectory() + outputFileName, ImageFormat.Png );
							break;
					}
					output.Write( "<img src=\"{0}{1}\">", GetVirtualTempDirectory(), outputFileName );
				}
				else
				{
					Guid sessionGuid = Guid.NewGuid();
					Page.Session.Add( "dotnetStormChart_config_" + sessionGuid.ToString(), configurationString );
					Page.Session.Add( "dotnetStormChart_data_" + sessionGuid.ToString(), dataString );
					output.Write( "<img src=\"{0}?sessionGuid={1}\">", chartAspxPage, sessionGuid.ToString() );
				}
			}
			catch( Exception exception )
			{
				output.Write( exception.Message );
				Context.Trace.Write("WebChartControl", "Exception", exception);
			}
		}

		private string ReadXml( string fileName )
		{
			XmlTextReader reader = new XmlTextReader( fileName );
			try
			{
				StringBuilder builder = new StringBuilder();
				while( reader.Read() )
				{
					builder.Append( reader.ReadOuterXml() );
				}
				return builder.ToString();			
			}
			finally
			{
				reader.Close();
			}
		}

		private string GetTempImageName()
		{
			return "temp" + this.ID;
		}
		
		private string GetVirtualTempDirectory()
		{
			string tempPath = "";
			if( tempDirectory.Length > 0 )
			{
				tempPath = tempDirectory;
				if( tempPath.EndsWith( @"\" ) == false )
				{
					tempPath += @"\";
				}
			}
			return tempPath;
		}

		private string GetPhysicalTempDirectory()
		{
			string tempPath = Page.Request.PhysicalApplicationPath;
			if( tempPath.EndsWith( @"\" ) == false )
			{
				tempPath += @"\";
			}
			if( tempDirectory.Length > 0 )
			{
				tempPath += tempDirectory;
				if( tempPath.EndsWith( @"\" ) == false )
				{
					tempPath += @"\";
				}
			}
			return tempPath;
		}
	}
}
