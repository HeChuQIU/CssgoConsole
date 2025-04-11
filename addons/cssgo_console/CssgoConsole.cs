namespace CssgoConsole;

using System;
using System.IO;
using Godot;

public partial class CssgoConsole : CanvasLayer
{
    private CssgoConsoleHost _host = new();
    private RichTextLabel _output;
    private CodeEdit _codeEdit;
    private Button _sendButton;
    private StringWriter _writer;

    public CssgoConsole()
    {
        Layer = 9999;
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Ready()
    {
        var console = GD.Load<PackedScene>("res://addons/cssgo_console/CssgoConsole.tscn").Instantiate();

        _output = console.GetNode<RichTextLabel>("%Output");
        _writer = new StringWriter();
        Console.SetOut(_writer);

        _codeEdit = console.GetNode<CodeEdit>("%CodeEdit");
        _codeEdit.Text =
            "(GetGlobal(\"_codeEdit\") as CodeEdit).AddThemeColorOverride(\"background_color\", new Color(0.1f, 0.2f, 0.5f));";
        _codeEdit.CodeCompletionEnabled = true;
        _codeEdit.CodeCompletionRequested += () =>
        {
            GD.Print("Code completion requested");
        };

        _sendButton = console.GetNode<Button>("%SendButton");
        _sendButton.Pressed += async () =>
        {
            dynamic vars = new System.Dynamic.ExpandoObject();
            vars.num = 1;
            var result = await _host.ExecuteAsync(_codeEdit.Text, _codeEdit);
            Console.WriteLine(result ?? "null");
        };

        AddChild(console);
    }

    public override void _Process(double delta)
    {
        _output.Text = _writer.ToString();
    }
}
