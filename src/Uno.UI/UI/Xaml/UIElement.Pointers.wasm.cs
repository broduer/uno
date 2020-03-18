﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Uno.Foundation;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.System;
using Windows.UI.Input;
using Uno.UI;
using Uno.UI.Xaml;

namespace Windows.UI.Xaml
{
	public partial class UIElement : DependencyObject
	{
		// Ref:
		// https://www.w3.org/TR/pointerevents/
		// https://developer.mozilla.org/en-US/docs/Web/API/PointerEvent

		private static readonly Dictionary<RoutedEvent, (string domEventName, EventArgsParser argsParser, RoutedEventHandlerWithHandled handler)> _pointerHandlers
			= new Dictionary<RoutedEvent, (string, EventArgsParser, RoutedEventHandlerWithHandled)>
			{
				// Note: we use 'pointerenter' and 'pointerleave' which are not bubbling natively
				//		 as on UWP, even if the event are RoutedEvents, PointerEntered and PointerExited
				//		 are routed only in some particular cases (entering at once on multiple controls),
				//		 it's easier to handle this in managed code.
				{PointerEnteredEvent, ("pointerenter", PayloadToEnteredPointerArgs, (snd, args) => ((UIElement)snd).OnNativePointerEnter((PointerRoutedEventArgs)args))},
				{PointerExitedEvent, ("pointerleave", PayloadToExitedPointerArgs, (snd, args) => ((UIElement)snd).OnNativePointerExited((PointerRoutedEventArgs)args))},
				{PointerPressedEvent, ("pointerdown", PayloadToPressedPointerArgs, (snd, args) => ((UIElement)snd).OnNativePointerDown((PointerRoutedEventArgs)args))},
				{PointerReleasedEvent, ("pointerup", PayloadToReleasedPointerArgs, (snd, args) => ((UIElement)snd).OnNativePointerUp((PointerRoutedEventArgs)args))},

				{PointerMovedEvent, ("pointermove", PayloadToMovedPointerArgs, (snd, args) => ((UIElement)snd).OnNativePointerMove((PointerRoutedEventArgs)args))},
				{PointerCanceledEvent, ("pointercancel", PayloadToCancelledPointerArgs, (snd, args) => ((UIElement)snd).OnNativePointerCancel((PointerRoutedEventArgs)args, isSwallowedBySystem: true))}, //https://www.w3.org/TR/pointerevents/#the-pointercancel-event
			};

		partial void OnGestureRecognizerInitialized(GestureRecognizer recognizer)
		{
			// When a gesture recognizer is initialized, we subscribe to pointer events in order to feed it.
			// Note: We subscribe to * all * pointer events in order to maintain a logical internal state of pointers over / press / capture

			foreach (var pointerEvent in _pointerHandlers.Keys)
			{
				AddPointerHandlerCore(pointerEvent);
			}
		}

		partial void AddPointerHandler(RoutedEvent routedEvent, int handlersCount, object handler, bool handledEventsToo)
		{
			if (handlersCount != 1 || _registeredRoutedEvents.HasFlag(routedEvent.Flag))
			{
				return;
			}

			// In order to ensure valid pressed and over state, we ** must ** subscribe to all the related events
			// before subscribing to other pointer events.
			if (!_registeredRoutedEvents.HasFlag(RoutedEventFlag.PointerEntered))
			{
				AddPointerHandlerCore(PointerEnteredEvent);
				AddPointerHandlerCore(PointerExitedEvent);
				AddPointerHandlerCore(PointerPressedEvent);
				AddPointerHandlerCore(PointerReleasedEvent);
			}

			AddPointerHandlerCore(routedEvent);
		}

		private void AddPointerHandlerCore(RoutedEvent routedEvent)
		{
			if (_registeredRoutedEvents.HasFlag(routedEvent.Flag))
			{
				return;
			}

			if (routedEvent == PointerCaptureLostEvent)
			{
				// Captures are handled in managed code only
				_registeredRoutedEvents |= routedEvent.Flag;
				return;
			}

			if (!_pointerHandlers.TryGetValue(routedEvent, out var evt))
			{
				Application.Current.RaiseRecoverableUnhandledException(new NotImplementedException($"Pointer event {routedEvent.Name} is not supported on this platform"));
				return;
			}

			_registeredRoutedEvents |= routedEvent.Flag;

			RegisterEventHandler(
				evt.domEventName,
				handler: evt.handler,
				onCapturePhase: false,
				canBubbleNatively: true,
				eventFilter: HtmlEventFilter.Default,
				eventExtractor: HtmlEventExtractor.PointerEventExtractor,
				payloadConverter: evt.argsParser
			);
		}

