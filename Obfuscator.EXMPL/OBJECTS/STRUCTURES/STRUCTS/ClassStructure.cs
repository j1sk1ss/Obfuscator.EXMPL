namespace Obfuscator.EXMPL.OBJECTS.STRUCTURES.STRUCTS;

public class ClassStructure : Structure {
    public ClassStructure(bool isConstant) : base(isConstant) {
        Name       = "name";
        Body       = new List<string>();
        Children   = new List<ClassStructure>();
        IsConstant = isConstant;
    }
    
    public List<ClassStructure> Children { get; set; }
    
    public override string GetLine() => Type + Name + "{" + string.Join("", Children.Select(x => x.GetLine())) +
                                        string.Join("", Body).Replace(Environment.NewLine, " ") + "}";

    public override Type GetType() => typeof(ClassStructure);

    public void UpdateName(string name, string newName) {
        foreach (var children in Children) {
            for (var j = 0; j < children.Body.Count; j++) 
                children.Body[j] = children.Body[j].Replace(name, newName);

            children.UpdateName(name, newName);
        }
    }
}