
using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (pickUpSpeedButton.Checked) 
			{
				Shape temp= dialogProcessor.ContainsPoint(e.Location);
				if(temp!=null)
				{
                    if (dialogProcessor.Selection.Contains(temp))
                    {
						dialogProcessor.Selection.Remove(temp);

                    }
                    else
					{ 
					dialogProcessor.Selection.Add(temp);
					}
				}
				if (dialogProcessor.Selection.Count < 2)
				{
					toolStripButton5.Enabled = false;

				}
				else
				{
					toolStripButton5.Enabled = true;

				}
				
				if (dialogProcessor.Selection != null)
				{
					statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
					dialogProcessor.IsDragging = true;
					dialogProcessor.LastLocation = e.Location;
					
					viewPort.Invalidate();
				}
			}
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomEllipse();

			statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

			viewPort.Invalidate();

		}

        private void pickUpSpeedButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomSquare();

			statusBar.Items[0].Text = "Последно действие: Рисуване на квадрат";

			viewPort.Invalidate();
		}

		private void ScaleF()
        {
			
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
			
		}

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

		}

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				viewPort.Invalidate();
				FScale(2);
				viewPort.Invalidate();
			}
		}

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

		private void FScale(double s)
        {

			
			if (dialogProcessor.Selection.Count>0)
			{
		

				for(int i = 0; i <dialogProcessor.Selection.Count; i++)
                {
					if(i< dialogProcessor.Selection.Count - 2)
                    {
						if(dialogProcessor.Selection[i].Height== dialogProcessor.Selection[i + 1].Height)
                        {
							dialogProcessor.Scale((int)(dialogProcessor.Selection[i].Width * s),
								(int)(dialogProcessor.Selection[i].Height * s),i,i+1,i);
						}
                        else
                        {
							dialogProcessor.Scale((int)(dialogProcessor.Selection[i].Width * s),
								(int)(dialogProcessor.Selection[i].Height * s), i-1, i ,i);
						}
                    }
                    else
                    {
						dialogProcessor.Scale((int)(dialogProcessor.Selection[i].Width * s), 
							(int)(dialogProcessor.Selection[i].Height * s), i, i + 1,i);
					}

					if (dialogProcessor.Selection[i].GetType().Name == "GroupShape")
					{
						
						dialogProcessor.SelectionGs.Add(dialogProcessor.Selection[i]);
						foreach (GroupShape gs in dialogProcessor.SelectionGs)
						{
							for (int j = 0; j < gs.SubShapes.Count; j++)
							{
								
								if (j < gs.SubShapes.Count - 2)
								{
									if (gs.SubShapes[j].Height == gs.SubShapes[j + 1].Height)
									{
										dialogProcessor.Scale((int)(gs.SubShapes[j].Width * s),
											(int)(gs.SubShapes[j].Height * s), j, j + 1,i);
									}
									else
									{
										dialogProcessor.Scale((int)(gs.SubShapes[j].Width * s),
											(int)(gs.SubShapes[j].Height * s), j - 1, j,i);
									}
								}
								else
								{
									dialogProcessor.Scale((int)(gs.SubShapes[j].Width * s),
										(int)(gs.SubShapes[j].Height * s), j, j + 1,i);
								}

								
							}
                           
							
						}

						
						dialogProcessor.SelectionGs.Remove(dialogProcessor.Selection[i]);
					}

				}

			}


			
		}

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

			if (dialogProcessor.Selection != null)
			{
				viewPort.Invalidate();
				FScale(0.25);
				viewPort.Invalidate();
			}
		}

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				viewPort.Invalidate();
				FScale(0.5); 
				viewPort.Invalidate();
			}
		}

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				viewPort.Invalidate();
				FScale(0.75);
				viewPort.Invalidate();
			}
		}

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				viewPort.Invalidate();
				FScale(1);
				viewPort.Invalidate();
			}
		}

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				viewPort.Invalidate();
				FScale(1.25);
				viewPort.Invalidate();
			}
		}

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				viewPort.Invalidate();
				FScale(1.5);
				viewPort.Invalidate();
			}
		}

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				viewPort.Invalidate();
				FScale(10);
				viewPort.Invalidate();
			}
		}
		
        private void наДясноС10pxToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Преместване";

				for(int i=0;i< dialogProcessor.Selection.Count; i++)
                {
					PointF p = new PointF(dialogProcessor.Selection[i].Location.X + 50, 
						dialogProcessor.Selection[i].Location.Y);

					dialogProcessor.TranslateToPoint(p,i,i+1,i);
					dialogProcessor.Selection[i].Location = p;
					if (dialogProcessor.Selection[i].GetType().Name == "GroupShape")
					{
						dialogProcessor.SelectionGs.Add(dialogProcessor.Selection[i]);
						foreach (GroupShape gs in dialogProcessor.SelectionGs)
						{
							for (int j = 0; j < gs.SubShapes.Count; j++)
							{
								PointF p2 = new PointF(gs.SubShapes[j].Location.X + 50,
									gs.SubShapes[j].Location.Y);

								dialogProcessor.TranslateToPoint(p2, j, j + 1,i);
								gs.SubShapes[j].Location = p2;
							}
						}
						dialogProcessor.SelectionGs.Remove(dialogProcessor.Selection[i]);
					}

				}
				viewPort.Invalidate();

			}
		}

        private void наЛявоС10pxToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Преместване";
				for (int i = 0; i < dialogProcessor.Selection.Count; i++)
				{
					PointF p = new PointF(dialogProcessor.Selection[i].Location.X - 50,
						dialogProcessor.Selection[i].Location.Y);

					dialogProcessor.TranslateToPoint(p, i, i + 1,i);
					dialogProcessor.Selection[i].Location = p;
					if (dialogProcessor.Selection[i].GetType().Name == "GroupShape")
					{
						dialogProcessor.SelectionGs.Add(dialogProcessor.Selection[i]);
						foreach (GroupShape gs in dialogProcessor.SelectionGs)
						{
							for (int j = 0; j < gs.SubShapes.Count; j++)
							{
								PointF p2 = new PointF(gs.SubShapes[j].Location.X - 50,
									gs.SubShapes[j].Location.Y);

								dialogProcessor.TranslateToPoint(p2, j, j + 1, i);
								gs.SubShapes[j].Location = p2;
							}
						}
						dialogProcessor.SelectionGs.Remove(dialogProcessor.Selection[i]);
					}

				}

				viewPort.Invalidate();
			}
		}

        private void наГореС10pxToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Преместване";
				for (int i = 0; i < dialogProcessor.Selection.Count; i++)
				{
					PointF p = new PointF(dialogProcessor.Selection[i].Location.X,
						dialogProcessor.Selection[i].Location.Y-50);

					dialogProcessor.TranslateToPoint(p, i, i + 1,i);
					dialogProcessor.Selection[i].Location = p;

					if (dialogProcessor.Selection[i].GetType().Name == "GroupShape")
					{

						dialogProcessor.SelectionGs.Add(dialogProcessor.Selection[i]);
						foreach (GroupShape gs in dialogProcessor.SelectionGs)
						{
							for (int j = 0; j < gs.SubShapes.Count; j++)
							{
								PointF p2 = new PointF(gs.SubShapes[j].Location.X,
									gs.SubShapes[j].Location.Y-50);

								dialogProcessor.TranslateToPoint(p2, j, j + 1, i);
								gs.SubShapes[j].Location = p2;
							}
						}
						dialogProcessor.SelectionGs.Remove(dialogProcessor.Selection[i]);
					}
				}
				viewPort.Invalidate();
			}
		}

        private void наДолуС10pxToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Преместване";

				for (int i = 0; i < dialogProcessor.Selection.Count; i++)
				{
					PointF p = new PointF(dialogProcessor.Selection[i].Location.X, 
										dialogProcessor.Selection[i].Location.Y+50);

					dialogProcessor.TranslateToPoint(p, i, i + 1,i);
					dialogProcessor.Selection[i].Location = p;
					if (dialogProcessor.Selection[i].GetType().Name == "GroupShape")
					{
						dialogProcessor.SelectionGs.Add(dialogProcessor.Selection[i]);
						foreach (GroupShape gs in dialogProcessor.SelectionGs)
						{
							for (int j = 0; j < gs.SubShapes.Count; j++)
							{
								PointF p2 = new PointF(gs.SubShapes[j].Location.X,
									gs.SubShapes[j].Location.Y+50);

								dialogProcessor.TranslateToPoint(p2, j, j + 1, i);
								gs.SubShapes[j].Location = p2;
							}
						}
						dialogProcessor.SelectionGs.Remove(dialogProcessor.Selection[i]);
					}
				}
				viewPort.Invalidate();
			}
		}

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
			
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Смяна на цвят";
				ColorDialog cd = new ColorDialog();
				cd.ShowDialog();
				foreach(Shape item in dialogProcessor.Selection)
                {
					item.FillColor= cd.Color;
					if (item.GetType().Name == "GroupShape")
					{
						dialogProcessor.SelectionGs.Add(item);
						foreach (GroupShape gs in dialogProcessor.SelectionGs)
						{
							foreach (Shape item2 in gs.SubShapes)
							{
								item2.FillColor = cd.Color;
							}
						}
						dialogProcessor.SelectionGs.Remove(item);
					}
				}

				//dialogProcessor.Selection.FillColor = cd.Color;
				viewPort.Invalidate();
			}
		}

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Смяна на цвят на рамка";
				ColorDialog cd = new ColorDialog();
				cd.ShowDialog();
				foreach(Shape item in dialogProcessor.Selection)
                {
					item.StrokeColor = cd.Color;
					if (item.GetType().Name == "GroupShape")
					{
						dialogProcessor.SelectionGs.Add(item);
						foreach (GroupShape gs in dialogProcessor.SelectionGs)
						{
							foreach (Shape item2 in gs.SubShapes)
							{
								item2.StrokeColor = cd.Color;
							}
						}
						dialogProcessor.SelectionGs.Remove(item);
					}
				}

				//dialogProcessor.Selection.StrokeColor = cd.Color;
				viewPort.Invalidate();
			}
		}

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }

		private void Border(int b)
        {


			if (dialogProcessor.Selection.Count > 0)
			{

				//statusBar.Items[0].Text = "Последно действие: Смяна на размера на рамката";
				foreach (Shape item in dialogProcessor.Selection)
				{
					
					item.BorderWidth = b;
					if (item.GetType().Name == "GroupShape")
					{
						dialogProcessor.SelectionGs.Add(item);
						foreach (GroupShape gs in dialogProcessor.SelectionGs)
						{
							foreach (Shape item2 in gs.SubShapes)
							{
								item2.BorderWidth = b;
							}
						}
						dialogProcessor.SelectionGs.Remove(item);
					}
				}

			}

		}

		private void pxToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Смяна на размера на рамка";
				Border(2);
				viewPort.Invalidate();
			}
		}

        private void pxToolStripMenuItem1_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Смяна на размера на рамка";
				Border(4);
				viewPort.Invalidate();
			}
		}

        private void pxToolStripMenuItem2_Click(object sender, EventArgs e)
        {
			if (dialogProcessor.Selection != null)
			{
				statusBar.Items[0].Text = "Последно действие: Смяна на размера на рамка";
				Border(6);
				viewPort.Invalidate();
			}
		}

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
			statusBar.Items[0].Text = "Последно действие: Групиране";
			dialogProcessor.GroupShapes();
			toolStripButton5.Enabled = false;
			toolStripButton6.Enabled = true;
			viewPort.Invalidate();
		}

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
			statusBar.Items[0].Text = "Последно действие: Дегрупиране";
			dialogProcessor.DeGroupShapes();
			toolStripButton6.Enabled = false;
			toolStripButton5.Enabled = true;
			viewPort.Invalidate();
		}

        private void MainForm_Load(object sender, EventArgs e)
        {
			toolStripButton5.Enabled = false;
			toolStripButton6.Enabled = false;
		}

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomRectangle();

			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

			viewPort.Invalidate();
		}

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomEllipse();

			statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

			viewPort.Invalidate();
		}

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
			dialogProcessor.AddRandomSquare();

			statusBar.Items[0].Text = "Последно действие: Рисуване на квадрат";

			viewPort.Invalidate();
		}

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
			
		}

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
			dialogProcessor.RotateShape(45);
			statusBar.Items[0].Text = "Последно действие: Завъртане";
			viewPort.Invalidate();
		}

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
			dialogProcessor.RotateShape(0);
			statusBar.Items[0].Text = "Последно действие: Завъртане";
			viewPort.Invalidate();
		}

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
			dialogProcessor.RotateShape(90);
			statusBar.Items[0].Text = "Последно действие: Завъртане";
			viewPort.Invalidate();
		}

        private void selectAllShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
			dialogProcessor.SelectAll();
			statusBar.Items[0].Text = "Последно действие: Избиране на всички";
			viewPort.Invalidate();
		}

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
			dialogProcessor.DeleteSelected();
			statusBar.Items[0].Text = "Последно действие: Изтриване";
			viewPort.Invalidate();
		}

        private void copySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
			dialogProcessor.CopySelected();
			statusBar.Items[0].Text = "Последно действие: Копиране";
			viewPort.Invalidate();
		}

        private void pasteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
			dialogProcessor.PasteSelected();
			statusBar.Items[0].Text = "Последно действие: Поставяне";
			viewPort.Invalidate();
		}

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
			SaveFileDialog sv = new SaveFileDialog();
			if (sv.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dialogProcessor.SerializeFile(dialogProcessor.ShapeList, sv.FileName);
			}
			statusBar.Items[0].Text = "Последно действие: Записване на файл.";
		}

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
			OpenFileDialog od = new OpenFileDialog();
			if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				dialogProcessor.ShapeList = (List<Shape>)dialogProcessor.DeSerializeFile(od.FileName);
				viewPort.Invalidate();
			}
			statusBar.Items[0].Text = "Последно действие: Отваряне на файл.";

		}

        private void unselectAllSelectedShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
			dialogProcessor.DeSelectAll();
			statusBar.Items[0].Text = "Последно действие: Махане на селекцията";
			viewPort.Invalidate();
		}

		public Button enterRotateBtn;
		public TextBox rotateTextBox;
		public Label textRotate;
		public Form rotateForm;

		public void RotateFormMethod(object sender, EventArgs e)
		{
			rotateForm = new Form();

			rotateForm.Text = "Enter rotate degree: ";
			enterRotateBtn = new Button();
			Button cancelRotateBtn = new Button();
			rotateTextBox = new TextBox();
			textRotate = new Label();
			textRotate.Text = "Degree(1-1000): ";
			enterRotateBtn.Text = "Set Rotate Radius";
			cancelRotateBtn.Text = "Cancel";
			textRotate.Location = new Point(90, 80);
			rotateTextBox.Location = new Point(textRotate.Left, textRotate.Height + textRotate.Top + 10);
			rotateForm.Controls.Add(textRotate);
			rotateForm.Controls.Add(rotateTextBox);
			enterRotateBtn.Location = new Point(rotateTextBox.Left, rotateTextBox.Height + rotateTextBox.Top + 10);
			cancelRotateBtn.Location = new Point(enterRotateBtn.Left, enterRotateBtn.Height + enterRotateBtn.Top + 10);
			// Set the accept button of the form to button1.
			rotateForm.AcceptButton = enterRotateBtn;

			// Set the cancel button of the form to button2.
			rotateForm.CancelButton = cancelRotateBtn;
			// Add enterBtn to the form.
			rotateForm.Controls.Add(enterRotateBtn);
			enterRotateBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			// Add cancelBtn to the form.
			rotateForm.Controls.Add(cancelRotateBtn);
			rotateForm.StartPosition = FormStartPosition.CenterScreen;
			rotateForm.ShowDialog();

			enterRotateBtn_Click(sender, e);
		}

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
			RotateFormMethod(sender, e);
		}
		private void enterRotateBtn_Click(object sender, EventArgs e)
		{
			try
			{
				if (rotateTextBox.Text == "")
				{
					rotateForm.Close();
				}
				else if ((float.Parse(rotateTextBox.Text) < 1) || (float.Parse(rotateTextBox.Text) > 1000))
				{
					string message = "Enter appropriate value(1-1000)!";
					string caption = "Error Detected in Input";
					MessageBoxButtons button = MessageBoxButtons.OK;
					DialogResult result;

					// Displays the MessageBox.
					result = MessageBox.Show(message, caption, button);
					if (result == System.Windows.Forms.DialogResult.OK)
					{

					}
				}
				else
				{

					dialogProcessor.RotateShape(float.Parse(rotateTextBox.Text));
					statusBar.Items[0].Text = "Последно действие: Завъртане на фигура/фигури.";
					viewPort.Invalidate();
				}
			}
			catch
			{
				rotateForm.Close();
			}

		}
    }
}
