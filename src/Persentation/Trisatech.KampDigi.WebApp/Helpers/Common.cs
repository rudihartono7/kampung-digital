using System.Globalization;

namespace Trisatech.KampDigi.Helpers;
public static class Common
{
    public static byte[] ToBytes(this Stream streamContent)
    {
        MemoryStream ms = new MemoryStream();

        streamContent.CopyTo(ms);

        return ms.ToArray();
    }
}