using LiteDB;
using RmlEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmlEditor.Services
{
    public interface IDatabaseService
    {
        void CreateReceipt(ReceiptModel receipt);
        bool DeleteReceipt(Guid id);
        void Dispose();
        List<ReceiptModel> GetAll();
        ReceiptModel GetSingle(Guid id);
        bool UpdateReceipt(ReceiptModel receipt);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<ReceiptModel> _collection;
        private readonly string _collectionName = "receipts";

        public DatabaseService()
        {
            var baseDirectory = AppContext.BaseDirectory;
            var databaseDirectory = Path.Combine(baseDirectory, "Databases");

            if (!Directory.Exists(databaseDirectory))
            {
                Directory.CreateDirectory(databaseDirectory);
            }

            var databasePath = Path.Combine(databaseDirectory, "ReceiptBuilderData.db");

            // Initialize the database and collection
            _database = new LiteDatabase(databasePath);
            _collection = _database.GetCollection<ReceiptModel>(_collectionName);
        }

        // Get a single receipt by Id
        public ReceiptModel GetSingle(Guid id)
        {
            return _collection.FindById(id);
        }

        // Get all receipts
        public List<ReceiptModel> GetAll()
        {
            return _collection.FindAll().ToList();
        }

        // Create a new receipt
        public void CreateReceipt(ReceiptModel receipt)
        {
            _collection.Insert(receipt);
        }

        // Update an existing receipt
        public bool UpdateReceipt(ReceiptModel receipt)
        {
            return _collection.Update(receipt); // Returns true if update succeeded
        }

        // Delete a receipt by Id
        public bool DeleteReceipt(Guid id)
        {
            return _collection.Delete(id); // Returns true if delete succeeded
        }

        // Dispose pattern for releasing resources
        public void Dispose()
        {
            _database?.Dispose();
        }
    }
}
