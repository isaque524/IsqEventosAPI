using IsqEventos.Persistencia;
using IsqEventos.Domain;
using Microsoft.AspNetCore.Mvc;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using IsqEventos.Application.Dtos;




namespace IsqEventosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{


    private readonly IEventosService _eventosService;
    private readonly IWebHostEnvironment _hostEnvironment;

    public EventoController(IEventosService eventosService, IWebHostEnvironment hostEnvironment)
    {
        _eventosService = eventosService;
        _hostEnvironment = hostEnvironment;


    }



    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var eventos = await _eventosService.GetAllEventosAsync(true);
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
            var eventos = await _eventosService.GetEventoByIdAsync(id, true);
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
            var eventos = await _eventosService.GetAllEventosByTemaAsync(tema, true);
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
            var evento = await _eventosService.GetEventoByIdAsync(eventoId, true);
            if (evento == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                DeleteImage(evento.ImagemURL);
                evento.ImagemURL = await SaveImage(file);
            }
            var eventoRetorno = await _eventosService.UpdateEvento(eventoId, evento);

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
            var eventos = await _eventosService.addEventos(model);
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
            var eventos = await _eventosService.UpdateEvento(id, model);
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
            var evento = await _eventosService.GetEventoByIdAsync(id, true);
            if (evento == null) return NoContent();

            if (await _eventosService.DeleteEvento(id))
            {
                DeleteImage(evento.ImagemURL);
                return Ok(new { message = "Deletado" });
            }
            else
            {
                throw new Exception("Ocorreu um problem não específico ao tentar deletar Evento.");
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


