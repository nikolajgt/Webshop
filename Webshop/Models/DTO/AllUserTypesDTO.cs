namespace Webshop.Models.DTO
{
    public class AllUserTypesDTO
    {
        public List<Customer> Customers { get; set; }
        public List<Admin> Admins { get; set; }

        public AllUserTypesDTO(List<Customer> customers, List<Admin> admins)
        {
            Customers = customers;
            Admins = admins;
        }
    }
}
