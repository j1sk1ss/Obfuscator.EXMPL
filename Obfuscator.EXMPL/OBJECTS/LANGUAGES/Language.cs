using Obfuscator.EXMPL.OBJECTS.STRUCTURES;
using Obfuscator.EXMPL.OBJECTS.STRUCTURES.STRUCTS;

namespace Obfuscator.EXMPL.OBJECTS.LANGUAGES;

public abstract class Language {
    protected Language() {
        Path = "path";
        Code = new List<string>();
    }

    protected string Path { get; init; }
    protected List<string> Code { get; init; }

    /// <summary>
    /// Contains directives of language
    /// </summary>
    /// <returns> list of directives </returns>
    protected abstract Dictionary<string, List<string>> Directives();

    public ImportStructure GetImports() {
        var imports = new ImportStructure(true);
        foreach (var line in Code.Where(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                     .Any(x => Directives()["Import"].Contains(x)))) 
            imports.Body.Add(line);

        return imports;
    }

    public unsafe List<ClassStructure> GetClasses() {
        var classes = new List<ClassStructure>();
        for (var i = 0; i < Code.Count; i++) {
            var codeWords = Code[i].Split(" ");
            if (codeWords.Any(word => word == Directives()["Class"][0])) 
                classes.Add(GetClass(&i));
        }
        
        return classes;
    }
    
    private unsafe ClassStructure GetClass(int* index) {
        var bracketStart   = 1;
        var classes        = new ClassStructure(false);

        var words = Code[(*index)++].Split(" ");
        for (var i = 0; i < words.Length; i++) 
            if (words[i] ==  Directives()["Class"][0]) {
                for (var j = 0; j < i + 1; j++) 
                    classes.Type += words[j] + " ";
                
                classes.Name = words[i + 1];
                break;
            }
        
        while (true) {
            if (*index >= Code.Count) return classes;
            
            var codeWords = Code[*index].Split(" ");
            if (codeWords.Any(word => word == Directives()["Class"][0])) classes.Children.Add(GetClass(&*index));
            bracketStart += codeWords.Count(x => x.Contains(Directives()["ClassBrackets"][0])) - 
                            codeWords.Count(x => x.Contains(Directives()["ClassBrackets"][1]));
            if (bracketStart <= 0) {
                (*index)++;
                return classes;
            }
            
            classes.Body.Add(Code[(*index)++]);
        }
    }

    public string GetObfuscated(List<Structure> structures) {
        foreach (var structureBody in structures) {
            var oldName = structureBody.Name;
            structureBody.Name = RandomString(2 + new Random().Next() % 10);
            foreach (var structure in structures) {
                for (var k = 0; k < structure.Body.Count; k++) 
                    structure.Body[k] = structure.Body[k].Replace(oldName, structureBody.Name);

                if (structure.GetType() == typeof(ClassStructure)) 
                    ((ClassStructure)structure).UpdateName(oldName, structureBody.Name);
            }
        }

        return string.Join(" ", structures.Select(x => x.GetLine()));
    }
    
    private static string RandomString(int length) {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[new Random(DateTime.Now.Microsecond).Next(s.Length)]).ToArray());
    }
}