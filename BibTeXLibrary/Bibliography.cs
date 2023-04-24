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

		private ObservableCollection<BibEntry> _entries = new ObservableCollection<BibEntry>();

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

		public void Read(string path)
		{
			BibParser parser = new BibParser(new StreamReader(path, Encoding.Default));
			_entries.Clear();
			foreach (BibEntry bibEntry in parser.GetAllResult())
			{
				_entries.Add(bibEntry);
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