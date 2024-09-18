using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using RepoPatt_Core.Interfaces;
using RepoPatt_Core.Models;
using RepoPatt_EF.Repos;
using System.Linq.Expressions;

namespace RepoPatt_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        //private readonly IBaseRepository<Author> _AuthRepository;

        //public AuthorsController(IBaseRepository<Author> baseRepo)
        //{
        //    _AuthRepository = baseRepo;
        //}

        //------------using unit of work
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllAsync()
        {
            var foundItems =await _unitOfWork.Authors.GetAllAsync();
            if(!foundItems.Any())
                return NotFound($"Noo Auths yet");
            return Ok(foundItems);
        } 

        [HttpGet("{PartOfName}")]
        public async Task<IActionResult> GetByPartName( string PartOfName)
        {
            var foundItems =await _unitOfWork.Authors.GetByPartNameAsync(PartOfName);
            if (!foundItems.Any())
                return NotFound($"Noo Auths with : {PartOfName}");
            return Ok(foundItems);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult>  GetByIdAsync(int id) { 
        var foundItem =await _unitOfWork.Authors.GetbyIdAsync(id);
            if (foundItem == null) 
                return NotFound($"Not Found With id : {id}");
            return Ok(foundItem);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Author author)
        {
            if (author == null)
                return BadRequest("CCreate New Author in Request Body");
            var AddedAuth =await _unitOfWork.Authors.AddAsync(author);
            if (AddedAuth == null)
                return BadRequest("Couid not Create New Author");
            return Ok(AddedAuth);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id,[FromBody] Author item)
        {
            var foundItem =await _unitOfWork.Authors.GetbyIdAsync(id);
            if(foundItem == null)
                return NotFound($"Noo Auth with id : {id}");
            _mapper.Map(item, foundItem); // with ignoring id in mappingProfile

            var updatedItem =_unitOfWork.Authors.Update(foundItem);
            if (updatedItem == null)
                return BadRequest($"Updating Not Completed");
            return Ok(foundItem);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var foundItem =await _unitOfWork.Authors.GetbyIdAsync(id);
            if (foundItem == null)
                return NotFound($"Noo Author with id : {id}");
            var DeletedItem = _unitOfWork.Authors.Delete(foundItem);
           return DeletedItem==null?BadRequest($"Couldnot be Deleted : {foundItem.Id}/{foundItem.Name}"):Ok($"Deleted Successfully : {DeletedItem.Id} / {DeletedItem.Name} ");

        }
    }
}
