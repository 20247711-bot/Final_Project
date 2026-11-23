using Final_Project.Models;
using Npgsql;
namespace Final_Project.Data;

public class ProjectRepository
{
    private readonly NpgsqlDataSource _db;
    public ProjectRepository(NpgsqlDataSource db) => _db = db;

    public async Task<List<Project>> GetAllAsync()
    {
        const string sql = @"SELECT p.project_id,p.course_id,p.title,p.description,p.youtube_url,
                                    c.course_id,c.course_code,c.course_name,c.description,c.course_year,
                                    i.image_id,i.image_url
                             FROM portfolio.project p
                             LEFT JOIN portfolio.course c ON c.course_id=p.course_id
                             LEFT JOIN portfolio.project_image i ON i.project_id=p.project_id
                             ORDER BY p.project_id DESC,i.image_id";
        var map = new Dictionary<int, Project>();
        await using var cmd = _db.CreateCommand(sql);
        await using var rd = await cmd.ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            var id = rd.GetInt32(0);
            if (!map.TryGetValue(id, out var p))
            {
                p = new Project
                {
                    ProjectId = id,
                    CourseId = rd.GetInt32(1),
                    Title = rd.GetString(2),
                    Description = rd.IsDBNull(3) ? null : rd.GetString(3),
                    VideoUrl = rd.IsDBNull(4) ? null : rd.GetString(4)
                };
                if (!rd.IsDBNull(5)) p.Course = new Course { CourseId = rd.GetInt32(5), CourseCode = rd.GetString(6), CourseName = rd.GetString(7), Description = rd.IsDBNull(8) ? null : rd.GetString(8), CourseYear = rd.IsDBNull(9) ? null : rd.GetString(9) };
                map[id] = p;
            }
            if (!rd.IsDBNull(10)) p.Images.Add(new ProjectImage { ImageId = rd.GetInt32(10), ProjectId = id, ImageUrl = rd.GetString(11) });
        }
        return map.Values.ToList();
    }

    public async Task<Project?> GetByIdAsync(long id) => (await GetAllAsync()).FirstOrDefault(x => x.ProjectId == id);

    public async Task AddAsync(Project project, IEnumerable<string> imageUrls)
    {
        await using var conn = await _db.OpenConnectionAsync();
        await using var tx = await conn.BeginTransactionAsync();
        const string sql = "INSERT INTO portfolio.project(course_id,title,description,youtube_url) VALUES(@course,@title,@description,@youtube) RETURNING project_id";
        await using var cmd = new NpgsqlCommand(sql, conn, tx);
        cmd.Parameters.AddWithValue("course", project.CourseId); cmd.Parameters.AddWithValue("title", project.Title);
        cmd.Parameters.AddWithValue("description", (object?)project.Description ?? DBNull.Value); cmd.Parameters.AddWithValue("youtube", (object?)project.VideoUrl ?? DBNull.Value);
        project.ProjectId = (int)(await cmd.ExecuteScalarAsync())!;
        foreach (var url in imageUrls.Distinct(StringComparer.OrdinalIgnoreCase))
        { await using var img = new NpgsqlCommand("INSERT INTO portfolio.project_image(project_id,image_url) VALUES(@id,@url) ON CONFLICT DO NOTHING", conn, tx); img.Parameters.AddWithValue("id", project.ProjectId); img.Parameters.AddWithValue("url", url); await img.ExecuteNonQueryAsync(); }
        await tx.CommitAsync();
    }

    public async Task UpdateAsync(Project project, IEnumerable<string> imageUrls)
    {
        await using var conn = await _db.OpenConnectionAsync(); await using var tx = await conn.BeginTransactionAsync();
        await using (var cmd = new NpgsqlCommand("UPDATE portfolio.project SET course_id=@course,title=@title,description=@description,youtube_url=@youtube WHERE project_id=@id", conn, tx))
        { cmd.Parameters.AddWithValue("course", project.CourseId); cmd.Parameters.AddWithValue("title", project.Title); cmd.Parameters.AddWithValue("description", (object?)project.Description ?? DBNull.Value); cmd.Parameters.AddWithValue("youtube", (object?)project.VideoUrl ?? DBNull.Value); cmd.Parameters.AddWithValue("id", project.ProjectId); await cmd.ExecuteNonQueryAsync(); }
        await using (var del = new NpgsqlCommand("DELETE FROM portfolio.project_image WHERE project_id=@id", conn, tx)) { del.Parameters.AddWithValue("id", project.ProjectId); await del.ExecuteNonQueryAsync(); }
        foreach (var url in imageUrls.Distinct(StringComparer.OrdinalIgnoreCase))
        { await using var img = new NpgsqlCommand("INSERT INTO portfolio.project_image(project_id,image_url) VALUES(@id,@url)", conn, tx); img.Parameters.AddWithValue("id", project.ProjectId); img.Parameters.AddWithValue("url", url); await img.ExecuteNonQueryAsync(); }
        await tx.CommitAsync();
    }
    public async Task DeleteAsync(long id) { await using var cmd = _db.CreateCommand("DELETE FROM portfolio.project WHERE project_id=@id"); cmd.Parameters.AddWithValue("id", id); await cmd.ExecuteNonQueryAsync(); }

    internal async Task AddAsync(Project project)
    {
        throw new NotImplementedException();
    }

    internal async Task UpdateAsync(Project project)
    {
        throw new NotImplementedException();
    }
}
