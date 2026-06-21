using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Dapper;
using CardCreator.Models;

namespace CardCreator.Data;

public static class CardRepository
{
    private static string GetConnectionString() => $"Data Source={Database.DbPath}";

    public static int Create(Card card)
    {
        using var connection = new SqliteConnection(GetConnectionString());
        connection.Open();

        var sql = """
            INSERT INTO card_data (
                cost, title, top_right, name_bar, 
                bottom_left, bottom_center, bottom_right, 
                body_text, bg_color, image_path, frame_color
            ) VALUES (
                @Cost, @Title, @TopRight, @NameBar,
                @BottomLeft, @BottomCenter, @BottomRight,
                @BodyText, @BgColor, @ImagePath, @FrameColor
            );
            SELECT last_insert_rowid();
            """;

        return connection.QuerySingle<int>(sql, card);
    }

    public static Card? Read(int id)
    {
        using var connection = new SqliteConnection(GetConnectionString());
        connection.Open();

        var sql = """
            SELECT 
                id, cost, title, 
                top_right AS TopRight, 
                name_bar AS NameBar, 
                bottom_left AS BottomLeft, 
                bottom_center AS BottomCenter, 
                bottom_right AS BottomRight, 
                body_text AS BodyText, 
                bg_color AS BgColor, 
                image_path AS ImagePath,
                frame_color AS FrameColor
            FROM card_data WHERE id = @Id
            """;
        return connection.QuerySingleOrDefault<Card>(sql, new { Id = id });
    }

    public static List<Card> ReadAll()
    {
        using var connection = new SqliteConnection(GetConnectionString());
        connection.Open();

        var sql = """
            SELECT 
                id, cost, title, 
                top_right AS TopRight, 
                name_bar AS NameBar, 
                bottom_left AS BottomLeft, 
                bottom_center AS BottomCenter, 
                bottom_right AS BottomRight, 
                body_text AS BodyText, 
                bg_color AS BgColor, 
                image_path AS ImagePath,
                frame_color AS FrameColor
            FROM card_data ORDER BY id DESC
            """;
        return connection.Query<Card>(sql).ToList();
    }

    public static void Update(Card card)
    {
        using var connection = new SqliteConnection(GetConnectionString());
        connection.Open();

        var sql = """
            UPDATE card_data SET
                cost = @Cost,
                title = @Title,
                top_right = @TopRight,
                name_bar = @NameBar,
                bottom_left = @BottomLeft,
                bottom_center = @BottomCenter,
                bottom_right = @BottomRight,
                body_text = @BodyText,
                bg_color = @BgColor,
                image_path = @ImagePath,
                frame_color = @FrameColor
            WHERE id = @Id
            """;

        connection.Execute(sql, card);
    }

    public static void Delete(int id)
    {
        using var connection = new SqliteConnection(GetConnectionString());
        connection.Open();

        var sql = "DELETE FROM card_data WHERE id = @Id";
        connection.Execute(sql, new { Id = id });
    }
}