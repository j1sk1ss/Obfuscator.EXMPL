namespace Obfuscator.EXMPL.OBJECTS.LANGUAGES.LANGS;

public class CSharp : Language {
    public CSharp(string path) {
        Path = path;
        Code = File.ReadAllText(Path).Split("\n").ToList();
    }
    
    private static readonly Dictionary<string, List<string>> DirectivesDictionary = new() {
        { "Class", new List<string> { "class" } },
        { "Import", new List<string> { "using", "namespace" } },
        {
            "ClassBrackets", new List<string> { "{", "}" }
        },
        { "Methods", new List<string> { "void", "string", "int", "double", "var", "class" } }
    };
        
    protected override Dictionary<string, List<string>> Directives() => DirectivesDictionary;
    
    public static Dictionary<string, List<string>> GetDirectivesDictionary() => DirectivesDictionary;
}