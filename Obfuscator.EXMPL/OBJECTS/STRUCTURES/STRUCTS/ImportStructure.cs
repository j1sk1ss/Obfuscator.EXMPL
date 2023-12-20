namespace Obfuscator.EXMPL.OBJECTS.STRUCTURES.STRUCTS;

public class ImportStructure : Structure {
    public ImportStructure(bool isConstant) : base(isConstant) {
        Body       = new List<string>();
        IsConstant = isConstant;
        Name       = "name";
        Type       = "";
    }
    
    public override Type GetType() => typeof(ImportStructure);
}