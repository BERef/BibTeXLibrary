using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BibTeXLibrary
{
	/// <summary>
	/// Internal representation of a bib file.
	/// </summary>
	public class Bibliography
	{
		#region Members

		private BindingList<BibEntry>			_entries		= new BindingList<BibEntry>();

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
		/// <param name="path">Full path to the bibliography file.</param>
		public void Read(string path)
		{
			_entries.Clear();

			BibParser parser = new BibParser(path);
			
			foreach (BibEntry bibEntry in parser.GetAllResult())
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
			BibEntryInitialization bibEntryInitialization = new BibEntryInitialization();
			bibEntryInitialization.Serialize(path + "inial.xml");

			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				foreach (BibEntry bibEntry in _entries)
				{
					streamWriter.WriteLine();
					streamWriter.Write(bibEntry.ToString(writeSettings));
					streamWriter.Write(streamWriter.NewLine);
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