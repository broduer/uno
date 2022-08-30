﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Windows.UI.Xaml;

public partial class FontFamilyHelper
{
	/// <summary>
	/// Pre-loads a font to minimize loading time and prevent potential text re-layouts.
	/// </summary>
	/// <returns>True is the font loaded successfuly, otherwise false.</returns>
	public static Task<bool> Preload(FontFamily family)
		=> FontFamily.Preload(family);

	/// <summary>
	/// Pre-loads a font to minimize loading time and prevent potential text re-layouts.
	/// </summary>
	/// <returns>True is the font loaded successfuly, otherwise false.</returns>
	public static Task<bool> Preload(string familyName)
		=> Preload(new FontFamily(familyName));
}
