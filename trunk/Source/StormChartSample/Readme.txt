To setup the Sample Application:

1) Setup write permissions on the ImageCache folder.
   - Right Click on ImageCache Folder
   - Properties->Security
   - Find the NETWORK SERVICE account (or whatever account you have setup 
     to run ASP in IIS. Give this account modify permissions to this folder.

2) Setup the IIS site (if you wish to use this project in VSS)
   
   - Right Click on the StormChartSample
   - Properties->Web Sharing
   - Share this Folder
   - Enable Read/Write/Directory Browsing
   - Leave the Alias as StormChartSample, click OK


3) Setup the IIS Site (if you do not need to use this in VSS)

   - Simply open IIS Manager, and create a Virtual Directory pointing at 
     the StormChartSample Folder


Browse to:

http://localhost/StormChartSample/ChartSample.aspx
