using Final_Project.Models;
using Npgsql;

namespace Final_Project.Data;

public class CourseRepository
{

    private readonly NpgsqlDataSource _dataSource;

    public CourseRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    private static Course ReadCourse(NpgsqlDataReader reader)
    {
        return new Course
        {
            CourseId = reader.GetInt32(0),
            CourseCode = reader.GetString(1),
            CourseName = reader.GetString(2),
            Description = reader.IsDBNull(3) ? null : reader.GetString(3),
            CourseYear = reader.IsDBNull(4) ? null : reader.GetString(4)
        };
    }

    public async Task<List<Course>> GetAllAsync()
    {
        const string sql = "SELECT course_id, course_code, course_name, description, course_year FROM portfolio.course ORDER BY course_name;";

        var course = new List<Course>();

        await using var command = _dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            course.Add(ReadCourse(reader));
        }

        return course;
    }

    public async Task AddAsync(Course course)
    {
        const string sql = "INSERT INTO portfolio.course (course_code, course_name, description, course_year) " +
                           "VALUES (@course_code, @course_name, @description, @course_year);";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("course_code", course.CourseCode);
        command.Parameters.AddWithValue("course_name", course.CourseName);
        command.Parameters.AddWithValue("description", (object?)course.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("course_year", (object?)course.CourseYear ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();
    }


    public async Task<Course?> GetByIdAsync(long id)
    {
        const string sql = "SELECT course_id, course_code, course_name, description, course_year FROM portfolio.course WHERE course_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? ReadCourse(reader) : null;
    }

    public async Task UpdateAsync(Course course)
    {
        const string sql = "UPDATE portfolio.course " +
                   "SET course_code = @course_code, course_name = @course_name, " +
                   "description = @description, course_year = @course_year " +
                   "WHERE course_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);

        command.Parameters.AddWithValue("course_code", course.CourseCode);
        command.Parameters.AddWithValue("course_name", course.CourseName);
        command.Parameters.AddWithValue("description", (object?)course.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("course_year", (object?)course.CourseYear ?? DBNull.Value);
        command.Parameters.AddWithValue("id", course.CourseId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(long id)
    {
        const string sql = "DELETE FROM portfolio.course WHERE course_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        await command.ExecuteNonQueryAsync();
    }

}
