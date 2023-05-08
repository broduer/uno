#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Microsoft.Web.WebView2.Core
{
	#if __ANDROID__ || __IOS__ || IS_UNIT_TESTS || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
	[global::Uno.NotImplemented]
	#endif
	public  partial class CoreWebView2ControllerOptions 
	{
		#if __ANDROID__ || __IOS__ || IS_UNIT_TESTS || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "IS_UNIT_TESTS", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  string ProfileName
		{
			get
			{
				throw new global::System.NotImplementedException("The member string CoreWebView2ControllerOptions.ProfileName is not implemented. For more information, visit https://aka.platform.uno/notimplemented?m=string%20CoreWebView2ControllerOptions.ProfileName");
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.Web.WebView2.Core.CoreWebView2ControllerOptions", "string CoreWebView2ControllerOptions.ProfileName");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || IS_UNIT_TESTS || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "IS_UNIT_TESTS", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  bool IsInPrivateModeEnabled
		{
			get
			{
				throw new global::System.NotImplementedException("The member bool CoreWebView2ControllerOptions.IsInPrivateModeEnabled is not implemented. For more information, visit https://aka.platform.uno/notimplemented?m=bool%20CoreWebView2ControllerOptions.IsInPrivateModeEnabled");
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Microsoft.Web.WebView2.Core.CoreWebView2ControllerOptions", "bool CoreWebView2ControllerOptions.IsInPrivateModeEnabled");
			}
		}
		#endif
		// Forced skipping of method Microsoft.Web.WebView2.Core.CoreWebView2ControllerOptions.ProfileName.get
		// Forced skipping of method Microsoft.Web.WebView2.Core.CoreWebView2ControllerOptions.ProfileName.set
		// Forced skipping of method Microsoft.Web.WebView2.Core.CoreWebView2ControllerOptions.IsInPrivateModeEnabled.get
		// Forced skipping of method Microsoft.Web.WebView2.Core.CoreWebView2ControllerOptions.IsInPrivateModeEnabled.set
	}
}
