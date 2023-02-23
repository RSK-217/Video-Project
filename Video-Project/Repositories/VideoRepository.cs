using Microsoft.Data.SqlClient;
using Video_Project.Interfaces;
using Video_Project.Models;

namespace Video_Project.Repositories
{
        public class VideoRepository : IVideo
        {
            private readonly string _connectionString;


            public VideoRepository(IConfiguration config)
            {
                _connectionString = config.GetConnectionString("DefaultConnection");
            }

            public SqlConnection Connection => new SqlConnection(_connectionString);

            private readonly string _baseSqlSelect = @"SELECT id, 
                                                          name, 
                                                          url, 
                                                          datePosted, 
                                                          isCurrent
                                                     FROM [Video]";


            public Video GetVideoById(int id)
            {
                using SqlConnection conn = Connection;
                {
                    conn.Open();
                    using SqlCommand cmd = conn.CreateCommand();
                    {
                        cmd.CommandText = $"{_baseSqlSelect} WHERE Id = @id";
                        cmd.Parameters.AddWithValue("id", id);

                        using SqlDataReader reader = cmd.ExecuteReader();
                        {
                            Video? result = null;
                            if (reader.Read())
                            {
                                return LoadFromData(reader);
                            }

                            return result;
                        }
                    }
                }
            }

            public List<Video> GetAllVideos()
            {
                using SqlConnection conn = Connection;
                {
                    conn.Open();
                    using SqlCommand cmd = conn.CreateCommand();
                    {
                        cmd.CommandText = _baseSqlSelect;

                        using SqlDataReader reader = cmd.ExecuteReader();
                        {
                            var results = new List<Video>();
                            while (reader.Read())
                            {
                                var order = LoadFromData(reader);

                                results.Add(order);
                            }

                            return results;
                        }
                    }
                }
            }

            public Video PostVideo(Video video)
            {
                using SqlConnection conn = Connection;
                {
                    conn.Open();
                    using SqlCommand cmd = conn.CreateCommand();
                    {
                        cmd.CommandText = @" INSERT INTO [Video] (
                                                              Name,
                                                              Url,
                                                              DatePosted,
                                                              IsCurrent
                                                             )
                                         OUTPUT INSERTED.ID 
                                         VALUES              (
                                                              @name,
                                                              @url,
                                                              @datePosted,
                                                              @isCurrent
                                                             )";
                        cmd.Parameters.AddWithValue("@name", video.Name);
                        cmd.Parameters.AddWithValue("@url", video.Url);
                        cmd.Parameters.AddWithValue("@datePosted", video.DatePosted);
                        cmd.Parameters.AddWithValue("@isCurrent", video.IsCurrent);

                        int id = (int)cmd.ExecuteScalar();

                        video.Id = id;
                        return video;
                    }
                }
            }

            public void ArchiveVideo(Video video)
            {
                using SqlConnection conn = Connection;
                {
                    conn.Open();
                }
                using SqlCommand cmd = conn.CreateCommand();
                {
                    cmd.CommandText = @"UPDATE [Video]
                                    SET Name = @name,
                                        Url = @url,
                                        DatePosted = @datePosted,
                                        IsCurrent = @isCurrent
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", video.Id);
                    cmd.Parameters.AddWithValue("@name", video.Name);
                    cmd.Parameters.AddWithValue("@url", video.Url);
                    cmd.Parameters.AddWithValue("@datePosted", video.DatePosted);
                    cmd.Parameters.AddWithValue("@isCurrent", video.IsCurrent);

                    cmd.ExecuteNonQuery();
                }
            }

            public void DeleteVideo(int id)
            {
                using SqlConnection conn = Connection;
                {
                    conn.Open();

                    using SqlCommand cmd = conn.CreateCommand();
                    {
                        cmd.CommandText = @"
                            DELETE FROM [Video]
                            WHERE Id = @id
                        ";

                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            private Video LoadFromData(SqlDataReader reader)
            {
                return new Video
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Url = reader.GetString(reader.GetOrdinal("url")),
                    DatePosted = reader.GetDateTime(reader.GetOrdinal("datePosted")),
                    IsCurrent = reader.GetBoolean(reader.GetOrdinal("isCurrent"))
                };
            }

        }
    }

