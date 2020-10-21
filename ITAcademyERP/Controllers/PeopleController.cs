using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Data;
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

        public async Task<IActionResult> UpdatePerson(PersonDTO personDTO)
        {
            var person = await _repository.GetPerson(personDTO.Id);

            person.Email = personDTO.Email;
            person.FirstName = personDTO.FirstName;
            person.LastName = personDTO.LastName;

            var update = await _repository.Update(person);

            var statusCode = GetHttpStatusCode(update).ToString();

            if (statusCode != "OK")
            {
                return update;
            }
            else
            {
                await _addressesController.CreateOrEditAddresses(personDTO.Addresses);
                return Ok();
            }
        }
    }
}
