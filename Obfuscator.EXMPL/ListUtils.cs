using Obfuscator.EXMPL.OBJECTS.STRUCTURES.STRUCTS;

namespace Obfuscator.EXMPL;

public static class ListUtils {
    public static List<ClassStructure> RandomSwipe(this List<ClassStructure> list) {
        for (var i = list.Count - 1; i >= 1; i--) {
            var j = new Random().Next(i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }

        return list;
    } 
}