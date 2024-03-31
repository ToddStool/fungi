using SvgToSolidity.Svg;
using System.Text;
using System.Xml;

namespace SvgToSolidity;

public static class SvgParser
{
    public static string GetSolidity(string name, string directory)
    {
        var rects = new List<Rect>();
        foreach (var l in Parse(name, directory).Data) {
            foreach (var i in l) {
                foreach (var rect in i) {
                    rects.Add(rect);
                }
            }
        }

        var sb = new StringBuilder("Rect[] private legsRects = [");
        sb.AppendLine();
        for (var i = 0; i < rects.Count; ++i) {
            for (var j = 0; j < 4; ++j) sb.Append(' ');
            sb.Append(rects[i].ToSolidityRect());
            if (i != rects.Count - 1) sb.AppendLine(",");
            else sb.AppendLine();
        }
        sb.AppendLine("];");

        return sb.ToString();
    }
    public static ParseData Parse(string name, string directory, int levelsCount = Defaults.LEVELS_COUNT)
    {
        var levels = new List<List<List<Rect>>>(levelsCount);
        for (var lvl = 0; lvl < levelsCount; ++lvl) {
            var dir = $"{directory}\\{name}\\{lvl}";
            if (!Directory.Exists(dir)) throw new Exception($"directory is not exists:\n{dir}");
            levels.Add(GetFilesRects(dir));
        }

        return new ParseData {
            Data = levels,
            LevelsCount = levelsCount,
            Name = name
        };
    }
    public static List<List<Rect>> GetFilesRects(string directory)
    {
        var files = Directory.GetFiles(directory, "*.svg")
            .OrderBy(i => int.TryParse(Path.GetFileNameWithoutExtension(i), out var v) ? v : throw new Exception($"incorrect file name {i}"))
            .ToArray();
        var items = new List<List<Rect>>(files.Length);
        items.AddRange(files.Select(GetRects));
        return items;
    }
    public static List<Rect> GetRects(string fileName)
    {
        var doc = new XmlDocument();
        doc.Load(fileName);
        var root = doc.DocumentElement;
        var res = new List<Rect>();
        if (root == null) return res;

        var rects = root.GetElementsByTagName("rect");

        foreach (XmlNode node in rects) {
            res.Add(new Rect {
                x = int.Parse(node.Attributes.GetNamedItem("x").Value),
                y = int.Parse(node.Attributes.GetNamedItem("y").Value),
                width = int.Parse(node.Attributes.GetNamedItem("width").Value),
                height = int.Parse(node.Attributes.GetNamedItem("height").Value),
                fill = node.Attributes.GetNamedItem("fill").Value
            });
        }

        return res;
    }
}