#if TOOLS
using Godot;
using System;

[Tool]
public partial class CssgoConsolePlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
        AddAutoloadSingleton("CssgoConsole", "res://addons/cssgo_console/CssgoConsole.cs");
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
	}
}
#endif
