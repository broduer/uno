﻿using System.Text;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Uno.UI.SourceGenerators.Tests.Verifiers;

namespace Uno.UI.SourceGenerators.Tests.Windows_UI_Xaml_Data.BindingTests;

using Verify = XamlSourceGeneratorVerifier;

[TestClass]
public class Given_Binding
{
	[TestMethod]
	public async Task When_Xaml_Object_With_Common_Properties()
	{
		var test = new TestSetup(xamlFileName: "Binding_Xaml_Object_With_Common_Properties.xaml", subFolder: Path.Combine("Uno.UI.Tests", "Windows_UI_Xaml_Data", "BindingTests", "Controls"));
		await Verify.AssertXamlGeneratorDiagnostics(test);
	}

	[TestMethod]
	public async Task When_Xaml_Object_With_Xaml_Object_Properties()
	{
		var test = new TestSetup(xamlFileName: "Binding_Xaml_Object_With_Xaml_Object_Properties.xaml", subFolder: Path.Combine("Uno.UI.Tests", "Windows_UI_Xaml_Data", "BindingTests", "Controls"));
		await Verify.AssertXamlGeneratorDiagnostics(test);
	}

	[TestMethod]
	public async Task When_Binding_ElementName_In_Template()
	{
		var test = new TestSetup(xamlFileName: "Binding_ElementName_In_Template.xaml", subFolder: Path.Combine("Uno.UI.Tests", "Windows_UI_Xaml_Data", "BindingTests", "Controls"))
		{
			PreprocessorSymbols =
			{
				"UNO_REFERENCE_API",
			},
		};
		await Verify.AssertXamlGeneratorDiagnostics(test);
	}

	[TestMethod]
	public async Task TestBaseTypeNotSpecifiedInCodeBehind()
	{
		var xamlFiles = new[]
		{
			new XamlFile("UserControl1.xaml", """
	<UserControl
		x:Class="TestRepro.UserControl1"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:local="using:TestRepro"
		xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		mc:Ignorable="d"
		d:DesignHeight="300"
		d:DesignWidth="400">

		<Grid></Grid>
	</UserControl>
	"""),
			new XamlFile("MainPage.xaml", """
	<Page
		x:Class="TestRepro.MainPage"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:local="using:TestRepro"
		xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		mc:Ignorable="d"
		Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">

		<Grid>
			<TextBlock Text="Hello, world!" Margin="20" FontSize="30" />
			<local:UserControl1 DataContext="{Binding PreviewDropViewModel}"/>
		</Grid>
	</Page>
	"""),
		};
		var test = new Verify.Test(xamlFiles)
		{
			TestState =
			{
				Sources =
				{
					"""
					namespace TestRepro
					{
						public sealed partial class UserControl1
						{
							public UserControl1()
							{
								this.InitializeComponent();
							}
						}
					}
					""",
					"""
					using Windows.UI.Xaml.Controls;

					namespace TestRepro
					{
						public sealed partial class MainPage : Page
						{
							public string PreviewDropViewModel { get; set; }

							public MainPage()
							{
								this.InitializeComponent();
							}
						}
					}
					"""
				}
			}
		};
		test.ExpectedDiagnostics.Add(
			// /0/Test0.cs(3,30): warning UXAML0002: TestRepro.UserControl1 does not explicitly define the Windows.UI.Xaml.Controls.UserControl base type in code behind.
			DiagnosticResult.CompilerWarning("UXAML0002").WithSpan(3, 30, 3, 42).WithArguments("TestRepro.UserControl1 does not explicitly define the Windows.UI.Xaml.Controls.UserControl base type in code behind.")
		);
		await test.RunAsync();
	}

