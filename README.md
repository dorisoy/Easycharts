# Easycharts
Easycharts is a very easy chart component based on skiasarp library, which can be used in various platforms.

# Screenshots

![gallery](Easycharts/blob/master/Screenshots/1.png)
![gallery](Easycharts/blob/master/Screenshots/2.png)


## Quickstart

### 1. Data entries

```csharp
	var entries = new[]
			{
				new Entry(212254.45f)
				{
					Label = "雪花勇闯天涯",
					ValueLabel = "212,254.45",
					Color = SKColor.Parse("#2c3e50")
				},
				new Entry(248254.45f)
				{
					Label = "青岛九度",
					ValueLabel = "248,254.45",
					Color = SKColor.Parse("#77d065")
				},
				new Entry(128254.45f)
				{
					Label = "雪花匠心营造",
					ValueLabel = "128,254.45",
					Color = SKColor.Parse("#b455b6")
				},
				new Entry(514254.45f)
				{
					Label = "雪花脸谱",
					ValueLabel = "514,254.45",
					Color = SKColor.Parse("#3498db")
				},new Entry(212254.45f)
				{
					Label = "雪花马尔斯绿",
					ValueLabel = "212,254.45",
					Color = SKColor.Parse("#2c3e50")
				},
				new Entry(222254.45f)
				{
					Label = "青岛纯生啤酒",
					ValueLabel = "222,254.45",
					Color = SKColor.Parse("#77d065")
				},
				new Entry(678254.45f)
				{
					Label = "喜力啤酒",
					ValueLabel = "678,254.45",
					Color = SKColor.Parse("#b455b6")
				},
				new Entry(934254.45f)
				{
					Label = "金威",
					ValueLabel = "934,254.45",
					Color = SKColor.Parse("#3498db")
				}
			};
```

### 2. Page

```csharp
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
  ```
  
  ### 3. XAML
  
  ```csharp
    <StackLayout Spacing="10"
                     BackgroundColor="#EEEEEE">
            <easycharts:ChartView x:Name="chart1"
                                   HeightRequest="140"
                                   BackgroundColor="White" />
            <easycharts:ChartView x:Name="chart2"
                                   HeightRequest="140"
                                   BackgroundColor="White" />
            <easycharts:ChartView x:Name="chart3"
                                   HeightRequest="140"
                                   BackgroundColor="White" />
            <easycharts:ChartView x:Name="chart4"
                                   HeightRequest="140"
                                   BackgroundColor="White" />
            <easycharts:ChartView x:Name="chart5"
                                   HeightRequest="140"
                                   BackgroundColor="White" />
            <easycharts:ChartView x:Name="chart6"
                                   HeightRequest="140"
                                   BackgroundColor="White" />
            <easycharts:ChartView x:Name="chart7"
                                   HeightRequest="140"
                                   BackgroundColor="White" />
        </StackLayout>
  ```
