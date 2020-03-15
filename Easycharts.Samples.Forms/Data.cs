namespace Easycharts.Samples.Forms
{
	using SkiaSharp;
	using System.Linq;

	public static class Data
	{
		#region Colors

		public static readonly SKColor TextColor = SKColors.Gray;

		public static readonly SKColor[] Colors =
		{
			SKColor.Parse("#266489"),
			SKColor.Parse("#68B9C0"),
			SKColor.Parse("#90D585"),
			SKColor.Parse("#F3C151"),
			SKColor.Parse("#F37F64"),
			SKColor.Parse("#424856"),
			SKColor.Parse("#8F97A4"),
			SKColor.Parse("#DAC096"),
			SKColor.Parse("#76846E"),
			SKColor.Parse("#DABFAF"),
			SKColor.Parse("#A65B69"),
			SKColor.Parse("#97A69D"),
		};

		private static int ColorIndex = 0;

		public static SKColor NextColor()
		{
			var result = Colors[ColorIndex];
			ColorIndex = (ColorIndex + 1) % Colors.Length;
			return result;
		}

		#endregion

		public static (string label, int value)[] PositiveData =
		{
			("January",     400),
			("February",    600),
			("March",       900),
			("April",       100),
			("May",         200),
			("June",        500),
			("July",        300),
			("August",      200),
			("September",   200),
			("October",     800),
			("November",    950),
			("December",    700),
		};

		public static (string label, int value)[] MixedData =
		{
			("January",     -400),
			("February",    600),
			("March",       900),
			("April",       100),
			("May",         -200),
			("June",        500),
			("July",        300),
			("August",      -200),
			("September",   200),
			("October",     800),
			("November",    950),
			("December",    -700),
		};

		public static (string label, int value)[] NegativeData =
		{
			("January",     -400),
			("February",    -600),
			("March",       -900),
			("April",       -100),
			("May",         -200),
			("June",        -500),
			("July",        -300),
			("August",      -200),
			("September",   -200),
			("October",     -800),
			("November",    -950),
			("December",    -700),
		};

		public static Chart[] CreateXamarinSample()
		{
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


			var entries2 = new[]
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
				}
			};

			return new Chart[]
			{
				new BarChart() { Entries = entries },
				new HorizontalBarChart() { Entries = entries2 },
				new PointChart() { Entries = entries },
				new LineChart()
				{
					Entries = entries,
					LineMode = LineMode.Straight,
					LineSize = 4,
					PointMode = PointMode.Circle,
					PointSize = 18,
				},
				new DonutChart() { Entries = entries },
				new RadialGaugeChart() { Entries = entries },
				new RadarChart() { Entries = entries },
			};
		}

		public static Chart[] CreateQuickstart()
		{
			var entries = new[]
			{
				new Entry(200)
				{
					Label = "January",
					ValueLabel = "200",
					Color = SKColor.Parse("#266489"),
				},
				new Entry(400)
				{
					Label = "February",
					ValueLabel = "400",
					Color = SKColor.Parse("#68B9C0"),
				},
				new Entry(-100)
				{
					Label = "March",
					ValueLabel = "-100",
					Color = SKColor.Parse("#90D585"),
				},
			};

			return new Chart[]
			{
				new BarChart() { Entries = entries },
				new PointChart() { Entries = entries },
				new LineChart() { Entries = entries },
				new DonutChart() { Entries = entries },
				new RadialGaugeChart() { Entries = entries },
				new RadarChart() { Entries = entries },
			};
		}

		public static Entry[] CreateEntries(int values, bool hasPositiveValues, bool hasNegativeValues, bool hasLabels, bool hasValueLabel, bool isSingleColor)
		{
			ColorIndex = 0;

			(string label, int value)[] data;

			if (hasPositiveValues && hasNegativeValues)
			{
				data = MixedData;
			}
			else if (hasPositiveValues)
			{
				data = PositiveData;
			}
			else if (hasNegativeValues)
			{
				data = NegativeData;
			}
			else
			{
				data = new (string label, int value)[0];
			}

			data = data.Take(values).ToArray();

			return data.Select(d => new Entry(d.value)
			{
				Label = hasLabels ? d.label : null,
				ValueLabel = hasValueLabel ? d.value.ToString() : null,
				TextColor = TextColor,
				Color = isSingleColor ? Colors[2] : NextColor(),
			}).ToArray();
		}
	}
}
