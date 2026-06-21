using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Dapper;

namespace CardCreator.Data;

public static class Database
{
    public static string DbPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CardCreator",
        "cards.db");

    public static void Initialize()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DbPath)!);

        using var connection = new SqliteConnection($"Data Source={DbPath}");
        connection.Open();

        var tableExists = connection.QuerySingleOrDefault<int>(
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='card_data'") > 0;

        if (!tableExists)
        {
            connection.Execute("""
                CREATE TABLE card_data (
                    id              INTEGER PRIMARY KEY AUTOINCREMENT,
                    cost            TEXT NOT NULL DEFAULT '',
                    title           TEXT NOT NULL DEFAULT '',
                    top_right       TEXT NOT NULL DEFAULT '',
                    name_bar        TEXT NOT NULL DEFAULT '',
                    bottom_left     TEXT NOT NULL DEFAULT '',
                    bottom_center   TEXT NOT NULL DEFAULT '',
                    bottom_right    TEXT NOT NULL DEFAULT '',
                    body_text       TEXT NOT NULL DEFAULT '',
                    bg_color        TEXT NOT NULL DEFAULT '#000000',
                    image_path      TEXT,
                    frame_color     TEXT NOT NULL DEFAULT 'silver'
                );
            """);
        }
        else
        {
            // Migrate existing db if column missing
            var colExists = connection.QuerySingleOrDefault<int>(
                "SELECT COUNT(*) FROM pragma_table_info('card_data') WHERE name='frame_color'") > 0;

            if (!colExists)
                connection.Execute("ALTER TABLE card_data ADD COLUMN frame_color TEXT NOT NULL DEFAULT 'silver'");
        }
    }
}