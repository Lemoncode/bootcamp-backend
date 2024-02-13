using System.Collections.Generic;
using tour_of_heroes_api.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class HeroRepository : IHeroRepository
{
    private readonly HeroContext _context;
    public HeroRepository(HeroContext context) => _context = context;

    public IEnumerable<Hero> GetAll() => _context.Heroes.ToList();

    public Hero? GetById(int id) => _context.Heroes.FirstOrDefault(h => h.Id == id);

    public void Add(Hero hero)
    {
        _context.Heroes.Add(hero);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var hero = GetById(id);

        if (hero == null) return;        

        _context.Heroes.Remove(hero);
        _context.SaveChanges();
    }

    public void Update(Hero hero)
    {
        var heroToUpdate = GetById(hero.Id);

        if (heroToUpdate == null) return;

        heroToUpdate.Name = hero.Name;
        heroToUpdate.AlterEgo = hero.AlterEgo;
        heroToUpdate.Description = hero.Description;

        _context.Entry(heroToUpdate).State = EntityState.Modified;       

        _context.Heroes.Update(heroToUpdate);
        _context.SaveChanges();
    }
}