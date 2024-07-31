﻿using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace Microsoft.UI.Xaml.Controls;

partial class ComboBox
{
	/// <summary>
	/// Gets or sets the style of the TextBox in the ComboBox when the ComboBox is editable.
	/// </summary>
	public Style TextBoxStyle
	{
		get => (Style)GetValue(TextBoxStyleProperty);
		set => SetValue(TextBoxStyleProperty, value);
	}

	/// <summary>
	/// Identifies the TextBoxStyle dependency property.
	/// </summary>
	public static DependencyProperty TextBoxStyleProperty { get; } =
		DependencyProperty.Register(
			nameof(TextBoxStyle),
			typeof(Style),
			typeof(ComboBox),
			new FrameworkPropertyMetadata(null));

	/// <summary>
	/// Gets or sets the text in the ComboBox.
	/// </summary>
	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	/// <summary>
	/// Identifies the Text dependency property.
	/// </summary>
	public static DependencyProperty TextProperty { get; } =
		DependencyProperty.Register(
			nameof(Text),
			typeof(string),
			typeof(ComboBox),
			new FrameworkPropertyMetadata(""));

	/// <summary>
	/// Gets or sets a brush that describes the color of placeholder text.
	/// </summary>
	public Brush PlaceholderForeground
	{
		get => (Brush)GetValue(PlaceholderForegroundProperty);
		set => SetValue(PlaceholderForegroundProperty, value);
	}

	/// <summary>
	/// Identifies the PlaceholderForeground dependency property.
	/// </summary>
	public static DependencyProperty PlaceholderForegroundProperty { get; } =
		DependencyProperty.Register(
			nameof(PlaceholderForeground),
			typeof(Brush),
			typeof(ComboBox),
			new FrameworkPropertyMetadata(null));

	/// <summary>
	/// Gets or sets a value that indicates whether the user can
	/// edit text in the text box portion of the ComboBox.
	/// </summary>
	public bool IsEditable
	{
		get => (bool)GetValue(IsEditableProperty);
		set => SetValue(IsEditableProperty, value);
	}

	/// <summary>
	/// Identifies the IsEditable dependency property.
	/// </summary>
	public static DependencyProperty IsEditableProperty { get; } =
		DependencyProperty.Register(
			nameof(IsEditable),
			typeof(bool),
			typeof(ComboBox),
			new FrameworkPropertyMetadata(false));

	/// <summary>
	/// Gets whether the SelectionBoxItem is highlighted.
	/// </summary>
	public bool IsSelectionBoxHighlighted { get; private set; }

	/// <summary>
	/// Gets or sets a value that specifies whether a user can jump to a value by typing.
	/// </summary>
	public bool IsTextSearchEnabled
	{
		get => (bool)GetValue(IsTextSearchEnabledProperty);
		set => SetValue(IsTextSearchEnabledProperty, value);
	}

	/// <summary>
	/// Identifies the IsTextSearchEnabled dependency property.
	/// </summary>
	public static DependencyProperty IsTextSearchEnabledProperty { get; } =
		DependencyProperty.Register(
			nameof(IsTextSearchEnabled),
			typeof(bool),
			typeof(ComboBox),
			new FrameworkPropertyMetadata(true));

	/// <summary>
	/// Occurs when the user submits some text that does not correspond to an item in the ComboBox dropdown list.
	/// </summary>
	public event TypedEventHandler<ComboBox, ComboBoxTextSubmittedEventArgs> TextSubmitted;
}
