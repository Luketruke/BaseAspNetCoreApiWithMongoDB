using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using MyBaseProject.Domain.Exceptions;
using MyBaseProject.Application.Interfaces;

namespace MyBaseProject.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly ILogger<BaseRepository<T>> _logger;

        public BaseRepository(IMongoCollection<T> collection, ILogger<BaseRepository<T>> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching all documents.");
                throw new DatabaseOperationException("Error fetching all documents.", ex);
            }
        }

        public async Task<T> GetByIdAsync(string id)
        {
            try
            {
                if (!ObjectId.TryParse(id, out var objectId))
                {
                    throw new InvalidIdFormatException(id);
                }

                var entity = await _collection.Find(Builders<T>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();

                if (entity == null)
                {
                    throw new EntityNotFoundException(typeof(T).Name, id);
                }

                return entity;
            }
            catch (InvalidIdFormatException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, $"Database error occurred while fetching document with id {id}.");
                throw new DatabaseOperationException("Error fetching document by ID.", ex);
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
            }
            catch (MongoWriteException ex)
            {
                _logger.LogError(ex, "Database write error occurred while adding a new document.");
                throw new DatabaseWriteException("Error adding document. It may be due to duplicate data or validation issues.", ex);
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "General database error occurred while adding a new document.");
                throw new DatabaseOperationException("General database error occurred while adding the document.", ex);
            }
        }

        public async Task UpdateAsync(string id, T entity)
        {
            try
            {
                if (!ObjectId.TryParse(id, out var objectId))
                {
                    throw new InvalidIdFormatException(id);
                }

                var result = await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", objectId), entity);

                if (result.MatchedCount == 0)
                {
                    throw new EntityNotFoundException(typeof(T).Name, id);
                }
            }
            catch (InvalidIdFormatException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, $"Database error occurred while updating document with id {id}.");
                throw new DatabaseOperationException("Error updating document.", ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                if (!ObjectId.TryParse(id, out var objectId))
                {
                    throw new InvalidIdFormatException(id);
                }

                var result = await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", objectId));

                if (result.DeletedCount == 0)
                {
                    throw new EntityNotFoundException(typeof(T).Name, id);
                }
            }
            catch (InvalidIdFormatException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                throw;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, $"Database error occurred while deleting document with id {id}.");
                throw new DatabaseOperationException("Error deleting document.", ex);
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _collection.Find(filter).ToListAsync();
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, "Database error occurred while finding documents with the specified filter.");
                throw new DatabaseOperationException("Error finding documents with the specified filter.", ex);
            }
        }
    }
}