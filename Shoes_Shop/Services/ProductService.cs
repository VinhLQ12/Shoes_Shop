using Shoes_Shop.Models;

namespace Shoes_Shop.Services {
    public class ProductService {
        private readonly IDatabaseService _databaseService;

        public ProductService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Product GetUserById(int id)
        {
            return _databaseService.GetById<Product>(id);
        }

        public void AddUser(Product product)
        {
            _databaseService.Add(product);
        }

        public void UpdateUser(Product product)
        {
            _databaseService.Update(product);
        }

        public void DeleteUser(Product product)
        {
            _databaseService.Delete(product);
        }
    }
}
