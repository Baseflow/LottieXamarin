using System;
using ObjCRuntime;

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
