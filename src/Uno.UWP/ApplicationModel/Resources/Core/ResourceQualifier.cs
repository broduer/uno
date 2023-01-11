﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Windows.ApplicationModel.Resources.Core;

/// <summary>
/// Defines the qualifiers associated with a ResourceCandidate.
/// </summary>
public partial class ResourceQualifier
{
	internal ResourceQualifier(string name, string value)
	{
		QualifierName = name;
		QualifierValue = value;
	}

	/// <summary>
	/// The name of the qualifier.
	/// </summary>
	public string QualifierName { get; }

	/// <summary>
	/// The value of the qualifier.
	/// </summary>
	public string QualifierValue { get; }

	internal static ResourceQualifier Parse(string str)
	{
		if (IsLanguageTag(str))
		{
			str = $"language-{str}";
		}

		if (str.Contains("-"))
		{
			var qualifierParts = str.Split(new[] { '-' }, 2);
			var name = qualifierParts[0].ToLowerInvariant();
			var value = qualifierParts[1];

			if (name == "lang")
			{
				name = "language";
			}

			if (name == "scale" || name == "language" || name == "theme" || name == "custom" )
			{
				return new ResourceQualifier(name, value);
			}
		}

		return null;
	}

	#region Language helpers

	private static HashSet<string> _languageTags;
	
	private static HashSet<string> LanguageTags
	{
		get
		{
			if (_languageTags == null)
			{
				var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
				var ietfLanguageTags = cultures.Select(c => c.IetfLanguageTag);
				var ietfLanguageParentTags = cultures.Select(c => c.Parent.IetfLanguageTag);
				var twoLetterLanguageTags = cultures.Select(c => c.TwoLetterISOLanguageName);

				var allCulture = Enumerable.Concat(
					ietfLanguageTags,
					twoLetterLanguageTags.Concat(ietfLanguageParentTags));

				_languageTags = new HashSet<string>(allCulture.Distinct());
			}

			return _languageTags;
		}
	}

	private static bool IsLanguageTag(string str)
	{
		return LanguageTags.Contains(str, StringComparer.InvariantCultureIgnoreCase);
	}

	#endregion
}
