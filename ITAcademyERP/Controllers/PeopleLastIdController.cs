using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITAcademyERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITAcademyERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleLastIdController : ControllerBase
    {
        private readonly ITAcademyERPContext _context;

        public PeopleLastIdController(ITAcademyERPContext context)
        {
            _context = context;
        }

        // GET: api/PeopleLastId
        [HttpGet]
        public int GetPeopleLastId()
        {
            return _context.Person
                .OrderBy(x => x.Id)
                .LastOrDefault().Id;
        }
    }
}
