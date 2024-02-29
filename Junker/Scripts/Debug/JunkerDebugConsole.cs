using Godot;
using Junker.Tags;


public partial class JunkerDebugConsole : Control {
    public static JunkerDebugConsole Instance;
    [ExportCategory("Setup")]
    [Export]
    public LineEdit CommandLine;

    [Export]
    public Label Output;

    [Export]
    public PackedScene TrackerArrow;

    [ExportGroup("Commands")]
    [Export]
    public JunkerConsoleCommand[] Keys;

    public TagSystem Tags;

    [Signal]
    public delegate void OnCommandExecutedEventHandler(string CommandName, string[] arguments);

    public static bool VerboseMode => Instance.Tags["VerboseMode"] == 1f;

    public bool HasContext => nodeContext != null;
    public Node GetContext => nodeContext == null ? this : nodeContext;

    public new T GetNode<T>(NodePath path) where T : class {
        if (nodeContext != null) {
            return nodeContext.GetNode<T>(path);
        }

        return base.GetNode<T>(path);
    }

    bool ContainsKey(string key) {
        for (int i = 0; i < Keys.Length; i++) {
            if (Keys[i].CommandKey.ToUpper() == key) {
                return true;
            }
        }

        return false;
    }

    JunkerConsoleCommand GetCommandKey(string key) {
        for (int i = 0; i < Keys.Length; i++) {
            if (Keys[i].CommandKey.ToUpper() == key) {
                return Keys[i];
            }
        }
        return null;
    }

    Node nodeContext;

    public override void _Ready() {
        if (Instance != null) {
            QueueFree();
            return;
        }
        Instance = this;

        Tags = GetNode<TagSystem>("TagSystem");
        
        base._Ready();
    }

    public override void _Process(double delta) {
        base._Process(delta);

        if (Input.IsActionJustPressed("Console Toggle")) {
            Visible = !Visible;
        }
    }

    public void OnCommandEnter(string text) {
        SendString("> " + text);

        string[] commandArray = text.Split(' ');
        CommandLine.Clear();

        string command = commandArray[0].ToUpper();

        string[] args = new string[commandArray.Length - 1];
        for (int i = 1; i < commandArray.Length; i++) {
            args[i - 1] = commandArray[i];
        }

        if (command == "HELP") {
            RunHelp(args[0]);
            return;
        }

        if (!ContainsKey(command)) {
            SendString(">Invalid command!");
            return;
        }
        JunkerConsoleCommand commandNode = GetCommandKey(command);
        string output = commandNode.Execute(args);

        SendString(output);
    }

    public void SendString(object msg) => Output.Text += "\n" + msg.ToString();

    public void SendVerboseString(object msg) {
        if (!VerboseMode) {
            return;
        }

        Output.Text += "\n" + msg.ToString();
    }

    public void SetContext(Node node) => nodeContext = node;

    public void SetTracking2D(bool track) {
        if (!track && HasContext && nodeContext.FindChild("DEBUG TRACKER", owned: false) != null) {
            nodeContext.GetNode<Node2D>("DEBUG TRACKER").QueueFree();
        }

        if (track && HasContext && nodeContext.FindChild("DEBUG TRACKER", owned: false) == null) {
            Node2D tracker = (Node2D)TrackerArrow.Instantiate();
            tracker.Name = "DEBUG TRACKER";

            nodeContext.AddChild(tracker);
            tracker.Position = Vector2.Zero;
        }
    }

    void RunHelp(string key) {
        if (!ContainsKey(key)) {
            SendString($"Command \'{key}\' does not exist!");
            return;
        }


        JunkerConsoleCommand commandKey = GetCommandKey(key);

        if (string.IsNullOrEmpty(commandKey.HelpString)) {
            SendString("There is no help for you...");
            return;
        }

        SendString(commandKey.HelpString);
    }
}
