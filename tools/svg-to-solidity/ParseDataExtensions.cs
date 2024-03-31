using System.Text;
using SvgToSolidity.Svg;

namespace SvgToSolidity;

public static class ParseDataExtensions
{
    public static string ToSolidityTuple(this ParseData data, Func<List<List<Rect>>, string> itemConverter , int fold = 0)
    {
        return ToSolidityTuple(data.Data, itemConverter, fold);
    }
    public static string ToSolidityTuple<T>(this List<T> list, Func<T, string> itemConverter, int fold = 0)
    {
        var sb = new StringBuilder();
        sb.Fold(fold).AppendLine("[");
        for (var i = 0; i < list.Count; ++i) {
            sb.Fold(fold).Append(itemConverter(list[i]));
            if (i != list.Count - 1) sb.AppendLine(",");
            else sb.AppendLine();
        }

        sb.Fold(fold).Append("]");
        return sb.ToString();
    }
    static StringBuilder Fold(this StringBuilder sb, int fold)
    {
        for (var i = 0; i < fold * 4; ++i) sb.Append(' ');
        return sb;
    }

    public static string ToInitializeTransactionParam(this ParseData data)
    {
        var list = data.Data;
        var sb = new StringBuilder();
        sb.AppendLine($"[");

        for (var lvl = 0; lvl < list.Count; ++lvl) {
            for (var file = 0; file < list[lvl].Count; ++file) {
                sb.Append("[");
                sb.AppendLine($"{lvl},{file},");
                sb.Append("[");
                for (var rect = 0; rect < list[lvl][file].Count; ++rect) {
                    sb.Append(list[lvl][file][rect].ToSolidityTuple());
                    if (rect != list[lvl][file].Count - 1) sb.AppendLine(",");
                    else sb.AppendLine();
                }
                sb.Append("]");
                sb.Append("]");
                if (file != list[lvl].Count - 1 || lvl != list.Count - 1) sb.AppendLine(",");
                else sb.AppendLine();
            }
        }

        sb.Append("]");
        return sb.ToString();
    }
    public static string ToLevelCounts(this ParseData data)
    {
        var sb = new StringBuilder();
        sb.Append($"uint8[levelsCount] {data.Name}LevelCounts = [");
        for (var i = 0; i < data.Data.Count; ++i) {
            sb.Append(data.Data[i].Count);
            if(i != data.Data.Count - 1) sb.Append(",");
        }
        sb.Append("];");
        return sb.ToString();
    }
}