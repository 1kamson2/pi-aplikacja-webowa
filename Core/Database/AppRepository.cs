using System.Data.Common;
using System.Diagnostics;
using Core.Singleton;
using Core.Utility;
using Microsoft.Data.Sqlite;

namespace Core.Database;

public record SqlQueryMetadata(string Key, string Sql, SqliteParameter[] Parameters);

public class AppRepository : Singleton<AppRepository>
{
    public static string ConnString { get; private set; } =
        $"Data Source={ApplicationPaths.ApplicationDatabasePath}";

    protected override void Setup()
    {
        // if repository is healthy, skip initialization
        if (CheckHealth())
        {
            return;
        }

        using var connection = new SqliteConnection(ConnString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = """
            create table if not exists image(
                image_hash text primary key,
                image_name text not null,
                path text not null,
                key blob not null,
                format text not null,
                width integer not null,
                height integer not null,
                filedata_blob blob not null,
                filedata_encrypted_blob blob not null,
                has_alpha bool not null
            );

            create table if not exists encryption_history(
                image_hash text primary key,
                image_name text not null,
                path text not null,
                key blob not null,
                format text not null,
                width integer not null,
                height integer not null,
                filedata_blob blob not null,
                filedata_encrypted_blob blob not null,
                has_alpha bool not null,
                created_at datetime not null default current_timestamp
            );

            create table if not exists histograms(
                image_hash text primary key,
                image_name text not null,
                path text not null,
                format text not null,
                width integer not null,
                height integer not null,
                filedata_blob blob not null,
                filedata_encrypted_blob blob not null,
                has_alpha bool not null,
                entropy real not null,
                created_at datetime not null default current_timestamp
            );
            """;

        command.ExecuteNonQuery();
    }

    private bool CheckHealth()
    {
        using var connection = new SqliteConnection(ConnString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = """
            select count(distinct name)
            from sqlite_master
            where type='table'
              and name in ('image', 'encryption_history', 'histograms');
            """;

        long count = (long)command.ExecuteScalar()!;
        return count == 3;
    }
}
