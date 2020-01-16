using System.IO;
using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using DevCandidateProject.Data.Domain;
using DevCandidateProject.Data.Parsing;

namespace DevCandidateProject.UI.ViewModels
{
	public class RootViewModel
		: PropertyChangedBase
	{
		private string _sourceGradesFolder = @"C:\Static\Classes\";
		private bool _isSourceGradesFolderValid;
		private bool _isViewingCalculationReport;
		private BindableCollection<FileInfo> _classFileList;
		private BindableCollection<ClassGrades> _calculatedClassGrades;
		private ClassGrades _bestPerformingClass;
		private double _averageOfAllClasses;
		private int _totalNumberOfNonZeroStudents;
		private int _totalNumberOfExcludedStudents;
		private int _totalNumberOfStudents;


		public string SourceGradesFolder
		{
			get => _sourceGradesFolder;
			set
			{
				_sourceGradesFolder = value;
				NotifyOfPropertyChange(() => SourceGradesFolder);

				checkSourceGradesFolderValid();
			}
		}

		public bool IsSourceGradesFolderValid
		{
			get => _isSourceGradesFolderValid;
			set
			{
				_isSourceGradesFolderValid = value;
				NotifyOfPropertyChange(() => IsSourceGradesFolderValid);
			}
		}

		public bool IsViewingCalculationReport
		{
			get => _isViewingCalculationReport;
			set
			{
				_isViewingCalculationReport = value;
				NotifyOfPropertyChange(() => IsViewingCalculationReport);
			}
		}

		public BindableCollection<FileInfo> ClassFileList
		{
			get => _classFileList;
			set
			{
				_classFileList = value;
				NotifyOfPropertyChange(() => ClassFileList);
			}
		}

		public BindableCollection<ClassGrades> CalculatedClassGrades
		{
			get => _calculatedClassGrades;
			set
			{
				_calculatedClassGrades = value;
				NotifyOfPropertyChange(() => CalculatedClassGrades);
			}
		}

		public ClassGrades BestPerformingClass
		{
			get => _bestPerformingClass;
			set
			{
				_bestPerformingClass = value;
				NotifyOfPropertyChange(() => BestPerformingClass);
			}
		}

		public double AverageOfAllClasses
		{
			get => _averageOfAllClasses;
			set
			{
				_averageOfAllClasses = value;
				NotifyOfPropertyChange(() => AverageOfAllClasses);
			}
		}

		public int TotalNumberOfNonZeroStudents
		{
			get => _totalNumberOfNonZeroStudents;
			set
			{
				_totalNumberOfNonZeroStudents = value;
				NotifyOfPropertyChange(() => TotalNumberOfNonZeroStudents);
			}
		}

		public int TotalNumberOfExcludedStudents
		{
			get => _totalNumberOfExcludedStudents;
			set
			{
				_totalNumberOfExcludedStudents = value;
				NotifyOfPropertyChange(() => TotalNumberOfExcludedStudents);
			}
		}

		public int TotalNumberOfStudents
		{
			get => _totalNumberOfStudents;
			set
			{
				_totalNumberOfStudents = value;
				NotifyOfPropertyChange(() => TotalNumberOfStudents);
			}
		}
		
		public ICommand CalculateGradesCommand => new Command(
			t =>
			{
				calculateGrades();
			});


		public RootViewModel()
		{
			checkSourceGradesFolderValid();
		}
		

		private void calculateGrades()
		{
			var inputDirectory = new DirectoryInfo(SourceGradesFolder);

			var allClassGrades = GradeFileParser.GetAllStudentGradesForAllFiles(inputDirectory)
				.ToArray();

			BestPerformingClass = allClassGrades
				.OrderBy(t => t.Average)
				.Last();

			AverageOfAllClasses = allClassGrades.Average(t => t.Average);

			TotalNumberOfNonZeroStudents = allClassGrades.Sum(t => t.GradesNonZero.Count);

			TotalNumberOfExcludedStudents = allClassGrades.Sum(t => t.GradesZero.Count);

			TotalNumberOfStudents = TotalNumberOfNonZeroStudents + TotalNumberOfExcludedStudents;

			var calculatedgrades = new BindableCollection<ClassGrades>(allClassGrades);

			CalculatedClassGrades = calculatedgrades;

			IsViewingCalculationReport = true;
		}

		private void checkSourceGradesFolderValid()
		{
			var directoryInfo = new DirectoryInfo(SourceGradesFolder);
			IsSourceGradesFolderValid = directoryInfo.Exists;

			if (!directoryInfo.Exists)
				return;
			
			ClassFileList = new BindableCollection<FileInfo>(directoryInfo.GetFiles("*.csv"));
		}
	}
}