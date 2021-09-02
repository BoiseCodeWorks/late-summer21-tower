using System;
using System.Collections.Generic;
using tower.Models;
using tower.Repositories;

namespace tower.Services
{
  public class EventsService
  {
    private readonly EventsRepository _repo;

    public EventsService(EventsRepository repo)
    {
      _repo = repo;
    }

    internal List<TowerEvent> Get()
    {
      return _repo.Get();
    }

    internal TowerEvent Get(int id)
    {
      TowerEvent evt = _repo.Get(id);
      if (evt == null)
      {
        throw new Exception("Invalid Id");
      }
      return evt;
    }

    internal TowerEvent Create(TowerEvent newTowerEvent)
    {
      return _repo.Create(newTowerEvent);
    }

    internal void Delete(int eventId, string userId)
    {
      TowerEvent eventToDelete = Get(eventId);
      if (eventToDelete.CreatorId != userId)
      {
        throw new Exception("You do NOT have permission to delete this event");
      }
      _repo.Delete(eventId);
    }
  }
}