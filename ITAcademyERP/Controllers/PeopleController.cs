using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Data;
using ITAcademyERP.Data.Resources;
using ITAcademyERP.Data.Repositories;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Employee")]
    [ApiController]
    public class PeopleController : GenericController<string, Person, PeopleRepository>
    {
        private readonly AddressesController _addressesController;
        private readonly PeopleRepository _repository;

        public PeopleController(
            AddressesController addressesController,
            PeopleRepository repository) : base (repository)
        {
            _addressesController = addressesController;
            _repository = repository;
        }

        public async Task<IActionResult> AddPerson(Person newPerson)
        {
            var person = new Person
            {
                Email = newPerson.Email,
                FirstName = newPerson.FirstName,
                LastName = newPerson.LastName
            };           

            var add = await _repository.Add(person);

            var statusCode = add
            .GetType()
            .GetProperty("StatusCode")
            .GetValue(add, null).ToString();

            if (statusCode != "200")
            {
                return add;
            }
            else
            {
                foreach (var address in newPerson.Addresses)
                {
                    address.PersonId = person.Id;
                }

                await _addressesController.CreateOrEditAddresses(newPerson.Addresses);
                
                return Ok();
            }
        }

        public async Task<IActionResult> UpdatePerson(Person personUpdate)
            {
            var person = await _repository.GetPerson(personUpdate.Id);

            person.Email = personUpdate.Email;
            person.FirstName = personUpdate.FirstName;
            person.LastName = personUpdate.LastName;

            var update = await _repository.Update(person);

            var statusCode = update
                .GetType()
                .GetProperty("StatusCode")
                .GetValue(update, null).ToString();

            if (statusCode != "200")
            {
                return update;
            }
            else
            {
                await _addressesController.CreateOrEditAddresses(personUpdate.Addresses);
                return Ok();
            }
        }
    }
}
