using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoPatt_Api.Constants;
using RepoPatt_Core.Interfaces;
using RepoPatt_Core.Models;
using System.Linq.Expressions;
using RepoPatt_Core;
using AutoMapper;

namespace RepoPatt_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //private readonly IBaseRepository<Book> _bookRepository;
        //public BooksController(IBaseRepository<Book> bookRepository)
        //{
        //    _bookRepository = bookRepository;
        //}
        //----------------------------using IUnitOfWork
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var foundItems = await _unitOfWork.Books.GetAllAsync();
            if (!foundItems.Any())
                return NotFound($"Noo Books yet");
            return Ok(foundItems);
        }

        [HttpGet("{PartOfName}")]
        public async Task<IActionResult> GetByPartName(string PartOfName)
        {
            var foundItems = await _unitOfWork.Books.GetByPartNameAsync(PartOfName);
            if (!foundItems.Any())
                return NotFound($"Noo Books with : {PartOfName}");
            return Ok(foundItems);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var foundItem = await _unitOfWork.Books.GetbyIdAsync(id);
            if (foundItem == null)
                return NotFound($"Not Found With id : {id}");
            return Ok(foundItem);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Book book)
        {
            if (book == null)
                return BadRequest("CCreate New Book in Request Body");
            var AddedAuth = await _unitOfWork.Books.AddAsync(book);
            if (AddedAuth == null)
                return BadRequest("Couid not Create New Author");
            return Ok(AddedAuth);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id, [FromBody] Book item)
        {
            var foundItem = await _unitOfWork.Books.GetbyIdAsync(id);
            if (foundItem == null)
                return NotFound($"Noo Auth with id : {id}");
            //foundItem.Name=item.Name;
            item.Id = id;
            foundItem.Name = item.Name;
            foundItem.AuthorId = item.AuthorId;

            var updatedItem = _unitOfWork.Books.Update(foundItem);
            if (updatedItem == null)
                return BadRequest($"Updating Not Completed");
            return Ok(foundItem);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var foundItem = await _unitOfWork.Books.GetbyIdAsync(id);
            if (foundItem == null)
                return NotFound($"Noo Book with id : {id}");
            var DeletedItem = _unitOfWork.Books.Delete(foundItem);
            return DeletedItem == null
             ? BadRequest($"Could not be deleted => {foundItem.Id}/{foundItem.Name}")
             : Ok($"Deleted successfully => {DeletedItem.Id} / {DeletedItem.Name}/{DeletedItem.Author.Name}");
        }

        //special Method for books only
        [HttpGet("GetAllEngTitles")]
        public async Task<IActionResult> GetAllEng()
        {
            var foundBooks = _unitOfWork.Books.GetEngTitlesAsync();
            if (!foundBooks.Any())
                return NotFound($"Noo Books with English Title");
            return Ok(foundBooks);  
        }

    }
}
