using Obfuscator.EXMPL.OBJECTS.LANGUAGES;

namespace Obfuscator.EXMPL.OBJECTS;

public class Obfuscator {
    public Obfuscator(Language language) {
        Language = language;
    }
    
    private Language Language { get; set; }
}