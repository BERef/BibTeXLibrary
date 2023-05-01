using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace BibTeXLibrary
{
	/// <summary>
	/// Internal representation of a bib file.
	/// </summary>
	public class Bibliography
	{
		#region Fields

		private readonly static string			_bibInitializationFileName		= "Bib Entry Initialization.xml";

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

	} // End class.
} // End namespace.