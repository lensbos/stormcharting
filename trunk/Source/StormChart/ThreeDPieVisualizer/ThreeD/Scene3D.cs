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

namespace BC.Controls.StormChart.ThreeDPieVisualizer.ThreeD
{
	public class Scene3D : Base3DObject
	{
		PlaneCollection planes;

		public PlaneCollection Planes
		{
			get
			{
				return planes;
			}
		}

		public Scene3D()
		{
			planes = new PlaneCollection();
		}

		public override void RotateAroundX( double angle )
		{
			foreach (Plane plane in planes)
			{
				plane.RotateAroundX( angle );
			}
		}

		public override void RotateAroundY( double angle )
		{
			foreach (Plane plane in planes)
			{
				plane.RotateAroundY( angle );
			}
		}

		public override void RotateAroundZ( double angle )
		{
			foreach (Plane plane in planes)
			{
				plane.RotateAroundZ( angle );
			}
		}

		public override void Move( Point3D moveVector )
		{
			foreach (Plane plane in planes)
			{
				plane.Move( moveVector );
			}
		}
		
		public class PlaneCollection : IList
		{
			private ArrayList list;

			public PlaneCollection()
			{
				list = new ArrayList();
			}
			
			public Plane this[ int index ]
			{
				get
				{
					return list[ index ] as Plane;
				}
				set
				{
					list[ index ] = value;
				}
			}

			public void Insert( int index, Plane value )
			{
				list.Insert( index, value );
			}

			public void Remove( Plane item )
			{
				list.Remove( item );
			}

			public bool Contains( Plane item )
			{
				return list.Contains( item );
			}

			public int IndexOf( Plane value )
			{
				return list.IndexOf( value );
			}

			public int Add( Plane value )
			{
				return list.Add( value );
			}

			public void AddRange( Plane[] values )
			{
				foreach( Plane plane in values )
				{
					Add( plane );
				}
			}
			
			#region IList Members

			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
			
			object IList.this[int index]
			{
				get
				{
					return list[ index ];
				}
				set
				{
					list[ index ] = value;
				}
			}

            public void RemoveAt(int index)
			{
				list.RemoveAt( index );
			}

			void IList.Insert(int index, object value)
			{
				list.Insert( index, value );
			}

			void IList.Remove(object value)
			{
				list.Remove( value );
			}

			bool IList.Contains(object value)
			{
				return list.Contains( value );
			}

			public void Clear()
			{
				list.Clear();
			}

			int IList.IndexOf(object value)
			{
				return list.IndexOf( value );
			}

			int IList.Add(object value)
			{
				return list.Add( value );
			}

			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			#endregion

			#region ICollection Members

			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			public int Count
			{
				get
				{
					return list.Count;
				}
			}

			public void CopyTo(Array array, int index)
			{
				list.CopyTo( array, index );
			}

			public object SyncRoot
			{
				get
				{
					return this;
				}
			}

			#endregion

			#region IEnumerable Members

			public IEnumerator GetEnumerator()
			{
				return list.GetEnumerator();
			}

			#endregion
		}
	}
}
