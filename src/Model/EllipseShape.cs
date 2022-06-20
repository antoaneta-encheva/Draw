using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
	[Serializable]
	class EllipseShape : Shape
	{
		#region Constructor

		public EllipseShape(RectangleF rect) : base(rect)
		{
		}

		public EllipseShape(RectangleShape rectangle) : base(rectangle)
		{
		}

		#endregion

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		public override bool Contains(PointF point)
		{
			PointF p2 = new PointF((float)(Math.Cos(45)*Location.X- Math.Sin(45) * Location.Y),(float)(Math.Sin(45) * Location.X+ Math.Cos(45) * Location.Y));
			double f = 1;

			double a = Width;
			double b = Height;

			float c = point.X - (Location.X + (Width / 2));
			float d = point.Y - (Location.Y - (-Height / 2));


			f = Math.Pow(c, 2) / Math.Pow(Width / 2, 2)
						+ Math.Pow(d, 2) / Math.Pow(Height / 2, 2);

			if (f<=1)
				// Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
				// В случая на правоъгълник - директно връщаме true
				return true;
			else
				// Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
				return false;
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			
			base.DrawSelf(grfx);
			base.RotateShape(grfx);

			grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			grfx.DrawEllipse(new Pen(StrokeColor, BorderWidth), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

			grfx.ResetTransform();
		}
	}
}
