using Dapper;
using Npgsql;
using sharecare_backend.Models.Problem;
using sharecare_backend.Models.User;

namespace sharecare_backend.Services
{
    public class DbService
    {
        private readonly NpgsqlDataSource _dataSource;

        public DbService(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task CheckAndCreateTablesAsync()
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            const string sql = """
            CREATE TABLE IF NOT EXISTS problems (
                id SERIAL PRIMARY KEY, 
                name TEXT NOT NULL, 
                description TEXT, 
                type_json JSONB, 
                time_json JSONB, 
                is_location_bound BOOLEAN NOT NULL, 
                location_json JSONB, 
                payment_json JSONB, 
                providers_id INT[], 
                searchers_id INT[]
            );
            CREATE TABLE IF NOT EXISTS users (
                id SERIAL PRIMARY KEY, 
                name TEXT NOT NULL, 
                email TEXT NOT NULL UNIQUE, 
                password_hash TEXT NOT NULL
            );
            """;

            await connection.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<ProblemDBEntity>> GetAllProblemsAsync()
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            const string sql = """
            SELECT 
                id AS Id, 
                name as Name, 
                description AS Description, 
                type_json AS TypeJson, 
                time_json AS TimeJson, 
                is_location_bound AS IsLocationBound, 
                location_json AS LocationJson, 
                payment_json AS PaymentJson, 
                providers_id AS ProvidersId,   
                searchers_id AS SearchersId    
            FROM problems
            """;

            return await connection.QueryAsync<ProblemDBEntity>(sql);
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            const string sql = """
            SELECT 
                id AS Id, 
                name AS Name, 
                email AS Email,
                password_hash AS PasswordHash
            FROM users
            """;

            return await connection.QueryAsync<UserEntity>(sql);
        }

        /*
        public async Task<ProblemDBEntity?> GetProblemByIdAsync(int id)
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            const string sql = """
            SELECT 
                id AS Id, 
                name as Name, 
                description AS Description, 
                type_json AS TypeJson, 
                time_json AS TimeJson, 
                is_location_bound AS IsLocationBound, 
                location_json AS LocationJson, 
                payment_json AS PaymentJson, 
                providers_id AS ProvidersId,   
                searchers_id AS SearchersId    
            FROM problems WHERE id = @Id
            """;

            return await connection.QueryFirstOrDefaultAsync<ProblemDBEntity>(sql, new { Id = id });
        }
        */

        public async Task<int> CreateProblemAsync(ProblemDBEntity problem)
        {
            using var connection = await _dataSource.OpenConnectionAsync();
            const string sql = """
            INSERT INTO problems (name, description, type_json, time_json, is_location_bound, location_json, payment_json, providers_id, searchers_id) 
            VALUES (@Name, @Description, @TypeJson::jsonb, @TimeJson::jsonb, @IsLocationBound, @LocationJson::jsonb, @PaymentJson::jsonb, @ProvidersId, @SearchersId) 
            RETURNING id;
            """;
            return await connection.ExecuteScalarAsync<int>(sql, problem);
        }  
        
        public async Task<int> CreateUserAsync(UserEntity user)
        {
            using var connection = await _dataSource.OpenConnectionAsync();
            const string sql = """
            INSERT INTO users (name, email, password_hash) 
            VALUES (@Name, @Email, @PasswordHash) 
            RETURNING id;
            """;
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            const string sql = """
            SELECT 
                id AS Id, 
                name AS Name, 
                email AS Email,
                password_hash AS PasswordHash
            FROM users WHERE email = @email
            """;

            return await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, new { email });
        }
    }
}
