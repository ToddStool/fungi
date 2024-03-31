using SvgToSolidity.Svg;

namespace SvgToSolidity;

public static class RectExtensions
{
    public static string ToSolidityString(this Rect rect)
    {
        return $"<rect x='{rect.x}' y='{rect.y}' width='{rect.width}' height='{rect.height}' fill='{rect.fill}'";
    }
    public static string ToSolidityTuple(this Rect rect)
    {
        return $"[{rect.x},{rect.y},{rect.width},{rect.height}]";
    }
    public static string ToSolidityRect(this Rect rect)
    {
        return $"Rect({rect.x},{rect.y},{rect.width},{rect.height})";
    }
}