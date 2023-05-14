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

		private BibliographyDOM					_bibliographyDOM				= new BibliographyDOM();

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
				return _bibliographyDOM.BibliographyEntries;
			}
		}

		/// <summary>
		/// The bibliography document object model.
		/// </summary>
		public BibliographyDOM DocumentObjectModel { get => _bibliographyDOM; }

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
			_bibliographyDOM = parser.GetAllResults();
		}

		/// <summary>
		/// Read the bibliography file.
		/// </summary>
		/// <param name="bibFilePath">Full path to the bibliography file.</param>
		/// <param name="bibEntryInitializationFile">Full path to the bibliography entry initialization file.</param>
		public void Read(string bibFilePath, string bibEntryInitializationFile)
		{
			BibParser parser = new BibParser(bibFilePath, bibEntryInitializationFile);
			_bibliographyDOM = parser.GetAllResults();
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
				foreach (string line in _bibliographyDOM.Header)
				{
					streamWriter.WriteLine(line);
				}

				// Write each entry with a blank line preceeding it.
				foreach (BibEntry bibEntry in _bibliographyDOM.BibliographyEntries)
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
			_bibliographyDOM.Dispose();
		}

		#endregion

		#region Key Generation

		private IAlphaNumericStringProvider GetSuffixGenerator()
		{
			// Provide a sequence of incremented strings.  For example, a,b,c or A,B,C.
			return new EnglishLowerCaseAlphabet();
		}

		/// <summary>
		/// Checks if the key follows the rules to be a valid auto key.
		/// </summary>
		/// <param name="entry">BibEntry to check.</param>
		public bool HasValidAutoCiteKey(BibEntry entry)
		{
			string keyBase = GenerateCiteKeyBase(entry);

			// If the key base is longer, it is definitely not valid and will cause an error when getting the sub string below.
			if (keyBase.Length > entry.Key.Length)
			{
				return false;
			}

			return keyBase == entry.Key.Substring(0, keyBase.Length);
		}

		/// <summary>
		/// Generates a new, unique key for the entry and sets it.
		/// </summary>
		/// <param name="entry">BibEntry to generate a key for.</param>

		public void GenerateUniqueCiteKey(BibEntry entry)
		{
			string key = GenerateCiteKeyBase(entry);

			// Needs to be last.
			key += GenerateCiteKeySuffix(key.ToString());

			entry.Key = key.ToString();
		}

		/// <summary>
		/// Generate the base of a cite key (absent the suffix).
		/// </summary>
		/// <param name="entry">BibEntry.</param>
		private string GenerateCiteKeyBase(BibEntry entry)
		{
			string prefix = "ref:";
			StringBuilder key = new StringBuilder(prefix);

			// This is setup to allow no conversion, lower case, upper case, et cetera in the future, but for now just assume lower case.
			key.Append(entry.GetFirstAuthorsName(NameFormat.Last, StringCase.LowerCase));
			key.Append(entry.Year);
			return key.ToString();
		}

		/// <summary>
		/// Generation a cite key suffix.
		/// </summary>
		/// <param name="baseKey">The cite key base.</param>
		/// <exception cref="IndexOutOfRangeException">Thrown if the algorithm runs out of suffixes to try.</exception>
		private string GenerateCiteKeySuffix(string baseKey)
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

			foreach (BibEntry entry in _bibliographyDOM.BibliographyEntries)
			{
				if (entry.Key.ToLower() == key)
				{
					return true;
				}
			}

			return false;
		}

		#endregion

		#endregion

	} // End class.
} // End namespace.