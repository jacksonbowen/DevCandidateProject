using System;

namespace DevCandidateProject.Data.Domain
{
	/// <summary>
	/// The <see cref="StudentGrade"/> class represents one student, their <see cref="Grade"/> (a
	/// <see cref="double"/> between 0 and 100, inclusively), and their <see cref="AdjustedGrade"/>,
	/// an <see cref="int"/> adjustment dropping off any decimals.
	/// </summary>
	public class StudentGrade
	{
		public int StudentGradeID { get; set; }

		/// <summary>
		/// The student's full name.
		/// </summary>
		public string StudentName { get; set; }

		/// <summary>
		/// The student's unadjusted <see cref="double"/> grade, including any decimals.
		/// </summary>
		public double Grade { get; set; }

		/// <summary>
		/// The student's adjusted <see cref="int"/> grade, dropping any decimals.
		/// </summary>
		public int AdjustedGrade
		{
			get => (int)Math.Floor(Grade);
		}


		public StudentGrade()
		{
		}

		/// <summary>
		/// The constructor for the <see cref="StudentGrade"/> class.
		/// </summary>
		/// <param name="studentName">
		/// The student's full name.
		/// </param>
		/// <param name="grade">
		/// The student's unadjusted <see cref="double"/> grade, including any decimals.
		/// </param>
		public StudentGrade(
			string studentName,
			double grade)
				: this()
		{
			StudentName = studentName;
			Grade = grade;
		}
	}
}
