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
	public class BibEntryInitialization
	{
		#region Fields

		private SerializableDictionary<string, string>			_typeToTemplateMap			= new SerializableDictionary<string, string>();
		private SerializableDictionary<string, List<string>>	_templates					= new SerializableDictionary<string, List<string>>();

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public BibEntryInitialization()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Type bibliography type to template map.
		/// </summary>
		[XmlElement("typetotemplatemap")]
		public SerializableDictionary<string, string> TypeToTemplateMap { get => _typeToTemplateMap; set => _typeToTemplateMap = value; }

		/// <summary>
		/// The templates used to initialize a BibEntry.
		/// </summary>
		[XmlElement("templates")]
		public SerializableDictionary<string, List<string>> Templates { get => _templates; set => _templates = value; }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the default set of (ordered) tags.
		/// </summary>
		/// <param name="bibEntry">BibTex entry type.</param>
		public List<string> GetTags(BibEntry bibEntry)
		{
			return GetTags(bibEntry.Type);
		}

		/// <summary>
		/// Gets the default set of (ordered) tags.
		/// </summary>
		/// <param name="type">BibTex entry type.</param>
		public List<string> GetTags(string type)
		{
			type = type.ToLower();

			if (_typeToTemplateMap.ContainsKey(type))
			{
				string template = _typeToTemplateMap[type];
				return _templates[template];
			}
			else
			{
				return new List<string>();
			}
		}

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
		public static BibEntryInitialization Deserialize(string path)
		{
			return Serialization.DeserializeObject<BibEntryInitialization>(path);
		}

		#endregion

	} // End class.
} // End namespace.