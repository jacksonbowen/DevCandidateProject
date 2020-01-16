using System.Collections.Generic;
using System.IO;
using DevCandidateProject.Data.Domain;

namespace DevCandidateProject.Data.Parsing
{
	public static class GradeFileParser
	{
		/// <summary>
		/// Gets all *.csv files located in the <paramref name="csvFileRootFolder"/> root, and returns
		/// an <see cref="IEnumerable{T}"/> of each parsed <see cref="ClassGrades"/> object.
		/// </summary>
		/// <param name="csvFileRootFolder">
		/// The <see cref="DirectoryInfo"/> representing the directory location of the csv files to parse. 
		/// </param>
		/// <returns>
		/// Returns an <see cref="IEnumerable{T}"/> of each parsed <see cref="ClassGrades"/> object.
		/// </returns>
		public static IEnumerable<ClassGrades> GetAllStudentGradesForAllFiles(
			DirectoryInfo csvFileRootFolder)
		{
			foreach (var csvGradeFile in csvFileRootFolder.GetFiles("*.csv"))
			{
				yield return ClassGrades.ParseFromCsvFile(csvGradeFile);
			}
		}
	}
}
