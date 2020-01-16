using System;
using System.Globalization;
using System.Windows.Data;

namespace DevCandidateProject.UI.Markup.ValueConverters
{
	public class DoubleToPercentageConverter 
		: IValueConverter
	{
		/// <inheritdoc />
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var numericalValue = System.Convert.ToDouble(value);
			var roundedNumericalValue = Math.Round(numericalValue, 2);
			return roundedNumericalValue.ToString("###.##") + "%";
		}

		/// <inheritdoc />
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
