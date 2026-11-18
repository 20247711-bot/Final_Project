using Portfolio.Models;
using Npgsql;

namespace Portfolio.Data;

public class ProjectRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ProjectRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    private static Project ReadProject(NpgsqlDataReader reader)
    {
        return new Project
        {
            ProjectId = reader.GetInt32(0),
            CourseId = reader.GetInt32(1),
            Title = reader.GetString(2),
            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
            VideoUrl = reader.IsDBNull(4) ? null : reader.GetString(4)
        };
    }

    public async Task<List<Project>> GetAllAsync()
    {
        const string sql = "SELECT project_id, course_id, title, description, video_url " +
                           "FROM portfolio.project ORDER BY title;";

        var projects = new List<Project>();

        await using var command = _dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            projects.Add(ReadProject(reader));
        }

        return projects;
    }

    public async Task AddAsync(Project project)
    {
        const string sql = "INSERT INTO portfolio.project " +
                           "(course_id, title, description, video_url) " +
                           "VALUES (@course_id, @title, @description, @video_url);";

        await using var command = _dataSource.CreateCommand(sql);

        command.Parameters.AddWithValue("course_id", project.CourseId);
        command.Parameters.AddWithValue("title", project.Title);
        command.Parameters.AddWithValue("description", (object?)project.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("video_url", (object?)project.VideoUrl ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<Project?> GetByIdAsync(long id)
    {
        const string sql = "SELECT project_id, course_id, title, description, video_url " +
                           "FROM portfolio.project WHERE project_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? ReadProject(reader) : null;
    }

    public async Task UpdateAsync(Project project)
    {
        const string sql = "UPDATE portfolio.project " +
                           "SET course_id = @course_id, title = @title, " +
                           "description = @description, video_url = @video_url " +
                           "WHERE project_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);

        command.Parameters.AddWithValue("course_id", project.CourseId);
        command.Parameters.AddWithValue("title", project.Title);
        command.Parameters.AddWithValue("description", (object?)project.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("video_url", (object?)project.VideoUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("id", project.ProjectId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(long id)
    {
        const string sql = "DELETE FROM portfolio.project WHERE project_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await command.ExecuteNonQueryAsync();
    }
}