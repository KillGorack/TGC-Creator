using System;
using System.IO;

namespace CardCreator.Data;

public static class ImageManager
{
    private static string ArtDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CardCreator",
        "Art");

    public static string GetCardArtPath(int cardId)
    {
        return Path.Combine(ArtDirectory, cardId.ToString(), "card_art.png");
    }

    public static void SaveCardArt(int cardId, string sourceImagePath)
    {
        if (cardId <= 0 || string.IsNullOrEmpty(sourceImagePath) || !File.Exists(sourceImagePath))
            return;

        // Create directory if it doesn't exist
        var cardArtDir = Path.Combine(ArtDirectory, cardId.ToString());
        Directory.CreateDirectory(cardArtDir);

        // Get destination path
        var destinationPath = GetCardArtPath(cardId);

        // Copy the file, overwriting if it exists
        File.Copy(sourceImagePath, destinationPath, overwrite: true);
    }

    public static bool CardArtExists(int cardId)
    {
        return cardId > 0 && File.Exists(GetCardArtPath(cardId));
    }

    public static void DeleteCardArt(int cardId)
    {
        if (cardId <= 0)
            return;

        var cardArtDir = Path.Combine(ArtDirectory, cardId.ToString());
        if (Directory.Exists(cardArtDir))
        {
            Directory.Delete(cardArtDir, recursive: true);
        }
    }
}
