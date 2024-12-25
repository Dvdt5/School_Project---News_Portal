using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School_Project___News_Portal.Models;
using School_Project___News_Portal.Repositories;
using School_Project___News_Portal.ViewModels;

namespace School_Project___News_Portal.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly TodoRepository _todoRepository;
        private readonly IMapper _mapper;
        ResultModel resultModel = new ResultModel();

        public TodoController(TodoRepository todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }



        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListAjax()
        {
            var todos = await _todoRepository.GetAllAsync();
            var todoModels = _mapper.Map<List<TodoModel>>(todos);

            return Json(todoModels);
        }

        public async Task<IActionResult> GetByIdAjax(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            var todoModel = _mapper.Map<TodoModel>(todo);
            return Json(todoModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddUpdateAjax(TodoModel model)
        {
            if (model.Id == 0)
            {
                var todo = new Todo();
                todo.Title = model.Title;
                todo.Description = model.Description;
                todo.IsOK = model.IsOK;
                todo.IsActive = true;
                todo.Created = DateTime.Now;
                todo.Updated = DateTime.Now;
                await _todoRepository.AddAsync(todo);
                resultModel.Status = true;
                resultModel.Message = "Task Added";
            }
            else
            {
                var todo = await _todoRepository.GetByIdAsync(model.Id);
                if (todo == null)
                {
                    resultModel.Status = false;
                    resultModel.Message = "Couldnt find the task!";
                    return Json(resultModel);
                }
                todo.Title = model.Title;
                todo.Description = model.Description;
                todo.IsOK = model.IsOK;
                todo.Updated = DateTime.Now;
                await _todoRepository.UpdateAsync(todo);
                resultModel.Status = true;
                resultModel.Message = "Task Updated";
            }
            return Json(resultModel);
        }
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
            {
                resultModel.Status = false;
                resultModel.Message = "Couldnt find the task!";
                return Json(resultModel);
            }
            await _todoRepository.DeleteAsync(id);
            resultModel.Status = true;
            resultModel.Message = "Task Deleted";
            return Json(resultModel);
        }
    }
}
