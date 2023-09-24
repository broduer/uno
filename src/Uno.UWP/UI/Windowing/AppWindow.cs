﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI;

#if HAS_UNO_WINUI
namespace Microsoft.UI.Windowing;
#else
namespace Windows.UI.WindowManagement;
#endif

public partial class AppWindow
{
	internal AppWindow()
	{
		WindowId = new(Interlocked.Increment(ref _windowIdIterator));
		_appWindowIdMap[WindowId] = this;
	}
}