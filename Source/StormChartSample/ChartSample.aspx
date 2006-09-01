<%@ Register TagPrefix="BC" Namespace="BC.Controls.StormChart" Assembly="BC.Controls.StormChart" %>
<%@ Page language="c#" Codebehind="ChartSample.aspx.cs" AutoEventWireup="false" Inherits="StormChartSample.ChartSample" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WebForm1</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<BC:WebStormChart id="wccSampleChart" runat="server" useFile="true" TempDirectory="ImageCache"></BC:WebStormChart>
		</form>
	</body>
</HTML>
