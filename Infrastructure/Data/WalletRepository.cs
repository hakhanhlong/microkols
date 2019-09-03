using Common;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure.Data
{
    public class WalletRepository : EfRepository<Wallet>, IWalletRepository
    {

        private readonly ILogger<WalletRepository> _logger;
        public WalletRepository(AppDbContext dbContext, ILogger<WalletRepository> logger) : base(dbContext)
        {
            _logger = logger;
        }
        public async Task<Wallet> GetWallet(EntityType entityType, int entityId)
        {
            var entity =  await _dbContext.Wallet.FirstOrDefaultAsync(m => m.EntityType == entityType && m.EntityId == entityId);
            if(entity== null)
            {
                entity = new Wallet()
                {
                    Balance = 0,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    EntityId = entityId,
                    EntityType = entityType,
                    UserCreated = "system",
                    UserModified = "system"
                };
                await _dbContext.Wallet.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

            }
            return entity;
        }

        public int CountAll()
        {
            return _dbContext.Wallet.Count();
        }


        public List<Wallet> Search(string keyword, EntityType entityType, AccountType? type, int pageindex, int pagesize, out int total)
        {
            var wallet = from w in _dbContext.Wallet
                         where w.EntityType == entityType
                         select w;
            
            if (!string.IsNullOrEmpty(keyword))
            {
                if(entityType == EntityType.Account)
                {

                    var list_account = from a in _dbContext.Account
                                       where (a.Name.Contains(keyword) || a.Email.Contains(keyword))
                                       && (type.Value == AccountType.All || a.Type == type.Value)
                                       select a;

                    wallet = from w in wallet
                             where list_account.Select(a => a.Id).Contains(w.EntityId)
                             select w;


                }
                else if(entityType == EntityType.Agency)
                {
                    var list_agency = from a in _dbContext.Agency
                                   where a.Name.Contains(keyword) || a.Email.Contains(keyword) || a.TaxIdNumber.Contains(keyword)
                                   select a;


                    wallet = from w in wallet
                             where list_agency.Select(a => a.Id).Contains(w.EntityId)
                             select w;
                }
            }
            else
            {
                if(entityType == EntityType.Account)
                {
                    var list_account = from a in _dbContext.Account
                                       where (type.Value == AccountType.All || a.Type == type.Value)
                                       select a;

                    wallet = from w in wallet
                             where list_account.Select(a => a.Id).Contains(w.EntityId)
                             select w;
                }
               
            }

            total = wallet.Count();

            return wallet.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
        }

        public async Task<int> GetWalletId(EntityType entityType, int entityId)
        {
            var entity = await GetWallet(entityType, entityId);
            return entity != null ? entity.Id : 0;
        }
        public async Task<long> GetBalance(EntityType entityType, int entityId)
        {
            var entity = await GetWallet(entityType, entityId);
            return entity != null ? entity.Balance : 0;
        }

        public async Task<int> CreateWallet(EntityType entityType, int entityId)
        {
            var entity = await _dbContext.Wallet.FirstOrDefaultAsync(m => m.EntityType == entityType && m.EntityId == entityId);
            if (entity == null)
            {
                entity = new Wallet()
                {
                    Balance = 0,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    EntityId = entityId,
                    EntityType = entityType,
                    UserCreated = "system",
                    UserModified = "system",
                };
                await _dbContext.Wallet.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            return entity.Id;
        }

        public async Task<int> GetSystemId()
        {
            return await GetWalletId(EntityType.System, 0);
            
        }

        public async Task<long> Exchange(int walletid, long value, string username)
        {
            long newamount = 0;
            var entity = await _dbContext.Wallet.FindAsync(walletid);
            if (entity == null)
            {
                return -1;
            }

            var saved = false;
            while (!saved)
            {
                try
                {
                    var oldamount = entity.Balance;
                    // Attempt to save changes to the database
                    long newamounttmp = entity.Balance + value;
                    if (newamounttmp < 0) return -2;

                    entity.Balance = newamounttmp;
                    entity.DateModified = DateTime.Now;
                    entity.UserModified = username;
                    await _dbContext.SaveChangesAsync();
                    newamount = newamounttmp;
                    saved = true;
                    _logger.LogInformation($"Exchange ->  {walletid} -> {oldamount} | {value} | {newamounttmp}");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogInformation($"Exchange -> {walletid} -> {value} -> DbUpdateConcurrencyException: {ex.Message}");
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Wallet)
                        {

                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();


                            /*
                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                // proposedValues[property] = <value to be saved>;
                            }
                            */

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            //throw new NotSupportedException(
                            //    "Don't know how to handle concurrency conflicts for "
                            //    + entry.Metadata.Name);
                            return -3;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Exchange ->  {walletid} -> Exception: {ex.Message}");
                    return -3;
                }
            }
            return newamount;
        }


    }
}
