using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Nige.EntityFrameworkCore.UnitOfWork
{
    /// <summary>
    ///     Class BaseDbContext.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class BaseDbContext<T> : DbContext where T : DbContext

    {
        /// <summary>
        ///     The connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        ///     The database type
        /// </summary>
        private readonly DatabaseType _databaseType;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseDbContext{T}" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public BaseDbContext(DbContextOptions<T> options)
            : base(options)
        {
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseDbContext{T}" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="databaseType">Type of the database.</param>
        public BaseDbContext(string connectionString = "", DatabaseType databaseType = DatabaseType.SqlServer)
        {
            _connectionString = connectionString;
            _databaseType = databaseType;
        }
        
         
    }
}
