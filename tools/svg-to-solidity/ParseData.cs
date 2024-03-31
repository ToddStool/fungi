using SvgToSolidity.Svg;

namespace SvgToSolidity;

public sealed class ParseData
{
    public List<List<List<Rect>>> Data { get; set; }
    public int LevelsCount { get; set; }
    public string Name { get; set; }
}