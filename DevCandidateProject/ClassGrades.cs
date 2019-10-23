using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevCandidateProject
{
	/// <summary>
	///		The <see cref="ClassGrades"/> class represents a parsed .csv file of one class's grades,
	///		consisting of 10 students. Contains information about the class, the average grades, and the
	///		best and worst performing students.
	/// </summary>
	public class ClassGrades
	{
		private readonly StudentGrade[] _grades;
		private readonly StudentGrade[] _gradesZero;
		private readonly StudentGrade[] _gradesNonZero;
		private readonly string _className;

		/// <summary>
		///		Gets the student with the highest average grade, expressed in a <see cref="StudentGrade"/>.
		/// </summary>
		public StudentGrade HighestPerformingStudent
		{
			get => _gradesNonZero.Last();
		}

		/// <summary>
		///		Gets the student with the lowest average grade, (grades of zero excluded), expressed in a
		///		<see cref="StudentGrade"/>.
		/// </summary>
		public StudentGrade LowestPerformingStudent
		{
			get => _gradesNonZero.First();
		}

		/// <summary>
		///		Gets all student grades that were parsed from the class's .csv file, including any students
		///		with grades of zero. The collection is sorted by <see cref="StudentGrade.Grade"/>, ascending.  
		/// </summary>
		public IReadOnlyCollection<StudentGrade> Grades
		{
			get => _grades;
		}

		/// <summary>
		///		Gets all student grades that were parsed from the class's .csv file, excluding any students
		///		with grades of zero. The collection is sorted by <see cref="StudentGrade.Grade"/>, ascending.  
		/// </summary>
		public IReadOnlyCollection<StudentGrade> GradesNonZero
		{
			get => _gradesNonZero;
		}

		/// <summary>
		///		Gets all student grades that were parsed from the class's .csv file that had grades of zero.  
		/// </summary>
		public IReadOnlyCollection<StudentGrade> GradesZero
		{
			get => _gradesZero;
		}

		/// <summary>
		///		Gets the class average of all students in the class, excluding any students with a grade
		///		of zero from the calculation.
		/// </summary>
		public double Average
		{
			get => _gradesNonZero.Average(t => t.Grade);
		}

		/// <summary>
		///		Gets the class name, identified by the csv file name, with a space inserted for formatting.
		/// </summary>
		public string ClassName
		{
			get => _className;
		}


		/// <summary>
		///		The constructor for the <see cref="ClassGrades"/> class.
		/// </summary>
		/// <param name="grades">
		///		An <see cref="IEnumerable{T}"/> consisting of all of the grades in the class parsed from
		///		the csv file.
		/// </param>
		/// <param name="csvFileName">
		///		The name of the csv file in which the <see cref="ClassGrades"/> were parsed from, excluding
		///		the file extension.
		/// </param>
		public ClassGrades(
			IEnumerable<StudentGrade> grades,
			string csvFileName)
		{
			_grades = grades
				.OrderBy(t => t.Grade)
				.ToArray();

			_gradesNonZero = _grades
				.Where(t => t.Grade != 0)
				.ToArray();

			_gradesZero = _grades
				.Where(t => t.Grade == 0)
				.ToArray();

			var lastChar = csvFileName.Last();

			_className = csvFileName.Substring(0, csvFileName.Length - 1) + $" {lastChar}";
		}
		

		/// <summary>
		///		Static method that parses the specified <paramref name="csvGradeFile"/> file into a
		///		<see cref="ClassGrades"/> object.
		/// </summary>
		/// <param name="csvGradeFile">
		///		A <see cref="FileInfo"/> object representing the source csv file containing the grades.
		/// </param>
		/// <returns>
		///		Returns a <see cref="ClassGrades"/> object from the parsed csv file.
		/// </returns>
		public static ClassGrades ParseFromCsvFile(
			FileInfo csvGradeFile)
		{
			var gradeList = new List<StudentGrade>();

			using (var reader = csvGradeFile.OpenRead())
			{
				using (var streamReader = new StreamReader(reader))
				{
					var line = streamReader.ReadLine();
					var firstLine = true;

					while (line != null)
					{
						if (firstLine)
						{
							firstLine = false;
							line = streamReader.ReadLine();
							continue;
						}

						var columns = line.Split(',')
							.Select(t => t.Trim())
							.ToArray();

						var studentName = columns[0];
						var gradePercentageStr = columns[1];

						if (!double.TryParse(gradePercentageStr, out var gradePercentage))
							throw new FormatException(
								$"\"{gradePercentageStr}\" is not parseable to the type 'double'.");

						gradeList.Add(
							new StudentGrade(
								studentName,
								gradePercentage));

						line = streamReader.ReadLine();
					}
				}

				return new ClassGrades(
					gradeList,
					csvGradeFile.GetFileNameWithoutExtension());
			}
		}
	}
}