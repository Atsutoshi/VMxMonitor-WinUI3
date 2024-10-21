using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace VMxMonitor.controls
{
	public sealed partial class WaveMonitor : UserControl
	{
		/* =====[ �萔 ]===== */

		private const double LINE_THICKNESS = 2.0;

		/* =====[ �v���p�e�B ]===== */

		public string Label
		{
			set
			{
				wText.Text = value;
			}
		}
		//
		private WaveCurve curve;

		/* =====[ �R���X�g���N�^ ]===== */

		public WaveMonitor()
		{
			this.InitializeComponent();

			// �v���p�e�B��������
			curve = null;
		}

		/* =====[ �p�u���b�N���\�b�h ]===== */

		public void Configure( int argMaxX, int argMinY, int argMaxY )
		{
			// �L�����o�X���N���A
			wCanvas.Children.Clear();

			// �J�[�u���쐬
			curve = new WaveCurve( argMaxX, argMinY, argMaxY )
			{
				Width = this.ActualWidth,
				Height = this.ActualHeight
			};
		}

		public void AddWave( int value )
		{
			// �g��ǉ�
			curve.AddWave( value );
		}

		public void Refresh()
		{
			// �ĕ`��
			wCanvas.Children.Clear();
			wCanvas.Children.Add( curve.GetPath() );
		}

		/* =====[ WaveCurve�N���X ]===== */

		private class WaveCurve
		{
			/* -----( �v���p�e�B )----- */

			public double Width { get; set; }
			public double Height { get; set; }
			public double MinY { get; set; }
			public double MaxY { get; set; }

			private int[] data;
			private int count;
			private int maxX;
			private Path path;
			private PathGeometry pathGeometry;
			private PathFigure pathFigure;

			/* -----( �R���X�g���N�^ )----- */

			public WaveCurve( int argMaxX, int argMinY, int argMaxY )
			{
				// �v���p�e�B��������
				data = new int[argMaxX];
				count = 0;
				maxX = argMaxX;
				MinY = argMinY;
				MaxY = argMaxY;
				path = new Path
				{
					Stroke = new SolidColorBrush( Microsoft.UI.Colors.Green ),
					StrokeThickness = LINE_THICKNESS
				};
				pathGeometry = new PathGeometry();
				pathFigure = new PathFigure();
				pathGeometry.Figures.Add( pathFigure );
				path.Data = pathGeometry;
			}

			/* ----- ( �p�u���b�N���\�b�h )----- */

			public void AddWave( int wave )
			{
				data[count % maxX] = wave;
				++count;
			}

			public Path GetPath()
			{
				pathFigure.Segments.Clear();
				if( maxX == 0 || MaxY == MinY )
				{
					return path;
				}

				// �g�̃J�[�u���쐬
				double xStep = Width / maxX;
				int offset = ( count / maxX ) * maxX;
				for( int i = offset; i < count - 1; ++i )
				{
					double x1 = i - offset;
					double x2 = ( i + 1 ) - offset;
					double y1 = data[i - offset];
					double y2 = data[( i + 1 ) - offset];
					Point p1 = new ( xStep * x1, Height - ( y1 - MinY ) / ( MaxY - MinY ) * Height );
					Point p2 = new ( xStep * x2, Height - ( y2 - MinY ) / ( MaxY - MinY ) * Height );
					if( i == offset )
					{
						pathFigure.StartPoint = p1;
					}
					pathFigure.Segments.Add( new LineSegment() { Point = p2 } );
				}

				return path;
			}
		}
	}
}
