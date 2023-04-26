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
	/// 
	/// </summary>
	public class Bibliography
	{
		#region Members

		private ObservableCollection<BibEntry>	_entries			= new ObservableCollection<BibEntry>();

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
		public ObservableCollection<BibEntry> Entries
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
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				foreach (BibEntry bibEntry in _entries)
				{
					streamWriter.WriteLine();
					streamWriter.Write(bibEntry.ToString());
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