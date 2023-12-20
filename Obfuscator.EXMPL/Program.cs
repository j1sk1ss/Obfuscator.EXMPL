using Obfuscator.EXMPL.OBJECTS.LANGUAGES.LANGS;
using Obfuscator.EXMPL.OBJECTS.STRUCTURES;

namespace Obfuscator.EXMPL {
    public class Program {
        public static void Main() {
            Console.WriteLine("Past path to the code file:");
            var path = Console.ReadLine();

            var variablesReplace = false;
            var structuresDelete = false;
            var classesSwipe = false;
            while (true) {
                Console.Clear();
                Console.WriteLine("Choose operations:\n" +
                                  "1) Replace variables ({0})\n" +
                                  "2) Delete structure ({1})\n" +
                                  "3) Swipe classes ({2})\n" +
                                  "4) Next",
                    variablesReplace ? "Active" : "Deactivated",
                    structuresDelete ? "Active" : "Deactivated",
                    classesSwipe ? "Active" : "Deactivated"
                    );
                
                switch (int.Parse(Console.ReadLine()!)) {
                    case 1:
                        variablesReplace = !variablesReplace;
                        continue;
                    
                    case 2:
                        structuresDelete = !structuresDelete;
                        continue;
                    
                    case 3:
                        classesSwipe = !classesSwipe;
                        continue;
                }
                
                break;
            }
            
            if (path[^3..] == "txt") {
                var language = new CSharp(path);
                var classes = language.GetClasses();
                var import = language.GetImports();

                if (structuresDelete) {
                    import.DeleteStructure();
                    foreach (var classStructure in classes)
                        classStructure.DeleteStructure();
                }

                if (variablesReplace)
                    foreach (var classStructure in classes)
                        classStructure.ReplaceVariables(CSharp.GetDirectivesDictionary()["Methods"]);

                if (classesSwipe)
                    classes.RandomSwipe();
                
                File.WriteAllText("obfuscated.txt", language.GetObfuscated(
                    new List<Structure> {import}.Concat(classes).ToList()));
            }
        }
    }
}