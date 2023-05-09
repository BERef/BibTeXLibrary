using DigitalProduction.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BibTeXLibrary
{
	/// <summary>
	/// Internal representation of a bib file.
	/// </summary>
	public class Bibliography
	{
		#region Fields

		public static string[]					_nameSuffixes					= { "jr", "jr.", "sr", "sr.", "ii", "iii", "iv", "v", @"p\`{e}re", "fils" };

		private List<string>					_header;
		private BindingList<BibEntry>			_entries						= new BindingList<BibEntry>();

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Bibliography()
		{	 
		}

		#endregion

		#region Properties


		/// <summary>
		/// BibTeX entries.
		/// </summary>
		[XmlIgnore()]
		public BindingList<BibEntry> Entries
		{
			get
			{
				return _entries;
			}
			set
			{
				_entries = value;
			}
		}

		#endregion

		#region Methods

		#region Reading and Writing

		/// <summary>
		/// Read the bibliography file.
		/// </summary>
		/// <param name="bibFilePath">Full path to the bibliography file.</param>
		public void Read(string bibFilePath)
		{
			BibParser parser = new BibParser(bibFilePath);
			GetResults(parser);
		}

		/// <summary>
		/// Read the bibliography file.
		/// </summary>
		/// <param name="bibFilePath">Full path to the bibliography file.</param>
		/// <param name="bibEntryInitializationFile">Full path to the bibliography entry initialization file.</param>
		public void Read(string bibFilePath, string bibEntryInitializationFile)
		{
			BibParser parser = new BibParser(bibFilePath, bibEntryInitializationFile);
			GetResults(parser);
		}

		/// <summary>
		/// Gets the results from the parser.
		/// </summary>
		/// <param name="parser">BibParser used to read the file.</param>
		private void GetResults(BibParser parser)
		{
			Tuple<List<string>, List<BibEntry>> results = parser.GetAllResults();

			_header = results.Item1;

			_entries.Clear();
			foreach (BibEntry bibEntry in results.Item2)
			{
				_entries.Add(bibEntry);
			}
		}

		/// <summary>
		/// Write the bibiography file.
		/// </summary>
		/// <param name="path">Full path to the bibiography file.</param>
		public void Write(string path)
		{
			Write(path, new WriteSettings());
		}

		/// <summary>
		/// Write the bibiography file.
		/// </summary>
		/// <param name="path">Full path to the bibiography file.</param>
		/// <param name="writeSettings">The settings for writing the bibliography file.</param>
		public void Write(string path, WriteSettings writeSettings)
		{
			//_bibEntryInitialization.Serialize(GetBibEntryInitializationPath(path));

			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				// Make sure the BibEntries use the expected line feed and carriage return character(s).
				writeSettings.NewLine = streamWriter.NewLine;

				// Write the header.  The header is stored as separate lines so when we write it we can use
				// the expected line ending type (\r\n, \n) used by the writer.
				foreach (string line in _header)
				{
					streamWriter.WriteLine(line);
				}

				// Write each entry with a blank line preceeding it.
				foreach (BibEntry bibEntry in _entries)
				{
					streamWriter.WriteLine();
					streamWriter.Write(bibEntry.ToString(writeSettings));
				}
			}
		}

		/// <summary>
		/// Clean up.
		/// </summary>
		public void Close()
		{
			_entries.Clear();
		}

		#endregion

		#region Key Generation

		private IAlphaNumericStringProvider GetSuffixGenerator()
		{
			// Provide a sequence of incremented strings.  For example, a,b,c or A,B,C.
			return new EnglishLowerCaseAlphabet();
		}

		/// <summary>
		/// Determines if the BibEntry's key follows the rules to be a valid auto key.  If the key is a
		/// valid auto generated key, nothing is done, otherwise an unique key is generated according to
		/// the rules.
		/// </summary>
		/// <param name="entry">BibEntry to use auto generated key.</param>
		public void AutoKeyEntry(BibEntry entry)
		{
			if (!ValidAutoKey(entry))
			{
				GenerateUniqueKey(entry);
			}
		}

		/// <summary>
		/// Checks if the key follows the rules to be a valid auto key.
		/// </summary>
		/// <param name="entry">BibEntry to check.</param>
		private bool ValidAutoKey(BibEntry entry)
		{
			string keyBase = GenerateKeyBase(entry);
			return keyBase == entry.Key.Substring(0, keyBase.Length);
		}

		/// <summary>
		/// Generates a new, unique key for the entry and sets it.
		/// </summary>
		/// <param name="entry">BibEntry to generate a key for.</param>

		private void GenerateUniqueKey(BibEntry entry)
		{
			string key = GenerateKeyBase(entry);

			// Needs to be last.
			key += GenerateSuffix(key.ToString());

			entry.Key = key.ToString();
		}

		private string GenerateKeyBase(BibEntry entry)
		{
			string prefix = "ref:";
			StringBuilder key = new StringBuilder(prefix);

			// This is setup to allow no conversion, lower case, upper case, et cetera in the future, but for now just assume lower case.
			key.Append(GetAuthorsName(entry, "last", StringCase.LowerCase));
			key.Append(entry.Year);
			return key.ToString();
		}

		private string GenerateSuffix(string baseKey)
		{
			foreach (string suffix in GetSuffixGenerator().Get())
			{
				if (!IsKeyInUse(baseKey+suffix))
				{
					return suffix;
				}
			}

			// We only go through one loop of a suffix generator.  If we run out it is an exception, we don't currently
			// handle this case.  An example would be using lower case letters and all names from
			// ref:shakespeare1597a to ref:shakespeare1597z were used.  Highly unlikely.
			throw new IndexOutOfRangeException("Ran out of suffix characters.");
		}

		private bool IsKeyInUse(string key)
		{
			// BibTeX seems to be case sensitive keys.  I.e.
			// @book{ref:shakespeare,
			// is different from
			// @book{ref:Shakespeare,
			// However, this could be confusing or error prone, so (for now anyway) we will do a case insensitive comparison.
			key = key.ToLower();

			foreach (BibEntry entry in _entries)
			{
				if (entry.Key.ToLower() == key)
				{
					return true;
				}
			}

			return false;
		}

		private string GetAuthorsName(BibEntry entry, string format, StringCase toCase)
		{
			// Get the authors and split on the "and" string.  If there are no authors, return a blank string.
			string[] authors = entry.Author.Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries);
			if (authors.Length == 0)
			{
				return "";
			}

			string firstAuthorName = "";
			string result = "";

			// Split the first author on a comma.  Author names can be in the formats of:
			// William Shakespeare
			// Shakespeare, William
			// If it is in the second format, we will reverse it so we have the name always specified in the same manner.
			// If there is no comma, we should only get 1 result.
			string[] firstAuthorArray	= authors[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (firstAuthorArray.Length == 1)
			{
				// William Shakespeare, nothing required.
				firstAuthorName = firstAuthorArray[0];
			}
			else
			{
				// Shakespeare, William, reverse the order.
				firstAuthorName = firstAuthorArray[1] + " " + firstAuthorArray[0];
			}

			switch (format)
			{
				case "full":
					result = firstAuthorName;
					break;

				case "first":
					result = (firstAuthorName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))[0];
					break;

				case "last":
					// Split the full name into separate words/name.
					firstAuthorArray = firstAuthorName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					// We don't want to return "Sr.", "Jr.", et cetera, so work backwards and ignore any of those.
					// The first word we find that is not in our rejected list, we will treat as the last name.
					for (int i = firstAuthorArray.Length-1;  i >= 0; i--)
					{
						if (!_nameSuffixes.Any(item => item == firstAuthorArray[i]))
						{
							result = firstAuthorArray[i];
							break;
						}
					}
					break;

				default:
					throw new NotSupportedException("The name format specified is not valid.");
			}

			return Format.ChangeCase(result, toCase);
		}

		#endregion

		#endregion

	} // End class.
} // End namespace.