	[TestMethod]
	public async Task TextXBindToStatic()
	{
		var xamlFiles = new[]
		{
			new XamlFile("MainPage.xaml", """
	<Page
		x:Class="TestRepro.MainPage"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:local="using:TestRepro"
		xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		mc:Ignorable="d"
		Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">

		<Grid>
			<TextBlock Text="{x:Bind local:App.Class.SubClass1.SubClass2.SubClass3.Message}" />
		</Grid>
	</Page>
	"""),
		};
		var test = new Verify.Test(xamlFiles)
		{
			TestBehaviors = TestBehaviors.None,
			TestState =
			{
				Sources =
				{
					"""
					namespace TestRepro
					{
						public sealed partial class App
						{
							internal static readonly Class Class;

							static App()
							{
								Class = new Class()
								{
									SubClass1 = new SubClass1()
									{
										SubClass2 = new SubClass2()
										{
											SubClass3 = new SubClass3()
											{
												Message = "Hello world!"
											}
										}
									}
								};
							}
						}
					}
					""",
					"""
					using Windows.UI.Xaml.Controls;

					namespace TestRepro
					{
						public sealed partial class MainPage : Page
						{
							public MainPage()
							{
								this.InitializeComponent();
							}
						}

						internal class Class
						{
							public SubClass1 SubClass1 { get;set;}
						}

						internal class SubClass1
						{
							public SubClass2 SubClass2 { get; set; }
						}

						internal class SubClass2
						{
							public SubClass3 SubClass3 { get; set; }
						}

						internal class SubClass3
						{
							public string Message { get; set; }
						}
					}
					"""
				},
				GeneratedSources =
				{
					("Uno.UI.SourceGenerators\\Uno.UI.SourceGenerators.XamlGenerator.XamlCodeGenerator\\LocalizationResources.cs", SourceText.From("[assembly: global::System.Reflection.AssemblyMetadata(\"UnoHasLocalizationResources\", \"False\")]", Encoding.UTF8)),
					("Uno.UI.SourceGenerators\\Uno.UI.SourceGenerators.XamlGenerator.XamlCodeGenerator\\MainPage_600e5db99cb0a5c835d2c4e1905b1bab.cs", SourceText.From("""
	// <autogenerated />
	#pragma warning disable CS0114
	#pragma warning disable CS0108
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Uno.UI;
	using Uno.UI.Xaml;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Controls.Primitives;
	using Windows.UI.Xaml.Data;
	using Windows.UI.Xaml.Documents;
	using Windows.UI.Xaml.Media;
	using Windows.UI.Xaml.Media.Animation;
	using Windows.UI.Xaml.Shapes;
	using Windows.UI.Text;
	using Uno.Extensions;
	using Uno;
	using Uno.UI.Helpers.Xaml;
	using MyProject;
	
	#if __ANDROID__
	using _View = Android.Views.View;
	#elif __IOS__
	using _View = UIKit.UIView;
	#elif __MACOS__
	using _View = AppKit.NSView;
	#elif UNO_REFERENCE_API || NET461
	using _View = Windows.UI.Xaml.UIElement;
	#endif
	
	namespace TestRepro
	{
		partial class MainPage : Windows.UI.Xaml.Controls.Page
		{
			[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
			private const string __baseUri_prefix_MainPage_600e5db99cb0a5c835d2c4e1905b1bab = "ms-appx:///TestProject/";
			[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
			private const string __baseUri_MainPage_600e5db99cb0a5c835d2c4e1905b1bab = "ms-appx:///TestProject/";
					global::Windows.UI.Xaml.NameScope __nameScope = new global::Windows.UI.Xaml.NameScope();
			private void InitializeComponent()
			{
				InitializeComponent_0();
			}
			private void InitializeComponent_0()
			{
				NameScope.SetNameScope(this, __nameScope);
				var __that = this;
				base.IsParsing = true;
				// Source ..\0\MainPage.xaml (Line 1:2)
				base.Content = 
				new global::Windows.UI.Xaml.Controls.Grid
				{
					IsParsing = true,
					// Source ..\0\MainPage.xaml (Line 11:3)
					Children = 
					{
						new global::Windows.UI.Xaml.Controls.TextBlock
						{
							IsParsing = true,
							// Source ..\0\MainPage.xaml (Line 12:4)
						}
						.MainPage_600e5db99cb0a5c835d2c4e1905b1bab_XamlApply((MainPage_600e5db99cb0a5c835d2c4e1905b1babXamlApplyExtensions.XamlApplyHandler0)(c0 => 
						{
							/* _isTopLevelDictionary:False */
							__that._component_0 = c0;
							c0.SetBinding(
								global::Windows.UI.Xaml.Controls.TextBlock.TextProperty,
								new Windows.UI.Xaml.Data.Binding()
								{
									Mode = BindingMode.OneTime,
								}
									.BindingApply(___b =>  /*defaultBindModeOneTime global::TestRepro.App.Class.SubClass1.SubClass2.SubClass3.Message*/ global::Uno.UI.Xaml.BindingHelper.SetBindingXBindProvider(___b, __that, ___ctx => ___ctx is global::TestRepro.MainPage ___tctx ? (object)(global::TestRepro.App.Class.SubClass1.SubClass2.SubClass3.Message) : null, null ))
							);
							global::Uno.UI.FrameworkElementHelper.SetBaseUri(c0, __baseUri_MainPage_600e5db99cb0a5c835d2c4e1905b1bab);
							c0.CreationComplete();
						}
						))
						,
					}
				}
				.MainPage_600e5db99cb0a5c835d2c4e1905b1bab_XamlApply((MainPage_600e5db99cb0a5c835d2c4e1905b1babXamlApplyExtensions.XamlApplyHandler1)(c1 => 
				{
					global::Uno.UI.FrameworkElementHelper.SetBaseUri(c1, __baseUri_MainPage_600e5db99cb0a5c835d2c4e1905b1bab);
					c1.CreationComplete();
				}
				))
				;
				
				this
				.GenericApply(((c2) => 
				{
					// Source /0/MainPage.xaml (Line 1:2)
					
					// WARNING Property c2.base does not exist on {http://schemas.microsoft.com/winfx/2006/xaml/presentation}Page, the namespace is http://www.w3.org/XML/1998/namespace. This error was considered irrelevant by the XamlFileGenerator
				}
				))
				.GenericApply(((c3) => 
				{
					/* _isTopLevelDictionary:False */
					__that._component_1 = c3;
					// Class TestRepro.MainPage
					global::Uno.UI.ResourceResolverSingleton.Instance.ApplyResource(c3, global::Windows.UI.Xaml.Controls.Page.BackgroundProperty, "ApplicationPageBackgroundThemeBrush", isThemeResourceExtension: true, isHotReloadSupported: false, context: global::MyProject.GlobalStaticResources.__ParseContext_);
					global::Uno.UI.FrameworkElementHelper.SetBaseUri(c3, __baseUri_MainPage_600e5db99cb0a5c835d2c4e1905b1bab);
					c3.CreationComplete();
				}
				))
				;
				OnInitializeCompleted();
	
				Bindings = new MainPage_Bindings(this);
				Loading += (s, e) => 
				{
					__that.Bindings.Update();
					__that.Bindings.UpdateResources();
				}
				;
			}
			partial void OnInitializeCompleted();
			private global::Windows.UI.Xaml.Markup.ComponentHolder _component_0_Holder  = new global::Windows.UI.Xaml.Markup.ComponentHolder(isWeak: true);
			private global::Windows.UI.Xaml.Controls.TextBlock _component_0
			{
				get
				{
					return (global::Windows.UI.Xaml.Controls.TextBlock)_component_0_Holder.Instance;
				}
				set
				{
					_component_0_Holder.Instance = value;
				}
			}
			private global::Windows.UI.Xaml.Markup.ComponentHolder _component_1_Holder  = new global::Windows.UI.Xaml.Markup.ComponentHolder(isWeak: true);
			private global::Windows.UI.Xaml.Controls.Page _component_1
			{
				get
				{
					return (global::Windows.UI.Xaml.Controls.Page)_component_1_Holder.Instance;
				}
				set
				{
					_component_1_Holder.Instance = value;
				}
			}
			private interface IMainPage_Bindings
			{
				void Initialize();
				void Update();
				void UpdateResources();
				void StopTracking();
			}
			#pragma warning disable 0169 //  Suppress unused field warning in case Bindings is not used.
			private IMainPage_Bindings Bindings;
			#pragma warning restore 0169
			[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
			private class MainPage_Bindings : IMainPage_Bindings
			{
				#if UNO_HAS_UIELEMENT_IMPLICIT_PINNING
				private global::System.WeakReference _ownerReference;
				private global::TestRepro.MainPage Owner { get => (global::TestRepro.MainPage)_ownerReference?.Target; set => _ownerReference = new global::System.WeakReference(value); }
				#else
				private global::TestRepro.MainPage Owner { get; set; }
				#endif
				public MainPage_Bindings(global::TestRepro.MainPage owner)
				{
					Owner = owner;
				}
				void IMainPage_Bindings.Initialize()
				{
				}
				void IMainPage_Bindings.Update()
				{
					var owner = Owner;
					owner._component_0.ApplyXBind();
				}
				void IMainPage_Bindings.UpdateResources()
				{
					var owner = Owner;
					owner._component_0.UpdateResourceBindings();
					owner._component_1.UpdateResourceBindings();
				}
				void IMainPage_Bindings.StopTracking()
				{
				}
			}
	
		}
	}
	namespace MyProject
	{
		static class MainPage_600e5db99cb0a5c835d2c4e1905b1babXamlApplyExtensions
		{
			public delegate void XamlApplyHandler0(global::Windows.UI.Xaml.Controls.TextBlock instance);
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
			public static global::Windows.UI.Xaml.Controls.TextBlock MainPage_600e5db99cb0a5c835d2c4e1905b1bab_XamlApply(this global::Windows.UI.Xaml.Controls.TextBlock instance, XamlApplyHandler0 handler)
			{
				handler(instance);
				return instance;
			}
			public delegate void XamlApplyHandler1(global::Windows.UI.Xaml.Controls.Grid instance);
			[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
			public static global::Windows.UI.Xaml.Controls.Grid MainPage_600e5db99cb0a5c835d2c4e1905b1bab_XamlApply(this global::Windows.UI.Xaml.Controls.Grid instance, XamlApplyHandler1 handler)
			{
				handler(instance);
				return instance;
			}
		}
	}
	
	""", Encoding.UTF8)),
					("Uno.UI.SourceGenerators\\Uno.UI.SourceGenerators.XamlGenerator.XamlCodeGenerator\\GlobalStaticResources.cs", SourceText.From("""
	// <autogenerated />
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using Uno.Extensions;
	using Uno;
	using System.Diagnostics;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Data;
	using Windows.UI.Xaml.Documents;
	using Windows.UI.Xaml.Media;
	using Windows.UI.Xaml.Media.Animation;
	using MyProject;

	#if __WASM__
	#error invalid internal source generator state. The __WASM__ DefineConstant was not propagated properly.
	#endif
	namespace MyProject
	{
		/// <summary>
		/// Contains all the static resources defined for the application
		/// </summary>
		public sealed partial class GlobalStaticResources
		{
			static bool _initialized;
			private static bool _stylesRegistered;
			private static bool _dictionariesRegistered;
			internal static global::Uno.UI.Xaml.XamlParseContext __ParseContext_ {get; } = new global::Uno.UI.Xaml.XamlParseContext()
			{
				AssemblyName = "TestProject",
			}
			;

			static GlobalStaticResources()
			{
				Initialize();
			}
			public static void Initialize()
			{
				if (!_initialized)
				{
					_initialized = true;
					global::Uno.UI.GlobalStaticResources.Initialize();
					global::Uno.UI.GlobalStaticResources.RegisterDefaultStyles();
					global::Uno.UI.GlobalStaticResources.RegisterResourceDictionariesBySource();
				}
			}
			public static void RegisterDefaultStyles()
			{
				if(!_stylesRegistered)
				{
					_stylesRegistered = true;
					RegisterDefaultStyles_MainPage_600e5db99cb0a5c835d2c4e1905b1bab();
				}
			}
			// Register ResourceDictionaries using ms-appx:/// syntax, this is called for external resources
			public static void RegisterResourceDictionariesBySource()
			{
				if(!_dictionariesRegistered)
				{
					_dictionariesRegistered = true;
				}
			}
			// Register ResourceDictionaries using ms-resource:/// syntax, this is called for local resources
			internal static void RegisterResourceDictionariesBySourceLocal()
			{
			}
			static partial void RegisterDefaultStyles_MainPage_600e5db99cb0a5c835d2c4e1905b1bab();
			[global::System.Obsolete("This method is provided for binary backward compatibility. It will always return null.")]
			[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
			public static object FindResource(string name) => null;
			
		}
	}
	
	""", Encoding.UTF8)),
				}
			}
		};

		await test.RunAsync();
	}
}
