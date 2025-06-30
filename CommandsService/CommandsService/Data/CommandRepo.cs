using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDBContext _context;

        public CommandRepo(AppDBContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;
            _context.Commands.Add(command); 
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null) throw new ArgumentNullException(nameof(plat));

            _context.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            var res = _context.Commands.FirstOrDefault(x => x.PlatformId == platformId && x.Id == commandId);

            return res ?? new Command();
        }

        public IEnumerable<Command> GetCommandForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExist(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0) ;
        }
    }
}
