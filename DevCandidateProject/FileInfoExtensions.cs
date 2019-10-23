using System.IO;
using System.Linq;

namespace DevCandidateProject
{
	/// <summary>
	///		Extension methods for the <see cref="FileInfo"/> type.
	/// </summary>
	public static class FileInfoExtensions
	{
		/// <summary>
		///		Gets the name of the <see cref="FileInfo"/> object, excluding the file's extension.
		/// </summary>
		/// <param name="this">
		///		The <see cref="FileInfo"/> in which to get the name of.
		/// </param>
		/// <returns>
		///		Returns the name of the <see cref="FileInfo"/> object, excluding the file's extension.
		/// </returns>
		public static string GetFileNameWithoutExtension(
			this FileInfo @this)
		{
			var extensionLength = @this
				.Name
				.Split('.')
				.Last()
				.Length;

			return @this.Name.Substring(0, @this.Name.Length - extensionLength - 1);
		}
	}
}