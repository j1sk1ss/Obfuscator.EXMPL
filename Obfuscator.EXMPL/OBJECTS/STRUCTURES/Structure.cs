using System.Text.RegularExpressions;

namespace Obfuscator.EXMPL.OBJECTS.STRUCTURES;

public abstract class Structure {
    protected Structure(bool isConstant) {
        IsConstant = isConstant;
        Type       = "type";
        Body       = new List<string>();
        Name       = "";
    }

    protected bool IsConstant { get; init; }
    public List<string> Body { get; protected init; }
    public string Name { get; set; }
    public string Type { get; set; }

    public void ReplaceVariables(List<string> directives) {
        if (IsConstant) return;

        var history = new Dictionary<string, string>();
        for (var i = 0; i < Body.Count; i++) {
            var prepared = Body[i].Replace("(", " ");
            prepared = prepared.Replace(")", " ");
            prepared = prepared.Replace("[", " ");
            prepared = prepared.Replace("]", " ");
            
            var words = prepared.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (var j = 0; j < words.Length - 1; j++) {
                if (directives.Any(x => x == words[j])) {
                    if (history.ContainsKey(words[j + 1])) continue;

                    var randomWord = RandomString(2 + new Random().Next() % 25);
                    var cleanWord = new string(words[j + 1].Where(char.IsLetterOrDigit).ToArray());
                    history.Add(cleanWord, randomWord);
                }
            }
            
            var endWords = Body[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (var j = 0; j < endWords.Length; j++) {
                history.Keys.ToList().ForEach(x => {
                        if (endWords[j].Contains(x)) 
                            endWords[j] = endWords[j].Replace(x, history[x]);
                    }
                );
            }

            Body[i] = string.Join(" ", endWords.ToList());
        }
    }

    public void DeleteStructure() {
        for (var i = 0; i < Body.Count; i++) {
            Body[i] = Body[i].Replace("\n", "").Replace("\r", "");
            Body[i] = Body[i].Replace("\t", "");
            Body[i] = Body[i].Replace("private", "");
        }

        Body.RemoveAll(x => x.Length <= 1);
        Type = Type.Replace("private", "");
    }

    public void DeleteComments() {
        for (var i = 0; i < Body.Count; i++) {
            Body[i] = Regex.Replace(Body[i], @"/\*(.*?)\*/", "");
            Body[i] = Regex.Replace(Body[i], @"//(.*?)\r?\n", "");
        }
    }
    
    public virtual string GetLine() => string.Join("", Body);
    
    private static string RandomString(int length) {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[new Random(DateTime.Now.Microsecond).Next(s.Length)]).ToArray());
    }

    public abstract Type GetType();
}