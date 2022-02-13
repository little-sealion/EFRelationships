using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFCoreRelationships.Controllers
{
    [Route("api/[controller]")]
    public class WeaponController : Controller
    {
        private readonly DataContext _context;

        public WeaponController(DataContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<Weapon>>> Get(int characterId)
        {
            var weapons = await _context.Weapons
                .Where(w => w.CharacterId == characterId)
                .ToListAsync();
            return Ok(weapons);

        }

        [HttpPost]
        public async Task<ActionResult<List<Weapon>>> CreateWeapon(WeaponDto weapon)
        {
            var character = await _context.Characters.FindAsync(weapon.CharacterId);
            if (character is null) return NotFound();

            var newWeapon = new Weapon
            {
                Name = weapon.Name,
                Damage = weapon.Damage,
                Character = character,
            };



            _context.Weapons.Add(newWeapon);
            await _context.SaveChangesAsync();

            return await Get(weapon.CharacterId);

        }



    }
}

