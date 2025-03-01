using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MyBaseProject.Domain.Exceptions;
using MyBaseProject.Application.Interfaces.Repositories;
using MyBaseProject.Domain.Entities;

namespace MyBaseProject.Infrastructure.Persistence.Repositories
{

    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IMongoCollection<Account> collection, ILogger<AccountRepository> logger)
            : base(collection, logger)
        {
        }
        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email cannot be null or empty", nameof(email));
                }

                var account = await _collection.Find(a => a.Email == email).FirstOrDefaultAsync();

                if (account == null)
                {
                    throw new UserNotFoundException(email);
                }

                return account;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, $"MongoDB Error while fetching account with email: {email}");
                throw new Exception("Database connection issue. Please verify the MongoDB connection.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error while fetching account with email: {email}");
                throw;
            }
        }

        public async Task<Account> CreateAsync(Account account)
        {
            try
            {
                var emailExists = await EmailExistsAsync(account.Email);
                if (emailExists)
                {
                    throw new EmailAlreadyExistsException(account.Email);
                }

                await _collection.InsertOneAsync(account);
                return account;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, $"Error inserting account with email {account.Email}.");
                throw new Exception("Database connection issue. Please verify the MongoDB connection.", ex);
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                var count = await _collection.CountDocumentsAsync(a => a.Email == email);
                return count > 0;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, $"Error checking if email {email} exists.");
                throw new Exception("Database connection issue. Please verify the MongoDB connection.", ex);
            }
        }

        public async Task<bool> UpdateAsync(string id, Account account)
        {
            try
            {
                var result = await _collection.ReplaceOneAsync(
                    Builders<Account>.Filter.Eq(a => a.Id, new ObjectId(id)),
                    account
                );

                if (result.MatchedCount == 0)
                {
                    throw new EntityNotFoundException("Account", id);
                }

                return result.ModifiedCount > 0;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, $"Error updating account with ID {id}.");
                throw new Exception("Database connection issue. Please verify the MongoDB connection.", ex);
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var result = await _collection.DeleteOneAsync(
                    Builders<Account>.Filter.Eq(a => a.Id, new ObjectId(id))
                );

                if (result.DeletedCount == 0)
                {
                    throw new EntityNotFoundException("Account", id);
                }

                return true;
            }
            catch (MongoException ex)
            {
                _logger.LogError(ex, $"Error deleting account with ID {id}.");
                throw new Exception("Database connection issue. Please verify the MongoDB connection.", ex);
            }
        }
    }
}