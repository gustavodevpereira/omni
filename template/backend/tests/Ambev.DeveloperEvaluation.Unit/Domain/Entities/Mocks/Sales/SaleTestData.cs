using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData.Sales
{
    /// <summary>
    /// Centraliza a geração de dados de teste para a entidade Sale, garantindo consistência entre os testes.
    /// </summary>
    public static class SaleTestData
    {
        // Configuração para geração de uma instância válida de Sale.
        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale(
                f.Commerce.Ean13(),
                DateTime.UtcNow,
                f.Random.AlphaNumeric(10),
                f.Name.FullName(),
                f.Random.AlphaNumeric(10),
                f.Company.CompanyName()
            ));

        /// <summary>
        /// Gera uma instância válida de Sale.
        /// </summary>
        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }

        /// <summary>
        /// Gera uma instância inválida de Sale, definindo um dos campos obrigatórios como vazio.
        /// O parâmetro invalidField define qual campo será inválido: "saleNumber", "customerExternalId",
        /// "customerName", "branchExternalId" ou "branchName".
        /// </summary>
        public static Sale GenerateInvalidSale(string invalidField)
        {
            var faker = new Faker();
            string saleNumber = invalidField == "saleNumber" ? "" : faker.Commerce.Ean13();
            DateTime saleDate = DateTime.UtcNow;
            string customerExternalId = invalidField == "customerExternalId" ? "" : faker.Random.AlphaNumeric(10);
            string customerName = invalidField == "customerName" ? "" : faker.Name.FullName();
            string branchExternalId = invalidField == "branchExternalId" ? "" : faker.Random.AlphaNumeric(10);
            string branchName = invalidField == "branchName" ? "" : faker.Company.CompanyName();

            return new Sale(saleNumber, saleDate, customerExternalId, customerName, branchExternalId, branchName);
        }

        /// <summary>
        /// Gera os dados de um produto para uso em testes de adição de itens à venda.
        /// </summary>
        /// <param name="quantity">Quantidade do produto (impacta na aplicação de descontos).</param>
        /// <param name="unitPrice">Preço unitário do produto (caso não informado, é gerado um valor aleatório entre 10 e 100).</param>
        /// <returns>Uma tupla contendo o productExternalId, productName, quantity e unitPrice.</returns>
        public static (string productExternalId, string productName, int quantity, decimal unitPrice) GenerateProductData(int quantity, decimal? unitPrice = null)
        {
            var faker = new Faker();
            return (
                productExternalId: faker.Commerce.Ean13(),
                productName: faker.Commerce.ProductName(),
                quantity: quantity,
                unitPrice: unitPrice ?? faker.Random.Decimal(10, 100)
            );
        }
    }
}
