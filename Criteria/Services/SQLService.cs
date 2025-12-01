using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Criteria.Services
{
    public class SQLService
    {
        SQLite.SQLiteAsyncConnection database;

        public SQLService (string DatabasePath)
        {
            database = new SQLite.SQLiteAsyncConnection(DatabasePath);
            database.CreateTableAsync<Models.Movie>().Wait();
        }

        public async Task<List<Models.Movie>> GetMoviesAsync()
        {
            return await database.Table<Models.Movie>().ToListAsync();
        }

        public async Task<int> SaveMovieAsync(Models.Movie movie)
        {
            if (movie.Id != 0)
            {
                return await database.UpdateAsync(movie);
            }
            else
            {
                return await database.InsertAsync(movie);
            }
        }

        public async Task<int> DeleteMovieAsync(Models.Movie movie)
        {
            return await database.DeleteAsync(movie);
        }
    }
}
