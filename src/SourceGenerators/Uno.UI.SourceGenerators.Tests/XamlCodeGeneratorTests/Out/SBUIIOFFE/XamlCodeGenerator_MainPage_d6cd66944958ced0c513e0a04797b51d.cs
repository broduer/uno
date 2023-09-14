﻿// <autogenerated />
#pragma warning disable CS0114
#pragma warning disable CS0108
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Uno.UI;
using Uno.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI.Text;
using Uno.Extensions;
using Uno;
using Uno.UI.Helpers;
using Uno.UI.Helpers.Xaml;
using MyProject;

#if __ANDROID__
using _View = Android.Views.View;
#elif __IOS__
using _View = UIKit.UIView;
#elif __MACOS__
using _View = AppKit.NSView;
#else
using _View = Microsoft.UI.Xaml.UIElement;
#endif

namespace TestRepro
{
	[global::System.Runtime.CompilerServices.CreateNewOnMetadataUpdate]
	partial class MainPage : global::Microsoft.UI.Xaml.Controls.Page
	{
		[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
		internal string __checksum() => "b9972598331e683fbb77aea763a897b0b45bc3d6";
		[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
		private const string __baseUri_prefix_MainPage_d6cd66944958ced0c513e0a04797b51d = "ms-appx:///TestProject/";
		[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
		private const string __baseUri_MainPage_d6cd66944958ced0c513e0a04797b51d = "ms-appx:///TestProject/";
		private global::Microsoft.UI.Xaml.NameScope __nameScope = new global::Microsoft.UI.Xaml.NameScope();
		private void InitializeComponent()
		{
			var __resourceLocator = new global::System.Uri("file:///C:/Project/0/MainPage.xaml");
			if(global::Uno.UI.ApplicationHelper.IsLoadableComponent(__resourceLocator))
			{
				global::Microsoft.UI.Xaml.Application.LoadComponent(this, __resourceLocator);
				return;
			}
			NameScope.SetNameScope(this, __nameScope);
			var __that = this;
			base.IsParsing = true;
			// Source 0\MainPage.xaml (Line 1:2)
			base.Content = 
			new global::Microsoft.UI.Xaml.Controls.Grid
			{
				IsParsing = true,
				// Source 0\MainPage.xaml (Line 8:3)
				Children = 
				{
					new global::Microsoft.UI.Xaml.Controls.TextBlock
					{
						IsParsing = true,
						Text = "Hello, world!",
						Margin = new global::Microsoft.UI.Xaml.Thickness(20),
						FontSize = 30d,
						// Source 0\MainPage.xaml (Line 9:4)
					}
					.GenericApply(__that, __nameScope, ((c0, __that, __nameScope) => 
					{
						global::Uno.UI.FrameworkElementHelper.SetBaseUri(c0, __baseUri_MainPage_d6cd66944958ced0c513e0a04797b51d, "file:///C:/Project/0/MainPage.xaml", 9, 4);
						c0.CreationComplete();
					}
					))
					,
				}
			}
			.GenericApply(__that, __nameScope, ((c1, __that, __nameScope) => 
			{
				global::Uno.UI.Toolkit.VisibleBoundsPadding.SetPaddingMask(c1, global::Uno.UI.Toolkit.VisibleBoundsPadding.PaddingMask.Top);
				global::Uno.UI.FrameworkElementHelper.SetBaseUri(c1, __baseUri_MainPage_d6cd66944958ced0c513e0a04797b51d, "file:///C:/Project/0/MainPage.xaml", 8, 3);
				c1.CreationComplete();
			}
			))
			;
			
			this
			.GenericApply(__that, __nameScope, ((c2, __that, __nameScope) => 
			{
				// Source 0\MainPage.xaml (Line 1:2)
				
				// WARNING Property c2.base does not exist on {http://schemas.microsoft.com/winfx/2006/xaml/presentation}Page, the namespace is http://www.w3.org/XML/1998/namespace. This error was considered irrelevant by the XamlFileGenerator
			}
			))
			.GenericApply(__that, __nameScope, ((c3, __that, __nameScope) => 
			{
				// Class TestRepro.MainPage
				global::Uno.UI.FrameworkElementHelper.SetBaseUri(c3, __baseUri_MainPage_d6cd66944958ced0c513e0a04797b51d, "file:///C:/Project/0/MainPage.xaml", 1, 2);
				c3.CreationComplete();
			}
			))
			;
			OnInitializeCompleted();

		}
		partial void OnInitializeCompleted();
	}
}