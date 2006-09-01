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
using System.Xml;

namespace BC.Controls.StormChart
{
	public class BaseXmlReader
	{
		protected XmlDocument xmlDocument;

		protected BaseXmlReader( string xmlData )
		{
			xmlDocument = new XmlDocument();
			xmlDocument.LoadXml( xmlData );
		}

		protected XmlElement FindElement( string elementName, XmlElement parentElement )
		{
			foreach( XmlNode node in parentElement.ChildNodes )
			{
				if( node is XmlElement )
				{
					XmlElement element = node as XmlElement;
					if( element.Name == elementName )
					{
						return element;
					}
				}
			}
			throw new Exception( "Element not found" );
		}
	}
}