		private static PointerRoutedEventArgs PayloadToEnteredPointerArgs(object snd, string payload) => PayloadToPointerArgs(snd, payload, isInContact: false, canBubble: false);
		private static PointerRoutedEventArgs PayloadToPressedPointerArgs(object snd, string payload) => PayloadToPointerArgs(snd, payload, isInContact: true);
		private static PointerRoutedEventArgs PayloadToMovedPointerArgs(object snd, string payload) => PayloadToPointerArgs(snd, payload, isInContact: true);
		private static PointerRoutedEventArgs PayloadToReleasedPointerArgs(object snd, string payload) => PayloadToPointerArgs(snd, payload, isInContact: true);
		private static PointerRoutedEventArgs PayloadToExitedPointerArgs(object snd, string payload) => PayloadToPointerArgs(snd, payload, isInContact: false, canBubble: false);
		private static PointerRoutedEventArgs PayloadToCancelledPointerArgs(object snd, string payload) => PayloadToPointerArgs(snd, payload, isInContact: false);

		private static PointerRoutedEventArgs PayloadToPointerArgs(object snd, string payload, bool isInContact, bool canBubble = true)
		{
			var parts = payload?.Split(';');
			if (parts?.Length != 11)
			{
				return null;
			}

			var pointerId = uint.Parse(parts[0], CultureInfo.InvariantCulture);
			var x = double.Parse(parts[1], CultureInfo.InvariantCulture);
			var y = double.Parse(parts[2], CultureInfo.InvariantCulture);
			var ctrl = parts[3] == "1";
			var shift = parts[4] == "1";
			var buttons = int.Parse(parts[5], CultureInfo.InvariantCulture);
			var buttonUpdate = int.Parse(parts[6], CultureInfo.InvariantCulture);
			var typeStr = parts[7];
			var srcHandle = int.Parse(parts[8], CultureInfo.InvariantCulture);
			var timestamp = double.Parse(parts[9], CultureInfo.InvariantCulture);
			var pressure = double.Parse(parts[10], CultureInfo.InvariantCulture);

			var src = GetElementFromHandle(srcHandle) ?? (UIElement)snd;
			var position = new Point(x, y);
			var pointerType = ConvertPointerTypeString(typeStr);
			var keyModifiers = VirtualKeyModifiers.None;
			if (ctrl) keyModifiers |= VirtualKeyModifiers.Control;
			if (shift) keyModifiers |= VirtualKeyModifiers.Shift;

			return new PointerRoutedEventArgs(
				timestamp,
				pointerId,
				pointerType,
				position,
				isInContact,
				(WindowManagerInterop.HtmlPointerButtonsState)buttons,
				(WindowManagerInterop.HtmlPointerButtonUpdate)buttonUpdate,
				keyModifiers,
				pressure,
				src,
				canBubble);
		}

		private static PointerDeviceType ConvertPointerTypeString(string typeStr)
		{
			PointerDeviceType type;
			switch (typeStr.ToUpper())
			{
				case "MOUSE":
				default:
					type = PointerDeviceType.Mouse;
					break;
				// Note: As of 2019-11-28, once pen pressed events pressed/move/released are reported as TOUCH on Firefox
				//		 https://bugzilla.mozilla.org/show_bug.cgi?id=1449660
				case "PEN":
					type = PointerDeviceType.Pen;
					break;
				case "TOUCH":
					type = PointerDeviceType.Touch;
					break;
			}

			return type;
		}

		#region Capture
		partial void OnManipulationModeChanged(ManipulationModes _, ManipulationModes newMode)
			=> SetStyle("touch-action", newMode == ManipulationModes.None ? "none" : "auto");

		partial void CapturePointerNative(Pointer pointer)
		{
			var command = "Uno.UI.WindowManager.current.setPointerCapture(" + HtmlId + ", " + pointer.PointerId + ");";
			WebAssemblyRuntime.InvokeJS(command);

			if (pointer.PointerDeviceType != PointerDeviceType.Mouse)
			{
				SetStyle("touch-action", "none");
			}
		}

