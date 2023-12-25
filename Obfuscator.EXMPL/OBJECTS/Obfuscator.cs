using System.Text.RegularExpressions;
using Obfuscator.EXMPL.OBJECTS.LANGUAGES;

namespace Obfuscator.EXMPL.OBJECTS;

public class Obfuscator {
    public Obfuscator(Language language) {
        Language = language;

        var random = new Random();
        int newVarBaseLength = random.Next(40, 70);

        for(int i=0; i<newVarBaseLength; i++)
        {
            if (i == 0)
                newVarBase += chars[random.Next(11, chars.Length)];
            newVarBase += chars[random.Next(0, chars.Length)];
        }
    }
    
    private Language Language { get; set; }
    
    string chars = "_1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string newVarBase="";
    List<string> usedVars = new List<string>();
    
    public string Obfuscate(string source) {
        source = Regex.Replace(source, @"/\*(.*?)\*/", ""); //remove comments
        source = Regex.Replace(source, @"//(.*?)\r?\n", ""); //remove comments
        source = Regex.Replace(source, @"\s+", " "); //remove breaklines

        List<string> classes = new List<string>();

        MatchCollection classMatchColecction = Regex.Matches(source, @"\w*\w*class\s([_|\w]+)"); //get classes

        foreach (Match colItem in classMatchColecction) {
            classes.Add(colItem.Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last());
        }

        for(int i=0; i<classes.Count;i++)
        {
            string newVar = getNewVarName(i);
            source = source.Replace($"class {classes[i]}", $"class {newVar}");
            usedVars.Add(newVar);
        }

        List<string> functions = new List<string>();
             
        MatchCollection functionMatchCollection = Regex.Matches(source, @"[{};]\s?[\w_\[\]\<\>\s,]{4,}\s[\w_\[\]\<\>\s,]+\([\w_\[\]\<\>\s,]+\)"); //get functions

        foreach (Match colItem in functionMatchCollection) {
            functions.Add(colItem.Value);               
        }

        MatchCollection varCollection = Regex.Matches(source, "\\s\\w+((\\s?=\\s?(\\w+|\"))|(\\sin\\s))"); //vars from function body
        string[] variableCollection = new string[varCollection.Count];
        for (int i = 0; i < varCollection.Count; i++)
            variableCollection[i] = varCollection[i].Value;

        List<string> variables = new List<string>();

        foreach (string func in functions)  {
            string[] funcVariables = func.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < funcVariables.Length; i++) {
                string variable = funcVariables[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last();
                if (!variables.Contains(variable) && !variable.Contains("<") && !variable.Contains(">"))
                    variables.Add(variable);
            }
        }

        foreach (string item in variableCollection) {
            string variable = item.Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries)[0];
            if (!variables.Contains(variable))
                variables.Add(variable); 
        }
        
        string drain = source;

        for(int i=0; i<variables.Count; i++) {
            string newVar = getNewVarName(i);
            Match match = null;

            do
            {
                match = Regex.Match(drain, $"[\\s\\(\\[\\)\\-\\+\\*\\/\\%\\=\\!\\>\\<\\.\\,{{]{variables[i]}[\\s\\)\\[\\]\\-\\+\\*\\/\\%\\.\\,\\=\\!\\<\\>}};]");
                if (match.Value != "")
                {
                    
                    drain = drain.Replace(match.Value, match.Value.Replace(variables[i], newVar));
                    usedVars.Add(newVar);
                }
            }
            while (match.Value!="");
        }
        
        return drain;
    }

    private string getNewVarName(int index)
    {
        Random random = new Random(DateTime.Now.Millisecond/(index+1));
        string insertion = "";
        for (int i = 0; i < random.Next(10, 20); i++)
            insertion += chars[random.Next(0, chars.Length)];

        string output = newVarBase.Insert(random.Next(15, newVarBase.Length - 10), insertion);
        if (!usedVars.Contains(output))
            return output;
        else
            return getNewVarName(index * 100);
    }
}