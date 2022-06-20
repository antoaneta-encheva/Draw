using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
	[Serializable]
	class GroupShape:Shape
    {
		#region Constructor

		public GroupShape(RectangleF rect) : base(rect)
		{
		}

		public GroupShape(RectangleShape rectangle) : base(rectangle)
		{
		}

		#endregion

		public List<Shape> SubShapes= new List<Shape>();

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		public override bool Contains(PointF point)
		{
			
			if (SubShapes.Count > 0)
			{
				foreach (Shape item in SubShapes)
				{
					if (item.Contains(point))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				return true;
			}
		
			else
			{
				return false;
			}
		
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);
			
			foreach(Shape item in SubShapes)
            {
				item.DrawSelf(grfx);
            }
			
		}
	}
}
