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

namespace BC.Controls.StormChart.Common
{
	public class MathHelper
	{

		public static double DegreeToRadian( double degree )
		{
			return degree / 180 * Math.PI;
		}
		
		public static int Floor( double value )
		{
			return ( int )Math.Floor( value );
		}
		
		public static int Ceiling( double value )
		{
			return ( int )Math.Ceiling( value );
		}

		public static int Ceiling( float value )
		{
			return ( int )Math.Ceiling( value );
		}

		public static int Round( double value )
		{
			return ( int )Math.Round( value );
		}

		public static int Round( float value )
		{
			return ( int )Math.Round( value );
		}
	}
}
