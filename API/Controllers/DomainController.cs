using API.Responses.Domain;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace API.Controllers
{
    public class DomainController : Controller
    {
        private readonly DatabaseContext _context;

        public DomainController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("class/{classKey}/properties")]
        public IActionResult GetProperties(string classKey = "user")
        {
            var x = _context.XClasses.ToList();
                //.Include(x => x.XProperties);
            //foreach (var item. in x)
            //{
            //}


            //var properties = res.XProperties.Select(n => {
            //    return new XPropertyResumeDTO
            //    {
            //        ID = n.ID,
            //        Name = n.Name,
            //        Key = n.Key,
            //        IsNullable = n.IsNullable,
            //        Lenght = n.Lenght,
            //        PropertyClassKey = n.PropertyClass?.Key
            //    };
            //});

            return Ok(x);
        }
    }
}
