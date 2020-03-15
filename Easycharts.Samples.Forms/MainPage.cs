using Xamarin.Forms;

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
		}
	}
}
