using IsqEventos.Persistencia;
using IsqEventos.Domain;
using Microsoft.AspNetCore.Mvc;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using IsqEventos.Application.Dtos;
using IsqEventosAPI.Extensions;
using Microsoft.AspNetCore.Authorization;




namespace IsqEventosAPI.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{


    private readonly IEventosService _eventosService;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IAccountService _accountService;

    public EventoController(IEventosService eventosService, IWebHostEnvironment hostEnvironment, IAccountService accountService)
    {
        _eventosService = eventosService;
        _hostEnvironment = hostEnvironment;
        _accountService = accountService;
    }



    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventosService.GetAllEventosAsync(User.GetUserId(), true);
            if (eventos == null) return NoContent();




            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar os eventos. Erro: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var eventos = await _eventosService.GetEventoByIdAsync(User.GetUserId(), id, true);
            if (eventos == null) return NoContent();

            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar o evento. Erro: {ex.Message}");
        }

    }


    [HttpGet("{tema}/tema")]
    public async Task<IActionResult> GetByTema(string tema)
    {
        try
        {
            var eventos = await _eventosService.GetAllEventosByTemaAsync(User.GetUserId(), tema, true);
            if (eventos == null) return NoContent();


            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar os eventos. Erro: {ex.Message}");
        }

    }


    [HttpPost("upload-image/{eventoId}")]
    public async Task<IActionResult> UploadImage(int eventoId)
    {
        try
        {
            var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
            if (evento == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                DeleteImage(evento.ImagemURL);
                evento.ImagemURL = await SaveImage(file);
            }
            var eventoRetorno = await _eventosService.UpdateEvento(User.GetUserId(), eventoId, evento);

            return Ok(eventoRetorno);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar os eventos. Erro: {ex.Message}");
        }
    }



    [HttpPost]
    public async Task<IActionResult> Post(EventoDto model)
    {
        try
        {
            var eventos = await _eventosService.addEventos(User.GetUserId(), model);
            if (eventos == null) return NoContent();

            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar os eventos. Erro: {ex.Message}");
        }
    }

    [HttpPut("{id}")]

    public async Task<IActionResult> Put(int id, EventoDto model)
    {
        try
        {
            var eventos = await _eventosService.UpdateEvento(User.GetUserId(), id, model);
            if (eventos == null) return NoContent();

            return Ok(eventos);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar Atualizar os eventos. Erro: {ex.Message}");
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), id, true);
            if (evento == null) return NoContent();

            if (await _eventosService.DeleteEvento(User.GetUserId(), id))
            {
                DeleteImage(evento.ImagemURL);
                return Ok(new { message = "Deletado" });
            }
            else
            {
                throw new Exception("Ocorreu um problema não específico ao tentar deletar Evento.");
            }
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar os eventos. Erro: {ex.Message}");
        }
    }

    [NonAction]
    public void DeleteImage(string imageName)
    {
        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
        if (System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);

    }

    [NonAction]
    public async Task<string> SaveImage(IFormFile imageFile)
    {
        string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');

        imageName = $"{imageName}{DateTime.UtcNow.ToString("vvmmssfff")}{Path.GetExtension(imageFile.FileName)}";

        var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);

        using (var fileStram = new FileStream(imagePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStram);
        }

        return imageName;

    }


}


