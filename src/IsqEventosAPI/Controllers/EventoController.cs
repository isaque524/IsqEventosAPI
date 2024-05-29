using IsqEventos.Persistencia;
using IsqEventos.Domain;
using Microsoft.AspNetCore.Mvc;
using IsqEventos.Persistencia.contextos;
using IsqEventos.Application.Contratos;
using Microsoft.AspNetCore.Http;
using IsqEventos.Application.Dtos;
using IsqEventosAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using IsqEventos.Persistencia.Models;
using IsqEventosAPI.Helpers;




namespace IsqEventosAPI.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{


    private readonly IEventosService _eventosService;
    private readonly IUtil _util;
    private readonly IAccountService _accountService;

    private readonly string _destino = "Images";

    public EventoController(IEventosService eventosService, IUtil util, IAccountService accountService)
    {
        _eventosService = eventosService;
        _util = util;
        _accountService = accountService;
    }



    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
    {
        try
        {
            var eventos = await _eventosService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
            if (eventos == null) return NoContent();


            Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

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
                _util.DeleteImage(evento.ImagemURL, _destino);
                evento.ImagemURL = await _util.SaveImage(file, _destino);
            }
            var eventoRetorno = await _eventosService.UpdateEvento(User.GetUserId(), eventoId, evento);

            return Ok(eventoRetorno);
        }
        catch (Exception ex)
        {

            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar realizar upload de foto do Evento. Erro: {ex.Message}");
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
                _util.DeleteImage(evento.ImagemURL, _destino);
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




}


