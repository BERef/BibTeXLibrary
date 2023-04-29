using DigitalProduction.XML.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BibTeXLibrary
{
	/// <summary>
	/// 
	/// </summary>
	[XmlRoot("bibentryinitialization")]
	public class BibEntryInitializer
	{
		#region Enumerations

		#endregion

		#region Delegates

		#endregion

		#region Events

		#endregion

		#region Fields

		private SerializableDictionary<string, string> _typeToTemplateMap;
		private SerializableDictionary<string, List<string>> _templates;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public BibEntryInitializer()
		{
		}

		#endregion

		#region Properties

		[XmlElement("typetotemplatemap")]
		public SerializableDictionary<string, string> TypeToTemplateMap { get => _typeToTemplateMap; set => _typeToTemplateMap = value; }

		[XmlElement("templates")]
		public SerializableDictionary<string, List<string>> Templates { get => _templates; set => _templates = value; }

		#endregion

		#region Methods

		#endregion

		#region XML

		/// <summary>
		/// Write this object to a file to the provided path.
		/// </summary>
		/// <param name="path">Path (full path and filename) to write to.</param>
		/// <exception cref="InvalidOperationException">Thrown when the projects path is not valid.</exception>
		public void Serialize(string path)
		{
			if (!DigitalProduction.IO.Path.PathIsWritable(path))
			{
				throw new InvalidOperationException("The file cannot be saved.  A valid path must be specified.");
			}
			SerializationSettings settings = new SerializationSettings(this, path);
			settings.XmlSettings.NewLineOnAttributes = false;
			Serialization.SerializeObject(settings);
		}

		/// <summary>
		/// Create an instance from a file.
		/// </summary>
		/// <param name="path">The file to read from.</param>
		public static BibEntryInitializer Deserialize(string path)
		{
			return Serialization.DeserializeObject<BibEntryInitializer>(path);
		}

		#endregion

	} // End class.
} // End namespace.