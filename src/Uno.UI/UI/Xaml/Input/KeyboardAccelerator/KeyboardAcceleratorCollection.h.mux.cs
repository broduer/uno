﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// MUX Reference dxaml\xcp\core\inc\CKeyboardAcceleratorCollection.h, 

using Uno.UI.Xaml;
using Uno.UI.Xaml.Core;

namespace Microsoft.UI.Xaml.Input;

internal class KeyboardAcceleratorCollection : DependencyObjectCollection<KeyboardAccelerator>
{
	private void Enter(DependencyObject pNamescopeOwner, EnterParams enterParams)
	{
		//TODO:MZ Implement this
		//base.Enter(pNamescopeOwner, enterParams);

		//if (enterParams.IsLive || enterParams.IsForKeyboardAccelerator)
		//{
		//	ContentRoot pContentRoot = VisualTree.GetContentRootForElement(this);
		//	if (pContentRoot != null)
		//	{
		//		pContentRoot.AddToLiveKeyboardAccelerators(this);
		//	}
		//}
	}

	private void Leave(DependencyObject pNamescopeOwner, LeaveParams leaveParams)
	{
		//TODO:MZ Implement this
		//base.Leave(pNamescopeOwner, leaveParams);
		//if (leaveParams.IsLive || leaveParams.IsForKeyboardAccelerator)
		//{
		//	ContentRoot pContentRoot = VisualTree.GetContentRootForElement(this);
		//	if (pContentRoot != null)
		//	{
		//		pContentRoot.RemoveFromLiveKeyboardAccelerators(this);
		//	}
		//}
	}
}