		partial void ReleasePointerNative(Pointer pointer)
		{
			var command = "Uno.UI.WindowManager.current.releasePointerCapture(" + HtmlId + ", " + pointer.PointerId + ");";
			WebAssemblyRuntime.InvokeJS(command);

			if (pointer.PointerDeviceType != PointerDeviceType.Mouse && ManipulationMode != ManipulationModes.None)
			{
				SetStyle("touch-action", "auto");
			}
		}
		#endregion

		#region HitTestVisibility
		internal void UpdateHitTest()
		{
			this.CoerceValue(HitTestVisibilityProperty);
		}

		private protected enum HitTestVisibility
		{
			/// <summary>
			/// The element and its children can't be targeted by hit-testing.
			/// </summary>
			/// <remarks>
			/// This occurs when IsHitTestVisible="False", IsEnabled="False", or Visibility="Collapsed".
			/// </remarks>
			Collapsed,

			/// <summary>
			/// The element can't be targeted by hit-testing.
			/// </summary>
			/// <remarks>
			/// This usually occurs if an element doesn't have a Background/Fill.
			/// </remarks>
			Invisible,

			/// <summary>
			/// The element can be targeted by hit-testing.
			/// </summary>
			Visible,
		}

		/// <summary>
		/// Represents the final calculated hit-test visibility of the element.
		/// </summary>
		/// <remarks>
		/// This property should never be directly set, and its value should always be calculated through coercion (see <see cref="CoerceHitTestVisibility(DependencyObject, object, bool)"/>.
		/// </remarks>
		private static readonly DependencyProperty HitTestVisibilityProperty =
			DependencyProperty.Register(
				"HitTestVisibility",
				typeof(HitTestVisibility),
				typeof(UIElement),
				new FrameworkPropertyMetadata(
					HitTestVisibility.Visible,
					FrameworkPropertyMetadataOptions.Inherits,
					coerceValueCallback: (s, e) => CoerceHitTestVisibility(s, e),
					propertyChangedCallback: (s, e) => OnHitTestVisibilityChanged(s, e)
				)
			);

		/// <summary>
		/// This calculates the final hit-test visibility of an element.
		/// </summary>
		/// <returns></returns>
		private static object CoerceHitTestVisibility(DependencyObject dependencyObject, object baseValue)
		{
			var element = (UIElement)dependencyObject;

			// The HitTestVisibilityProperty is never set directly. This means that baseValue is always the result of the parent's CoerceHitTestVisibility.
			var baseHitTestVisibility = (HitTestVisibility)baseValue;

			// If the parent is collapsed, we should be collapsed as well. This takes priority over everything else, even if we would be visible otherwise.
			if (baseHitTestVisibility == HitTestVisibility.Collapsed)
			{
				return HitTestVisibility.Collapsed;
			}

			// If we're not locally hit-test visible, visible, or enabled, we should be collapsed. Our children will be collapsed as well.
			if (!element.IsHitTestVisible || element.Visibility != Visibility.Visible || !element.IsEnabledOverride())
			{
				return HitTestVisibility.Collapsed;
			}

			// If we're not hit (usually means we don't have a Background/Fill), we're invisible. Our children will be visible or not, depending on their state.
			if (!element.IsViewHit())
			{
				return HitTestVisibility.Invisible;
			}

			// If we're not collapsed or invisible, we can be targeted by hit-testing. This means that we can be the source of pointer events.
			return HitTestVisibility.Visible;
		}

		private static void OnHitTestVisibilityChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			if (dependencyObject is UIElement element
				&& args.OldValue is HitTestVisibility oldValue
				&& args.NewValue is HitTestVisibility newValue)
			{
				element.OnHitTestVisibilityChanged(oldValue, newValue);
			}
		}

		private protected virtual void OnHitTestVisibilityChanged(HitTestVisibility oldValue, HitTestVisibility newValue)
		{
			if (newValue == HitTestVisibility.Visible)
			{
				// By default, elements have 'pointer-event' set to 'auto' (see Uno.UI.css .uno-uielement class).
				// This means that they can be the target of hit-testing and will raise pointer events when interacted with.
				// This is aligned with HitTestVisibilityProperty's default value of Visible.
				SetStyle("pointer-events", "auto");
			}
			else
			{
				// If HitTestVisibilityProperty is calculated to Invisible or Collapsed,
				// we don't want to be the target of hit-testing and raise any pointer events.
				// This is done by setting 'pointer-events' to 'none'.
				SetStyle("pointer-events", "none");
			}

			if (FeatureConfiguration.UIElement.AssignDOMXamlProperties)
			{
				UpdateDOMProperties();
			}
		}
		#endregion
	}
}
