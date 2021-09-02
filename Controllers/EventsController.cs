using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tower.Models;
using tower.Services;

namespace tower.Controllers
{
  [ApiController]
  [Route("/api/[controller]")]
  public class EventsController : ControllerBase
  {
    private readonly EventsService _eventsService;

    public EventsController(EventsService eventsService)
    {
      _eventsService = eventsService;
    }

    [HttpGet]
    public ActionResult<List<TowerEvent>> Get()
    {
      try
      {
        List<TowerEvent> events = _eventsService.Get();
        return Ok(events);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }


    [HttpGet("{id}")]
    public ActionResult<TowerEvent> Get(int id)
    {
      try
      {
        TowerEvent evt = _eventsService.Get(id);
        return Ok(evt);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpPost]
    [Authorize]
    // NOTE Task allows us to use async/await
    public async Task<ActionResult<TowerEvent>> Create([FromBody] TowerEvent newTowerEvent)
    {
      try
      {
        // NEVER TRUST THE CLIENT TO TELL YOU WHO THEY ARE
        // req.userInfo : GetUserInfoAsync
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // req.body.creatorId = req.userinfo.id
        newTowerEvent.CreatorId = userInfo.Id;
        TowerEvent evt = _eventsService.Create(newTowerEvent);
        return Ok(evt);
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<String>> Delete(int id)
    {
      try
      {
        // NEVER TRUST THE CLIENT TO TELL YOU WHO THEY ARE
        // req.userInfo : GetUserInfoAsync
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // req.body.creatorId = req.userinfo.id
        _eventsService.Delete(id, userInfo.Id);
        return Ok("Delorted");
      }
      catch (Exception err)
      {
        return BadRequest(err.Message);
      }
    }

  }

}