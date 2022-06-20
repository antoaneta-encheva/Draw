using Draw.src.Model;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection= new List<Shape>();
		public List<Shape> Selection {
			get { return selection; }
			set { selection = value; }
		}
		
		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		private List<Shape> selectionGs = new List<Shape>();
		public List<Shape> SelectionGs
		{
			get { return selectionGs; }
			set { selectionGs = value; }
		}
		private List<Shape> selectionGsO = new List<Shape>();
		public List<Shape> SelectionGsO
		{
			get { return selectionGsO; }
			set { selectionGsO = value; }
		}
		private List<Shape> selectionC = new List<Shape>();
		public List<Shape> SelectionC
		{
			get { return selectionC; }
			set { selectionC = value; }
		}
		#endregion

		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.White;
			rect.StrokeColor = Color.Black;
			rect.BorderWidth = 2;

			ShapeList.Add(rect);
		}

		public void AddRandomEllipse()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
			//ellipse.FillColor = Color.FromArgb(100, Color.Red);
			ellipse.FillColor = Color.White;
			ellipse.StrokeColor = Color.Red;
			ellipse.BorderWidth = 2;

			ShapeList.Add(ellipse);
		}

		public void AddRandomSquare()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			SquareShape square = new SquareShape(new Rectangle(x, y, 100, 100));

			square.FillColor = Color.White;
			square.StrokeColor = Color.Black;
			square.BorderWidth = 2;
			ShapeList.Add(square);
		}


		public void RotateShape(float rotateAngle)
		{
			if (Selection.Count > 0)
			{
				foreach (Shape item in Selection)
				{
					if (item.GetType().Name == "GroupShape")
					{
						selectionGs.Add(item);
						foreach (GroupShape gs in SelectionGs)
						{
							foreach (Shape item2 in gs.SubShapes)
							{
								item2.ShapeAngle = rotateAngle;
								
							}
						}
						selectionGs.Remove(item);
					}
					else
					{
						item.ShapeAngle = rotateAngle;
						
					}
					
				}
				
			}
		}
		
		public void DeleteSelected()
        {
			foreach(Shape item in selection)
            {
				selectionGsO.Add(item);
            }
			foreach(Shape item in selectionGsO)
            {
				selection.Remove(item);
				ShapeList.Remove(item);
            }
			selectionGsO.Clear();
        }
		public void CopySelected()
		{
			selectionC.Clear();
			foreach (Shape item in selection)
			{
				selectionC.Add(item);
			}
			
		}
		public void PasteSelected()
		{
			
			foreach (Shape item in selectionC)
			{
                if (item.GetType().Name == "EllipseShape")
                {
					AddRandomEllipse();
                }
				if (item.GetType().Name == "SquareShape")
				{
					AddRandomSquare();
				}
				if (item.GetType().Name == "RectangleShape")
				{
					AddRandomRectangle();
				}
				if (item.GetType().Name == "GroupShape")
				{
					selectionGs.Add(item);
					foreach(Shape item2 in selectionGs)
                    {
						if (item2.GetType().Name == "EllipseShape")
						{
							AddRandomEllipse();
						}
						if (item2.GetType().Name == "SquareShape")
						{
							AddRandomSquare();
						}
						if (item2.GetType().Name == "RectangleShape")
						{
							AddRandomRectangle();
						}
					}
					selectionGs.Remove(item);
				}
                
			}

		}

		//Serialization method
		public void SerializeFile(object currentObject, string path = null)
		{

			Stream stream;
			IFormatter binaryFormatter = new BinaryFormatter();
			if (path == null)
			{
				stream = new FileStream("DrawFile.asd", FileMode.Create, FileAccess.Write, FileShare.None);
			}
			else
			{
				string preparePath = path + ".asd";
				stream = new FileStream(preparePath, FileMode.Create);

			}
			binaryFormatter.Serialize(stream, currentObject);
			stream.Close();
		}


		//Deserialization method
		public object DeSerializeFile(string path = null)
		{
			object currentObject;

			Stream stream;
			IFormatter binaryFormatter = new BinaryFormatter();
			if (path == null)
			{
				stream = new FileStream("DrawFile.asd", FileMode.Open);

			}
			else
			{
				stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
			}
			currentObject = binaryFormatter.Deserialize(stream);
			stream.Close();
			return currentObject;
		}
		public void GroupShapes()
        {
			
			float minx = float.PositiveInfinity;
			float maxx = float.NegativeInfinity;
			float miny = float.PositiveInfinity;
			float maxy = float.NegativeInfinity;
				
			foreach(Shape item in Selection)
            {
                if (minx > item.Location.X)
                {
					minx = item.Location.X;
                }
				if(maxx<item.Location.X+item.Width)
                {
					maxx = item.Location.X + item.Width;
                }
				if (miny > item.Location.Y)
				{
					miny = item.Location.Y;
				}
				if (maxy < item.Location.Y + item.Height)
				{
					maxy = item.Location.Y + item.Height;
				}
			}
			float width = maxx - minx;
			float height = maxy - miny;
			RectangleF s = new RectangleF(minx, miny, width, height);
			GroupShape gs = new GroupShape(s);
            foreach (Shape item in Selection)
            {
				gs.SubShapes.Add(item);
				ShapeList.Remove(item);
				
            }
            foreach (Shape item in gs.SubShapes)
            {
				Selection.Remove(item);
				item.FillColor = Color.White;
				item.StrokeColor = Color.Black;
				item.BorderWidth = 2;
			
			}
			ShapeList.Add(gs);
			Selection.Add(gs);
		}

		public void DeGroupShapes()
		{
			foreach (Shape item in Selection)
			{
				selectionGsO.Add(item);
			}
			foreach (Shape item in selectionGsO)
            {
                if (item.GetType().Name == "GroupShape")
                {
					selectionGs.Add(item);
					foreach (GroupShape gs in SelectionGs)
					{
						foreach (Shape item2 in gs.SubShapes)
						{
							ShapeList.Add(item2);
							selection.Add(item2);
						}
						ShapeList.Remove(gs);
						selection.Remove(gs);
					}
					selectionGs.Remove(item);

				}
			}
			selectionGsO.Clear();
			
			
		}

		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
				
						
					return ShapeList[i];
				}	
			}
			return null;
		}
		
		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{
			if (selection.Count>0)
			{
				foreach(Shape item in Selection)
                {
                    Console.WriteLine(item.GetType().ToString());
					item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);
                    if (item.GetType().Name == "GroupShape")
                    {
						selectionGs.Add(item);
						foreach (GroupShape gs in SelectionGs)
						{
							foreach (Shape item2 in gs.SubShapes)
							{
								item2.Location = new PointF(item2.Location.X + p.X - lastLocation.X, item2.Location.Y + p.Y - lastLocation.Y);

							}
						}
						selectionGs.Remove(item);
					}
				}
				
				lastLocation = p;
			}
		}

		public void TranslateToPoint(PointF p,int n,int j,int g)
		{
			

			if (selection.Count > 0)
			{
				if(selection[g].GetType().Name != "GroupShape")
                {
					for (int i = n; i < j; i++)
					{
						selection[i].Location = new PointF(selection[i].Location.X + p.X - lastLocation.X, selection[i].Location.Y + p.Y - lastLocation.Y);

					}
				}
				else
                {
					foreach(GroupShape gs in selectionGs)
                    {
						for (int i = n; i < j; i++)
						{
							gs.SubShapes[i].Location = new PointF(gs.SubShapes[i].Location.X + p.X - lastLocation.X,
								gs.SubShapes[i].Location.Y + p.Y - lastLocation.Y);

						}
					}
					
				}					
				
				lastLocation = p;
			}
		}

		public override void DrawShape(Graphics grfx, Shape item)
        {
            base.DrawShape(grfx, item);

            if (selection.Contains(item))
            {
				grfx.DrawRectangle(new Pen(Color.Gray),
					item.Location.X-3,
					item.Location.Y-3,
					item.Width+6,
					item.Height+6);
				
			}

		}
		public void SelectAll()
		{
			foreach (Shape item in ShapeList)
			{
				if (!selection.Contains(item))
				{
					selection.Add(item);
				}
			}
		}
		public void DeSelectAll()
		{
			foreach (Shape item in ShapeList)
			{
				if (selection.Contains(item))
				{
					selection.Remove(item);
				}
			}
		}
		public void Scale(int w,int h,int n,int j,int g)
		{
			int m = j;
			for (int i=n ; i < j; i++)
			{
				if(selection[g].GetType().Name!="GroupShape")
                {
					selection[i].Width = w;
					selection[i].Height = h;
                }
                else
                {
                   
                    
						selectionGsO.Add(selection[g]);
						foreach (GroupShape gs in SelectionGs)
						{
							for (int p = n; p < m; p++)
							{
								gs.SubShapes[p].Width = w;
								gs.SubShapes[p].Height = h;
							
							}
						}
						selectionGsO.Remove(selection[g]);
					
					
					
				}
				
			}
			
		}

	}
}
