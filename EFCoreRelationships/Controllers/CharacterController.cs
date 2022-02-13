using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EFCoreRelationships.Controllers
{
    [Route("api/[controller]")]
    public class CharacterController : Controller
    {
        private readonly DataContext _context;

        public CharacterController(DataContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await _context.Characters
                .Where(c => c.UserId == userId)
                .Include(c => c.Weapon)
                .Include(c=>c.Skills)
                .ToListAsync();
            return characters;

        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> CreateCharacter(CreateCharacterDto character)
        {
            var user = await _context.Users.FindAsync(character.UserId);
            if (user is null) return NotFound();

            var newCharacter = new Character{
                Name = character.Name,
                RpgClass = character.RpgClass,
                User = user,
                UserId = character.UserId
            };

       

            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            return await Get(character.UserId);
        }

        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDto request)
        {
            var character = await _context.Characters
                .Where(c => c.Id == request.CharacterId)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync();
            if (character is null) return NotFound();

            var skill = await _context.Skills.FindAsync(request.SkillId);
            if (skill is null) return NotFound();

            character.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return character;
        }


    }
}

