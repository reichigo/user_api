namespace MyPethere.User.CrossCutting.ExtensionsMethods;

public static class Extensions
{
    public static string JoinWithNewLine(this List<string> strings)
    {
        return string.Join(Environment.NewLine, strings);
    }
}
