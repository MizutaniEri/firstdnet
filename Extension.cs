using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 拡張メソッド用クラス
/// </summary>
/// <remarks>ForEachクラスくらい標準で用意してほしいクラス</remarks>
public static class Extension
{
    /// <summary>
    /// Array用ForEachクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        Array.ForEach<T>(array, obj => action(obj));
    }

    /// <summary>
    /// IEnumerable用ForEachクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequence"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (T item in sequence)
            action(item);
    }

    /// <summary>
    /// IEnumerable用ForEachクラスインデックス付
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        int index = 0;
        foreach (var x in source)
        {
            action(x, index++);
        }
    }
    
    /// <summary>
    /// 指定されたファイルを品質を指定してJPEGで保存する
    /// </summary>
    /// <param name="fileName">画像ファイル名</param>
    /// <param name="quality">品質</param>
    public static void SaveImage(this Image img, string fileName, int quality)
    {
        //画像ファイルを読み込む
        Bitmap bmp = img as Bitmap;

        //EncoderParameterオブジェクトを1つ格納できる
        //EncoderParametersクラスの新しいインスタンスを初期化
        //ここでは品質のみ指定するため1つだけ用意する
        var eps = new EncoderParameters(1);
        //品質を指定
        System.Drawing.Imaging.EncoderParameter ep =
            new System.Drawing.Imaging.EncoderParameter(
            System.Drawing.Imaging.Encoder.Quality, quality);
        //EncoderParametersにセットする
        eps.Param[0] = ep;

        //イメージエンコーダに関する情報を取得する
        var ici = GetEncoderInfo("image/jpeg");

        //新しいファイルの拡張子を取得する
        string ext = ici.FilenameExtension.Split(';')[0];
        ext = Path.GetExtension(ext).ToLower();

        //保存するファイル名を決定（拡張子を変える）
        string saveName = Path.ChangeExtension(fileName, ext);
        //保存する
        bmp.Save(saveName, ici, eps);
        fileName = saveName;
    }

    /// <summary>
    /// MimeTypeで指定されたImageCodecInfoを探して返す
    /// </summary>
    /// <param name="mineType"></param>
    /// <returns></returns>
    public static ImageCodecInfo GetEncoderInfo(this string mineType)
    {
        //GDI+ に組み込まれたイメージ エンコーダに関する情報をすべて取得
        var encs = ImageCodecInfo.GetImageEncoders();
        //指定されたMimeTypeを探して見つかれば返す
        foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs)
            if (enc.MimeType == mineType)
                return enc;
        return null;
    }
}
