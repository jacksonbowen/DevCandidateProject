using System.IO;
using System.Windows.Input;
using Caliburn.Micro;
using DevCandidateProject.Data.Domain;

namespace DevCandidateProject.UI.ViewModels
{
	public class RootViewModel
		: PropertyChangedBase
	{
		private string _sourceGradesFolder = @"C:\Static\Classes\";
		private bool _isSourceGradesFolderValid;
		private BindableCollection<FileInfo> _classFileList;

		
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

		public BindableCollection<FileInfo> ClassFileList
		{
			get => _classFileList;
			set
			{
				_classFileList = value;
				NotifyOfPropertyChange(() => ClassFileList);
			}
		}

		private BindableCollection<ClassGrades> _calculatedClassGrades;
		public BindableCollection<ClassGrades> CalculatedClassGrades
		{
			get => _calculatedClassGrades;
			set
			{
				_calculatedClassGrades = value;
				NotifyOfPropertyChange(() => CalculatedClassGrades);
			}
		}


		public ICommand CalculateGradesCommand => new Command(
			t =>
			{
				CalculatedClassGrades = new BindableCollection<ClassGrades>
				{
				};
			});


		public RootViewModel()
		{
			checkSourceGradesFolderValid();
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