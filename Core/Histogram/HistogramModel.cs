using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using Core.Database;
using Core.Utility;
using Microsoft.Data.Sqlite;

namespace Core.History;

public class HistogramModel : IDatabaseAccess<ImageMetadata>
{
    private Dictionary<string, SqlQueryMetadata> Queries { get; } =
        new Dictionary<string, SqlQueryMetadata>
        {
            {
                "SelectAllHistograms",
                new SqlQueryMetadata(
                    "SelectAllHistograms",
                    """
                    SELECT * FROM histograms
                    ORDER BY created_at DESC
                    """,
                    []
                )
            },
            {
                "InsertHistogram",
                new SqlQueryMetadata(
                    "InsertHistogram",
                    """
                    INSERT OR IGNORE INTO histograms(
                        image_hash, image_name, path, format, width, height,
                        filedata_blob, filedata_encrypted_blob, has_alpha, entropy
                    )
                    VALUES(
                        @image_hash, @image_name, @path, @format, @width, @height,
                        @filedata_blob, @filedata_encrypted_blob, @has_alpha, @entropy
                    );
                    """,
                    [
                        new SqliteParameter("@image_hash", SqliteType.Text),
                        new SqliteParameter("@image_name", SqliteType.Text),
                        new SqliteParameter("@path", SqliteType.Text),
                        new SqliteParameter("@format", SqliteType.Text),
                        new SqliteParameter("@width", SqliteType.Integer),
                        new SqliteParameter("@height", SqliteType.Integer),
                        new SqliteParameter("@filedata_blob", SqliteType.Blob),
                        new SqliteParameter("@filedata_encrypted_blob", SqliteType.Blob),
                        new SqliteParameter("@has_alpha", SqliteType.Integer),
                        new SqliteParameter("@entropy", SqliteType.Real),
                    ]
                )
            },
            {
                "DeleteHistogram",
                new SqlQueryMetadata(
                    "DeleteHistogram",
                    """
                    DELETE FROM histograms
                    WHERE image_hash = @image_hash
                    """,
                    [new SqliteParameter("@image_hash", SqliteType.Text)]
                )
            },
        };

    public List<ImageMetadata> ExecuteSelect(string sqlKey)
    {
        using var connection = new SqliteConnection(AppRepository.Instance.ConnString);
        connection.Open();

        SqlQueryMetadata queryMetadata = Queries[sqlKey];
        using var command = connection.CreateCommand();
        command.CommandText = queryMetadata.Sql;
        using var reader = command.ExecuteReader();
        List<ImageMetadata> imageMetadataBuffer = new List<ImageMetadata>();
        while (reader.Read())
        {
            ImageMetadata imageMetadata = new ImageMetadata.ImageMetadataBuilder()
                .WithImageHash(reader["image_hash"].ToString())
                .WithFilename(reader["image_name"].ToString())
                .WithPath(reader["path"].ToString())
                .WithImageFormat(reader["format"].ToString())
                .WithDimensions(
                    new(Convert.ToUInt32(reader["width"]), Convert.ToUInt32(reader["height"]))
                )
                .WithFiledataBlob((byte[])reader["filedata_blob"])
                .WithFiledataEncryptedBlob((byte[])reader["filedata_encrypted_blob"])
                .WithHasAlpha(Convert.ToBoolean(reader["has_alpha"]))
                .Build();
            imageMetadataBuffer.Add(imageMetadata);
        }
        return imageMetadataBuffer;
    }

    public ImageMetadata ExecuteInsert(string sqlKey, ImageMetadata imageMetadata)
    {
        using var connection = new SqliteConnection(AppRepository.Instance.ConnString);
        connection.Open();

        SqlQueryMetadata queryMetadata = Queries[sqlKey];
        using var command = connection.CreateCommand();
        command.CommandText = queryMetadata.Sql;
        foreach (SqliteParameter template in queryMetadata.Parameters)
        {
            var param = new SqliteParameter(template.ParameterName, template.SqliteType)
            {
                Value = template.ParameterName switch
                {
                    "@image_hash" => (object?)imageMetadata.ImageHash ?? DBNull.Value,
                    "@image_name" => (object?)imageMetadata.Filename ?? DBNull.Value,
                    "@path" => (object?)imageMetadata.Path ?? "some/path/to/image",
                    "@format" => (object?)imageMetadata.ImageFormat?.ToString() ?? DBNull.Value,
                    "@width" => (object?)imageMetadata.Dimensions?.Width ?? DBNull.Value,
                    "@height" => (object?)imageMetadata.Dimensions?.Height ?? DBNull.Value,
                    "@filedata_blob" => (object?)imageMetadata.FiledataBlob ?? DBNull.Value,
                    "@filedata_encrypted_blob" => (object?)imageMetadata.FiledataEncryptedBlob
                        ?? DBNull.Value,
                    "@has_alpha" => imageMetadata.HasAlpha ? 1 : 0,
                    "@entropy" => -1,
                    _ => DBNull.Value,
                },
            };
            command.Parameters.Add(param);
        }
        command.ExecuteNonQuery();
        return imageMetadata;
    }

    public List<ImageMetadata> ExecuteUpdate(string sqlKey)
    {
        return new();
    }

    public List<ImageMetadata> ExecuteDelete(string sqlKey)
    {
        return new();
    }
}
