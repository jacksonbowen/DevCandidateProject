using System;
using System.IO;

namespace DevCandidateProject
{
	/// <summary>
	///		Extension methods for the <see cref="StreamWriter"/> type.
	/// </summary>
	public static class StreamWriterExtensions
	{
		/// <summary>
		///		Extension method for <see cref="StreamWriter"/> objects to write the <see cref="text"/> to
		///		both the provided <see cref="StreamWriter"/>, as well as to the <see cref="Console"/>.
		/// </summary>
		/// <param name="this">
		///		The <see cref="StreamWriter"/> in which to write to.
		/// </param>
		/// <param name="text">
		///		The text to write to both the <see cref="StreamWriter"/> and the <see cref="Console"/>.
		/// </param>
		/// <returns>
		///		Returns the original <see cref="StreamWriter"/> object for method chaining.
		/// </returns>
		public static StreamWriter WriteLineEx(
			this StreamWriter @this,
			string text = "")
		{
			@this.WriteLine(text);
			Console.WriteLine(text);

			return @this;
		}
	}
}
