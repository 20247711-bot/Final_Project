using Final_Project.Models;
using Npgsql;

namespace Final_Project.Data;

public class ProjectImageRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ProjectImageRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    private static ProjectImage ReadImage(NpgsqlDataReader reader)
    {
        return new ProjectImage
        {
            ImageId = reader.GetInt32(0),
            ProjectId = reader.GetInt32(1),
            ImageUrl = reader.GetString(2)
        };
    }

    public async Task<List<ProjectImage>> GetAllAsync()
    {
        const string sql = "SELECT image_id, project_id, image_url " +
                           "FROM portfolio.project_image ORDER BY image_id;";

        var images = new List<ProjectImage>();

        await using var command = _dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            images.Add(ReadImage(reader));
        }

        return images;
    }

    public async Task AddAsync(ProjectImage image)
    {
        const string sql = "INSERT INTO portfolio.project_image " +
                           "(project_id, image_url) " +
                           "VALUES (@project_id, @image_url);";

        await using var command = _dataSource.CreateCommand(sql);

        command.Parameters.AddWithValue("project_id", image.ProjectId);
        command.Parameters.AddWithValue("image_url", image.ImageUrl);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<ProjectImage?> GetByIdAsync(long id)
    {
        const string sql = "SELECT image_id, project_id, image_url " +
                           "FROM portfolio.project_image WHERE image_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? ReadImage(reader) : null;
    }

    public async Task UpdateAsync(ProjectImage image)
    {
        const string sql = "UPDATE portfolio.project_image " +
                           "SET project_id = @project_id, image_url = @image_url " +
                           "WHERE image_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);

        command.Parameters.AddWithValue("project_id", image.ProjectId);
        command.Parameters.AddWithValue("image_url", image.ImageUrl);
        command.Parameters.AddWithValue("id", image.ImageId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(long id)
    {
        const string sql = "DELETE FROM portfolio.project_image " +
                           "WHERE image_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await command.ExecuteNonQueryAsync();
    }
}