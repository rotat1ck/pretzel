namespace SngFileFormat;

public static class SngDecoder
{
    public static async Task DecodeSong(string sngPath)
    {
        await SngCli.SngDecode.DecodeSong(sngPath);
    }
}
