//using System;
//using System.Globalization;
//using System.Windows.Data;

//namespace DevCandidateProject.UI.Markup.ValueConverters
//{
//	public class ResponsiveItemWidthConfiguration
//	{
//		public double MinimumWColumnWidth { get; set; }

//		public double MaximumColumnWidth { get; set; }
//	}


//	public class ResponsiveItemWidthConverter 
//		: IValueConverter
//	{
//		// 900px
//		// min: 200px 
//		// max: 300px
//		/*
//		 maximumColumnCount = 4.5
//		 minimumColumnCount = 3

		 
//		 */
//		/// <inheritdoc />
//		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//		{
//			var configuration = parameter as ResponsiveItemWidthConfiguration;
//			if (configuration == null)
//				throw new NotSupportedException();

//			var hostWidth = System.Convert.ToDouble(value);

//			var maximumColumnCount = hostWidth / configuration.MinimumWColumnWidth;
//			var minimumColumnCount = hostWidth / configuration.MaximumColumnWidth;




//		}

//		/// <inheritdoc />
//		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//		{
//			throw new NotImplementedException();
//		}
//	}
//}

