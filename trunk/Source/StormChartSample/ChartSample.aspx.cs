using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace StormChartSample
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class ChartSample : System.Web.UI.Page
	{
		protected BC.Controls.StormChart.WebStormChart wccSampleChart;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// pie configuration
			//wccSampleChart.ConfigurationFile = "http://localhost/StormChartSample/Config/sample_pie_configuration.xml";
			//wccSampleChart.DataFile = "http://localhost/StormChartSample/Data/sample_pie_data.xml";

			// line config
			//wccSampleChart.ConfigurationFile = "http://localhost/StormChartSample/Config/sample_line_configuration.xml";
			//wccSampleChart.DataFile = "http://localhost/StormChartSample/Data/sample_line_data.xml";

			// bar config
			wccSampleChart.ConfigurationFile = "http://localhost/StormChartSample/Config/sample_bar_configuration.xml";
			wccSampleChart.DataFile = "http://localhost/StormChartSample/Data/sample_bar_data.xml";

			wccSampleChart.DataBind();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
