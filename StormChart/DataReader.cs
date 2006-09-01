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
using System.Xml.XPath;
using BC.Controls.StormChart.Configuration;

namespace BC.Controls.StormChart
{
	public class DataReader : BaseXmlReader
	{
		private ValueData[] values;

		public ValueData[] Values
		{
			get
			{
				return values;
			}
		}

		public DataReader( string xmlData, SetTag setTag) : base( xmlData )
		{
			try
			{
				ParseData( setTag );
			}
			catch( DataBrokenException exception )
			{
				throw exception;
			}
			catch( Exception exception )
			{
				throw new DataBrokenException( exception, string.Format( "({0})", exception.Message ) );
			}
		}

		private void ParseData( SetTag setTag )
		{
			XPathNavigator containerXPath = xmlDocument.DocumentElement.CreateNavigator();
			XPathNodeIterator xPathContainerIterator;
			try
			{
				 xPathContainerIterator = containerXPath.Select( ConvertPathToXPath( setTag.ContainerPath ) );
			}
			catch( Exception exception )
			{
				throw new DataBrokenException( exception, string.Format( "(Container path \"{0}\" is incorrect)", setTag.ContainerPath ) );
			}
			ArrayList valuesList = new ArrayList();
			while( xPathContainerIterator.MoveNext() )
			{
				if( setTag.FilterTagPath.Length > 0 )
				{
					bool isMatch = true;
					XPathNodeIterator xPathFilterIterator;
					try
					{
						xPathFilterIterator = xPathContainerIterator.Current.Select( ConvertPathToXPath( setTag.FilterTagPath ) );
					}
					catch( Exception exception )
					{
						throw new DataBrokenException( exception, string.Format( "(Filter path \"{0}\" is incorrect)", setTag.FilterTagPath ) );
					}
					int filtersCount = 0;
					while( xPathFilterIterator.MoveNext() )
					{
						filtersCount++;
						if( xPathFilterIterator.Current.Value != setTag.FilterValue )
						{
							isMatch = false;
							break;
						}
					}
					if( isMatch == false || filtersCount < 1 )
					{
						continue;
					}
				}
				if( setTag.ValuePath.Length == 0 && setTag.XValuePath.Length == 0 && setTag.YValuePath.Length == 0 )
				{
					throw new DataBrokenException();
				}
				ValueData newData = new ValueData();
				if( setTag.ValuePath.Length > 0 )
				{
					XPathNodeIterator xPathXValueIterator;
					try
					{
						xPathXValueIterator = xPathContainerIterator.Current.Select( ConvertPathToXPath( setTag.ValuePath ) );
					}
					catch( Exception exception )
					{
						throw new DataBrokenException( exception, string.Format( "(Value path \"{0}\" is incorrect)", setTag.ValuePath ) );
					}
					int count = 0;
					while( xPathXValueIterator.MoveNext() )
					{
						count++;
						if( count > 1 )
						{
							throw new DataBrokenException( string.Format( "(more then one Value has name \"{0}\" in the one Container element)", setTag.ValuePath ) );
						}
						newData.Value = xPathXValueIterator.Current.Value;
					}
					if( count == 0 )
					{
						throw new DataBrokenException( string.Format( "(there is no Value element has name \"{0}\" in the Container element)", setTag.ValuePath ) );
					}
				}
				if( setTag.XValuePath.Length > 0 )
				{
					XPathNodeIterator xPathXValueIterator;
					try
					{
						xPathXValueIterator = xPathContainerIterator.Current.Select( ConvertPathToXPath( setTag.XValuePath ) );
					}
					catch( Exception exception )
					{
						throw new DataBrokenException( exception, string.Format( "(XValue path \"{0}\" is incorrect)", setTag.XValuePath ) );
					}
					int count = 0;
					while( xPathXValueIterator.MoveNext() )
					{
						count++;
						if( count > 1 )
						{
							throw new DataBrokenException( string.Format( "(more then one XValue element has name \"{0}\" in the one Container element)", setTag.XValuePath ) );
						}
						newData.XValue = xPathXValueIterator.Current.Value;
					}
					if( count == 0 )
					{
						throw new DataBrokenException( string.Format( "(there is no XValue element has name \"{0}\" in the Container element)", setTag.XValuePath ) );
					}
				}
				if( setTag.YValuePath.Length > 0 )
				{
					XPathNodeIterator xPathYValueIterator;
					try
					{
						xPathYValueIterator = xPathContainerIterator.Current.Select( ConvertPathToXPath( setTag.YValuePath ) );
					}
					catch( Exception exception )
					{
						throw new DataBrokenException( exception, string.Format( "(YValue path \"{0}\" is incorrect)", setTag.YValuePath ) );
					}
					int count = 0;
					while( xPathYValueIterator.MoveNext() )
					{
						count++;
						if( count > 1 )
						{
							throw new DataBrokenException( string.Format( "(more then one YValue element has name \"{0}\" in the one Container element)", setTag.YValuePath ) );
						}
						newData.YValue = xPathYValueIterator.Current.Value;
					}
					if( count == 0 )
					{
						throw new DataBrokenException( string.Format( "(there is no YValue element has name \"{0}\" in the Container element)", setTag.YValuePath ) );
					}
				}
				if( setTag.GroupBy.Length > 0 )
				{
					XPathNodeIterator xPathGroupDataIterator;
					try
					{
						xPathGroupDataIterator = xPathContainerIterator.Current.Select( ConvertPathToXPath( setTag.GroupBy ) );
					}
					catch( Exception exception )
					{
						throw new DataBrokenException( exception, string.Format( "(GroupBy path \"{0}\" is incorrect)", setTag.GroupBy ) );
					}
					int count = 0;
					while( xPathGroupDataIterator.MoveNext() )
					{
						count++;
						if( count > 1 )
						{
							throw new DataBrokenException( string.Format( "(more then one Group element has name \"{0}\" in the one Container element)", setTag.GroupBy ) );
						}
						newData.GroupData = xPathGroupDataIterator.Current.Value;
					}
					if( count == 0 )
					{
						throw new DataBrokenException( string.Format( "(there is no Group element has name \"{0}\" in the Container element)", setTag.GroupBy ) );
					}
				}
				valuesList.Add( newData	);
			}
			if( valuesList.Count == 0 )
			{
				throw new DataBrokenException( string.Format( "(there are no Container elements have name \"{0}\" in the Data file)", setTag.ContainerPath ) );	
			}
			values = new ValueData[ valuesList.Count ];
			valuesList.CopyTo( values );
		}

		private string ConvertPathToXPath( string path )
		{
			string result = path;
            result = result.Replace( @"\", @"/" );
			result = result.Replace( @"//", @"/" );
			if( result.StartsWith( @"/" ) )
			{
				result = result.Remove( 0, 1 );
			}
			return result;
		}
	}
}
