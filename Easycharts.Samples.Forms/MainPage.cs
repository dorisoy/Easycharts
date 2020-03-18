using Xamarin.Forms;
using SkiaSharp;

namespace Easycharts.Samples.Forms
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var charts = Data.CreateXamarinSample();
			this.chart1.Chart = charts[0];
			this.chart2.Chart = charts[1];
			this.chart3.Chart = charts[2];
			this.chart4.Chart = charts[3];
			this.chart5.Chart = charts[4];
			this.chart6.Chart = charts[5];
			this.chart7.Chart = charts[6];
			this.chart8.Chart = charts[7];
			this.chart9.Chart = charts[8];
			this.chart10.Chart = charts[9];


			var fontManager = SKFontManager.Default;
			var typeface = fontManager.MatchCharacter('雪');
			//SKTypeface.FromFamilyName(fontName);

			this.chart1.Chart.Typeface = typeface;
			this.chart2.Chart.Typeface = typeface;
			this.chart3.Chart.Typeface = typeface;
			this.chart4.Chart.Typeface = typeface;
			this.chart5.Chart.Typeface = typeface;
			this.chart6.Chart.Typeface = typeface;
			this.chart7.Chart.Typeface = typeface;
			this.chart8.Chart.Typeface = typeface;
			this.chart9.Chart.Typeface = typeface;
			this.chart10.Chart.Typeface = typeface;
		}
	}
}
