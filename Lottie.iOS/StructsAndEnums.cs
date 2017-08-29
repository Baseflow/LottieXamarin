using System;
using ObjCRuntime;

namespace Airbnb.Lottie
{
	[Native]
	public enum LOTViewContentMode : ulong
	{
		ScaleToFill,
		ScaleAspectFit,
		ScaleAspectFill,
		Redraw,
		Center,
		Top,
		Bottom,
		Left,
		Right,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}
}
