using MediCase.WebAPI.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.WebAPI.Repositories.Content.Interfaces;
using MediCase.WebAPI.Exceptions;

namespace MediCase.WebAPI.Repositories.Content
{
    public class EntityTranslationFilesRepository : IEntityTranslationFilesRepository
    {
        private readonly MediCaseContentContext _context;
        private readonly ILogger _logger;

        public EntityTranslationFilesRepository(MediCaseContentContext context, ILogger<EntityTranslationFilesRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<string> GetActualFilePathAsync(string hashedFileName)
        {
            var file = await _context.EntityTranslationFiles.Where(x => x.FilePathHashed == hashedFileName).FirstOrDefaultAsync();
            if (file == null) throw new NotFoundException("File not found exception");
            return file.FilePath;
        }

        public async Task<string> GetActualFilePathAsync(uint fileId)
        {
            var file = await _context.EntityTranslationFiles.Where(x => x.FileId == fileId).FirstOrDefaultAsync();
            if (file == null) throw new NotFoundException("File not found exception");
            return file.FilePath;
        }
    }
}
