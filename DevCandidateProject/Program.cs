using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DevCandidateProject
{
	public class Program
	{
		/// <summary>
		/// The application's main entry point and terminal loop.
		/// </summary>
		/// <param name="args">
		/// Any command line arguments passed to the executable.
		/// </param>
		public static void Main(string[] args)
		{
			Console.WriteLine("Developer Candidate Project Terminal");

			var hasQuit = false;
			var command = "";

			// gets the output directory, a subfolder on the user's desktop
			var outputDirectory = new DirectoryInfo(
				Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
				+ @"\Calculated Grades\");

			// checks if the directory exists, if not, creates it
			if (!outputDirectory.Exists)
				outputDirectory.Create();

			// loop for entry of commands, while the user has not chosen to quit the application
			while (!hasQuit)
			{
				Console.WriteLine("type \"query\" to query all the student grades, or type \"quit\" to quit.");

				command = Console.ReadLine();
				command = command?.ToLower();

				switch (command)
				{
					// the 'query' command
					case "query":
						{
							var inputDirectory = resolveInputDirectory();
							var outputFilePath = outputDirectory + @"\output.txt";

							// gets the ClassGrades for each csv file in the input directory, and converts it to an array
							var allClassGrades = GetAllStudentGradesForAllFiles(inputDirectory)
								.ToArray();

							// opens a fileStream for the output file, with Create and ReadWrite permissions
							using (var fileStream = File.Open(outputFilePath,
								FileMode.Create,
								FileAccess.ReadWrite))
							{
								using (var streamWriter = new StreamWriter(fileStream))
								{
									// orders all of the class grades by their average grades, and takes the last element
									var bestPerformingClass = allClassGrades
										.OrderBy(t => t.Average)
										.Last();

									streamWriter
										.WriteLineEx("=============================================")
										.WriteLineEx($"|| Best Performing Class:          {bestPerformingClass.ClassName} ||")
										.WriteLineEx("=============================================")
										.WriteLineEx();

									// gets the average of all of the classes' averages
									var averageOfAllClasses = allClassGrades.Average(t => t.Average);

									streamWriter.WriteLineEx($"Average of All Classes             {averageOfAllClasses:##.00}%");

									var totalNumberOfNonZeroStudents =
										allClassGrades.Sum(t => t.GradesNonZero.Count);

									var totalNumberOfExcludedStudents =
										allClassGrades.Sum(t => t.GradesZero.Count);

									var totalNumberOfStudents =
										totalNumberOfNonZeroStudents + totalNumberOfExcludedStudents;

									var totalNumberOfClasses = allClassGrades.Length;

									streamWriter
										.WriteLineEx($"Total Students in Calculation      {totalNumberOfNonZeroStudents} ({totalNumberOfExcludedStudents} excluded)")
										.WriteLineEx($"Total Students:                    {totalNumberOfStudents} in {totalNumberOfClasses} classes")
										.WriteLineEx();

									// writes each classGrade object's detailed information to the file / console
									foreach (var classGrades in allClassGrades)
									{
										streamWriter.WriteLineEx($"{classGrades.ClassName} Info");

										streamWriter
											.WriteLineEx($"  Number of Students in Calc:      {classGrades.GradesNonZero.Count}")
											.WriteLineEx($"  Class Average:                   {classGrades.Average:##.00}%")
											.WriteLineEx($"    Excluded Students              {classGrades.GradesZero.Count}");

										// lists each student that were excluded from the calculation due to having a grade of 0
										foreach (var excludedStudents in classGrades.GradesZero)
										{
											streamWriter.WriteLineEx($"      {excludedStudents.StudentName}");
										}
										streamWriter.WriteLineEx();
									}
								}
							}
							onQueryComplete(outputFilePath);
							break;
						}
					case "quit":
					case "q":
						{
							hasQuit = true;
							break;
						}
					default:
						{
							Console.WriteLine("not a recognized command.");
							break;
						}
				}
			}
		}

		/// <summary>
		/// Asks the user if they would like to select a custom input folder, ensures the entry's
		/// validity, or defaults to the Visual Studio solution level folder's 'Classes' subfolder.
		/// </summary>
		/// <returns>
		/// Returns the resolved <see cref="DirectoryInfo"/> indicating where the user would like to
		/// perform the query on.
		/// </returns>
		private static DirectoryInfo resolveInputDirectory()
		{
			while (true)
			{
				Console.Write($"Would you like to select a custom input folder? (Y \\ N) ");
				var userEntry = (char)Console.Read();

				switch (char.ToUpper(userEntry))
				{
					case 'Y':
						{
							while (true)
							{
								Console.WriteLine($"Enter a custom input folder: ");
								Console.ReadLine();
								var directoryUserEntry = Console.ReadLine();

								if (!string.IsNullOrWhiteSpace(directoryUserEntry))
								{
									var directoryInfo = new DirectoryInfo(directoryUserEntry);

									if (directoryInfo.Exists)
										return directoryInfo;

									Console.WriteLine(
										$"The input text \"{directoryUserEntry}\" is not a valid directory.");
								}
							}
						}
					case 'N':
						{
							var currentDirectory = Directory.GetCurrentDirectory();

							var workingDirectory = new DirectoryInfo(currentDirectory);

							var projectPath = workingDirectory.Parent?.Parent?.Parent;

							if (projectPath == null)
								throw new NotSupportedException(
									$"Cannot find source folder in project directory.");

							var classesFolderPath = new DirectoryInfo(
								$@"{projectPath}\Classes\");

							Console.WriteLine($"Using default working directory \"{classesFolderPath.FullName}\".");

							return classesFolderPath;

						}
					default:
						{
							Console.WriteLine($"Invalid Entry.");
							break;
						}
				}
			}
		}

		/// <summary>
		/// Notifies the user that the query is complete, and asks the user if they would like to
		/// open the generated output file in notepad.
		/// </summary>
		/// <param name="outputFilePath">
		/// The <see cref="string"/> output path of the generated file.
		/// </param>
		private static void onQueryComplete(
			string outputFilePath)
		{
			while (true)
			{
				Console.WriteLine($"Query finished! Output file is located at \"{outputFilePath}\".");
				Console.WriteLine("Would you like to open the file now? (Y \\ N) ");

				var userEntry = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(userEntry))
					userEntry = Console.ReadLine();

				switch (userEntry.ToUpper())
				{
					case "Y":
						{
							// opens the file in notepad by passing the output file path in as an argument
							Process.Start("notepad.exe", outputFilePath);
							return;
						}
					case "N":
						{
							return;
						}
					default:
						{
							Console.WriteLine($"Invalid Entry.");
							break;
						}
				}
			}
		}

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