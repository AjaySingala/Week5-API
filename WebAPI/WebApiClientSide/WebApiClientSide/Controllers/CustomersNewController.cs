using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiClientSide.Models;

namespace WebApiClientSide.Controllers
{
    [RoutePrefix("api/CustomersNew")]
    public class CustomersNewController : ApiController
    {
        public static IList<Customer> _customers = new List<Customer>
        {
            new Customer { Id = 101, Firstname = "Ajay", Lastname = "Singala" },
            new Customer { Id = 102, Firstname = "John", Lastname = "Smith" },
            new Customer { Id = 103, Firstname = "Mary", Lastname = "Jane" }
        };

        // GET: api/CustomersNew
        public IEnumerable<Customer> Get()
        {
            return _customers.ToList();
        }

        // GET: api/CustomersNew/5
        public Customer Get(int id)
        {
            var customer = _customers.
                Where(c => c.Id == id)
                .SingleOrDefault<Customer>();

            return customer;
        }

        // POST: api/CustomersNew
        public void Post([FromBody]Customer customer)
        {
            _customers.Add(customer);
        }

        // PUT: api/CustomersNew/5
        public void Put(int id, [FromBody]Customer customer)
        {
            //var customerToUpdate = this.Get(id);
            this.Delete(id);
            _customers.Add(customer);
        }

        // DELETE: api/CustomersNew/5
        public void Delete(int id)
        {
            var customerToDelete = this.Get(id);
            _customers.Remove(customerToDelete);
        }

        //{"Firstname":"Ajay","Lastname":"Singala"}
        [HttpPost]
        [Route("Search")]
        //[Route("api/CustomersNew/Search")]
        //[Route("api/CustomersNew/Search/{firstname}/{lastname}")]
        //[Route("Search/{firstname}/{lastname}")]
        //public IList<Customer> Search(string firstname, string lastname)
        public IList<Customer> SearchCustomerOnFirstnameAndLastname(Search searchCriteria)
        {
            //var customers = _customers
            //    .Where(c => c.Firstname.Equals(firstname, StringComparison.OrdinalIgnoreCase) &&
            //        c.Lastname.Equals(lastname, StringComparison.OrdinalIgnoreCase))
            //    .ToList<Customer>();

            var customers = _customers
                .Where(c => c.Firstname.Equals(searchCriteria.Firstname, StringComparison.OrdinalIgnoreCase) &&
                    c.Lastname.Equals(searchCriteria.Lastname, StringComparison.OrdinalIgnoreCase))
                .ToList<Customer>();

            return customers;
        }


    }

    public class Search
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }


}
